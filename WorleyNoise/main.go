package main

import (
	"fmt"
	"image"
	"image/color"
	"image/png"
	"math"
	"math/rand"
	"os"
	"path/filepath"
	"time"
)

const PATH = "img/image.png"

type WorleyNoiseGenerator struct {
	width, height, numberOfPoints int
}

type Point = image.Point

func main() {
	generator := WorleyNoiseGenerator{
		input("Width: "),
		input("Height: "),
		input("Number of points: "),
	}

	t := time.Now()

	img := generator.generate()

	elapsed := time.Since(t)

	dirs := filepath.Dir(PATH)

	if dirs != "." && dirs != "/" {
		os.MkdirAll(dirs, 0777)
	}

	file, err := os.Create(PATH)

	if err != nil {
		fmt.Printf("Something went wrong when creating a file at path %v\n", PATH)
		return
	}

	defer file.Close()

	png.Encode(file, img)

	fmt.Printf("Done! (%.3fs)", elapsed.Seconds())
}

func (gen WorleyNoiseGenerator) generate() *image.RGBA {
	img := newRGBAImage(gen.width, gen.height)

	points := gen.generatePoints()

	for x := 0; x < gen.width; x++ {
		for y := 0; y < gen.height; y++ {
			point := Point{x, y}

			distance := squaredDistanceToClosestPoint(point, points)

			greyscale := 255 - uint8(clamp(distance/28, 0, 255))

			color := color.RGBA{greyscale, greyscale, greyscale, 255}

			img.Set(x, y, color)
		}
	}

	return img
}

func (gen WorleyNoiseGenerator) generatePoints() []Point {
	points := make([]Point, gen.numberOfPoints)

	for i := 0; i < gen.numberOfPoints; i++ {
		fmt.Println(i)
		x := randomRange(0, gen.width)
		y := randomRange(0, gen.height)
		points = append(points, Point{x, y})
	}

	return points
}

func newRGBAImage(width, height int) *image.RGBA {
	upperLeft := Point{0, 0}
	bottomRight := Point{width, height}
	imgRect := image.Rectangle{upperLeft, bottomRight}

	return image.NewRGBA(imgRect)
}

func input(msg string) int {
	var n int
	fmt.Print(msg)
	fmt.Scan(&n)
	return n
}

func squaredDistanceToClosestPoint(current Point, points []Point) int {
	min := math.MaxInt

	for _, point := range points {
		distance := squaredDistance(current, point)

		if distance < min {
			min = distance
		}
	}

	return min
}

func squaredDistance(p1, p2 Point) int {
	a := p1.X - p2.X
	b := p1.Y - p2.Y

	return a*a + b*b
}

func clamp(v, min, max int) int {
	if v < min {
		return min
	}

	if v > max {
		return max
	}

	return v
}

func randomRange(min, max int) int {
	return rand.Intn(max-min) + min
}
