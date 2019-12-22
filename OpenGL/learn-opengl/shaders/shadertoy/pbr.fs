#version 330 core

// in out
in vec2 fragCoord;
out vec4 fragColor;

// uniforms
uniform vec3 iResolution;
uniform float iTime;
//uniform sampler2D iChannel0;

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

#define SphereRows 				5
#define SphereCols 				5
#define NumSpheres 				(SphereRows * SphereCols)

#define PI 						3.1415926536
#define MAX_CUBEMAP_SAMPLES 	100

#define USE_ANALYTICAL			1
#define USE_IBL 				0
#define COLORIZE 				0

#define HEIGHT_CORRELATED_SMITH false
#define FRESNEL_SHLICK			true

#define DIFFUSE_SCALE 			1.0
#define SPECULAR_SCALE 			1.0

const vec4 lightPosition = vec4(0.0, 40.0, 0.0, 0.0);
const vec3 lightColor = vec3(1.0);
const vec3 ambientColor = vec3(0.005);

const float spheresRadius = 2.5;
const float gap = 0.2;

struct Sphere
{
    vec3 cen;
    float rad;
} spheres[NumSpheres];

struct Material
{
    vec3 baseColor;
    float roughness;
    float metallness;
} materials[NumSpheres];

Material planeMaterial;

struct LightVariables
{
    float LdotN;
    float VdotN;
    float HdotN;
    float HdotV;
    float HdotL;
    float shadow;
    float roughness;
    vec4 f0;
};

void prepareScene();
float calculateSphereIntersection(in vec3 ro, in vec3 rd, in Sphere sph);
float calculatePlaneIntersection(in vec3 ro, in vec3 rd);
vec3 getNormal(in vec3 pos, in Sphere sph);

float randomFloat(vec3 co);
vec3 perpendicularVector(in vec3 nrm);
vec3 randomDirection(in vec3 base, in vec3 u, in vec3 v, in float r2, in vec3 seed);
vec3 sampleDiffuseEnvironment(in vec3 dir);
vec3 sampleReflectedEnvironment(in vec3 dir, in float r, in float f0);

#define SHADOW_SAMPLES 50

float calculateShadow(in vec3 pos)
{
    vec3 n = normalize(lightPosition.xyz - pos * lightPosition.w);
    vec3 u = perpendicularVector(n);
	vec3 v = cross(u, n);
    vec3 dir = n;
    
    float result = 0.0;
    for (int s = 0; s < SHADOW_SAMPLES; ++s)
    {
        dir = randomDirection(n, u, v, 0.02, dir + pos + result);
	    for (int i = 0; i < NumSpheres; ++i)
        {
    	    result += float(calculateSphereIntersection(pos, dir, spheres[i]) > 0.0);
        }
    }
    
    return 1.0 - result / float(SHADOW_SAMPLES);
}    

    
float D_GGX(float r4, float NoH)
{
    float d = NoH * NoH * (r4 - 1.0) + 1.0;
    return r4 / (d * d);
}

float G_Smith(in float NoV, in float NoL, in float r4)
{
	float Vis_SmithV = NoV + sqrt(NoV * (NoV - NoV * r4) + r4);
    float Vis_SmithL = NoL + sqrt(NoL * (NoL - NoL * r4) + r4);
    return 1.0 / (Vis_SmithV * Vis_SmithL);
}

float G_SmithJointApprox(float NoV, float NoL, in float r2)
{
	float Vis_SmithV = NoL * (NoV * (1.0 - r2) + r2);
    float Vis_SmithL = NoV * (NoL * (1.0 - r2) + r2);
	return 0.5 / (Vis_SmithV + Vis_SmithL);
}

vec3 F_Fresnel(vec3 clr, float VoH)
{
	vec3 clrSqrt = sqrt(clamp(vec3(0.0, 0.0, 0.0), vec3(0.99, 0.99, 0.99), clr));
	vec3 n = (1.0 + clrSqrt) / (1.0 - clrSqrt);
	vec3 g = sqrt(n * n + VoH * VoH - 1.0);
    vec3 s = (g - VoH) / (g + VoH);
    vec3 p = ((g + VoH) * VoH - 1.0) / ((g - VoH) * VoH + 1.0);
	return 0.5 * s * s * (1.0 + p * p);
}

vec3 F_Shlick(in vec3 f0, in vec3 fd90, float VoH)
{
    return f0 + (fd90 - f0) * pow(1.0 - VoH, 5.0);
}

float microfacetSpecular(in LightVariables lv)
{
#if (USE_ANALYTICAL)
    float r2 = lv.roughness * lv.roughness;
    
    float r4 = r2 * r2;
    
    float D = D_GGX(r4, lv.HdotN);
    
    float G = HEIGHT_CORRELATED_SMITH ? 
        G_SmithJointApprox(lv.VdotN, lv.LdotN, r2) : 
    	G_Smith(lv.VdotN, lv.LdotN, r4);
        
    return D * G * lv.LdotN / PI;
#else
    return 0.0;
#endif
}


float lambertDiffuse(in LightVariables lv)
{
    return lv.LdotN / PI;
}

