using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Jumpscare clásico con sacudida violenta (shake) de imagen, sonido y oscurecimiento con desvanecimiento.
/// </summary>
public class JumpscareTrigger : MonoBehaviour
{
    [Header("Oscuridad Realista (Volume)")]
    public GameObject volumenOscuridad;
    public float tiempoRetencionOscuridad = 1.0f; // Tiempo que queda oscuro tras quitarse la imagen
    public float tiempoDesvanecimiento = 1.5f;     // Tiempo que tarda la luz en volver suavemente (fade out)

    [Header("Elementos del Susto")]
    public GameObject imagenSusto;       // La imagen en el Canvas
    public AudioSource audioSusto;        // Sonido de grito/terror
    public AudioSource audioLatido;       // Sonido de latido de corazón (opcional)
    public float duracionSusto = 1.2f;    // Duración en segundos

    [Header("Efecto de Sacudida (Shake)")]
    public float fuerzaSacudida = 25f;    // Qué tan fuerte tiembla la imagen

    private bool yaSeActivo = false;
    private Vector3 posicionOriginalImagen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            yaSeActivo = true;

            // Desactivar collider para evitar activaciones múltiples
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            StartCoroutine(EfectoSusto());
        }
    }

    private IEnumerator EfectoSusto()
    {
        // 1. Activar volumen de oscuridad al máximo (weight = 1)
        Volume vol = null;
        if (volumenOscuridad != null)
        {
            volumenOscuridad.SetActive(true);
            vol = volumenOscuridad.GetComponent<Volume>();
            if (vol != null)
                vol.weight = 1f;
        }

        // 2. Activar imagen del susto
        if (imagenSusto != null)
        {
            posicionOriginalImagen = imagenSusto.transform.localPosition;
            imagenSusto.SetActive(true);
        }

        // 3. Reproducir sonidos
        if (audioSusto != null)
            audioSusto.Play();

        if (audioLatido != null)
            audioLatido.Play();

        // 4. Sacudir la imagen mientras dure el susto
        float tiempoPasado = 0f;
        while (tiempoPasado < duracionSusto)
        {
            if (imagenSusto != null)
            {
                // Mover aleatoriamente a los lados y arriba/abajo
                float offsetX = Random.Range(-fuerzaSacudida, fuerzaSacudida);
                float offsetY = Random.Range(-fuerzaSacudida, fuerzaSacudida);
                imagenSusto.transform.localPosition = posicionOriginalImagen + new Vector3(offsetX, offsetY, 0f);
            }

            tiempoPasado += Time.deltaTime;
            yield return null; // Espera al siguiente fotograma
        }

        // 5. APAGAR LA IMAGEN PRIMERO: Ahora la pantalla se despeja y se ve la escena oscura
        if (imagenSusto != null)
        {
            imagenSusto.transform.localPosition = posicionOriginalImagen;
            imagenSusto.SetActive(false);
        }

        // 6. Mantener la oscuridad unos segundos después de que se va la imagen
        if (tiempoRetencionOscuridad > 0f)
        {
            yield return new WaitForSeconds(tiempoRetencionOscuridad);
        }

        // 7. Desvanecimiento suave (fade out) del oscurecimiento hacia la iluminación normal
        if (vol != null && tiempoDesvanecimiento > 0f)
        {
            float tiempoFade = 0f;
            while (tiempoFade < tiempoDesvanecimiento)
            {
                tiempoFade += Time.deltaTime;
                vol.weight = Mathf.Lerp(1f, 0f, tiempoFade / tiempoDesvanecimiento);
                yield return null;
            }
            vol.weight = 0f;
        }
        else if (tiempoDesvanecimiento > 0f)
        {
            yield return new WaitForSeconds(tiempoDesvanecimiento);
        }

        // 8. Desactivar el volumen y restaurar weight para futuros usos
        if (volumenOscuridad != null)
        {
            volumenOscuridad.SetActive(false);
            if (vol != null)
                vol.weight = 1f;
        }

        // 9. Detener el latido si aún está sonando
        if (audioLatido != null && audioLatido.isPlaying)
        {
            audioLatido.Stop();
        }

        Destroy(gameObject);
    }
}
