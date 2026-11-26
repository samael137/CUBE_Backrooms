using UnityEngine;

public class SimpleTerrorPlayer : MonoBehaviour
{
    [Header("Referencias")]
    public Transform camara;
    public CharacterController controller;

    [Header("Velocidades")]
    public float velocidadCaminar = 2.8f;
    public float velocidadCorrer = 5.0f;
    public float sensibilidadMouse = 2.0f;
    public float gravedad = 15.0f;

    [Header("Balanceo de Pasos (Headbob)")]
    public float velocidadPaso = 9.0f;
    public float alturaPaso = 0.035f;

    [Header("Audio: Pasos")]
    public AudioSource audioPasos;
    public AudioClip clipPaso; // Solo UN sonido de paso
    [Range(0f, 1f)] public float volumenPasos = 0.8f;

    [Header("Audio: Respiración")]
    public AudioSource audioRespiracion;
    public AudioClip clipRespiracion; // Solo UN sonido de respiración
    [Range(0f, 1f)] public float volumenReposo = 0.2f;   // Volumen al estar quieto o caminar
    [Range(0f, 1f)] public float volumenCorriendo = 0.8f; // Volumen al correr con Shift

    private float rotacionX = 0f;
    private float velocidadY = 0f;
    private float alturaCamaraOriginal;
    private float timerPasos = 0f;
    private Vector3 velocidadActual = Vector3.zero;
    private bool pasoDisparado = false;

    void Start()
    {
        InicializarComponentes();
        IniciarRespiracion();
        ConfigurarCursor(true);
    }

    void Update()
    {
        RotarCamara();

        bool estaCorriendo = Input.GetKey(KeyCode.LeftShift);
        float velocidadHorizontal = MoverJugador(estaCorriendo);

        ActualizarBalanceoYPasos(estaCorriendo, velocidadHorizontal);
        ActualizarRespiracion(estaCorriendo, velocidadHorizontal);
        AlternarCursorConEscape();
    }

    /// <summary>
    /// Obtiene componentes faltantes y guarda la altura base de la cámara.
    /// </summary>
    private void InicializarComponentes()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (camara != null)
            alturaCamaraOriginal = camara.localPosition.y;
    }

    /// <summary>
    /// Inicia el audio continuo de respiración en volumen de reposo.
    /// </summary>
    private void IniciarRespiracion()
    {
        if (audioRespiracion == null || clipRespiracion == null) return;

        audioRespiracion.clip = clipRespiracion;
        audioRespiracion.loop = true;
        audioRespiracion.volume = volumenReposo;
        audioRespiracion.Play();
    }

    /// <summary>
    /// Rota horizontalmente el cuerpo del jugador y verticalmente la cámara.
    /// </summary>
    private void RotarCamara()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        rotacionX = Mathf.Clamp(rotacionX - mouseY, -80f, 80f);

        if (camara != null)
            camara.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        transform.Rotate(0f, mouseX, 0f);
    }

    /// <summary>
    /// Aplica movimiento WASD con inercia y gravedad. Devuelve la velocidad horizontal actual.
    /// </summary>
    private float MoverJugador(bool estaCorriendo)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        float velocidadDeseada = estaCorriendo ? velocidadCorrer : velocidadCaminar;
        Vector3 direccion = (transform.right * x + transform.forward * z).normalized * velocidadDeseada;

        // Inercia suave (aceleración humana)
        velocidadActual = Vector3.Lerp(velocidadActual, direccion, Time.deltaTime * 6f);

        if (controller.isGrounded)
            velocidadY = -2f;
        else
            velocidadY -= gravedad * Time.deltaTime;

        Vector3 movimientoFinal = velocidadActual;
        movimientoFinal.y = velocidadY;
        controller.Move(movimientoFinal * Time.deltaTime);

        return new Vector3(velocidadActual.x, 0f, velocidadActual.z).magnitude;
    }

    /// <summary>
    /// Aplica el balanceo senoidal de cámara y dispara el sonido de paso al apoyar el pie.
    /// </summary>
    private void ActualizarBalanceoYPasos(bool estaCorriendo, float velocidadHorizontal)
    {
        if (camara == null) return;

        if (!controller.isGrounded || velocidadHorizontal <= 0.2f)
        {
            timerPasos = 0f;
            pasoDisparado = false;
            float resetY = Mathf.Lerp(camara.localPosition.y, alturaCamaraOriginal, Time.deltaTime * 5f);
            camara.localPosition = new Vector3(camara.localPosition.x, resetY, camara.localPosition.z);
            return;
        }

        float multiplicadorPaso = estaCorriendo ? 1.4f : 1.0f;
        timerPasos += Time.deltaTime * velocidadPaso * multiplicadorPaso;

        float seno = Mathf.Sin(timerPasos);
        float nuevoY = alturaCamaraOriginal + seno * alturaPaso;
        camara.localPosition = new Vector3(camara.localPosition.x, nuevoY, camara.localPosition.z);

        // Tocar el suelo = disparar sonido de paso
        if (seno < -0.85f && !pasoDisparado)
        {
            if (audioPasos != null && clipPaso != null)
                audioPasos.PlayOneShot(clipPaso, volumenPasos);

            pasoDisparado = true;
        }
        else if (seno > 0f)
        {
            pasoDisparado = false;
        }
    }

    /// <summary>
    /// Ajusta suavemente el volumen de respiración según si el jugador corre o reposa.
    /// </summary>
    private void ActualizarRespiracion(bool estaCorriendo, float velocidadHorizontal)
    {
        if (audioRespiracion == null) return;

        float volumenDeseado = (estaCorriendo && velocidadHorizontal > 0.2f) ? volumenCorriendo : volumenReposo;
        audioRespiracion.volume = Mathf.Lerp(audioRespiracion.volume, volumenDeseado, Time.deltaTime * 2f);
    }

    /// <summary>
    /// Alterna el bloqueo del cursor al presionar Escape.
    /// </summary>
    private void AlternarCursorConEscape()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        bool estaBloqueado = Cursor.lockState == CursorLockMode.Locked && !Cursor.visible;
        ConfigurarCursor(!estaBloqueado);
    }

    private void ConfigurarCursor(bool bloquear)
    {
        Cursor.lockState = bloquear ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !bloquear;
    }
}
