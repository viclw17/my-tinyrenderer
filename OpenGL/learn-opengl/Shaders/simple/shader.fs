//#version 330 core
//out vec4 FragColor;
//void main()
//{
//    FragColor = vec4(0,1,1, 1.0f);
//}


#version 330 core
out vec4 FragColor;
in vec3 ourColor;

uniform float iTime; // we set this variable in the OpenGL code.

void main(){
    float sine = (sin(iTime*5)+1)*.5;
    FragColor = vec4(ourColor * sine, 1.0f);
}
