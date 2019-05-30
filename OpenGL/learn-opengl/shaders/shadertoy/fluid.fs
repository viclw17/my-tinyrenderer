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

#define IX(x, y, z) ((x) + (y) * N)

struct FluidCube {
    int size;
    float dt;
    float diff;
    float visc;
    
    float *s;
    float *density;
    
    float *Vx;
    float *Vy;

    float *Vx0;
    float *Vy0;
};

FluidCube *FluidCubeCreate(int size, int diffusion, int viscosity, float dt)
{
    FluidCube *cube = malloc(sizeof(*cube));
    int N = size;
    
    cube->size = size;
    cube->dt = dt;
    cube->diff = diffusion;
    cube->visc = viscosity;
    
    cube->s = calloc(N * N, sizeof(float));
    cube->density = calloc(N * N * N, sizeof(float));
    
    cube->Vx = calloc(N * N * N, sizeof(float));
    cube->Vy = calloc(N * N * N, sizeof(float));
    cube->Vz = calloc(N * N * N, sizeof(float));
    
    cube->Vx0 = calloc(N * N * N, sizeof(float));
    cube->Vy0 = calloc(N * N * N, sizeof(float));
    cube->Vz0 = calloc(N * N * N, sizeof(float));
    
    return cube;
}

void FluidCubeFree(FluidCube *cube)
{
    free(cube->s);
    free(cube->density);
    
    free(cube->Vx);
    free(cube->Vy);
    free(cube->Vz);
    
    free(cube->Vx0);
    free(cube->Vy0);
    free(cube->Vz0);
    
    free(cube);
}

static void set_bnd(int b, float *x, int N)
{
    for(int j = 1; j < N - 1; j++) {
        for(int i = 1; i < N - 1; i++) {
            x[IX(i, j, 0  )] = b == 3 ? -x[IX(i, j, 1  )] : x[IX(i, j, 1  )];
            x[IX(i, j, N-1)] = b == 3 ? -x[IX(i, j, N-2)] : x[IX(i, j, N-2)];
        }
    }
    for(int k = 1; k < N - 1; k++) {
        for(int i = 1; i < N - 1; i++) {
            x[IX(i, 0  , k)] = b == 2 ? -x[IX(i, 1  , k)] : x[IX(i, 1  , k)];
            x[IX(i, N-1, k)] = b == 2 ? -x[IX(i, N-2, k)] : x[IX(i, N-2, k)];
        }
    }
    for(int k = 1; k < N - 1; k++) {
        for(int j = 1; j < N - 1; j++) {
            x[IX(0  , j, k)] = b == 1 ? -x[IX(1  , j, k)] : x[IX(1  , j, k)];
            x[IX(N-1, j, k)] = b == 1 ? -x[IX(N-2, j, k)] : x[IX(N-2, j, k)];
        }
    }
    
    x[IX(0, 0, 0)]       = 0.33f * (x[IX(1, 0, 0)]
                                  + x[IX(0, 1, 0)]
                                  + x[IX(0, 0, 1)]);
    x[IX(0, N-1, 0)]     = 0.33f * (x[IX(1, N-1, 0)]
                                  + x[IX(0, N-2, 0)]
                                  + x[IX(0, N-1, 1)]);
    x[IX(0, 0, N-1)]     = 0.33f * (x[IX(1, 0, N-1)]
                                  + x[IX(0, 1, N-1)]
                                  + x[IX(0, 0, N)]);
    x[IX(0, N-1, N-1)]   = 0.33f * (x[IX(1, N-1, N-1)]
                                  + x[IX(0, N-2, N-1)]
                                  + x[IX(0, N-1, N-2)]);
    x[IX(N-1, 0, 0)]     = 0.33f * (x[IX(N-2, 0, 0)]
                                  + x[IX(N-1, 1, 0)]
                                  + x[IX(N-1, 0, 1)]);
    x[IX(N-1, N-1, 0)]   = 0.33f * (x[IX(N-2, N-1, 0)]
                                  + x[IX(N-1, N-2, 0)]
                                  + x[IX(N-1, N-1, 1)]);
    x[IX(N-1, 0, N-1)]   = 0.33f * (x[IX(N-2, 0, N-1)]
                                  + x[IX(N-1, 1, N-1)]
                                  + x[IX(N-1, 0, N-2)]);
    x[IX(N-1, N-1, N-1)] = 0.33f * (x[IX(N-2, N-1, N-1)]
                                  + x[IX(N-1, N-2, N-1)]
                                  + x[IX(N-1, N-1, N-2)]);
}

static void lin_solve(int b, float *x, float *x0, float a, float c, int iter, int N)
{
    float cRecip = 1.0 / c;
    for (int k = 0; k < iter; k++) {
            for (int j = 1; j < N - 1; j++) {
                for (int i = 1; i < N - 1; i++) {
                    x[IX(i, j, m)] =
                        (x0[IX(i, j)]
                            + a*(    x[IX(i+1, j  )]
                                    +x[IX(i-1, j  )]
                                    +x[IX(i  , j+1)]
                                    +x[IX(i  , j-1)]
                                    +x[IX(i  , j  )]
                                    +x[IX(i  , j  )]
                           )) * cRecip;
                }
            }
        }
        set_bnd(b, x, N);
    }
}

