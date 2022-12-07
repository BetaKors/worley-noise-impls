using BetaKors.Imaging.Core;
using SixLabors.ImageSharp;
using System.Diagnostics;

IImageGenerator generator;

int IntInput(string msg)
{
    int input;

    do
    {
        Console.Write(msg);
    }
    while (!int.TryParse(Console.ReadLine()!, out input));

    return input;
}

string choice = string.Empty;

while (choice != "CPU" && choice != "GPU")
{
    Console.Write("Which do you wish to use [CPU/GPU]: ");
    choice = Console.ReadLine()!.ToUpper();
}

if (choice == "CPU")
{
    generator = new BetaKors.Imaging.CPU.WorleyNoise(
        IntInput("Width: "),
        IntInput("Height: "),
        IntInput("Number of points: ")
    );
}
else
{
    generator = new BetaKors.Imaging.GPU.WorleyNoise(
        IntInput("Width: "),
        IntInput("Height: "),
        IntInput("Number of points: ")
    );
}

Stopwatch watch = Stopwatch.StartNew();

Image img = generator.Generate();

watch.Stop();

if (!Directory.Exists("img")) Directory.CreateDirectory("img");

img.Save($"img/{generator.GetType().Name}-{choice}.bmp".ToLower());

string secondsElapsed = string.Format("{0:0.000}", watch.Elapsed.TotalSeconds).Replace(',', '.');

Console.WriteLine($"Done! ({secondsElapsed}s)");

#if !DEBUG
Console.ReadKey();
#endif
