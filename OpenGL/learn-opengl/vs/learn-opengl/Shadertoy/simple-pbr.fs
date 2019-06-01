#version 330 core

// in out
in vec2 fragCoord;
out vec4 fragColor;

// uniforms
uniform vec3 iResolution;
uniform float iTime;
uniform sampler2D iChannel0;

#define PI 						3.1415926536
#define SphereRows 				5
#define SphereCols 				5
#define NumSpheres 				(SphereRows * SphereCols)

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

struct LightVariables
{
    float LdotN;
    float VdotN;
    float HdotN;
    float HdotV;
    float HdotL;
    float roughness;
	vec4 f0;
};

void prepareScene();
float calculateSphereIntersection(in vec3 ro, in vec3 rd, in Sphere sph);
vec3 getNormal(in vec3 pos, in Sphere sph);

float randomFloat(vec3 co);
vec3 perpendicularVector(in vec3 nrm);
vec3 randomDirection(in vec3 base, in vec3 u, in vec3 v, in float r2, in vec3 seed);
vec3 sampleDiffuseEnvironment(in vec3 dir);
vec3 sampleReflectedEnvironment(in vec3 dir, in float r, in float f0);

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

vec3 F_Shlick(in vec3 f0, in vec3 fd90, float VoH)
{
    return f0 + (fd90 - f0) * pow(1.0 - VoH, 5.0);
}

float microfacetSpecular(in LightVariables lv)
{
    float r2 = lv.roughness * lv.roughness;
    
    float r4 = r2 * r2;
    
    float D = D_GGX(r4, lv.HdotN);
    
    float G = HEIGHT_CORRELATED_SMITH ? 
        G_SmithJointApprox(lv.VdotN, lv.LdotN, r2) : 
    	G_Smith(lv.VdotN, lv.LdotN, r4);
        
    return D * G * lv.LdotN / PI;
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
   
    LightVariables lv;
    lv.shadow = calculateShadow(pos + 0.001 * nrm);
    lv.LdotN = max(0.0, dot(nrm, light));
    lv.VdotN = max(0.0, dot(nrm, view));
    lv.HdotN = max(0.0, dot(nrm, h));
    lv.HdotV = max(0.0, dot(view, h));
    lv.HdotL = max(0.0, dot(light, h));
    lv.roughness = max(0.02, mtl.roughness * mtl.roughness);
	lv.f0 = specularColor;

    vec3 F = F_Shlick(vec3(lv.f0.w), vec3(1.0), lv.VdotN);
    
	vec3 diffuse = diffuseColor  * lambertDiffuse(lv);
	diffuse *= sampleDiffuseEnvironment(nrm;
    
    vec3 specular = specularColor * F * microfacetSpecular(lv);
    specular *= sampleReflectedEnvironment(vr, lv.roughness, lv.f0.w);
    
    return (diffuse * DIFFUSE_SCALE + specular * SPECULAR_SCALE) * 
	(lightColor) + ambientColor;
}










float calculateSphereIntersection(in vec3 ro, in vec3 rd, in Sphere sph)
{
    vec3 dv = sph.cen - ro;
	float b = dot(rd, dv);
	float d = b * b - dot(dv, dv) + sph.rad * sph.rad;
	return (d < 0.0) ? -1.0 : b - sqrt(d);
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
        
        materials[i].baseColor = vec3(1.0);
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
    
    vec3 F = F_Shlick(vec3(f0), vec3(1.0), VoN); 
                    
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