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
        Volume vol = ActivarOscuridad();
        ActivarImagenYSonidos();

        yield return StartCoroutine(SacudirImagen());

        OcultarImagen();

        if (tiempoRetencionOscuridad > 0f)
            yield return new WaitForSeconds(tiempoRetencionOscuridad);

        yield return StartCoroutine(DesvanecerOscuridad(vol));

        RestaurarEstadoFinal(vol);
        Destroy(gameObject);
    }

    private Volume ActivarOscuridad()
    {
        if (volumenOscuridad == null) return null;

        volumenOscuridad.SetActive(true);
        Volume vol = volumenOscuridad.GetComponent<Volume>();
        if (vol != null)
            vol.weight = 1f;

        return vol;
    }

    private void ActivarImagenYSonidos()
    {
        if (imagenSusto != null)
        {
            posicionOriginalImagen = imagenSusto.transform.localPosition;
            imagenSusto.SetActive(true);
        }

        if (audioSusto != null)
            audioSusto.Play();

        if (audioLatido != null)
            audioLatido.Play();
    }

    private IEnumerator SacudirImagen()
    {
        float tiempoPasado = 0f;
        while (tiempoPasado < duracionSusto)
        {
            if (imagenSusto != null)
            {
                float offsetX = Random.Range(-fuerzaSacudida, fuerzaSacudida);
                float offsetY = Random.Range(-fuerzaSacudida, fuerzaSacudida);
                imagenSusto.transform.localPosition = posicionOriginalImagen + new Vector3(offsetX, offsetY, 0f);
            }

            tiempoPasado += Time.deltaTime;
            yield return null;
        }
    }

    private void OcultarImagen()
    {
        if (imagenSusto == null) return;

        imagenSusto.transform.localPosition = posicionOriginalImagen;
        imagenSusto.SetActive(false);
    }

    private IEnumerator DesvanecerOscuridad(Volume vol)
    {
        if (tiempoDesvanecimiento <= 0f) yield break;

        if (vol == null)
        {
            yield return new WaitForSeconds(tiempoDesvanecimiento);
            yield break;
        }

        float tiempoFade = 0f;
        while (tiempoFade < tiempoDesvanecimiento)
        {
            tiempoFade += Time.deltaTime;
            vol.weight = Mathf.Lerp(1f, 0f, tiempoFade / tiempoDesvanecimiento);
            yield return null;
        }
        vol.weight = 0f;
    }

    private void RestaurarEstadoFinal(Volume vol)
    {
        if (volumenOscuridad != null)
        {
            volumenOscuridad.SetActive(false);
            if (vol != null)
                vol.weight = 1f;
        }

        if (audioLatido != null && audioLatido.isPlaying)
            audioLatido.Stop();
    }
}
