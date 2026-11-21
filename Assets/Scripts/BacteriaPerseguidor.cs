using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Control de IA para el enemigo Bacteria Perseguidor en los Backrooms.
/// Patrulla por el escenario buscando al jugador. Al detectarlo, corre hacia él.
/// Al atraparlo, frena, pasa a IDLE, enfoca la cámara a su cabeza y genera sacudida con rugido de muerte.
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

    [Header("Velocidades")]
    public float velocidadPatrullaje = 1.8f;   // Velocidad al deambular
    public float velocidadPersecucion = 3.6f;  // Velocidad al perseguir

    [Header("Patrullaje / Deambular")]
    public bool patrullajeActivo = true;
    public float radioPatrullaje = 15f;        // Radio alrededor para buscar nuevo destino
    public float tiempoEsperaMin = 2.0f;       // Tiempo mínimo que espera al llegar a un punto
    public float tiempoEsperaMax = 4.0f;       // Tiempo máximo que espera al llegar a un punto

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
    public GameObject panelGameOver;       // Panel de Game Over (Paso 3)

    private NavMeshAgent agent;
    private bool persiguiendo = false;
    private bool atrapado = false;
    private bool esperandoEnPunto = false;
    private float timerEspera = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrullajeActivo ? velocidadPatrullaje : velocidadPersecucion;
        agent.stoppingDistance = 0.5f;

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

        // Iniciar patrulla si está activa
        if (patrullajeActivo)
        {
            BuscarNuevoPuntoPatrulla();
        }
    }

    void Update()
    {
        if (atrapado) return; // Al atraparte, se frena y el Animator queda en Idle

        // Actualizar velocidad para el Animator (transición Idle / Walk / Run)
        if (animator != null)
        {
            float velocidadActual = agent.velocity.magnitude;
            animator.SetFloat("Velocidad", velocidadActual);
        }

        // 1. Comprobar si el jugador está dentro del rango de detección
        if (jugador != null)
        {
            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

            if (distanciaAlJugador <= distanciaDeteccion)
            {
                // Iniciar persecución
                if (!persiguiendo)
                {
                    persiguiendo = true;
                    esperandoEnPunto = false;
                    agent.speed = velocidadPersecucion;
                    agent.stoppingDistance = distanciaAtaque;

                    if (audioEnemigo != null && !audioEnemigo.isPlaying)
                        audioEnemigo.Play();
                }

                agent.SetDestination(jugador.position);

                // Si alcanzó al jugador para atraparlo
                if (distanciaAlJugador <= distanciaAtaque)
                {
                    AtraparJugador();
                }
                return;
            }
        }

        // 2. Si estaba persiguiendo y el jugador escapó más allá de la detección
        if (persiguiendo)
        {
            persiguiendo = false;
            agent.speed = velocidadPatrullaje;
            agent.stoppingDistance = 0.5f;

            if (audioEnemigo != null && audioEnemigo.isPlaying)
                audioEnemigo.Stop();

            BuscarNuevoPuntoPatrulla();
        }

        // 3. Patrullaje / Deambular libremente por el escenario
        if (patrullajeActivo)
        {
            ActualizarPatrullaje();
        }
    }

    private void ActualizarPatrullaje()
    {
        // Si no tiene ruta o está atascado, buscar nuevo punto
        if (!agent.hasPath && !esperandoEnPunto)
        {
            BuscarNuevoPuntoPatrulla();
            return;
        }

        // Si ya llegó cerca del punto de patrullaje
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.3f)
        {
            if (!esperandoEnPunto)
            {
                esperandoEnPunto = true;
                timerEspera = Random.Range(tiempoEsperaMin, tiempoEsperaMax);
            }
            else
            {
                timerEspera -= Time.deltaTime;
                if (timerEspera <= 0f)
                {
                    esperandoEnPunto = false;
                    BuscarNuevoPuntoPatrulla();
                }
            }
        }
    }

    private void BuscarNuevoPuntoPatrulla()
    {
        Vector3 direccionAleatoria = Random.insideUnitSphere * radioPatrullaje;
        direccionAleatoria += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(direccionAleatoria, out hit, radioPatrullaje, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
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

        // Mostrar menú de Game Over si existe
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Debug.Log(">>> PANTALLA GAME OVER <<<");
    }
}
