from random import randrange
from PIL import Image  # type: ignore


def generate(width, height, number_of_points) -> Image.Image:
    img = Image.new('L', (width, height))

    points = tuple(
        (randrange(0, width), randrange(0, height))
        for _ in range(number_of_points)
    )

    gen = (
        255 - min(distance_squared(x, y, p) for p in points) // 28
        for y in range(height) for x in range(width)
    )

    img.putdata(list(gen))

    return img


def distance_squared(x, y, p):
    x1 = x - p[0]
    y1 = y - p[1]
    return x1 * x1 + y1 * y1


if __name__ == '__main__':
    from time import perf_counter

    int_input = lambda msg: int(input(msg))

    width            = int_input('Width: ')
    height           = int_input('Height: ')
    number_of_points = int_input('Number of points: ')

    timer1 = perf_counter()

    img = generate(width, height, number_of_points)

    timer2 = perf_counter()

    print(f'Done! ({timer2 - timer1:.3f}s)')

    img.save('./worley-noise.bmp', optimize=True)
