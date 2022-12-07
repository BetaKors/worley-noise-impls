mod worley_noise;

use std::io::stdout;
use std::io::Write;
use std::num::NonZeroU32;
use text_io::read;
use worley_noise::worley_noise::WorleyNoiseImageGenerator;

fn main() {
    const PATH: &str = "img/image.png";

    let generator = WorleyNoiseImageGenerator::new(
        input("Width: "),
        input("Height: "),
        input("Number of points: "),
    );

    let instant = std::time::Instant::now();

    let img = generator.generate();

    let elapsed = instant.elapsed();

    if let Some(dirs) = std::path::Path::new(PATH).parent() {
        std::fs::create_dir_all(dirs).expect("Couldn't create directories.");
    }

    img.save(PATH)
        .expect(&format!("Unable to save image at path {}", PATH));

    println!("Done! ({:.3}s)", elapsed.as_secs_f32());
}

fn print_flush(msg: &str) {
    print!("{}", msg);
    stdout().lock().flush().expect("Couldn't flush stdout!");
}

fn input(msg: &str) -> u32 {
    print_flush(msg);

    let v: NonZeroU32 = read!();
    u32::from(v)
}
