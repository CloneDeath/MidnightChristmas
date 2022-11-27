shader_type spatial;
render_mode vertex_lighting, skip_vertex_transform, 
			specular_disabled, diffuse_lambert_wrap;

uniform sampler2D albedo: hint_default_white, filter_nearest, repeat_enable;
uniform sampler2D specular: hint_default_black, filter_nearest, repeat_enable;
uniform sampler2D emission: hint_default_black, filter_nearest, repeat_enable;

uniform float jitter: hint_range(0.0, 1.0) = 0.5;
uniform bool jitter_z_coordinate = true;
uniform bool jitter_depth_independent = true;
uniform bool affine_texture_mapping = true;
uniform float alpha_scissor: hint_range(0.0, 1.0) = 1.0;

void vertex() {
	vec3 vertex = (MODELVIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;

	float z_orig = vertex.z;
	//float i = (1.0 - jitter) * min(VIEWPORT_SIZE.x, VIEWPORT_SIZE.y) / 2.0;
	//float i = 100.0;
	float i = (1.0 - jitter) * max(min(VIEWPORT_SIZE.x, VIEWPORT_SIZE.y), 128) / 2.0;
	
	if (jitter_depth_independent) {
		float w = (PROJECTION_MATRIX * vec4(vertex, 1.0)).w;
		vertex = round(vertex / w * i) / i * w;
	} else {
		vertex = round(vertex * i) / i;
	}
	
	if (!jitter_z_coordinate) {
		vertex.z = z_orig;
	}

	if (affine_texture_mapping) {
		UV *= vertex.z;
	}
	
	VERTEX = vertex;
}

void fragment() {
	vec2 uv = UV;
	
	if (affine_texture_mapping) {
		uv /= VERTEX.z;
	}
	
	ALBEDO = texture(albedo, uv).rgb;
	ALPHA = texture(albedo, uv).a;
	ALPHA_SCISSOR_THRESHOLD = alpha_scissor;
	EMISSION = texture(emission, uv).rgb * texture(emission, uv).a;
	SPECULAR = texture(specular, uv).r;
}