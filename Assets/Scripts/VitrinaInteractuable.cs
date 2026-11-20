using UnityEngine;

public class VitrinaInteractuable : MonoBehaviour, IInteractuable
{
    public string TextoInteraccion => "Abrir Vitrina";

    private bool yaAbierta = false;

    public void Interactuar()
    {
        if (yaAbierta) return; // Para que no se abra dos veces

        yaAbierta = true;
        Debug.Log(">>> Vitrina abierta. La esfera cae al suelo.");

        // TODO: Aquí después activamos la esfera para que caiga con gravedad
        // Por ahora solo probamos que el mensaje se ejecute
    }
}