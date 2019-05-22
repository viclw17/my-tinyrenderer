#version 330 core
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
//uniform sampler2D texture0;
//uniform sampler2D texture1;

//uniform vec3 objectColor;
//uniform vec3 lightColor; // for ambient
//uniform vec3 lightPos; // to calculate diffuse
uniform vec3 viewPos; // to calculate specular

struct Material{
//    vec3 ambient;
//    vec3 diffuse;
//    vec3 specular;
    sampler2D diffuse; // use diffuse map
    sampler2D specular;
    float     shininess;
};
uniform Material material;

struct DirLight{
    vec3 direction; // for directional light & spot light
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight{
    vec3 position; // for point light, spot light; no longer necessery for directional light
    
    float constant; // for point light
    float linear;
    float quadratic;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
#define NR_POINT_LIGHTS 4
uniform PointLight pointLights[NR_POINT_LIGHTS];

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

struct Light {
    
    
    float cutOff;
    float outerCutOff;
    
    
};
uniform Light light;

void main(){
    
    // time, textures
//    float sine = (sin(iTime*5)+1)*.5;
//    vec4 texColor = mix(texture(texture0, TexCoord), texture(texture1, TexCoord), 0);
    
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    vec3 result = CalcDirLight(dirLight, norm, viewDir);
    
    for(int i=0; i<NR_POINT_LIGHTS; i++)
        result += CalcPointLight(pointLight[i], norm, FragPos, viewDir);
    
    // spotlight (soft edges)
//    float theta = dot(lightDir, normalize(-light.direction));
//    float epsilon = (light.cutOff - light.outerCutOff);
//    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
//    diffuse  *= intensity;
//    specular *= intensity;
    
    FragColor = vec4(result, 1.0);
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir){
    // if use directional light
    vec3 lightDir = normalize(-light.direction); // light.direction is pointing from light source
    
    // AMBIENT
    //float ambientStrength = 0.1;
    //vec3 ambient = light.ambient * material.ambient;
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    
    // DIFFUSE
    float diff = max(dot(norm, lightDir), 0);
    vec3 diffuse = light.diffuse * (diff * vec3(texture(material.diffuse, TexCoords)));
    
    // SPECULAR
    //float specularStrength = 0.5;
    vec3 reflectDir = reflect(-lightDir, norm); // reflect expects the first vector to point from the light source towards the fragment’s position
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * vec3(texture(material.specular, TexCoords)));
    
    return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir){
    // if use simple/point light
    vec3 lightDir = normalize(light.position - fragPos); // this direction is pointing towards light source
    
    // point light
    float distance    = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
                               light.quadratic * (distance * distance));
    
    // AMBIENT
    //float ambientStrength = 0.1;
    //vec3 ambient = light.ambient * material.ambient;
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    
    // DIFFUSE
    float diff = max(dot(norm, lightDir), 0);
    vec3 diffuse = light.diffuse * (diff * vec3(texture(material.diffuse, TexCoords)));
    
    // SPECULAR
    //float specularStrength = 0.5;
    vec3 reflectDir = reflect(-lightDir, norm); // reflect expects the first vector to point from the light source towards the fragment’s position
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * vec3(texture(material.specular, TexCoords)));
    
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    return (ambient + diffuse + specular);
}


