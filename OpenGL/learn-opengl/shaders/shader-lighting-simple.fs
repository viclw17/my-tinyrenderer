#version 330 core

#define LIGHT_TYPE 0 // 0:directional; 1:point; 2:spot
#define VIS_DEPTH 0
#define REFLECTION 0
#define REFRACTION 0

/*
Vertex Shader的输出在Clip Space，那Fragment Shader的输入在什么空间？
不是NDC，而是屏幕空间Screen Space。我们前面说到Vertex Shader的输出在Clip Space，接着GPU会做透视除法变到NDC。
这之后GPU还有一步，应用视口变换，转换到Screen Space，输入给Fragment Shader：
(Vertex Shader) => Clip Space => (透视除法) => NDC => (视口变换) => Screen Space => (Fragment Shader)。
 */

in vec2 TexCoords;
in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

uniform float iTime; // we set this variable in the OpenGL code.
uniform vec3 viewPos; // camera position
uniform samplerCube skybox;

struct Material{
    vec3 ambient;
//    vec3 diffuse;
    sampler2D diffuse;
    vec3 specular;
    //sampler2D specular;
    float shininess;
};
uniform Material material;

struct Light {
    vec3 position; // for point light & spot light; not necessery for directional light
    vec3 direction; // for directional light & spot light
    float cutOff;
    float outerCutOff;
    
	// lightColor
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    
    float constant; // for point light
    float linear;
    float quadratic;
};
uniform Light light;

// vis depth buffer
float near = 0.1;
float far = 100.0;
float LinearizeDepth(float depth){
    float z = depth * 2.0 - 1.0; // back to NDC
    return (2.0 * near * far) / (far + near - z * (far - near));
}

void main(){
    
    // prepare for lighting calculation
    vec3 norm = normalize(Normal);

#if LIGHT_TYPE == 0
    vec3 lightDir = normalize(-light.direction); // light.direction is pointing from light source
#else
    vec3 lightDir = normalize(light.position - FragPos); // this direction is pointing towards light source
#endif
    
    // AMBIENT
    //float ambientStrength = 0.1;
//    vec3 ambient = light.ambient * material.ambient;
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    
    // DIFFUSE
    float diff = max(dot(norm, lightDir), 0);
//    vec3 diffuse = light.diffuse * diff * material.diffuse;
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
    
    // SPECULAR
    //float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    // "reflect" expects the first vector to point from the light source towards the fragment’s position
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = light.specular * spec * material.specular;
    //vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));

#if LIGHT_TYPE == 1 // point light, not directional
    float distance    = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
                               light.quadratic * (distance * distance));
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
#endif
    
#if LIGHT_TYPE == 2 // spotlight (soft edges), directional!
    float theta = dot(lightDir, normalize(-light.direction));
    float epsilon = (light.cutOff - light.outerCutOff);
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
    diffuse  *= intensity;
    specular *= intensity;
#endif
    
    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);

#if VIS_DEPTH == 1
    //FragColor = vec4(vec3(gl_FragCoord.z), 1.0);
    float depth = LinearizeDepth(gl_FragCoord.z) / far; // divide by far for demonstration
    FragColor = vec4(vec3(depth), 1.0);
#endif
#if REFLECTION == 1
    vec3 I = normalize(FragPos - viewPos);
    vec3 R = reflect(I, normalize(Normal));
    FragColor = vec4(texture(skybox, R).rgb, 1.0);
#endif
#if REFRACTION == 1
    float ratio = 1.00 / 1.52;
    vec3 I = normalize(FragPos - viewPos);
    vec3 R = refract(I, normalize(Normal), ratio);
    FragColor = vec4(texture(skybox, R).rgb, 1.0);
#endif
}
