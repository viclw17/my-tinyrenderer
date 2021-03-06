#version 330 core
in vec2 TexCoord;
out vec4 FragColor;

uniform float iTime; // we set this variable in the OpenGL code.
uniform sampler2D texture0;
uniform sampler2D texture1;

void main(){
    float sine = (sin(iTime*5)+3)*.25;
    FragColor = mix(texture(texture0, TexCoord), texture(texture1, TexCoord), 0);//0.5*(sin(iTime*2)+1));
}
