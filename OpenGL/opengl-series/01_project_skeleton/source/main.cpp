#include "platform.hpp"

#include <GL/glew.h>
//#include <glew.c> // fix - Undefined symbols for architecture x86_64: "___glewAttachShader", referenced from:...
//no need if added in build phase
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

// standard c++ libraries
#include <cassert>
#include <iostream>
#include <stdexcept>
#include <cmath>

// tdogl classes
#include "Program.h"

// constants
const glm::vec2 SCREEN_SIZE(400,400);

// globals
GLFWwindow* gWindow = NULL;
GLuint gVAO = 0;
GLuint gVBO = 0;
tdogl::Program* gProgram = NULL;


// loads the vertex shader and fragment shader, and links them to make the global gProgram
static void LoadShaders(){
    std::vector<tdogl::Shader> shaders;
    shaders.push_back(tdogl::Shader::shaderFromFile(
    ResourcePath("vertex-shader.txt"), GL_VERTEX_SHADER));
    shaders.push_back(tdogl::Shader::shaderFromFile(
    ResourcePath("fragment-shader.txt"), GL_FRAGMENT_SHADER));
	gProgram = new tdogl::Program(shaders);
}

static void LoadTriangle(){
    glGenVertexArrays(1, &gVAO);
    glBindVertexArray(gVAO);
    glGenBuffers(1, &gVBO);
    glBindBuffer(GL_ARRAY_BUFFER, gVBO);

    GLfloat vertexData[] = {
        // X       Y     Z
         0.000f,  0.5f, 0.0f,
        -0.577f, -0.5f, 0.0f,
         0.577f, -0.5f, 0.0f,
    };

    glBufferData(GL_ARRAY_BUFFER, sizeof(vertexData), vertexData, GL_STATIC_DRAW);
    glEnableVertexAttribArray(gProgram->attrib("vert"));
    glVertexAttribPointer(gProgram->attrib("vert"), 3, GL_FLOAT, GL_FALSE, 0, NULL);

    glBindBuffer(GL_ARRAY_BUFFER,0);
    glBindVertexArray(0);
}

static void Render(){
    glClearColor(0,0,0,1);
    glClear(GL_COLOR_BUFFER_BIT);

    glUseProgram(gProgram->object());
    glBindVertexArray(gVAO);
	glDrawArrays(GL_TRIANGLES,0,3);

    glBindVertexArray(0);
	glUseProgram(0);

    glfwSwapBuffers(gWindow);
}

// void OnError(int errorCode, const char* msg) {
//     throw std::runtime_error(msg);
// }
//
// static void key_callback(GLFWwindow* window, int key, int scancode, int action, int mods){
//     if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
//         glfwSetWindowShouldClose(window, 1);
// }

void AppMain(){
    // initialise GLFW
    glfwInit(); //victor, simple
//    glfwSetErrorCallback(OnError);
//    if(!glfwInit())
//        throw std::runtime_error("glfwInit failed");

    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR,3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR,2);
    glfwWindowHint(GLFW_RESIZABLE, GL_FALSE);

	gWindow = glfwCreateWindow((int)SCREEN_SIZE.x,(int)SCREEN_SIZE.y,"test",NULL,NULL);

//    if(!gWindow)
//        throw std::runtime_error("glfwCreateWindow failed. Can your hardware handle OpenGL 3.2?");
    
    // Victor
    // Receiving input events
//    glfwSetKeyCallback(gWindow, key_callback);
    
    // GLFW settings
    // Before you can use the OpenGL API, you must have a current OpenGL context.
    glfwMakeContextCurrent(gWindow);
    
    // initialise GLEW
    glewExperimental = GL_TRUE; //stops glew crashing on OSX :-/
    glewInit(); //victor, simple
//    if(glewInit() != GLEW_OK)
//        throw std::runtime_error("glewInit failed");

    LoadShaders();
    LoadTriangle();

    while(!glfwWindowShouldClose(gWindow)){
        glfwPollEvents();
        Render();
    }
    glfwTerminate();
}

int main(int argc, char *argv[]){
    AppMain();
    // try{
    //     AppMain();
    // }catch(const std::exception& e){
    //     std::cerr << "ERROR: " << e.what() << std::endl;
    //     return EXIT_FAILURE;
    // }
    // return EXIT_SUCCESS;
    return 0;
}
