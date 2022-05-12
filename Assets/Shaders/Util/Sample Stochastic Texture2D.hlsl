//UNITY_SHADER_NO_UPGRADE
#ifndef SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED
#define SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED

// Code based on https://www.shadertoy.com/view/Xtl3zf (from https://iquilezles.org/articles/texturerepetition/ under Technique 3)

float sum(float3 v)
{
    return v.x + v.y + v.z;
}

void SampleStochasticTexture2D_float(UnityTexture2D Texture, float2 UV, UnitySamplerState Sampler, float Noise, out float4 RGBA)
{
    float l = Noise * 8.0;
    float f = frac(l);

    float ia = floor(l + 0.5);
    float ib = floor(l);
    f = min(f, 1.0 - f) * 2.0;

    float2 offa = sin(float2(3.0, 7.0) * ia); // can replace with any other hash
    float2 offb = sin(float2(3.0, 7.0) * ib); // can replace with any other hash

    float2 dx = ddx(UV);
    float2 dy = ddy(UV);

    float3 cola = Texture.SampleGrad(Sampler, UV + offa, dx, dy).xyz;
    float3 colb = Texture.SampleGrad(Sampler, UV + offb, dx, dy).xyz;

    RGBA = float4(lerp(cola, colb, smoothstep(0.2, 0.8, f - 0.1 * sum(cola - colb))), 1.0);
}

#endif //SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED
