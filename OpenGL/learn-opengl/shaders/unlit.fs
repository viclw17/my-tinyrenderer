#version 330 core

#define VIS_DEPTH 0

in vec2 TexCoords;
out vec4 FragColor;

uniform sampler2D diffuseTex;

// vis depth buffer
float near = 0.1;
float far = 100.0;
float LinearizeDepth(float depth){
    float z = depth * 2.0 - 1.0; // back to NDC
    return (2.0 * near * far) / (far + near - z * (far - near));
}

void main(){
	FragColor = vec4(vec3(texture(diffuseTex, TexCoords)),1);

#if VIS_DEPTH == 1
    float depth = LinearizeDepth(gl_FragCoord.z) / far; // divide by far for demonstration
    FragColor = vec4(vec3(depth), 1.0);
#endif
}