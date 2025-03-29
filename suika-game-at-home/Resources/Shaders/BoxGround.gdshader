shader_type canvas_item;

uniform float top_width_ratio : hint_range(0.0, 1.0) = 0.5;  // Width ratio of the top (0-1)
uniform float bottom_width_ratio : hint_range(0.0, 1.0) = 1.0;  // Width ratio of the bottom (0-1)

void fragment() {
    float height = UV.y; // 0 at the top, 1 at the bottom

    // Calculate left and right edges of the trapezoid
    float left_edge = mix((1.0 - bottom_width_ratio) / 2.0, (1.0 - top_width_ratio) / 2.0, height);
    float right_edge = mix((1.0 + bottom_width_ratio) / 2.0, (1.0 + top_width_ratio) / 2.0, height);

    // Discard pixels outside the trapezoid
    if (UV.x < left_edge || UV.x > right_edge) {
        discard;
    }
}
