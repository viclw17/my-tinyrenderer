const float MATH_PI = float(3.14159);

float3 normal = normalDirection;
float3 viewDir = viewDirection;
float3 lightDir = lightDirection;

float3 halfVec = normalize(viewDir + lightDir);
float ndoth = saturate(dot(normal, halfVec));
float ndotv = saturate(dot(normal, viewDir));
float ndotl = saturate(dot(normal, lightDir));

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

vec3 F_Shlick(in vec3 f0, in vec3 fd90, float VoH)
{
    return f0 + (fd90 - f0) * pow(1.0 - VoH, 5.0);
}

float microfacetSpecular()
{
    float r2 = roughness * roughness;
    float r4 = r2 * r2;
    float D = D_GGX(r4, ndoth);
    float G = G_Smith(ndotv, ndotl, r4);
        
    return D * G * ndotl / PI;
}

float lambertDiffuse()
{
    return ndotl / PI;
}

ec3 shadeFragment()
{
    float m2 = metallness * metallness;
    vec3 reflectance = vec3(0.04);
    
    vec3 diffuseColor = baseColor * (1.0 - m2);
    vec3 specularColor = mix(reflectance, baseColor, metallness);
    float maxF0 = max(specularColor.x, max(specularColor.y, specularColor.z));

    roughness = max(0.02, roughness * roughness);
    f0 = vec4(specularColor, maxF0);
        
    vec3 F = F_Shlick(vec3(lv.f0.w), vec3(1.0), lv.VdotN);
            
    vec3 diffuse = diffuseColor * lambertDiffuse();
    
    vec3 specular = F * specularColor * microfacetSpecular();
    
    return (diffuse + specular ) * lightColor  + ambientColor;
}