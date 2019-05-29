#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

glm::vec3 cameraPos = glm::vec3(0,0,3);
glm::vec3 cameraTarget = glm::vec3(0,0,0);
glm::vec3 cameraDirection = glm::normalize(cameraPos-cameraTarget);

glm::vec3 up = glm::vec3(0,1,0);
glm::vec3 cameraRight = glm::normalize(glm::cross(up, cameraDirection));
glm::vec3 cameraUp = glm::cross(cameraDirection, cameraRight);

glm::mat4 view = glm::lookAt(cameraPos, cameraTarget, up);

// rotate camera around target
float radius = 10.0f;
float camX = sin(glfwGetTime())*radius;
float camZ = cos(glfwGetTime())*radius;
glm::mat4 view = glm::lookAt(glm::vec3(camX,0,camZ), glm::vec3(0,0,0), glm::vec3(0,1,0));

// -----
glm::vec3 cameraPos = glm::vec3(0,0,3);
glm::vec3 cameraFront = glm::vec3(0,0,-1);
glm::vec3 cameraUp = glm::vec3(0,1,0);
glm::mat4 view = glm::lookAt(cameraPos, cameraPos + cameraFront, cameraUp);
void processInput(GLFWwindow *window)
{
    ...
    float cameraSpeed = 0.05f; // adjust accordingly
    if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
    cameraPos += cameraSpeed * cameraFront;
    if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
    cameraPos -= cameraSpeed * cameraFront;
    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
    cameraPos -= glm::normalize(glm::cross(cameraFront, cameraUp)) * cameraSpeed;
    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
    cameraPos += glm::normalize(glm::cross(cameraFront, cameraUp)) * cameraSpeed;
}

direction.y = sin(glm::radians(pitch));
