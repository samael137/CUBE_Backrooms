using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Transform camara;              // La cámara en primera persona
    public TMPro.TextMeshProUGUI textoAviso; // El texto que muestra [E] en pantalla

    [Header("Configuración")]
    public float distanciaMaxima = 3f;     // Qué tan lejos se puede interactuar
    public LayerMask capaInteractuable;    // Capa para filtrar solo objetos interactuables

    private IInteractuable objetivoActual = null; // El objeto que estamos mirando ahora

    void Update()
    {
        // Si el cursor está desbloqueado (menú de Esc, Game Over, etc.), no interactuar
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            LimpiarObjetivo();
            return;
        }

        objetivoActual = BuscarInteractuableEnMira();

        if (objetivoActual == null)
        {
            OcultarAviso();
            return;
        }

        MostrarAviso(objetivoActual.TextoInteraccion);

        if (Input.GetKeyDown(KeyCode.E))
        {
            objetivoActual.Interactuar();
        }
    }

    /// <summary>
    /// Lanza un rayo desde la cámara y devuelve el IInteractuable apuntado, o null si no hay ninguno.
    /// </summary>
    private IInteractuable BuscarInteractuableEnMira()
    {
        if (camara == null) return null;

        if (Physics.Raycast(camara.position, camara.forward, out RaycastHit hit, distanciaMaxima, capaInteractuable))
            return hit.collider.GetComponent<IInteractuable>();

        return null;
    }

    private void MostrarAviso(string textoInteraccion)
    {
        if (textoAviso == null) return;

        textoAviso.gameObject.SetActive(true);
        textoAviso.text = "[E] " + textoInteraccion;
    }

    private void OcultarAviso()
    {
        if (textoAviso != null)
            textoAviso.gameObject.SetActive(false);
    }

    private void LimpiarObjetivo()
    {
        objetivoActual = null;
        OcultarAviso();
    }
}