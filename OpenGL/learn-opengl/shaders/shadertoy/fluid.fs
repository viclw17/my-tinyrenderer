#version 330 core

// in out
in vec2 fragCoord;
out vec4 fragColor;

// uniforms
uniform vec3 iResolution;
uniform float iTime;
uniform sampler2D iChannel0;

/*
uniform float     iTimeDelta;            // render time (in seconds)
uniform int       iFrame;                // shader playback frame
uniform float     iChannelTime[4];       // channel playback time (in seconds)
uniform vec3      iChannelResolution[4]; // channel resolution (in pixels)
uniform vec4      iMouse;                // mouse pixel coords. xy: current (if MLB down), zw: click
uniform samplerXX iChannel0..3;          // input channel. XX = 2D/Cube
uniform vec4      iDate;                 // (year, month, day, time in seconds)
uniform float     iSampleRate;           // sound sample rate (i.e., 44100)
*/

struct Ray{
	vec3 origin;
    vec3 direction;
};

struct Sphere{
	vec3 center;
    float radius;
};

float RaySphereIntersection(Ray ray, Sphere sphere){
	vec3 oc = ray.origin - sphere.center;
    float a = dot(ray.direction, ray.direction);
    float b = 2.*dot(oc,ray.direction);
    float c = dot(oc,oc)-sphere.radius*sphere.radius;
    float discriminant = b*b-4.*a*c;
    
    if(discriminant<0.)
        return -1.;
    else
        return (-b-sqrt(discriminant))/(2.*a);
}

void main()
{
    // Scale and bias uv
    // [0.0, iResolution.x] -> [0.0, 1.0]
    // [0.0, 1.0] 			-> [-1.0, 1.0]
    vec2 xy = fragCoord / iResolution.xy;
	xy = xy * 2.- vec2(1.);
	xy.x *= iResolution.x/iResolution.y;
        
    Sphere sphere = Sphere(vec3(0.), 1.0); // Sphere position at (0,0,0)
    
	vec3 pixelPos = vec3(xy, 2.); // Image plane at (0,0,2)
    vec3 eyePos = vec3(0.,0.,5.); // Camera position at (0,0,5)
    vec3 rayDir = normalize(pixelPos - eyePos);
     
    float dist = RaySphereIntersection(Ray(eyePos, rayDir), sphere);

    // Didn't hit anything
    if (dist < 0.) {
        fragColor = vec4(0.);
		return;
    }

    // Hit on the surface
    fragColor = vec4(1.,0.,0.,1.);
}