#version 330 core
in vec2 TexCoords;
out vec4 FragColor;

//uniform vec3 color;

uniform sampler2D diffuseTex;

void main(){
	//FragColor = vec4(1,0,0,1);
	FragColor = vec4(vec3(texture(diffuseTex, TexCoords)),1);
}