vec3 shadeFragment(in vec3 pos, in vec3 nrm, in vec3 view, in Material mtl)
{
    vec3 light = normalize(lightPosition.xyz - pos * lightPosition.w);
    vec3 h = normalize(view + light);
    vec3 vr = reflect(-view, nrm);
        
    float m2 = mtl.metallness * mtl.metallness;
    vec3 reflectance = vec3(0.04);
    
    vec3 diffuseColor = mtl.baseColor * (1.0 - m2);
    vec3 specularColor = mix(reflectance, mtl.baseColor, mtl.metallness);
    float maxF0 = max(specularColor.x, max(specularColor.y, specularColor.z));
        
    LightVariables lv;
    lv.shadow = calculateShadow(pos + 0.001 * nrm);
    lv.LdotN = max(0.0, dot(nrm, light));
    lv.VdotN = max(0.0, dot(nrm, view));
    lv.HdotN = max(0.0, dot(nrm, h));
    lv.HdotV = max(0.0, dot(view, h));
    lv.HdotL = max(0.0, dot(light, h));
    lv.roughness = max(0.02, mtl.roughness * mtl.roughness);
    lv.f0 = vec4(specularColor, maxF0);
        
    vec3 F = FRESNEL_SHLICK ? 
        F_Shlick(vec3(lv.f0.w), vec3(1.0), lv.VdotN) :
        F_Fresnel(vec3(lv.f0.w), lv.VdotN);
            
    vec3 diffuse = diffuseColor * 
        sampleDiffuseEnvironment(nrm) * lambertDiffuse(lv);
    
    vec3 specular = F * specularColor *
        (sampleReflectedEnvironment(vr, lv.roughness, lv.f0.w) + microfacetSpecular(lv));
    
    return (diffuse * DIFFUSE_SCALE + specular * SPECULAR_SCALE) * 
        (lightColor * lv.shadow) + ambientColor;
}

/*
 * Main image
 */
void main()
{
    prepareScene();
    
    float rotSpeed = 0.25;
    float rotOffset = 3.0 * PI / 4.0 - PI / 6.0;
	vec2 p = (2.0 * fragCoord.xy - iResolution.xy) / iResolution.y;
    
	//vec3 viewPoint = vec3(66.0 * cos(rotOffset + rotSpeed * iTime), 
    //                     35.0 + 25.0 * cos(rotOffset + 0.5 * rotSpeed * iTime), 
    //                      66.0 * sin(rotOffset + rotSpeed * iTime));
    
	vec3 viewPoint = vec3(0,60,60);
	vec3 viewCenter = vec3(0.0, 0.0, 0.0);
    vec3 up = vec3(0.0, 1.0, 0.0);
    vec3 ww = normalize(viewCenter - viewPoint);
    vec3 uu = normalize(cross(ww, up));
    vec3 vv = normalize(cross(uu, ww));
	vec3 rd = normalize(p.x * uu + p.y * vv + 6.0 * ww);    
    
    float tmin = 1.0e+10;
    float t1 = calculatePlaneIntersection(viewPoint, rd);
    if (t1 > 0.0)
        tmin = t1;

    Material material;
    int sphereIndex = -1;
    vec3 nrm = vec3(0.0, 1.0, 0.0);
    for (int i = 0; i < NumSpheres; ++i)
    {
        float t2 = calculateSphereIntersection(viewPoint, rd, spheres[i]);
        if ((t2 > 0.0) && (t2 < tmin))
        {
            tmin = t2;
            nrm = getNormal(viewPoint + t2 * rd, spheres[i]);
            material = materials[i];
            sphereIndex = i;
        }
    }
    
    if (tmin > 10000.0)
    {
        fragColor = vec4(0.0);
        return;
    }

    vec3 pos = viewPoint + tmin * rd;
    vec3 linearColor = vec3(0.0);
    if (sphereIndex == -1)
    {
        float fade = exp(-0.025 * length(viewPoint - pos));
        linearColor = fade * shadeFragment(pos, nrm, normalize(viewPoint - pos), planeMaterial);
    }
    else 
    {
    	linearColor = shadeFragment(pos, nrm, normalize(viewPoint - pos), material);
    }
    
    fragColor = vec4(pow(linearColor, vec3(1.0 / 2.2)), 1.0);
}

/*
 * Service functions
 */
float calculateSphereIntersection(in vec3 ro, in vec3 rd, in Sphere sph)
{
    vec3 dv = sph.cen - ro;
	float b = dot(rd, dv);
	float d = b * b - dot(dv, dv) + sph.rad * sph.rad;
	return (d < 0.0) ? -1.0 : b - sqrt(d);
}

float calculatePlaneIntersection(in vec3 ro, in vec3 rd)
{
    return (-1.0 - ro.y) / rd.y;
}

vec3 getNormal(in vec3 pos, in Sphere sph)
{
    return normalize((pos - sph.cen) / sph.rad);
}

