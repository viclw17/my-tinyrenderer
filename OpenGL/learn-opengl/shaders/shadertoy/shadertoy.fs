#version 330 core

// in out
in vec2 fragCoord;
out vec4 fragColor;

// uniforms
uniform vec3 iResolution;
uniform float iTime;
uniform sampler2D iChannel0;

//void mainImage(out vec4, in vec2);
//void main(void) { mainImage(fragColor, iResolution.xy); }

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

/*
// shadertoy template
void main(){
    float sine = (sin(iTime*5)+3)*.25; // debug sine
    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = fragCoord/iResolution.xy;
    // Time varying pixel color
    vec3 col = 0.5 + 0.5*cos(iTime+uv.xyx+vec3(0,2,4));
    // Output to screen
    fragColor = vec4(col,1.0);
}
*/



// Progressive Mandelbrot Plotting
/*const float RES = 10.;
const float max_iter = 100.;
void main(){
    vec2 uv = (2.*fragCoord-iResolution.xy)/iResolution.y;

    float s = 2.;
    uv *= s;
    float sin01 = (sin((iTime))+1.)*.5;
    float counter = floor(sin01*RES);
    vec2 c = floor((uv)*counter+vec2(.5))/counter;
    
    // Main algorithm
    vec2 z = vec2(0);
    float iter = 0.;
    for(float i = 0.; i < max_iter; i++){
        // z = z^2 + c
        z = vec2(z.x*z.x-z.y*z.y, 2.*z.x*z.y)+c;
        if(length(z)>2.) break;
        iter ++;
    }
    
    float f = iter/max_iter;
    vec3 gold = vec3(1, .874, 0);
    vec3 col = mix(vec3(.5), gold, f);
    
    // Grids and bands
    uv *= 2.;
    if(fract(abs(uv.x))<.03||fract(abs(uv.y))<.03) col = vec3(0);
    //col *= (abs(uv.x)>4.)?0.:1.;
	// test texture
	uv = (2.*fragCoord-iResolution.xy)/iResolution.y;
	vec4 texCol = texture(iChannel0, uv);
    col = (abs(uv.x)>1) ? texCol.xyz : col;
    fragColor = vec4(col,1);
}*/

