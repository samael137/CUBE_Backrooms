using UnityEngine;

public class PiezaColeccionable : MonoBehaviour, IInteractuable
{
    [Header("Configuración de la pieza")]
    public string nombrePieza = "Pieza";      // Ej: "Esfera", "Cubo 4D", "Triángulo", "Estrella"
    public bool esPiezaOficial = false;        // Marcar true solo en Esfera y Cubo 4D

    private bool yaRecogida = false;

    public string TextoInteraccion => $"Recoger {nombrePieza}";

    public void Interactuar()
    {
        if (yaRecogida) return;

        yaRecogida = true;
        Debug.Log($">>> {nombrePieza} recogida.");

        // Si es pieza oficial, avisar al inventario del jugador
        if (esPiezaOficial)
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador != null)
            {
                InventarioJugador inventario = jugador.GetComponent<InventarioJugador>();
                if (inventario != null)
                {
                    inventario.AgregarPieza();
                }
            }
        }

        gameObject.SetActive(false); // Desaparece de la escena
    }
}