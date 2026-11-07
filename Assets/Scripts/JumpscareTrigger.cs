using System.Collections;
using UnityEngine;

/// <summary>
/// Jumpscare clásico con sacudida violenta (shake) de imagen y sonido.
/// </summary>
public class JumpscareTrigger : MonoBehaviour
{
    [Header("Elementos del Susto")]
    public GameObject imagenSusto;       // La imagen en el Canvas
    public AudioSource audioSusto;        // Sonido de grito/terror
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
            StartCoroutine(EfectoSusto());
        }
    }

    private IEnumerator EfectoSusto()
    {
        if (imagenSusto != null)
        {
            posicionOriginalImagen = imagenSusto.transform.localPosition;
            imagenSusto.SetActive(true);
        }

        if (audioSusto != null)
            audioSusto.Play();

        // Sacudir la imagen mientras dure el susto
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

        // Restaurar posición y apagar
        if (imagenSusto != null)
        {
            imagenSusto.transform.localPosition = posicionOriginalImagen;
            imagenSusto.SetActive(false);
        }

        Destroy(gameObject);
    }
}
