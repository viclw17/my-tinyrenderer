#version 330 core
in vec2 TexCoord;
in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

uniform float iTime; // we set this variable in the OpenGL code.
uniform sampler2D texture0;
uniform sampler2D texture1;

uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 lightPos;

void main(){
//    float sine = (sin(iTime*5)+3)*.25;
    vec4 texColor = mix(texture(texture0, TexCoord), texture(texture1, TexCoord), 0.5);//*(sin(iTime*10)+1));
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0);
    vec3 diffuse = diff * lightColor;
    vec3 result = (ambient + diffuse) * texColor.xyz;//objectColor;

    FragColor = vec4(result, 1.0);
}
