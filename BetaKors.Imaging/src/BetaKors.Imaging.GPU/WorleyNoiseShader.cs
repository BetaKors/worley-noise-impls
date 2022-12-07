using ComputeSharp;

namespace BetaKors.Imaging.GPU;

[AutoConstructor]
internal partial struct WorleyNoiseShader : IPixelShader<float4>
{
    public readonly ReadOnlyBuffer<int2> points;

    public float4 Execute()
    {
        float minDistanceSquared = float.MaxValue;

        for (int i = 0; i < points.Length; i++)
        {
            float distanceSquared = DistanceSquared(ThreadIds.XY, points[i]);

            if (distanceSquared < minDistanceSquared)
            {
                minDistanceSquared = distanceSquared;
            }
        }

        return (255 - Hlsl.Clamp(minDistanceSquared / 28, 0, 255)) / 255;
    }

    private static float DistanceSquared(int2 p1, int2 p2)
    {
        float x = p2.X - p1.X;
        float y = p2.Y - p1.Y;
        return x * x + y * y;
    }
}