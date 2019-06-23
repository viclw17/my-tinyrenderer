#version 330 core

#define POST_PROCESSING 0

in vec2 TexCoords;
out vec4 FragColor;

uniform sampler2D screenTexture;

const float offset = 1.0 / 500.0;

void main(){

	// PP_inversion
	//FragColor = vec4(vec3(1.0 - col), 1.0);
	
	// PP_greyscale
	//float average = (col.r + col.g + col.b) / 3.0;
	//float average = 0.2126 * col.r + 0.7152 * col.g + 0.0722 * col.b;
	//FragColor = vec4(average, average, average, 1.0);

	vec2 offsets[9] = vec2[](
		vec2(-offset, offset), // top-left
		vec2( 0.0f, offset), // top-center
		vec2( offset, offset), // top-right
		vec2(-offset, 0.0f), // center-left
		vec2( 0.0f, 0.0f), // center-center
		vec2( offset, 0.0f), // center-right
		vec2(-offset, -offset), // bottom-left
		vec2( 0.0f, -offset), // bottom-center
		vec2( offset, -offset) // bottom-right
	);

	float kernel_sharpen[9] = float[](
		-1, -1, -1,
		-1,  9, -1,
		-1, -1, -1
	);

	float kernel_blur[9] = float[](
		1.0 / 16, 2.0 / 16, 1.0 / 16,
		2.0 / 16, 4.0 / 16, 2.0 / 16,
		1.0 / 16, 2.0 / 16, 1.0 / 16
	);

	float kernel_edge[9] = float[](
		1,1,1,
		1,-8,1,
		1,1,1
	);

	vec3 col = vec3(0.0);

#if POST_PROCESSING == 1
	vec3 sampleTex[9];
	for(int i = 0; i < 9; i++)
		sampleTex[i] = vec3(texture(screenTexture, TexCoords.st + offsets[i]));

	for(int i = 0; i < 9; i++)
		col += sampleTex[i] * kernel_edge[i];

#else
	col = texture(screenTexture, TexCoords).rgb;
#endif

	FragColor = vec4(col, 1.0);
}