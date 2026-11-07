using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Control de IA para el enemigo Bacteria Perseguidor en los Backrooms.
/// Controla NavMeshAgent, audio 3D y animaciones según la velocidad.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class BacteriaPerseguidor : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform jugador;             // El objeto Player a perseguir

    [Header("Distancias")]
    public float distanciaDeteccion = 15f; // Distancia para empezar a perseguir
    public float distanciaAtaque = 1.5f;   // Distancia para atrapar al jugador

    [Header("Velocidad de Movimiento")]
    public float velocidadPersecucion = 3.8f; // Velocidad del NavMeshAgent al perseguir

    [Header("Animación")]
    public Animator animator;             // Componente Animator del modelo

    [Header("Audio")]
    public AudioSource audioEnemigo;       // Sonido de rugido / pasos de bacteria
    public AudioClip clipPersecucion;

    private NavMeshAgent agent;
    private bool persiguiendo = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = velocidadPersecucion;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // Si no se asignó manualmente el jugador, buscarlo por etiqueta
        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                jugador = playerObj.transform;
        }

        // Configurar audio de persecución en bucle
        if (audioEnemigo != null && clipPersecucion != null)
        {
            audioEnemigo.clip = clipPersecucion;
            audioEnemigo.loop = true;
            audioEnemigo.playOnAwake = false;
        }
    }

    void Update()
    {
        // Actualizar el parámetro Velocidad en el Animator según qué tan rápido se mueve el NavMeshAgent
        if (animator != null)
        {
            float velocidadActual = agent.velocity.magnitude;
            animator.SetFloat("Velocidad", velocidadActual);
        }

        if (jugador == null) return;

        // Calcular distancia entre el enemigo y el jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // 1. Iniciar persecución si el jugador entra en su rango
        if (distancia <= distanciaDeteccion)
        {
            if (!persiguiendo)
            {
                persiguiendo = true;
                if (audioEnemigo != null && !audioEnemigo.isPlaying)
                    audioEnemigo.Play();
            }

            // Ordenar al NavMeshAgent que camine hacia el jugador
            agent.SetDestination(jugador.position);

            // 2. Si atrapa al jugador
            if (distancia <= distanciaAtaque)
            {
                AtraparJugador();
            }
        }
        else
        {
            // Si el jugador escapa muy lejos, se detiene
            if (persiguiendo)
            {
                persiguiendo = false;
                agent.ResetPath();
                if (audioEnemigo != null && audioEnemigo.isPlaying)
                    audioEnemigo.Stop();
            }
        }
    }

    void AtraparJugador()
    {
        Debug.Log("¡El Bacteria te ha atrapado! GAME OVER");
    }
}
