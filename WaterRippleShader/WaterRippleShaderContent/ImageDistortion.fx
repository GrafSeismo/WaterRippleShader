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

// The distortion map.
uniform extern texture DistortionTexture;
sampler DistortionSampler = sampler_state
{
    Texture = <DistortionTexture>;
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
    AddressU = CLAMP;
    AddressV = CLAMP;
};

// If you do not use quad textures, compensate.
float AspectRatio; // = 1.7778f;

// 0.1 seems to work nicely.
float MultiFactor = 0.1f;

// 0.5, 0.5 is the screen center for distortion
float2 CenterCoordinate; // = float2(0.5f, 0.5f);

// 1.0 = fullscreen, >1 = smaller, <1 = larger
float2 Scale; // = float2(1.0f, 1.0f);

float4 ImageDistortionPixelShaderFunction(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{
    float4 distortionColor = tex2D(DistortionSampler, (TextureCoordinate - CenterCoordinate) * Scale * float2(AspectRatio, 1.0f) + float2(0.5f, 0.5f));
    float multi = (distortionColor.b * MultiFactor);
    return tex2D(ScreenSampler, TextureCoordinate + (float2(distortionColor.r, distortionColor.g) * multi) - multi * 0.5f);
}

technique ImageRippleTechnique
{
    pass Pass1
    {
        //Sampler[0] = (ScreenSampler);        
        //Sampler[1] = (DistortionSampler);       
        PixelShader = compile ps_2_0 ImageDistortionPixelShaderFunction();
    }
}