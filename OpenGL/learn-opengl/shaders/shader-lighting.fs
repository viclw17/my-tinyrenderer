#version 330 core
/*
Vertex Shader的输出在Clip Space，那Fragment Shader的输入在什么空间？
不是NDC，而是屏幕空间Screen Space。我们前面说到Vertex Shader的输出在Clip Space，接着GPU会做透视除法变到NDC。
这之后GPU还有一步，应用视口变换，转换到Screen Space，输入给Fragment Shader：
(Vertex Shader) => Clip Space => (透视除法) => NDC => (视口变换) => Screen Space => (Fragment Shader)。
 */
in vec2 TexCoord;
in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

uniform float iTime; // we set this variable in the OpenGL code.
uniform sampler2D texture0;
uniform sampler2D texture1;

uniform vec3 objectColor;
uniform vec3 lightColor; // for ambient
uniform vec3 lightPos; // to calculate diffuse
uniform vec3 viewPos; // to calculate specular

void main(){
    
    float sine = (sin(iTime*5)+1)*.5;
    vec4 texColor = mix(texture(texture0, TexCoord), texture(texture1, TexCoord), 0);
    vec3 color = mix(objectColor, texColor.xyz, sine);
    
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;
    
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0);
    vec3 diffuse = diff * lightColor;
    
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;
    
    vec3 result = (ambient + diffuse + specular) * color;

    FragColor = vec4(result, 1.0);
}
