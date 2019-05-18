#ifndef CAMERA_H
#define CAMERA_H

#include <glad/glad.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>

#include <vector>

// Defines several possible options for camera movement.
// Used as abstraction to stay away from window-system specific input methods
enum Camera_Movement{
    FORWARD,
    BACKWARD,
    LEFT,
    RIGHT
};

// default cam values
const float YAW         = -90; // yaw is initialized to -90.0 degrees since a yaw of 0.0 results in a direction vector pointing to the right so we initially rotate a bit to the left.
const float PITCH       = 0;
const float SPEED       = 2.5;
const float SENSITIVITY = 0.1;
const float ZOOM        = 45; // fov

// abstract camera class
// 1. process input
// 2. calculate euler angles/ vectors/ matrices
class Camera{
public:
    //camera attibutes
    glm::vec3 Position;
    glm::vec3 Front;
    glm::vec3 Up;
    glm::vec3 Right;
    glm::vec3 WorldUp;
    // euler angles
    float Yaw;
    float Pitch;
    // camera options
    float MovementSpeed;
    float MouseSensitivity;
    float Zoom;
    
    // constructor with vectors
    Camera(glm::vec3 position = glm::vec3(0,0,0),
           glm::vec3 up       = glm::vec3(0,1,0),
           float yaw   = YAW,
           float pitch = PITCH) :
    Front(glm::vec3(0,0,-1)),
    MovementSpeed(SPEED),
    MouseSensitivity(SENSITIVITY),
    Zoom(ZOOM) {
        Position = position;
        WorldUp  = up;
        Yaw      = yaw;
        Pitch    = pitch;
        
        updateCameraVectors();
    }
    
    // constructors with scalar values
    Camera(float posX, float posY, float posZ,
           float upX,  float upY,  float upZ,
           float yaw,  float pitch) :
    Front(glm::vec3(0,0,-1)),
    MovementSpeed(SPEED),
    MouseSensitivity(SENSITIVITY),
    Zoom(ZOOM) {
        Position = glm::vec3(posX,posY,posZ);
        WorldUp  = glm::vec3(upX,upY,upZ);
        Yaw      = yaw;
        Pitch    = pitch;
        
        updateCameraVectors();
    }
    
    glm::mat4 GetViewMatrix(){
        // glm::mat4 view = glm::lookAt(cameraPos, cameraPos + cameraFront, cameraUp);
        return glm::lookAt(Position, Position + Front, Up);
    }
    
    // accept input param in form of camera defiend ENUM, to abstract it from windowing system
    void ProcessKeyboard(Camera_Movement direction, float deltaTime){
        float velocity = MovementSpeed * deltaTime;
        if (direction == FORWARD)
            Position += Front * velocity;
        if (direction == BACKWARD)
            Position -= Front * velocity;
        if (direction == LEFT)
            Position -= Right * velocity;
        if (direction == RIGHT)
            Position += Right * velocity;
    }
    
    void ProcessMouseMovement(float xoffset, float yoffset, GLboolean constrainPitch = true){
        xoffset *= MouseSensitivity;
        yoffset *= MouseSensitivity;
        Yaw += xoffset;
        Pitch += yoffset;
        
//        if (constrainPitch)
//        {
//            if (Pitch > 89.0f)
//                Pitch = 89.0f;
//            if (Pitch < -89.0f)
//                Pitch = -89.0f;
//        }
        
        updateCameraVectors();
    }
    
    void ProcessMouseScroll(float yoffset)
    {
        if (Zoom >= 1.0f && Zoom <= 45.0f)
            Zoom -= yoffset;
        if (Zoom <= 1.0f)
            Zoom = 1.0f;
        if (Zoom >= 45.0f)
            Zoom = 45.0f;
    }
    
private:
    // Calculates the front vector from the Camera's (updated) Euler Angles
    void updateCameraVectors()
    {
        // Calculate the new Front vector
        glm::vec3 front;
        front.x = cos(glm::radians(Yaw)) * cos(glm::radians(Pitch));
        front.y = sin(glm::radians(Pitch));
        front.z = sin(glm::radians(Yaw)) * cos(glm::radians(Pitch));
        Front = glm::normalize(front);
        // Also re-calculate the Right and Up vector
        Right = glm::normalize(glm::cross(Front, WorldUp));  // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
        Up    = glm::normalize(glm::cross(Right, Front));
    }
};
#endif
