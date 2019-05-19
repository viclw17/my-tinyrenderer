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
//    vec3      specular;
    sampler2D diffuse; // use diffuse map
    sampler2D specular;
    float     shininess;
};
uniform Material material;

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform Light light;

void main(){
    
//    float sine = (sin(iTime*5)+1)*.5;
//    vec4 texColor = mix(texture(texture0, TexCoord), texture(texture1, TexCoord), 0);
    
//    float ambientStrength = 0.1;
//    vec3 ambient = light.ambient * material.ambient;
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0);
    vec3 diffuse = light.diffuse * (diff * vec3(texture(material.diffuse, TexCoords)));
    
//    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * vec3(texture(material.specular, TexCoords)));
    
    vec3 result = ambient + diffuse + specular;

    FragColor = vec4(result, 1.0);
}
