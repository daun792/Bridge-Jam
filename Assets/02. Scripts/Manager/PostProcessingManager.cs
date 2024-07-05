using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingManager : Manager
{
    private Volume volume;

    protected override void Awake()
    {
        base.Awake();

        volume = GetComponent<Volume>();
        //TODO: Additional Initialize
    }

    public void SetSaturation(float _value)
    {
        volume.profile.TryGet(out UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments);
        colorAdjustments.saturation.value = _value;
    }

    public void SetLensDistortion()
    {
        volume.profile.TryGet(out UnityEngine.Rendering.Universal.LensDistortion lensDistortion);
        lensDistortion.intensity.value = -0.4f;
    }

    public void SetFlashBack()
    {
        volume.profile.TryGet(out UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration);
        chromaticAberration.intensity.value = 0.4f;
    }

    public void SetGlitch(float _value)
    {
        volume.profile.TryGet(out URPGlitch.Runtime.DigitalGlitch.DigitalGlitchVolume digitalGlitch);
        digitalGlitch.intensity.value = _value;
    }

    public void SetAnalogGlitch(bool _isActive)
    {
        volume.profile.TryGet(out URPGlitch.Runtime.AnalogGlitch.AnalogGlitchVolume analogGlitch);
        analogGlitch.scanLineJitter.value = _isActive ? 0.2f : 0f;
        analogGlitch.verticalJump.value = _isActive ? 0.05f : 0f;
    }
}
