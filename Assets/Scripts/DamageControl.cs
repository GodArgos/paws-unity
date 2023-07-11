using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class DamageControl : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public float maxHealth = 100f;
    public float transitionSpeed = 1f;
    private Volume volume;
    private Vignette vignette;
    private float initialIntensity;
    private Color initialColor;
    private float initialSmoothness;
    private float targetIntensity;
    private Color targetColor;
    private float targetSmoothness;

    private void Start()
    {
        volume = GetComponent<Volume>();

        if (volume.profile.TryGet(out vignette))
        {
            initialIntensity = vignette.intensity.value;
            initialColor = vignette.color.value;
            initialSmoothness = vignette.smoothness.value;
            targetIntensity = initialIntensity;
            targetColor = initialColor;
            targetSmoothness = initialSmoothness;
        }
        else
        {
            Debug.LogWarning("No se encontró el componente Vignette en el perfil del volumen.");
            enabled = false;
        }
    }

    private void Update()
    {
        float currentHealth = playerController.currentHealth;

        // Calcular la intensidad y la suavidad del vignette en función de la vida actual
        float intensity = 1f - (currentHealth / maxHealth);
        float smoothness = 1f - (currentHealth / maxHealth);

        // Ajustar los valores objetivo para la transición del vignette
        if (currentHealth > 0 && currentHealth < 100)
        {
            targetIntensity = intensity;
            targetColor = Color.red;
            targetSmoothness = smoothness;
        }
        else
        {
            targetIntensity = 0.4f;
            targetColor = initialColor;
            targetSmoothness = 0.2f;
        }

        // Realizar una interpolación suave (lerp) para los valores del vignette
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, Time.deltaTime * transitionSpeed);
        vignette.color.value = Color.Lerp(vignette.color.value, targetColor, Time.deltaTime * transitionSpeed);
        vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, targetSmoothness, Time.deltaTime * transitionSpeed);
    }
}
