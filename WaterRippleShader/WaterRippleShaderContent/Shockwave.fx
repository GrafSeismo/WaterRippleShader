// The actual rendered scene.
uniform extern texture ScreenTexture;
sampler ScreenSampler = sampler_state
{
    Texture = <ScreenTexture>;
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
    AddressU = CLAMP;
    AddressV = CLAMP;
};

// 0.5, 0.5 is the screen center for distortion
float2 CenterCoordinate; // = float2(0.5f, 0.5f);

float Width; // = 0.0f;

// Positive pulls, negative pushs, set for e. g. curve over this, like a sin wave that fades out for a realistic wave.
float Magnitude; // = 0.0f; 

// If you do not use quad textures, compensate.
float AspectRatio; // = 1.7778;

// Reciproc scale before set.
float2 Scale; // = float2(1.0f, 1.0f);

float2 CalculateShockwave(float2 TextureCoordinate : TEXCOORD0) : TEXCOORD0
{
	float2 pixelDistance = TextureCoordinate - CenterCoordinate;
	float scalar = length(pixelDistance * Scale * float2(AspectRatio, 1.0f)) - Width; 
	if (scalar < 0.1f && scalar > -0.2f)
	{ 
		float t = abs(scalar); 
		if (scalar < 0.0f)
		{ 
			t = (0.2f - t) * 0.5f; 
		}
		else
		{ 
			t = (0.1f - t);
		} 

		return TextureCoordinate - (pixelDistance * t * Magnitude);
	}

	return TextureCoordinate;
}

float4 ShockwavePixelShaderFunction(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{
	return tex2D(ScreenSampler, CalculateShockwave(TextureCoordinate));
} 

technique ShockwaveTechnique
{
    pass Pass1
    { 
        PixelShader = compile ps_2_0 ShockwavePixelShaderFunction(); 
    } 
}