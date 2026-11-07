using System.Collections;
using UnityEngine;

/// <summary>
/// Dispara un jumpscare con imagen y sonido al pisar una zona invisible.
/// </summary>
public class JumpscareTrigger : MonoBehaviour
{
    [Header("Elementos del Susto")]
    public GameObject imagenSusto; // La imagen UI que salta en pantalla
    public AudioSource audioSusto;  // Parlante con el sonido/grito fuerte
    public float duracionSusto = 1.2f; // Segundos que dura en pantalla

    private bool yaSeActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        // Solo se activa si el que entra es el jugador y no se ha usado antes
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            yaSeActivo = true;
            StartCoroutine(MostrarSusto());
        }
    }

    private IEnumerator MostrarSusto()
    {
        // 1. Mostrar la imagen en pantalla
        if (imagenSusto != null)
            imagenSusto.SetActive(true);

        // 2. Reproducir sonido de terror
        if (audioSusto != null)
            audioSusto.Play();

        // 3. Esperar la duración del susto
        yield return new WaitForSeconds(duracionSusto);

        // 4. Ocultar la imagen
        if (imagenSusto != null)
            imagenSusto.SetActive(false);

        // 5. Destruir este trigger para que no vuelva a sonar
        Destroy(gameObject);
    }
}
