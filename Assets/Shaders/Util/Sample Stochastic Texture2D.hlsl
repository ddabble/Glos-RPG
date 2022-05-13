//UNITY_SHADER_NO_UPGRADE
#ifndef SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED
#define SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED

// Code based on https://www.shadertoy.com/view/WdVGWG

struct InterpNodes2
{
    float2 seeds;
    float2 weights;
};

InterpNodes2 GetNoiseInterpNodes(float smoothNoise)
{
    const float2 globalPhases = smoothNoise * 0.5 + float2(0.5, 0.0);
    const float2 phases = frac(globalPhases);
    const float2 seeds = floor(globalPhases) * 2.0 + float2(0.0, 1.0);
    const float2 weights = min(phases, 1.0 - phases) * 2.0;

    const InterpNodes2 nodes = {seeds, weights};
    return nodes;
}

float3 hash33(float3 p)
{
    p = float3(
        dot(p, float3(127.1, 311.7, 74.7)),
        dot(p, float3(269.5, 183.3, 246.1)),
        dot(p, float3(113.5, 271.9, 124.6))
    );
    return frac(sin(p) * 43758.5453123);
}

float4 GetTextureSample(UnityTexture2D Texture, bool IsNormalTexture, UnitySamplerState Sampler, float2 UV, float seed)
{
    const float3 hash = hash33(float3(seed, 0.0, 0.0));
    const float ang = hash.x * 2.0 * PI;
    const float2x2 rotation = float2x2(cos(ang), sin(ang), -sin(ang), cos(ang));

    const float4 sample = Texture.Sample(Sampler, mul(rotation, UV) + hash.yz);
    return IsNormalTexture ? float4(UnpackNormal(sample), 1.0) : sample;
}

//Qizhi Yu, Fabrice Neyret, Eric Bruneton, and Nicolas Holzschuch. 2011. 
//Lagrangian Texture Advection: Preserving Both Spectrum and Velocity Field.
//IEEE Transactions on Visualization and Computer Graphics 17, 11 (2011), 1612–1623
float4 PreserveVariance(float4 linearColor, float4 meanColor, float moment2)
{
    return (linearColor - meanColor) / sqrt(moment2) + meanColor;
}

void SampleStochasticTexture2D_float(UnityTexture2D Texture, bool IsNormalTexture, float2 UV, UnitySamplerState Sampler, float LayersCount, float Noise,
                                     out float4 RGBA)
{
    float4 fragColor = 0.0;
    const InterpNodes2 interpNodes = GetNoiseInterpNodes(Noise * LayersCount);
    float moment2 = 0.0;
    for (int i = 0; i < 2; i++)
    {
        float weight = interpNodes.weights[i];
        moment2 += weight * weight;
        fragColor += GetTextureSample(Texture, IsNormalTexture, Sampler, UV, interpNodes.seeds[i]) * weight;
    }

    const float4 meanColor = Texture.SampleLevel(Sampler, 0.0, 10.0);
    fragColor = PreserveVariance(fragColor, meanColor, moment2);
    RGBA = fragColor;
}

#endif //SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED
