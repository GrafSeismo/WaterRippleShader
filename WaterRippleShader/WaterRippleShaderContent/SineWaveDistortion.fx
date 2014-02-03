// The actual rendered scene.
uniform extern texture ScreenTexture;
sampler ScreenSampler = sampler_state
{
    texture = <ScreenTexture>;
};

// 0.5, 0.5 is the screen center
float2 CenterCoordinate = float2(0.5f, 0.5f);

// 1 is a good default
float Magnitude = 1.0f;

// pi / 0.75 is a good default
float Wave = 4.1887902f;

float Width;

float AspectRatio;

float2 Scale;

float4 SineWaveDistortionPixelShaderFunction(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{
	float2 direction = abs(TextureCoordinate - CenterCoordinate) * float2(AspectRatio, 1.0f) * Scale;
	
    float distance = length(direction) - Width;
	
    float distort = sin(clamp((distance / Wave), -1, 1) * 3.1415) * Magnitude;
    
    float2 offset = normalize(direction);

    return tex2D(ScreenSampler, lerp(TextureCoordinate, TextureCoordinate - (distort * offset), distort));
/*
    float scalar = length(abs(TextureCoordinate - CenterCoordinate) * Scale * float2(AspectRatio, 1.0f)) - Width;

    // Calculate how far to distort for this pixel.
    float sinoffset = sin(scalar / Wave);
    sinoffset = clamp(sinoffset, 0, 1);
    
    // Calculate which direction to distort.
    float sinsign = cos(scalar / Wave);
    
    // Reduce the distortion effect.
    sinoffset = sinoffset * Magnitude / 32;
    
    // Pick a pixel on the screen for this pixel,
	// based on the calculated offset and direction.
    return tex2D(ScreenSampler, TextureCoordinate + (sinoffset * sinsign));
*/
}

technique SineWaveDistortionTechnique
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 SineWaveDistortionPixelShaderFunction();
    }
}