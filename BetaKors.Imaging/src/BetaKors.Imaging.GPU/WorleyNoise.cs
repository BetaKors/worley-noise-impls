using ImageSharpRgba32 = SixLabors.ImageSharp.PixelFormats.Rgba32;
using SixLabors.ImageSharp;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BetaKors.Imaging.Core;
using ComputeSharp;

namespace BetaKors.Imaging.GPU;

public sealed class WorleyNoise : IImageGenerator
{
    public int Width { get; }
    public int Height { get; }
    public int NumberOfPoints { get; }
    public bool Seamless { get; }

    public WorleyNoise(int width, int height, int numberOfPoints, bool seamless=false)
    {
        Width = width;
        Height = height;
        NumberOfPoints = numberOfPoints;
        Seamless = seamless;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public Image<ImageSharpRgba32> Generate()
    {
        using var texture = GraphicsDevice.Default.AllocateReadWriteTexture2D<Rgba32, float4>(Width, Height);
        using var points = GraphicsDevice.Default.AllocateReadOnlyBuffer<int2>(GeneratePoints());

        GraphicsDevice.Default.ForEach(texture, new WorleyNoiseShader(points));

        Image<ImageSharpRgba32> image = new(Width, Height);

        image.TryGetSinglePixelSpan(out var span);

        texture.CopyTo(MemoryMarshal.Cast<ImageSharpRgba32, Rgba32>(span));

        return image;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private int2[] GeneratePoints()
    {
        int2[] points = new int2[NumberOfPoints];
        Random random = new Random();

        for (int i = 0; i < NumberOfPoints; i++)
        {
            points[i] = new int2
            {
                X = random.Next(Width),
                Y = random.Next(Height)
            };
        }

        if (Seamless)
        {
            List<int2> pointsList = new(points);
            List<int2> allPoints = new();
            
            pointsList.ForEach(p => {
                int2 l = new int2
                {
                    X = p.X - Width,
                    Y = p.Y
                };
                int2 r = new int2
                {
                    X = p.X + Width,
                    Y = p.Y
                };
                int2 t = new int2
                {
                    X = p.X,
                    Y = p.Y - Height
                };
                int2 b = new int2
                {
                    X = p.X,
                    Y = p.Y + Height
                };
                int2 bl = new int2
                {
                    X = p.X - Width,
                    Y = p.Y + Height
                };
                int2 br = new int2
                {
                    X = p.X + Width,
                    Y = p.Y + Height
                };
                int2 tl = new int2
                {
                    X = p.X - Width,
                    Y = p.Y - Height
                };
                int2 tr = new int2
                {
                    X = p.X + Width,
                    Y = p.Y - Height
                };

                allPoints.Add(p);
                allPoints.Add(l);
                allPoints.Add(r);
                allPoints.Add(t);
                allPoints.Add(b);
                allPoints.Add(bl);
                allPoints.Add(br);
                allPoints.Add(tl);
                allPoints.Add(tr);
            });

            return allPoints.ToArray();
        }

        return points;
    }
}
