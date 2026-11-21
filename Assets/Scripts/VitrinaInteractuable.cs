using UnityEngine;

public class VitrinaInteractuable : MonoBehaviour, IInteractuable
{
    public string TextoInteraccion => "Abrir Vitrina";
    public GameObject esfera;   // <-- Arrastra aquí el objeto Esfera desde la Hierarchy

    private bool yaAbierta = false;

    public void Interactuar()
    {
        if (yaAbierta) return; // Para que no se abra dos veces

        yaAbierta = true;
        Debug.Log(">>> Vitrina abierta. La esfera cae al suelo.");

        if (esfera != null)
        {
            esfera.SetActive(true); // Aparece la esfera

            Rigidbody rb = esfera.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false; // Ahora sí cae con gravedad
        }
    }
}