/*#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoords;

out vec2 TexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    TexCoords = aTexCoords;    
    gl_Position = projection * view * model * vec4(aPos, 1.0);
}*/

#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aFragCoord;
//layout(location = 1) in vec2 aTexCoords;

//out vec2 TexCoords;
out vec2 fragCoord;

void main()
{
	//TexCoords = aTexCoords;
	fragCoord = aFragCoord;
	gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
}