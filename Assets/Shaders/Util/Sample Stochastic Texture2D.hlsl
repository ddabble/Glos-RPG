//UNITY_SHADER_NO_UPGRADE
#ifndef SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED
#define SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED

// Code based on the 2019 paper by Thomas Deliot and Eric Heitz: https://drive.google.com/file/d/1QecekuuyWgw68HU9tg6ENfrCTCVIjm6l/view
// (from https://blog.unity.com/technology/procedural-stochastic-texturing-in-unity)

// Compute local triangle barycentric coordinates and vertex IDs
void TriangleGrid(float2 uv,
                  out float w1, out float w2, out float w3,
                  out int2 vertex1, out int2 vertex2, out int2 vertex3)
{
    // Scaling of the input
    uv *= 3.46410162; // 2 * sqrt(3)
    // Skew input space into simplex triangle grid
    const float2x2 gridToSkewedGrid = float2x2(1.0, 0.0, -0.57735027, 1.15470054);
    const float2 skewedCoord = mul(gridToSkewedGrid, uv);
    // Compute local triangle vertex IDs and local barycentric coordinates
    const int2 baseId = int2(floor(skewedCoord));
    float3 temp = float3(frac(skewedCoord), 0);
    temp.z = 1.0 - temp.x - temp.y;
    if (temp.z > 0.0)
    {
        w1 = temp.z;
        w2 = temp.y;
        w3 = temp.x;
        vertex1 = baseId;
        vertex2 = baseId + int2(0, 1);
        vertex3 = baseId + int2(1, 0);
    }
    else
    {
        w1 = - temp.z;
        w2 = 1.0 - temp.y;
        w3 = 1.0 - temp.x;
        vertex1 = baseId + int2(1, 1);
        vertex2 = baseId + int2(1, 0);
        vertex3 = baseId + int2(0, 1);
    }
}

float2 hash(float2 p)
{
    return frac(sin(mul(float2x2(127.1, 311.7, 269.5, 183.3), p)) * 43758.5453);
}

float4 ProceduralTilingAndBlending(UnityTexture2D Texture, float2 UV, UnitySamplerState Sampler)
{
    // Get triangle info
    float w1, w2, w3;
    int2 vertex1, vertex2, vertex3;
    TriangleGrid(UV, w1, w2, w3, vertex1, vertex2, vertex3);
    // Assign random offset to each triangle vertex
    const float2 uv1 = UV + hash(vertex1);
    const float2 uv2 = UV + hash(vertex2);
    const float2 uv3 = UV + hash(vertex3);
    // Precompute UV derivatives
    const float2 dUVdx = ddx(UV);
    const float2 dUVdy = ddy(UV);
    // Fetch inputs
    const float4 I1 = Texture.SampleGrad(Sampler, uv1, dUVdx, dUVdy);
    const float4 I2 = Texture.SampleGrad(Sampler, uv2, dUVdx, dUVdy);
    const float4 I3 = Texture.SampleGrad(Sampler, uv3, dUVdx, dUVdy);
    // Linear blending
    float4 color = w1 * I1 + w2 * I2 + w3 * I3;
    return color;
}

void SampleStochasticTexture2D_float(UnityTexture2D Texture, bool IsNormalTexture, float2 UV, UnitySamplerState Sampler,
                                     out float4 RGBA)
{
    float4 fragColor = ProceduralTilingAndBlending(Texture, UV, Sampler);
    RGBA = IsNormalTexture ? float4(UnpackNormal(fragColor), 1.0) : fragColor;
}

#endif //SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED
