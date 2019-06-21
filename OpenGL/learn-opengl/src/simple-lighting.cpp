#include <glad/glad.h>
#include <GLFW/glfw3.h>
#define STB_IMAGE_IMPLEMENTATION
#include "stb_image.h"

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <iostream>

#include <learnopengl/shader_victor.h>
#include <learnopengl/camera_victor.h>

// callback
void processInput              (GLFWwindow *window);
void mouse_callback            (GLFWwindow* window, double xpos,    double ypos);
void scroll_callback           (GLFWwindow* window, double xoffset, double yoffset);
void framebuffer_size_callback (GLFWwindow* window, int    width,   int    height);

// settings
const unsigned int SCR_WIDTH  = 800;
const unsigned int SCR_HEIGHT = 800;

// camera
//Camera camera = Camera(); // by default camera position at 0,0,0
glm::vec3 cameraPos = glm::vec3(0,0,5);
Camera camera(cameraPos);
bool firstMouse = true;
float lastX =  SCR_WIDTH / 2.0; // center of screen
float lastY =  SCR_HEIGHT / 2.0;

// timing
float deltaTime = 0.0f;
float lastFrame = 0.0f;

// lighting
glm::vec3 lightPos(1.2f, 0, 2.0f);



int main() {
    // glfw: init and config
    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
#ifdef __APPLE__
    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE); // uncomment this statement to fix compilation on OS X
#endif

    // create glfw window
    GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "LearnOpenGL", NULL, NULL);
    if (window == NULL) {
        std::cout << "Failed to create GLFW window" << std::endl;
        glfwTerminate();
        return -1;
    }

    glfwMakeContextCurrent(window);
    // setup callback
    glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
    glfwSetCursorPosCallback(window, mouse_callback);
    glfwSetScrollCallback(window, scroll_callback);
    glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

    // glad: load all OpenGL function pointers
    if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress)) {
        std::cout << "Failed to initialize GLAD" << std::endl;
        return -1;
    }
    
    // gl functions should be called after setup glad
    glEnable(GL_DEPTH_TEST);
    glDepthFunc(GL_LESS);
//    glDepthMask(GL_FALSE);
    
    //glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

    
    // Load/Compile/Link Shaders
    // MacOS
    #ifdef __APPLE__
    Shader lightingShader = Shader(
        "/Users/wei_li/Git/my-tinyrenderer/OpenGL/learn-opengl/shaders/shader-lighting.vs",
        "/Users/wei_li/Git/my-tinyrenderer/OpenGL/learn-opengl/shaders/shader-lighting.fs"
    );
    Shader lampShader = Shader(
       "/Users/wei_li/Git/my-tinyrenderer/OpenGL/learn-opengl/shaders/light.vs",
       "/Users/wei_li/Git/my-tinyrenderer/OpenGL/learn-opengl/shaders/light.fs"
       );
    #else
    // Windows
    Shader lightingShader = Shader(
        "../../../shaders/shader-lighting.vs",
        "../../../shaders/shader-lighting.fs"
    );
    Shader lampShader = Shader(
       "../../../shaders/light.vs",
       "../../../shaders/light.fs"
    );
    #endif
    
    
    // load vertices
    float vertices[] = {
        //x    y      z       nx     ny     nz     u     v
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
         0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
        
        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
        
        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
        
         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
       
        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
        
        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
    };

    // world space positions of our cubes
    glm::vec3 cubePositions[] = {
        glm::vec3( 0.0f,  0.0f,  0.0f),
        glm::vec3( 2.0f,  5.0f, -15.0f),
        glm::vec3(-1.5f, -2.2f, -2.5f),
        glm::vec3(-3.8f, -2.0f, -12.3f),
        glm::vec3( 2.4f, -0.4f, -3.5f),
        glm::vec3(-1.7f,  3.0f, -7.5f),
        glm::vec3( 1.3f, -2.0f, -2.5f),
        glm::vec3( 1.5f,  2.0f, -2.5f),
        glm::vec3( 1.5f,  0.2f, -1.5f),
        glm::vec3(-1.3f,  1.0f, -1.5f)
    };
    
    unsigned int VBO;
    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
    
    unsigned int  cubeVAO;
    glGenVertexArrays(1, &cubeVAO);
    glBindVertexArray(cubeVAO);
    
    // (index,size,normalized,stride,offset) index <-> shader layout
    // position attrib
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);
    // normal attrib
    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
    glEnableVertexAttribArray(1);
    // texcoord attrib
    glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(6 * sizeof(float)));
    glEnableVertexAttribArray(2);
    
    unsigned int lampVAO; // use the same VBO
    glGenVertexArrays(1, &lampVAO);
    glBindVertexArray(lampVAO);
    
    // position attrib
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);

    // load and create a texture
    // texture 0
    unsigned int texture0;
    glGenTextures(1, &texture0);
    glBindTexture(GL_TEXTURE_2D, texture0);
    // all upcoming GL_TEXTURE_2D operations now have effect on this "texture" object
    // set texture wrapping parameters
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT); // default wrapping method
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
    // set texture filtering parameters
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    // load image
    int width, height, nrChannels;
	unsigned char* data;
#ifdef __APPLE__
	data = stbi_load("/Users/wei_li/Git/my-tinyrenderer/OpenGL/learn-opengl/textures/container2_diffuse.png",
                     &width, &height, &nrChannels, 0);
#else
	data = stbi_load("../../../textures/container2_diffuse.png",
                     &width, &height, &nrChannels, 0);
#endif
    if (data)
    {
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data); // create texture
        glGenerateMipmap(GL_TEXTURE_2D); // generate mipmaps
    }
    else
    {
        std::cout << "Failed to load texture" << std::endl;
    }
    stbi_image_free(data);
    
    // texture 1
    unsigned int texture1;
    glGenTextures(1, &texture1);
    glBindTexture(GL_TEXTURE_2D, texture1);
    // all upcoming GL_TEXTURE_2D operations now have effect on this "texture" object
    // set texture wrapping parameters
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT); // default wrapping method
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
    // set texture filtering parameters
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    // flip
	stbi_set_flip_vertically_on_load(true); // tell stb_image.h to flip loaded texture's on the y-axis.
    // load image
#ifdef __APPLE__
    data = stbi_load("/Users/wei_li/Git/my-tinyrenderer/OpenGL/learn-opengl/textures/container2_specular.png",
                     &width, &height, &nrChannels, 0);
#else
	data = stbi_load("../../../textures/container2_specular.png",
                     &width, &height, &nrChannels, 0);
#endif
    if (data)
    {
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data); // create texture, here use GL_RGBA for png!
        glGenerateMipmap(GL_TEXTURE_2D); // generate mipmaps
    }
    else
    {
        std::cout << "Failed to load texture" << std::endl;
    }
    stbi_image_free(data);
    
    
    
    // tell opengl for each sampler to which texture unit it belongs to
    lightingShader.use(); // activate the shader before setting uniforms
//    unsigned int texture0Loc = glGetUniformLocation(lightingShader.ID, "texture0");
//    unsigned int texture1Loc = glGetUniformLocation(lightingShader.ID, "texture1");
//    glUniform1i(texture0Loc, 0);
//    glUniform1i(texture1Loc, 1);
    lightingShader.setInt("material.diffuse", 0); // lightmaps
    lightingShader.setInt("material.specular", 1);
    

    
    // render loop
    while (!glfwWindowShouldClose(window))
    {
        // per-frame time logic
        float currentFrame = glfwGetTime();
        deltaTime = currentFrame - lastFrame;
        lastFrame = currentFrame;

        // input
        processInput(window);

        // clear
        glClearColor(0,0,0, 1);
        //glClearColor(.2,.2,.2, 1);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        
        // bind textures to texture units
        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, texture0);
        glActiveTexture(GL_TEXTURE1);
        glBindTexture(GL_TEXTURE_2D, texture1);
        
        lightingShader.use();
//        float iTime = glGetUniformLocation(lightingShader.ID, "iTime");
//        glUniform1f(iTime, glfwGetTime());
        lightingShader.setFloat("iTime",         glfwGetTime());
        lightingShader.setVec3("viewPos",        camera.Position);
        lightingShader.setVec3("light.position", lightPos);
        
        // directional light
        lightingShader.setVec3("light.direction", -0.2, -1.0, -0.3); // light.direction point from light source
        
		// point light
		lightingShader.setFloat("light.constant", 1.0f);
		lightingShader.setFloat("light.linear", 0.09f);//0.09
		lightingShader.setFloat("light.quadratic", 0.032f);//0.032

        // spot light
