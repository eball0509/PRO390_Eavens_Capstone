using UnityEngine;

[ExecuteAlways]
public class DayNightCycle : MonoBehaviour
{
    [Header("Time")]
    [Range(0f, 1f)]
    public float timeOfDay = 0.25f; // 0 = midnight, 0.5 = noon
    public float dayDurationSeconds = 120f;
    public bool running = true;

    [Header("Skybox")]
    public Material skyboxMaterial;

    static readonly int BlendID = Shader.PropertyToID("_Blend");

    void Update()
    {
        if (running && Application.isPlaying)
            timeOfDay = (timeOfDay + Time.deltaTime / dayDurationSeconds) % 1f;

        ApplyCycle();
    }

    void ApplyCycle()
    {
        if (skyboxMaterial == null) return;

        // Smooth blend: 0 = full day, 1 = full night
        float blend = Mathf.Clamp01((Mathf.Cos(timeOfDay * Mathf.PI * 2f) * -0.5f) + 0.5f);
        skyboxMaterial.SetFloat(BlendID, blend);
    }

#if UNITY_EDITOR
    void OnValidate() => ApplyCycle();
#endif
}