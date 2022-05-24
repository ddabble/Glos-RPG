//UNITY_SHADER_NO_UPGRADE
#ifndef SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED
#define SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED

// Code based on the 2019 paper by Thomas Deliot and Eric Heitz: https://drive.google.com/file/d/1QecekuuyWgw68HU9tg6ENfrCTCVIjm6l/view
// (from https://blog.unity.com/technology/procedural-stochastic-texturing-in-unity)

// Compute local triangle barycentric coordinates and vertex IDs
void TriangleGrid(float2 uv,
                  out float3 weights, out int3x2 vertices)
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
        weights = float3(
            temp.z,
            temp.y,
            temp.x
        );
        vertices = int3x2(
            baseId,
            baseId + int2(0, 1),
            baseId + int2(1, 0)
        );
    }
    else
    {
        weights = float3(
            -temp.z,
            1.0 - temp.y,
            1.0 - temp.x
        );
        vertices = int3x2(
            baseId + int2(1, 1),
            baseId + int2(1, 0),
            baseId + int2(0, 1)
        );
    }
}

float2 hash(float2 p)
{
    return frac(sin(mul(float2x2(127.1, 311.7, 269.5, 183.3), p)) * 43758.5453);
}

float4 ProceduralTilingAndBlending(UnityTexture2D Texture, float2 UV, UnitySamplerState Sampler)
{
    // Get triangle info
    float3 weights;
    int3x2 vertices;
    TriangleGrid(UV, weights, vertices);
    // Assign random offset to each triangle vertex
    const float3x2 uvs = float3x2(
        UV + hash(vertices[0]),
        UV + hash(vertices[1]),
        UV + hash(vertices[2])
    );
    // Precompute UV derivatives
    const float2 dUVdx = ddx(UV);
    const float2 dUVdy = ddy(UV);
    // Fetch inputs
    const float3x4 inputs = float3x4(
        SAMPLE_TEXTURE2D_GRAD(Texture, Sampler, uvs[0], dUVdx, dUVdy),
        SAMPLE_TEXTURE2D_GRAD(Texture, Sampler, uvs[1], dUVdx, dUVdy),
        SAMPLE_TEXTURE2D_GRAD(Texture, Sampler, uvs[2], dUVdx, dUVdy)
    );
    // Linear blending
    // (weights[0] * input[0] + weights[1] * input[1] + weights[2] * input[2])
    float4 color = mul(transpose(inputs), weights);
    return color;
}

void SampleStochasticTexture2D_float(UnityTexture2D Texture, bool IsNormalTexture, float2 UV, UnitySamplerState Sampler,
                                     out float4 RGBA)
{
    float4 fragColor = ProceduralTilingAndBlending(Texture, UV, Sampler);
    RGBA = IsNormalTexture ? float4(UnpackNormal(fragColor), 1.0) : fragColor;
}

#endif //SAMPLE_STOCHASTIC_TEXTURE_2D_INCLUDED
