#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal; // normal vectors
layout (location = 2) in vec3 aTexCoord;

// pass to frag shader
out vec3 FragPos;
out vec3 Normal;
out vec2 TexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    // Clip Space是一个顶点乘以MVP矩阵之后所在的空间，Vertex Shader的输出就是在Clip Space上（划重点），接着由GPU自己做透视除法将顶点转到NDC。
    //gl_Position = projection * view * model * vec4(aPos, 1.0f); // achieve 3d
    
    // pass to frag shader
    //FragPos = vec3(model * vec4(aPos,1.0)); // from local space to world space
    //Normal = aNormal;
    
    FragPos = vec3(model * vec4(aPos, 1.0));
    Normal = mat3(transpose(inverse(model))) * aNormal;
    gl_Position = projection * view * vec4(FragPos, 1.0);
    
    TexCoords = vec2(aTexCoord.x, aTexCoord.y);
}
