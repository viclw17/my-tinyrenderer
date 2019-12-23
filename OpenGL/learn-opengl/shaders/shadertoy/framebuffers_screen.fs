#version 330 core
out vec4 fragColor;

in vec2 TexCoords;

uniform vec3 iResolution;
uniform int iFrame;
uniform sampler2D screenTexture;


void main()
{
    vec3 col = texture(screenTexture, TexCoords).rgb;
    fragColor = vec4(col, 1.0);
	//fragColor = vec4(1,0,0, 1.0);
} 