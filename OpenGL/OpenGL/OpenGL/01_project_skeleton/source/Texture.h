#include <GL/glew.h>
#include "Bitmap.h"
//---
#include <stdexcept>
using namespace tdogl;

// hearder declaration
namespace tdogl{
    class Texture{
    public:
        Texture(const Bitmap& bitmap,
        GLint minMagFilter = GL_LINEAR,
        GLint wrapMode = GL_CLAMP_TO_EDGE);
        ~Texture();

        GLuint object() const;
        GLfloat originalWidth() const;
        GLfloat originalHeight() const;
    private:
        GLuint _object;
        GLfloat _originalWidth;
        GLfloat _originalHeight;

        Texture(const Texture&);
        const Texture& operator=(const Texture&);
    }
}

// cpp defination
// static utility function
static GLenum TextureFormatForBitmapFormat(Bitmap::Format format){
    switch(format){
        case Bitmap::Format_Grayscale: return GL_LUMINANCE;
        case Bitmap::Format_GayscaleAlpha: return GL_LUMINANCE_ALPHA;
        case Bitmap::Format_RGB: return GL_RGB;
        case Bitmap::Format_RGBA: return GL_RGBA;
        default: throw std::runtime_error("cant recognize");
    }
}

Texture::Texture(const Bitmap& bitmap, GLint minMagFilter, GLint wrapMode) :
_originalWidth((GLfloat)bitmap.width()),
_originalHeight((GLfloat)bitmap.height()){
    glGenTexture(1, &_object);
    glBindTexture(GL_TEXTURE_2D, _object);

    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, minMagFiler);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, minMagFiler);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, wrapMode);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, wrapMode);

    glTexImage2D(
        GL_TEXTURE_2D,
        0,
        TextureFormatForBitmapFormat(bitmap.format()),
        (GLsizei)bitmap.width(),
        (GLsizei)bitmap.height(),
        0,
        TextureFormatForBitmapFormat(bitmap.format()),
        GL_UNSIGNED_BYTE,
        bitmap.pixelBuffer());

    glBindTexture(GL_TEXTURE_2D, 0);
}

Texture::~Texture(){
    glDeleteTextures(1, &_object);
}

GLuint Texture::object() const{
    return _object;
}

GLfloat Texture::originalWidth() const{
    return _originalWidth;
}

GLfloat Texture::originalHeight() const{
    return _originalHeight;
}
