using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Transform camara;              // La cámara en primera persona
    public UnityEngine.UI.Text textoAviso; // El texto que muestra [E] en pantalla

    [Header("Configuración")]
    public float distanciaMaxima = 3f;     // Qué tan lejos se puede interactuar
    public LayerMask capaInteractuable;    // Capa para filtrar solo objetos interactuables

    private IInteractuable objetivoActual = null; // El objeto que estamos mirando ahora

    void Update()
    {
        // Si el cursor está desbloqueado (menú de Esc, Game Over, etc.), no interactuar
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (textoAviso != null)
                textoAviso.gameObject.SetActive(false);
            return;
        }

        // Disparar un rayo desde la cámara hacia donde mira el jugador
        if (Physics.Raycast(camara.position, camara.forward, out RaycastHit hit, distanciaMaxima, capaInteractuable))
        {
            // ¿El objeto golpeado implementa IInteractuable?
            IInteractuable interactuable = hit.collider.GetComponent<IInteractuable>();

            if (interactuable != null)
            {
                objetivoActual = interactuable;

                // Mostrar el aviso en pantalla
                if (textoAviso != null)
                {
                    textoAviso.gameObject.SetActive(true);
                    textoAviso.text = "[E] " + interactuable.TextoInteraccion;
                }

                // Si presionaste E, ejecutar la interacción
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactuable.Interactuar();
                }
            }
            else
            {
                // Golpeaste algo que no es interactuable
                objetivoActual = null;
                if (textoAviso != null)
                    textoAviso.gameObject.SetActive(false);
            }
        }
        else
        {
            // No golpeaste nada dentro de la distancia
            objetivoActual = null;
            if (textoAviso != null)
                textoAviso.gameObject.SetActive(false);
        }
    }
}