using UnityEngine;

public class SimpleTerrorPlayer : MonoBehaviour
{
    public Transform camara;
    public CharacterController controller;

    public float velocidadCaminar = 2.8f;
    public float velocidadCorrer = 5.0f;
    public float sensibilidadMouse = 2.0f;
    public float gravedad = 15.0f;

    // Para el balanceo de pasos (Headbob)
    public float velocidadPaso = 9.0f;
    public float alturaPaso = 0.035f;

    private float rotacionX = 0f;
    private float velocidadY = 0f;
    private float alturaCamaraOriginal;
    private float timerPasos = 0f;
    private Vector3 velocidadActual = Vector3.zero;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (camara != null)
            alturaCamaraOriginal = camara.localPosition.y;

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

        float velocidadDeseada = Input.GetKey(KeyCode.LeftShift) ? velocidadCorrer : velocidadCaminar;
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

        // 3. Balanceo de pasos al caminar (Headbob)
        if (camara != null && controller.isGrounded && velocidadActual.magnitude > 0.2f)
        {
            timerPasos += Time.deltaTime * velocidadPaso;
            float nuevoY = alturaCamaraOriginal + Mathf.Sin(timerPasos) * alturaPaso;
            camara.localPosition = new Vector3(camara.localPosition.x, nuevoY, camara.localPosition.z);
        }
        else if (camara != null)
        {
            timerPasos = 0f;
            float resetY = Mathf.Lerp(camara.localPosition.y, alturaCamaraOriginal, Time.deltaTime * 5f);
            camara.localPosition = new Vector3(camara.localPosition.x, resetY, camara.localPosition.z);
        }

        // 4. Salir del juego o liberar ratón con Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = (CursorLockMode.Locked == CursorLockMode.Locked && !Cursor.visible) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }
}
