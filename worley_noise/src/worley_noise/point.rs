#[derive(Clone, Copy)]
pub struct Point {
    pub x: f32,
    pub y: f32,
}

impl Point {
    pub fn new(x: f32, y: f32) -> Self {
        Point { x, y }
    }

    pub fn squared_dist(&self, other: Point) -> f32 {
        let a = self.x - other.x;
        let b = self.y - other.y;

        a * a + b * b
    }
}
