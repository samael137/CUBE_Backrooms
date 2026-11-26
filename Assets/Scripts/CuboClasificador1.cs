using UnityEngine;
using UnityEngine.SceneManagement;

public class CuboClasificador1 : MonoBehaviour, IInteractuable
{
    public string TextoInteraccion => "Activar Cubo Clasificador";

    private bool yaActivado = false;

    public void Interactuar()
    {
        if (yaActivado) return;

        InventarioJugador inventario = ObtenerInventarioJugador();
        if (inventario == null) return;

        if (!inventario.TieneTodasLasPiezas())
        {
            Debug.Log(">>> Te faltan piezas para activar el cubo clasificador.");
            return;
        }

        yaActivado = true;
        ActivarSaltoNoclip();
    }

    private InventarioJugador ObtenerInventarioJugador()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador == null)
        {
            Debug.Log(">>> No se encontró al jugador.");
            return null;
        }

        InventarioJugador inventario = jugador.GetComponent<InventarioJugador>();
        if (inventario == null)
        {
            Debug.Log(">>> El jugador no tiene InventarioJugador.");
            return null;
        }

        return inventario;
    }

    private void ActivarSaltoNoclip()
    {
        Debug.Log(">>> Cubo Clasificador activado. Saltando a Escenario 2...");

        // TODO: Aquí después agregamos el efecto visual y sonoro de Noclip
        // Por ahora cargamos directamente el Escenario 2
        SceneManager.LoadScene("Escenario_2");
    }
}