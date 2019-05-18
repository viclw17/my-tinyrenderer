#version 330 core
out vec4 FragColor;

uniform float iTime;

void main()
{
    float sine = (sin(iTime*5)+3)*.25;
    FragColor = vec4(vec3(1.0), 1);
}
