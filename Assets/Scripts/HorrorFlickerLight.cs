using System.Collections;
using UnityEngine;

/// <summary>
/// Controla el parpadeo errático y los apagones estilo película de terror / Backrooms.
/// </summary>
public class HorrorFlickerLight : MonoBehaviour
{
    [Header("Componentes")]
    [Tooltip("La luz que va a parpadear. Si se deja vacía, toma la del mismo GameObject")]
    public Light targetLight;

    [Tooltip("(Opcional) El Mesh Renderer del foco/lámpara para apagar su brillo (emisión) al apagarse")]
    public Renderer bulbRenderer;

    [Header("Intensidad")]
    [Tooltip("Intensidad normal de la luz encendida")]
    public float baseIntensity = 4.0f;

    [Tooltip("Intensidad mínima durante un parpadeo leve")]
    public float minFlickerIntensity = 0.2f;

    [Header("Tiempos de Parpadeo Rápido")]
    [Tooltip("Tiempo mínimo entre micro-parpadeos (segundos)")]
    public float minFlickerSpeed = 0.04f;

    [Tooltip("Tiempo máximo entre micro-parpadeos (segundos)")]
    public float maxFlickerSpeed = 0.15f;

    [Header("Apagón Total (Blackout)")]
    [Tooltip("Permite que la luz se corte por completo de forma aleatoria")]
    public bool enableBlackouts = true;

    [Tooltip("Tiempo mínimo entre apagones (segundos)")]
    public float minTimeBetweenBlackouts = 6.0f;

    [Tooltip("Tiempo máximo entre apagones (segundos)")]
    public float maxTimeBetweenBlackouts = 16.0f;

    [Tooltip("Duración mínima del apagón a oscuras (segundos)")]
    public float minBlackoutDuration = 0.8f;

    [Tooltip("Duración máxima del apagón a oscuras (segundos)")]
    public float maxBlackoutDuration = 3.5f;

    private Material bulbMaterial;
    private Color originalEmissionColor;
    private bool isBlackout = false;
    private static readonly int EmissionColorProperty = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        if (targetLight != null)
            baseIntensity = targetLight.intensity;

        if (bulbRenderer != null)
        {
            bulbMaterial = bulbRenderer.material;
            if (bulbMaterial.HasProperty(EmissionColorProperty))
            {
                originalEmissionColor = bulbMaterial.GetColor(EmissionColorProperty);
            }
        }
    }

    private void Start()
    {
        if (targetLight == null)
        {
            Debug.LogWarning($"[HorrorFlickerLight] No hay ningún componente Light asignado en {gameObject.name}");
            return;
        }

        StartCoroutine(FlickerLoop());

        if (enableBlackouts)
        {
            StartCoroutine(BlackoutLoop());
        }
    }

    private IEnumerator FlickerLoop()
    {
        while (true)
        {
            if (!isBlackout)
            {
                float roll = Random.value;
                float currentIntensity;

                if (roll > 0.45f)
                {
                    // Luz casi normal con leve vibración
                    currentIntensity = Random.Range(baseIntensity * 0.75f, baseIntensity);
                }
                else if (roll > 0.15f)
                {
                    // Bajón notable de voltaje
                    currentIntensity = Random.Range(minFlickerIntensity, baseIntensity * 0.45f);
                }
                else
                {
                    // Corte instantáneo de milisegundos
                    currentIntensity = 0f;
                }

                SetLight(currentIntensity);
            }

            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        }
    }

    private IEnumerator BlackoutLoop()
    {
        while (true)
        {
            // Esperar el intervalo antes del siguiente apagón
            yield return new WaitForSeconds(Random.Range(minTimeBetweenBlackouts, maxTimeBetweenBlackouts));

            // Chispazos rápidos previos al apagón
            int sparks = Random.Range(3, 6);
            for (int i = 0; i < sparks; i++)
            {
                SetLight(0f);
                yield return new WaitForSeconds(0.04f);
                SetLight(baseIntensity * 1.2f);
                yield return new WaitForSeconds(0.05f);
            }

            // APAGÓN TOTAL
            isBlackout = true;
            SetLight(0f);

            // Tiempo a oscuras en silencio
            yield return new WaitForSeconds(Random.Range(minBlackoutDuration, maxBlackoutDuration));

            // Vuelve la luz
            isBlackout = false;
            SetLight(baseIntensity);
        }
    }

    private void SetLight(float intensity)
    {
        if (targetLight != null)
        {
            targetLight.intensity = intensity;
            targetLight.enabled = intensity > 0.01f;
        }

        // Si hay lámpara asignada, atenuar o apagar su emisión
        if (bulbMaterial != null && bulbMaterial.HasProperty(EmissionColorProperty))
        {
            if (intensity > 0.01f)
            {
                float factor = Mathf.Clamp01(intensity / baseIntensity);
                bulbMaterial.SetColor(EmissionColorProperty, originalEmissionColor * factor);
            }
            else
            {
                bulbMaterial.SetColor(EmissionColorProperty, Color.black);
            }
        }
    }
}
