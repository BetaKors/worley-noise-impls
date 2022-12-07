using Rgba32 = SixLabors.ImageSharp.PixelFormats.Rgba32;
using SixLabors.ImageSharp;
using System.Runtime.CompilerServices;
using BetaKors.Imaging.Core;

namespace BetaKors.Imaging.CPU;

public sealed class WorleyNoise : IImageGenerator
{
    public int Width { get; }
    public int Height { get; }
    public int NumberOfPoints { get; }

    public WorleyNoise(int width, int height, int numberOfPoints)
    {
        Width = width;
        Height = height;
        NumberOfPoints = numberOfPoints;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public Image<Rgba32> Generate()
    {
        Image<Rgba32> img = new(Width, Height);
        Point[] points = GeneratePoints();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                img[x, y] = CalculateColorAt(x, y, points);
            }
        }

        return img;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static Rgba32 CalculateColorAt(int x, int y, Point[] points)
    {
        float minDistanceSquared = float.MaxValue;

        for (int i = 0; i < points.Length; i++)
        {
            float distanceSquared = DistanceSquared(x, y, points[i]);

            if (distanceSquared < minDistanceSquared)
            {
                minDistanceSquared = distanceSquared;
            }
        }

        byte color = (byte) (255 - Math.Clamp((int) minDistanceSquared / 28, 0, 255));

        return new Rgba32(color, color, color, 255);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private Point[] GeneratePoints()
    {
        Point[] points = new Point[NumberOfPoints];
        Random random = new Random();

        for (int i = 0; i < NumberOfPoints; i++)
        {
            points[i] = new Point
            {
                X = random.Next(Width),
                Y = random.Next(Height)
            };
        }

        return points;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float DistanceSquared(float x, float y, Point p2)
    {
        float x1 = p2.X - x;
        float y1 = p2.Y - y;
        return x1 * x1 + y1 * y1;
    }
}
