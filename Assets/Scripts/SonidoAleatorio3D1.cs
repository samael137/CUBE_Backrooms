using System.Collections;
using UnityEngine;

/// <summary>
/// Hace que un sonido se reproduzca a intervalos aleatorios con momentos de silencio,
/// ideal para ruidos ambientales o sustos dentro de habitaciones en juegos de terror.
/// </summary>
public class SonidoAleatorio3D : MonoBehaviour
{
    [Header("Componente")]
    public AudioSource audioSource;

    [Header("Tiempos de Silencio (segundos)")]
    public float esperaMinima = 8f;
    public float esperaMaxima = 20f;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        StartCoroutine(CicloSonido());
    }

    IEnumerator CicloSonido()
    {
        while (true)
        {
            // 1. Espera un tiempo aleatorio en silencio total
            float tiempoEspera = Random.Range(esperaMinima, esperaMaxima);
            yield return new WaitForSeconds(tiempoEspera);

            // 2. Reproduce el sonido si hay un clip asignado
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();

                // Espera a que termine de reproducirse antes de reiniciar el conteo
                yield return new WaitForSeconds(audioSource.clip.length);
            }
        }
    }
}
