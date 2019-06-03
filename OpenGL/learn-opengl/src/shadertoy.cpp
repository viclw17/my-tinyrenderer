#include <glad/glad.h>
#include <GLFW/glfw3.h>
#define STB_IMAGE_IMPLEMENTATION
#include "stb_image.h"

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <iostream>

#include <learnopengl/shader_victor.h>

// callback
void processInput(GLFWwindow *window);
void framebuffer_size_callback(GLFWwindow* window, int width, int height);

const unsigned int SCR_WIDTH  = 800;
const unsigned int SCR_HEIGHT = 600;

int main() {
    // glfw: initialize and configure
    // ------------------------------
    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
#ifdef __APPLE__
    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE); // uncomment this statement to fix compilation on OS X
#endif

    // glfw window creation
    GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "LearnOpenGL", NULL, NULL);
    if (window == NULL) {
        std::cout << "Failed to create GLFW window" << std::endl;
        glfwTerminate();
        return -1;
    }

    glfwMakeContextCurrent(window);
    glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);


    // glad: load all OpenGL function pointers
    if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress)) {
        std::cout << "Failed to initialize GLAD" << std::endl;
        return -1;
    }

    // Load/Compile/Link Shaders
    //MacOS
    #ifdef __APPLE__
    Shader ourShader = Shader(
        "/Users/wei_li/Git/my-tinyrenderer/OpenGL/learn-opengl/shaders/shadertoy/shadertoy.vs",
        "/Users/wei_li/Git/my-tinyrenderer/OpenGL/learn-opengl/shaders/shadertoy/pbr.fs"
    );
    #else
    //Windows
    Shader ourShader = Shader(
        "../../../shaders/shadertoy/shadertoy.vs",
        //"../../../shaders/shadertoy/pbr.fs"
		"simple-pbr.fs"
    );
    #endif

    // set up vertex data (and buffer(s)) and configure vertex attributes
	float xFragAmount = float(SCR_WIDTH);
	float yFragAmount = float(SCR_HEIGHT);
    float vertices[] = {
         // positions          // frag coords
		 1.0f,  1.0f,  0.0f,   xFragAmount, yFragAmount,// UR
		 1.0f, -1.0f,  0.0f,   xFragAmount, 0.0f,		// BR
        -1.0f, -1.0f,  0.0f,   0.0f,		0.0f,		// BL
        -1.0f,  1.0f,  0.0f,   0.0f,		yFragAmount // UL
    };
    
    unsigned int indices[] = {
        0, 1, 3, // first triangle
        1, 2, 3  // second triangle
    };
    
    unsigned int VBO, VAO, EBO;
    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO);
    glGenBuffers(1, &EBO);

    // bind the Vertex Array Object first, then bind and set vertex buffer(s), and then configure vertex attributes(s).
    // bind VAO
    glBindVertexArray(VAO);
    // copy vertices array in a vertex buffer
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
    // copy indices array in a element buffer
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);
    
    // set the vertex attributes pointers
    //layout (location = 0) in vec3 aPos;
    //layout (location = 1) in vec3 aColor;
    //layout (location = 2) in vec2 aTexCoord;
    // position attribute
    // (index,size,normalized,stride,offset)
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);
	glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)(3 * sizeof(float)));
	glEnableVertexAttribArray(1);
    
    // note that this is allowed, the call to glVertexAttribPointer registered VBO as the vertex attribute's bound vertex buffer object so afterwards we can safely unbind
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    
    // remember: do NOT unbind the EBO while a VAO is active as the bound element buffer object IS stored in the VAO; keep the EBO bound.
//    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
    
    // You can unbind the VAO afterwards so other VAO calls won't accidentally modify this VAO, but this rarely happens. Modifying other
    // VAOs requires a call to glBindVertexArray anyways so we generally don't unbind VAOs (nor VBOs) when it's not directly necessary.
    glBindVertexArray(0);

    // load and create a texture
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
	data = stbi_load("/Users/wei_li/Git/my-tinyrenderer/OpenGL/learn-opengl/textures/parchment.jpg",
                                    &width, &height, &nrChannels, 0);
#else
	data = stbi_load("../../../textures/parchment.jpg",
		&width, &height, &nrChannels, 0);
#endif
    if (data) {
        // create texture
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, data);
        glGenerateMipmap(GL_TEXTURE_2D); // generate mipmaps
    }
    else{
        std::cout << "Failed to load texture" << std::endl;
    }
    stbi_image_free(data);

    
    
    // get and set unifroms, here only do once
	// tell opengl for each sampler to which texture unit it belongs to
    ourShader.use(); // don't forget to activate/use the shader before setting uniforms!
    unsigned int texture0Loc = glGetUniformLocation(ourShader.ID, "iChannel0");
    glUniform1i(texture0Loc, 0); // set it manually
    //ourShader.setInt("iChannel0", 0); // glUniform1i(glGetUniformLocation(ID, name.c_str()), value);

	glm::vec3 iResolution(xFragAmount, yFragAmount, 1);
	ourShader.setVec3("iResolution", iResolution);
    
    // uncomment this call to draw in wireframe polygons.
    //glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

    // render loop
    while (!glfwWindowShouldClose(window))
    {
        // input
        processInput(window);

        // clean up
        glClearColor(0,0,0, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT);

        ourShader.use();
        
        // get and set uniform, here do on every loop
        float iTime = glGetUniformLocation(ourShader.ID, "iTime");
        glUniform1f(iTime, glfwGetTime());
		
        // bind textures on corresponding texture units
        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, texture0);

        glBindVertexArray(VAO);
        //glDrawArrays(GL_TRIANGLES, 0, 3);
        glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
        //glBindVertexArray(0);
        
        // glfw: swap buffers and poll IO events
        glfwSwapBuffers(window);
        glfwPollEvents();
    }

    // optional: de-allocate all resources once they've outlived their purpose:
    glDeleteVertexArrays(1, &VAO);
    glDeleteBuffers(1, &VBO);
    glDeleteBuffers(1, &EBO);

    // glfw: terminate, clearing all previously allocated GLFW resources.
    glfwTerminate();
    return 0;
}





// process all input: query GLFW whether relevant keys are pressed/released this frame and react accordingly
void processInput(GLFWwindow *window)
{
if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
glfwSetWindowShouldClose(window, true);
}

// glfw: whenever the window size changed (by OS or user resize) this callback function executes
void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
    // make sure the viewport matches the new window dimensions; note that width and
    // height will be significantly larger than specified on retina displays.
    glViewport(0, 0, width, height);
}


