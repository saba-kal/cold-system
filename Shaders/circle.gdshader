shader_type spatial;

uniform float scale = 1.0;
uniform float outline_width = 0.1;
uniform vec4 color : source_color;


void fragment() {
    float dist = length(UV - vec2(0.5));
    float inner_radius = 0.5 - (outline_width / scale);
    float outer_radius = 0.5;
    float smoothness = 0.005 / scale;
    float inner_circle = smoothstep(inner_radius - smoothness, inner_radius + smoothness, dist);
    float outer_circle = smoothstep(outer_radius - smoothness, outer_radius + smoothness, dist);
    ALBEDO = vec3(inner_circle - outer_circle) * vec3(color.r, color.g, color.b);
    ALPHA = (inner_circle - outer_circle) * color.a;
}

