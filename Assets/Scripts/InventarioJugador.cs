using UnityEngine;
using TMPro;  // Agregá esto si usás TextMeshPro

public class InventarioJugador : MonoBehaviour
{
    [Header("Contador de piezas oficiales")]
    public int piezasRecogidas = 0;
    public int piezasNecesarias = 2;

    [Header("UI")]
    public TextMeshProUGUI textoContador;  // Arrastra aquí el texto desde la Hierarchy

    void Start()
    {
        ActualizarTexto();
    }

    public void AgregarPieza()
    {
        if (piezasRecogidas < piezasNecesarias)
        {
            piezasRecogidas++;
            Debug.Log($">>> Pieza oficial recogida. Llevás {piezasRecogidas}/{piezasNecesarias}");
            ActualizarTexto();
        }
    }

    public bool TieneTodasLasPiezas()
    {
        return piezasRecogidas >= piezasNecesarias;
    }

    void ActualizarTexto()
    {
        if (textoContador != null)
            textoContador.text = $"Piezas: {piezasRecogidas}/{piezasNecesarias}";
    }
}