void prepareScene()
{
    int col = 0;
    int row = 0;
    float xStart = float(1 - SphereCols) * spheresRadius - float(SphereCols - 1) * gap;
    float xPos = xStart;
    float zPos = float(1 - SphereRows) * spheresRadius - float(SphereRows - 1) * gap;
    
    for (int i = 0; i < NumSpheres; ++i)
    {
    	spheres[i] = Sphere(vec3(xPos, spheresRadius, zPos), spheresRadius);
        xPos += 2.0 * (spheresRadius + gap);
        
        float tc = float(col) / float(SphereCols - 1);
        float tr = float(row) / float(SphereRows - 1);
        
    #if (COLORIZE)
        float r = 0.5 + 0.5 * cos(0.5 * PI * tc);
        float g = 0.5 + 0.5 * sin(0.5 * PI * tr);
        float b = sqrt(1.0 - (r * r * g * g));
        materials[i].baseColor = pow(vec3(r, g, b), vec3(2.2));
    #else
        materials[i].baseColor = vec3(1.0);
    #endif
        
        materials[i].roughness = 0.05 + 0.9 * tc;
        materials[i].metallness = tr;
        
        ++col;
        if (col == SphereCols)
        {
        	zPos += 2.0 * (spheresRadius + gap);
            xPos = xStart;
            col = 0;
            row++;
        }
	}
    
    planeMaterial.baseColor = vec3(1.0, 1.0, 1.0);
    planeMaterial.roughness = 0.33333;
    planeMaterial.metallness = 0.75;
}

float randomFloat(vec3 co)
{
    return fract(sin(dot(co, vec3(12.9898, 78.233, 59.5789))) * 43758.5453);
}

vec3 randomDirection(in vec3 base, in vec3 u, in vec3 v, in float r2, in vec3 seed)
{
	float phi = 2.0 * PI * randomFloat(seed);
	float Xi = randomFloat(seed * phi + phi);
    
	float cosTheta = sqrt((1.0 - Xi) / ((r2 - 1.0) * Xi + 1.0));
	float sinTheta = sqrt(1.0 - cosTheta * cosTheta);
    
	return normalize((u * cos(phi) + v * sin(phi)) * sinTheta + base * cosTheta);
}

vec3 perpendicularVector(in vec3 nrm)
{
	vec3 componentsLength = nrm * nrm;
    
	if (componentsLength.x > 0.5)
	{
		float scaleFactor = sqrt(componentsLength.z + componentsLength.x);
		return nrm.zyx * vec3(1.0 / scaleFactor, 0.0, -1.0 / scaleFactor);
	}
	else if (componentsLength.y > 0.5)
	{
		float scaleFactor = sqrt(componentsLength.y + componentsLength.x);
		return nrm.yxz * vec3(-1.0 / scaleFactor, 1.0 / scaleFactor, 0.0);
	}
	
    float scaleFactor = sqrt(componentsLength.z + componentsLength.y);
	return nrm.xzy * vec3(0.0, -1.0 / scaleFactor, 1.0 / scaleFactor);
}

float microfacetWeight(in float r2, in float f0, in float HoN, in float VoN, in float LoN)
{
    float r4 = r2 * r2;
    
    float D = D_GGX(r4, HoN);
    
    float G = HEIGHT_CORRELATED_SMITH ? 
        G_SmithJointApprox(VoN, LoN, r2) : 
    	G_Smith(VoN, LoN, r4);
    
    vec3 F = FRESNEL_SHLICK ? 
        F_Shlick(vec3(f0), vec3(1.0), VoN) :
        F_Fresnel(vec3(f0), VoN); 
                    
    return (F.x * D * G) * (LoN / PI);
}

vec3 sampleReflectedEnvironment(in vec3 Wi, in float r, in float f0)
{
#if (USE_IBL)    
    float r2 = r * r;
    float samples = 0.0;
    vec3 u = perpendicularVector(Wi);
	vec3 v = cross(u, Wi);
    vec3 result = vec3(0.0);
    vec3 Wo = Wi;
    for (int i = 0; i < MAX_CUBEMAP_SAMPLES; ++i)
    {
        Wo = randomDirection(Wi, u, v, r2, Wo + result);
        vec3 H = normalize(Wi + Wo);
        float weight = microfacetWeight(r2, f0, dot(H, Wi), 1.0, dot(Wo, Wi));
        result += weight * texture(iChannel0, -Wo).xyz;
        samples += weight;
	}    
    return result / samples;
	// */
#else
    return vec3(0.0);
#endif
}

vec3 sampleDiffuseEnvironment(in vec3 Wi)
{
#if (USE_IBL)
    float samples = 0.0;
    
    vec3 u = perpendicularVector(Wi);
	vec3 v = cross(u, Wi);
    vec3 result = vec3(0.0);
    vec3 Wo = Wi;
    for (int i = 0; i < MAX_CUBEMAP_SAMPLES; ++i)
    {
        Wo = randomDirection(Wi, u, v, 1.0, Wo + result);
        float weight = dot(Wi, Wo);
        result += weight * texture(iChannel0, -Wo).xyz;
        samples += weight;
	}    
    return result / samples;
#else
    return vec3(1.0);
#endif
}
