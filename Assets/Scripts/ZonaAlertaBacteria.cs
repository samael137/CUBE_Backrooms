using UnityEngine;

/// <summary>
/// Zona trigger que alerta a la Bacteria cuando el jugador entra a un lugar específico.
/// La Bacteria se dirige al punto y, si no encuentra al jugador, vuelve a deambular.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ZonaAlertaBacteria : MonoBehaviour
{
    public BacteriaPerseguidor bacteria;
    public float tiempoRecarga = 5f;

    private float proximoTiempoDisponible = 0f;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    private void Start()
    {
        if (bacteria == null)
            bacteria = FindFirstObjectByType<BacteriaPerseguidor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time < proximoTiempoDisponible) return;
        if (bacteria == null) return;

        bacteria.InvestigarLugar(other.transform.position);
        proximoTiempoDisponible = Time.time + tiempoRecarga;
    }
}
