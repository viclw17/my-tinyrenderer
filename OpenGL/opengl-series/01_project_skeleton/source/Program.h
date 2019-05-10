#include "Shader.h"
#include <vector>
// ---
#include <stdexcept>

namespace tdogl {
	class Program{
	public:
		Program(const std::vector<Shader>& shaders);
		~Program();
		GLuint object() const;
		GLint attrib(const GLchar* attribName) const;
		GLint uniform(const GLchar* uniformName) const;
	private:
		GLuint _object;
		Program(const Program&);
		const Program& operator=(const Program&);
	};
}

using namespace tdogl;

Program::Program(const std::vector<Shader>& shaders):
_object(0){
    if(shaders.size()<=0)
        // throw

    _object = glCreateProgram();

    if(_object==0)
        // throw

    // attach all the shaders
    for(unsigned i = 0; i < shaders.size(); ++i)
        glAttachShader(_object, shaders[i].object());
    // link shaders together
    glLinkProgram(_object);
    // detach all the shaders
    for(unsigned i=0; i<shaders.size(); ++i)
        glDetachShader(_object,shaders[i].object());

    GLint status;
    glGetProgramiv(_object,GL_LINK_STATUS,&status);
    if(status == GL_FALSE){
        // throw
    }
}

Program::~Program() {
    //might be 0 if ctor fails by throwing exception
    if(_object != 0) glDeleteProgram(_object);
}

GLuint Program::object() const {
    return _object;
}

GLint Program::attrib(const GLchar* attribName) const {
    // if(!attribName)
    //     throw std::runtime_error("attribName was NULL");

    GLint attrib = glGetAttribLocation(_object, attribName);

    // if(attrib == -1)
    //     throw std::runtime_error(std::string("Program attribute not found: ") + attribName);

    return attrib;
}

GLint Program::uniform(const GLchar* uniformName) const {
    // if(!uniformName)
    //     throw std::runtime_error("uniformName was NULL");

    GLint uniform = glGetUniformLocation(_object, uniformName);

    // if(uniform == -1)
    //     throw std::runtime_error(std::string("Program uniform not found: ") + uniformName);

    return uniform;
}
