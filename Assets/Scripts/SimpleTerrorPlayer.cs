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
    public AudioClip[] clipsPasos; // Puedes arrastrar uno o varios sonidos de pasos
    public float volumenPasos = 0.6f;

    [Header("Audio: Respiración")]
    public AudioSource audioRespiracion;
    public AudioClip clipRespiracion;
    [Range(0f, 1f)] public float volumenReposo = 0.15f;  // Cuando camina o está quieto
    [Range(0f, 1f)] public float volumenCorriendo = 0.7f; // Cuando corre con Shift

    private float rotacionX = 0f;
    private float velocidadY = 0f;
    private float alturaCamaraOriginal;
    private float timerPasos = 0f;
    private Vector3 velocidadActual = Vector3.zero;
    private bool pasoDisparado = false;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (camara != null)
            alturaCamaraOriginal = camara.localPosition.y;

        // Iniciar sonido de respiración en bucle
        if (audioRespiracion != null && clipRespiracion != null)
        {
            audioRespiracion.clip = clipRespiracion;
            audioRespiracion.loop = true;
            audioRespiracion.volume = volumenReposo;
            audioRespiracion.Play();
        }

        // Ocultar y centrar el ratón
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Mirar con el ratón
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);

        if (camara != null)
            camara.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        transform.Rotate(0f, mouseX, 0f);

        // 2. Movimiento con teclado
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        bool estaCorriendo = Input.GetKey(KeyCode.LeftShift);
        float velocidadDeseada = estaCorriendo ? velocidadCorrer : velocidadCaminar;
        Vector3 direccion = (transform.right * x + transform.forward * z).normalized * velocidadDeseada;

        // Aceleración suave (inercia humana)
        velocidadActual = Vector3.Lerp(velocidadActual, direccion, Time.deltaTime * 6f);

        // Gravedad
        if (controller.isGrounded)
            velocidadY = -2f;
        else
            velocidadY -= gravedad * Time.deltaTime;

        Vector3 movimientoFinal = velocidadActual;
        movimientoFinal.y = velocidadY;
        controller.Move(movimientoFinal * Time.deltaTime);

        // 3. Balanceo de pasos (Headbob) y Sonido de Pisadas
        float velocidadHorizontal = new Vector3(velocidadActual.x, 0f, velocidadActual.z).magnitude;

        if (camara != null && controller.isGrounded && velocidadHorizontal > 0.2f)
        {
            float multiplicadorPaso = estaCorriendo ? 1.4f : 1.0f;
            timerPasos += Time.deltaTime * velocidadPaso * multiplicadorPaso;

            float seno = Mathf.Sin(timerPasos);
            float nuevoY = alturaCamaraOriginal + seno * alturaPaso;
            camara.localPosition = new Vector3(camara.localPosition.x, nuevoY, camara.localPosition.z);

            // Cada vez que el pie toca el suelo (valle de la onda senoidal)
            if (seno < -0.85f && !pasoDisparado)
            {
                ReproducirPaso();
                pasoDisparado = true;
            }
            else if (seno > 0f)
            {
                pasoDisparado = false; // Se reinicia para el siguiente paso
            }
        }
        else if (camara != null)
        {
            timerPasos = 0f;
            pasoDisparado = false;
            float resetY = Mathf.Lerp(camara.localPosition.y, alturaCamaraOriginal, Time.deltaTime * 5f);
            camara.localPosition = new Vector3(camara.localPosition.x, resetY, camara.localPosition.z);
        }

        // 4. Modular volumen de respiración según si corre o camina
        if (audioRespiracion != null)
        {
            float volumenDeseado = (estaCorriendo && velocidadHorizontal > 0.2f) ? volumenCorriendo : volumenReposo;
            audioRespiracion.volume = Mathf.Lerp(audioRespiracion.volume, volumenDeseado, Time.deltaTime * 2f);
        }

        // 5. Salir del juego o liberar ratón con Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = (CursorLockMode.Locked == CursorLockMode.Locked && !Cursor.visible) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }

    // Reproduce un sonido de paso con variación natural de tono (pitch)
    void ReproducirPaso()
    {
        if (audioPasos == null || clipsPasos == null || clipsPasos.Length == 0)
            return;

        // Elegir clip al azar si hay varios
        AudioClip clipSeleccionado = clipsPasos[Random.Range(0, clipsPasos.Length)];
        if (clipSeleccionado != null)
        {
            // Variar ligeramente el tono entre 0.9 y 1.1 para que suene orgánico
            audioPasos.pitch = Random.Range(0.9f, 1.1f);
            audioPasos.PlayOneShot(clipSeleccionado, volumenPasos);
        }
    }
}
