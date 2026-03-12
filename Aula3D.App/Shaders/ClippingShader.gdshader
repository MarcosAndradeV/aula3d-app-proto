shader_type spatial;
render_mode cull_disabled;

global uniform vec3 plane_normal_x = vec3(1.0, 0.0, 0.0);
global uniform float plane_distance_x = 1000.0;

global uniform vec3 plane_normal_y = vec3(0.0, 1.0, 0.0);
global uniform float plane_distance_y = 1000.0;

global uniform vec3 plane_normal_z = vec3(0.0, 0.0, 1.0);
global uniform float plane_distance_z = 1000.0;

uniform vec4 albedo_color : source_color = vec4(1.0);
uniform sampler2D texture_albedo : source_color, filter_linear_mipmap, repeat_enable;
uniform float roughness : hint_range(0,1) = 0.5;
uniform float metallic : hint_range(0,1) = 0.0;
uniform vec4 emission_color : source_color = vec4(0.0);

void fragment() {
    vec3 world_pos = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;

    if (dot(world_pos, plane_normal_x) < plane_distance_x ||
        dot(world_pos, plane_normal_y) < plane_distance_y ||
        dot(world_pos, plane_normal_z) < plane_distance_z) {
        discard;
    }

    vec4 albedo_tex = texture(texture_albedo, UV);
    ALBEDO = albedo_color.rgb * albedo_tex.rgb;
    METALLIC = metallic;
    ROUGHNESS = roughness;
    EMISSION = emission_color.rgb;

    if (!FRONT_FACING) {
        ALBEDO = albedo_color.rgb * 0.5;
    }
}
