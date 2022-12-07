use super::point::Point;
use image::GrayImage;
use image::Luma;
use rand::prelude::*;

pub struct WorleyNoiseImageGenerator {
    width: u32,
    height: u32,
    points: Vec<Point>,
}

impl WorleyNoiseImageGenerator {
    pub fn new(width: u32, height: u32, number_of_points: u32) -> Self {
        let mut rng = rand::thread_rng();

        let points = (0..=number_of_points)
            .map(|_| Point {
                x: rng.gen_range(0..=width) as f32,
                y: rng.gen_range(0..=height) as f32,
            })
            .collect();

        WorleyNoiseImageGenerator {
            width,
            height,
            points,
        }
    }

    pub fn generate(&self) -> GrayImage {
        GrayImage::from_fn(self.width, self.height, |x, y| {
            let pos = Point::new(x as f32, y as f32);

            let closest = &self
                .points
                .iter()
                .map(|p| p.squared_dist(pos))
                .min_by(|a, b| a.partial_cmp(b).unwrap())
                .unwrap();

            let color = 255 - (closest / 28.0).clamp(0.0, 255.0) as u8;

            Luma([color])
        })
    }
}
