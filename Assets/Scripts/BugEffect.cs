using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BugEffect : MonoBehaviour
{
    [Header("Settings")]
    public float lerpDuration = 2f;
    public VolumeProfile profile;

    [Range(-1f, 1f)] public float lensDistortionIntensity = 0f;
    [Range(0f, 1f)] public float chromaticAberrationIntensity = 0f;
    [Range(0f, 2f)] public float postExposureIntensity = 0f;

    private LensDistortion _lensDistortion;
    private ChromaticAberration _chromaticAberration;
    private ColorAdjustments _colorAdjustments;
    public UIManager UIManager;

    void Start()
    {
        profile.TryGet(out _lensDistortion);
        profile.TryGet(out _chromaticAberration);
        profile.TryGet(out _colorAdjustments);
    }

    void Update()
    {
        if (_lensDistortion != null)
            _lensDistortion.intensity.value = lensDistortionIntensity;

        if (_chromaticAberration != null)
            _chromaticAberration.intensity.value = chromaticAberrationIntensity;

        if (_colorAdjustments != null)
            _colorAdjustments.postExposure.value = postExposureIntensity;
    }

    public void CallEnum()
    {
        StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    {
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t*t*t;

            lensDistortionIntensity = Mathf.Lerp(0f, 1f, t);
            chromaticAberrationIntensity = Mathf.Lerp(0f, 1f, t);
            postExposureIntensity = Mathf.Lerp(0f, 3f, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        lensDistortionIntensity = 1f;
        chromaticAberrationIntensity = 1f;
        postExposureIntensity = 3f;
        UIManager.LoadGame();
    }
}