static void diffuse (int b, float *x, float *x0, float diff, float dt, int iter, int N)
{
    float a = dt * diff * (N - 2) * (N - 2);
    lin_solve(b, x, x0, a, 1 + 6 * a, iter, N);
}

static void advect(int b, float *d, float *d0,  float *velocX, float *velocY, float dt, int N)
{
    float i0, i1, j0, j1;
    
    float dtx = dt * (N - 2);
    float dty = dt * (N - 2);
    
    float s0, s1, t0, t1;
    float tmp1, tmp2, x, y;
    
    float Nfloat = N;
    float ifloat, jfloat;
    int i, j;
    
        for(j = 1, jfloat = 1; j < N - 1; j++, jfloat++) { 
            for(i = 1, ifloat = 1; i < N - 1; i++, ifloat++) {
                tmp1 = dtx * velocX[IX(i, j, k)];
                tmp2 = dty * velocY[IX(i, j, k)];
                x    = ifloat - tmp1; 
                y    = jfloat - tmp2;
                
                if(x < 0.5f) x = 0.5f; 
                if(x > Nfloat + 0.5f) x = Nfloat + 0.5f; 
                i0 = floorf(x); 
                i1 = i0 + 1.0f;
                if(y < 0.5f) y = 0.5f; 
                if(y > Nfloat + 0.5f) y = Nfloat + 0.5f; 
                j0 = floorf(y);
                j1 = j0 + 1.0f; 

                
                s1 = x - i0; 
                s0 = 1.0f - s1; 
                t1 = y - j0; 
                t0 = 1.0f - t1;
  
                
                int i0i = i0;
                int i1i = i1;
                int j0i = j0;
                int j1i = j1;

                
                d[IX(i, j, k)] = 
                
                    s0 * ( t0 * d0[IX(i0i, j0i)]
                        +  t1 * d0[IX(i0i, j1i)])
                   +s1 * ( t0 * d0[IX(i1i, j0i)]
                        +  t1 * d0[IX(i1i, j1i)]);
            }
        }
    }
    set_bnd(b, d, N);
}

static void project(float *velocX, float *velocY, float *p, float *div, int iter, int N)
{
        for (int j = 1; j < N - 1; j++) {
            for (int i = 1; i < N - 1; i++) {
                div[IX(i, j)] = -0.5f*(
                         velocX[IX(i+1, j)]
                        -velocX[IX(i-1, j)]
                        +velocY[IX(i  , j+1)]
                        -velocY[IX(i  , j-1)]
                    )/N;
                p[IX(i, j)] = 0;
            }
        }
    }
    set_bnd(0, div, N); 
    set_bnd(0, p, N);
    lin_solve(0, p, div, 1, 6, iter, N);
    
        for (int j = 1; j < N - 1; j++) {
            for (int i = 1; i < N - 1; i++) {
                velocX[IX(i, j)] -= 0.5f * (  p[IX(i+1, j)]
                                                -p[IX(i-1, j)]) * N;
                velocY[IX(i, j)] -= 0.5f * (  p[IX(i, j+1)]
                                                -p[IX(i, j-1)]) * N;

            }
        }
    }
    set_bnd(1, velocX, N);
    set_bnd(2, velocY, N);
}

void FluidCubeStep(FluidCube *cube)
{
    int N          = cube->size;
    float visc     = cube->visc;
    float diff     = cube->diff;
    float dt       = cube->dt;
    float *Vx      = cube->Vx;
    float *Vy      = cube->Vy;
    float *Vx0     = cube->Vx0;
    float *Vy0     = cube->Vy0;
    float *s       = cube->s;
    float *density = cube->density;
    
    diffuse(1, Vx0, Vx, visc, dt, 4, N);
    diffuse(2, Vy0, Vy, visc, dt, 4, N);
    
    project(Vx0, Vy0, Vx, Vy, 4, N);
    
    advect(1, Vx, Vx0, Vx0, Vy0, dt, N);
    advect(2, Vy, Vy0, Vx0, Vy0, dt, N);
    
    project(Vx, Vy, Vz, Vx0, Vy0, 4, N);
    
    diffuse(0, s, density, diff, dt, 4, N);
    advect(0, density, s, Vx, Vy, dt, N);
}

void FluidCubeAddDensity(FluidCube *cube, int x, int y, float amount)
{
    int N = cube->size;
    cube->density[IX(x, y)] += amount;
}

void FluidCubeAddVelocity(FluidCube *cube, int x, int y, float amountX, float amountY)
{
    int N = cube->size;
    int index = IX(x, y);
    
    cube->Vx[index] += amountX;
    cube->Vy[index] += amountY;
}

/*
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
*/