//        lightingShader.setVec3("light.position", camera.Position); // for camera based spot light
        lightingShader.setVec3("light.direction", glm::vec3(-.5,0,-1)); // for spot light
        lightingShader.setFloat("light.cutOff", glm::cos(glm::radians(12.5f)));
        lightingShader.setFloat("light.outerCutOff", glm::cos(glm::radians(17.5f)));
        
    
        // light properties
        glm::vec3 lightColor;
        lightColor = glm::vec3(1);
        glm::vec3 diffuseColor = lightColor ;//  * glm::vec3(0.5f);
        glm::vec3 ambientColor = diffuseColor * glm::vec3(0.5f);
        lightingShader.setVec3("light.ambient", ambientColor);
        lightingShader.setVec3("light.diffuse", diffuseColor);
        lightingShader.setVec3("light.specular", 1.0f, 1.0f, 1.0f);
        
        // material properties
        lightingShader.setVec3("material.ambient",  1.0f, 0.5f, 0.31f);
        lightingShader.setVec3("material.diffuse",  1.0f, 0.5f, 0.31f);
        lightingShader.setVec3("material.specular", 0.5f, 0.5f, 0.5f);
        lightingShader.setFloat("material.shininess", 32.0f);//32
        
        // MVP
        // view/projection transformations
        glm::mat4 projection = glm::perspective(glm::radians(camera.Zoom), (float)SCR_WIDTH / (float)SCR_HEIGHT, 0.1f, 100.0f);
        glm::mat4 view = camera.GetViewMatrix();//glm::lookAt(cameraPos, cameraPos + cameraFront, cameraUp);
        lightingShader.setMat4("projection", projection);
        lightingShader.setMat4("view",       view);
        // world transformation
        glm::mat4 model;
        lightingShader.setMat4("model", model);
        // render the cube
        glBindVertexArray(cubeVAO);
//        glDrawArrays(GL_TRIANGLES, 0, 36);
        for(unsigned int i = 0; i<10; i++){
            glm::mat4 model;
            model = glm::translate(model, cubePositions[i]);
            float angle = 20 * i;
            model = glm::rotate(model, glm::radians(angle), glm::vec3(1,0.3,0.5));
            lightingShader.setMat4("model", model);
            glDrawArrays(GL_TRIANGLES,0,36);
        }
        
        // LIGHT SOURCE
        lampShader.use();
        lampShader.setFloat("iTime",     glfwGetTime());
        lampShader.setMat4("projection", projection);
        lampShader.setMat4("view",       view);
        model = glm::mat4();
        lightPos = lightPos + glm::vec3(0,0.01*sin(5*glfwGetTime()),0);
        model = glm::translate(model, lightPos);
        model = glm::scale(model, glm::vec3(0.2f));
        lampShader.setMat4("model",      model);
        // render the lamp cube
        glBindVertexArray(lampVAO);
        glDrawArrays(GL_TRIANGLES, 0, 36);
        
        // glfw: swap buffers and poll IO events (keys pressed/released, mouse moved etc.)
        glfwSwapBuffers(window);
        glfwPollEvents();
    }

    // optional: de-allocate all resources once they've outlived their purpose:
    glDeleteVertexArrays(1, &cubeVAO);
    glDeleteVertexArrays(1, &lampVAO);
    glDeleteBuffers(1, &VBO);

    // glfw: terminate, clearing all previously allocated GLFW resources.
    glfwTerminate();
    return 0;
}





// process all input
// glfw: whenever keys are pressed/released
void processInput(GLFWwindow *window){
    if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
        glfwSetWindowShouldClose(window, true);
    
    if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
        camera.ProcessKeyboard(FORWARD, deltaTime);
    if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
        camera.ProcessKeyboard(BACKWARD, deltaTime);
    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
        camera.ProcessKeyboard(LEFT, deltaTime);
    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
        camera.ProcessKeyboard(RIGHT, deltaTime);
    
//    if(glfwGetKey(window,GLFW_KEY_Q)==GLFW_PRESS)
//        cameraPos += cameraSpeed * cameraUp ;
//    if(glfwGetKey(window, GLFW_KEY_E)==GLFW_PRESS)
//        cameraPos -= cameraSpeed * cameraUp ;
}

// glfw: whenever the mouse moves, this callback is called
void mouse_callback(GLFWwindow* window, double xpos, double ypos){
    if (firstMouse){
        lastX = xpos;
        lastY = ypos;
        firstMouse = false;
    }
    
    float xoffset = xpos - lastX;
    float yoffset = lastY - ypos; // reversed since y-coordinates go from bottom to top
    // update last position
    lastX = xpos;
    lastY = ypos;
    
    camera.ProcessMouseMovement(xoffset, yoffset);

    // calculate direction vector
    // now done by Camera Class!
//    glm::vec3 front;
//    front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
//    front.y = sin(glm::radians(pitch));
//    front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
//    cameraFront = glm::normalize(front);
    
}

// glfw: whenever the mouse scroll wheel scrolls, this callback is called
void scroll_callback(GLFWwindow* window, double xoffset, double yoffset){
    camera.ProcessMouseScroll(yoffset);
}

// glfw: whenever the window size changed (by OS or user resize) this callback function executes
void framebuffer_size_callback(GLFWwindow* window, int width, int height){
    // make sure the viewport matches the new window dimensions; note that width and
    // height will be significantly larger than specified on retina displays.
    glViewport(0, 0, width, height);
}
