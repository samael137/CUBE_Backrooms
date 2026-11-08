using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Control de IA para el enemigo Bacteria Perseguidor en los Backrooms.
/// Persigue al jugador por NavMesh. Al atraparlo, frena, pasa a IDLE,
/// enfoca la cámara a su cabeza y genera sacudida con rugido de muerte.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class BacteriaPerseguidor : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform jugador;             // El objeto Player a perseguir
    public Camera camaraJugador;          // La cámara en primera persona

    [Header("Distancias")]
    public float distanciaDeteccion = 12f; // Distancia para empezar a perseguir
    public float distanciaAtaque = 2.0f;   // Distancia donde te atrapa frente a frente

    [Header("Velocidad")]
    public float velocidadPersecucion = 3.6f;

    [Header("Animación")]
    public Animator animator;

    [Header("Audio")]
    public AudioSource audioEnemigo;
    public AudioClip clipPersecucion;
    public AudioSource audioGritoMuerte;

    [Header("Muerte / Jumpscare")]
    public Transform puntoCabeza;          // Hueso Bip001_Head
    public float fuerzaTemblorCamara = 0.08f;
    public float duracionSusto = 2.2f;

    private NavMeshAgent agent;
    private bool persiguiendo = false;
    private bool atrapado = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = velocidadPersecucion;
        agent.stoppingDistance = distanciaAtaque;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                jugador = playerObj.transform;
        }

        if (camaraJugador == null && jugador != null)
        {
            camaraJugador = jugador.GetComponentInChildren<Camera>();
        }

        if (audioEnemigo != null && clipPersecucion != null)
        {
            audioEnemigo.clip = clipPersecucion;
            audioEnemigo.loop = true;
            audioEnemigo.playOnAwake = false;
        }
    }

    void Update()
    {
        if (atrapado) return; // Al atraparte, se frena y el Animator queda en Idle

        // Actualizar velocidad para cambiar entre Idle y Run
        if (animator != null)
        {
            float velocidadActual = agent.velocity.magnitude;
            animator.SetFloat("Velocidad", velocidadActual);
        }

        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaDeteccion)
        {
            if (!persiguiendo)
            {
                persiguiendo = true;
                if (audioEnemigo != null && !audioEnemigo.isPlaying)
                    audioEnemigo.Play();
            }

            agent.SetDestination(jugador.position);

            if (distancia <= distanciaAtaque)
            {
                AtraparJugador();
            }
        }
        else
        {
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
        atrapado = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // 1. Poner a la bacteria en IDLE inmediatamente
        if (animator != null)
        {
            animator.SetFloat("Velocidad", 0f);
        }

        // 2. Congelar movimiento del jugador
        if (jugador != null)
        {
            SimpleTerrorPlayer scriptPlayer = jugador.GetComponent<SimpleTerrorPlayer>();
            if (scriptPlayer != null)
                scriptPlayer.enabled = false;
        }

        StartCoroutine(CinematicaMuerte());
    }

    IEnumerator CinematicaMuerte()
    {
        if (audioEnemigo != null && audioEnemigo.isPlaying)
            audioEnemigo.Stop();

        if (audioGritoMuerte != null)
            audioGritoMuerte.Play();

        // Que la bacteria mire fijamente al jugador
        transform.LookAt(new Vector3(jugador.position.x, transform.position.y, jugador.position.z));

        Vector3 posOriginalCamara = Vector3.zero;
        if (camaraJugador != null)
            posOriginalCamara = camaraJugador.transform.localPosition;

        float tiempo = 0f;
        while (tiempo < duracionSusto)
        {
            if (camaraJugador != null)
            {
                // Enfocar directamente a la cabeza del monstruo
                Vector3 objetivo = (puntoCabeza != null) ? puntoCabeza.position : transform.position;
                Vector3 dir = (objetivo - camaraJugador.transform.position).normalized;
                Quaternion rotDeseada = Quaternion.LookRotation(dir);
                camaraJugador.transform.rotation = Quaternion.Slerp(camaraJugador.transform.rotation, rotDeseada, Time.deltaTime * 18f);

                // Sacudida de cámara
                float temblorX = Random.Range(-fuerzaTemblorCamara, fuerzaTemblorCamara);
                float temblorY = Random.Range(-fuerzaTemblorCamara, fuerzaTemblorCamara);
                camaraJugador.transform.localPosition = posOriginalCamara + new Vector3(temblorX, temblorY, 0f);
            }

            tiempo += Time.deltaTime;
            yield return null;
        }

        if (camaraJugador != null)
            camaraJugador.transform.localPosition = posOriginalCamara;

        Debug.Log(">>> PANTALLA GAME OVER <<<");
    }
}
