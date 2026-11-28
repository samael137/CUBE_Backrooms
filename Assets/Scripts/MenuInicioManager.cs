using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// Controla el menú de inicio: al pulsar Iniciar reproduce el video intro, luego el video backrooms y finalmente carga el juego.
/// </summary>
public class MenuInicioManager : MonoBehaviour
{
    [Header("Escena destino")]
    public string nombreEscenaJuego = "Juego";

    [Header("UI y Video")]
    public GameObject panelMenu;
    public GameObject pantallaVideo;
    public VideoPlayer videoPlayer;

    [Header("Secuencia de Videos")]
    public VideoClip videoIntro;
    public VideoClip videoBackrooms;
    [Range(0.1f, 2f)] public float velocidadReproduccion = 0.6f;

    private bool reproduciendoVideo = false;
    private bool reproduciendoSegundoVideo = false;
    private bool cargandoEscena = false;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (panelMenu != null)
            panelMenu.SetActive(true);

        if (pantallaVideo != null)
            pantallaVideo.SetActive(false);
    }

    private void Update()
    {
        if (!reproduciendoVideo || cargandoEscena) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            CargarJuego();
    }

    /// <summary>
    /// Asignar al botón "Iniciar" en el Canvas.
    /// </summary>
    public void IniciarJuego()
    {
        if (cargandoEscena || reproduciendoVideo) return;

        if (videoPlayer == null || pantallaVideo == null)
        {
            CargarJuego();
            return;
        }

        if (panelMenu != null)
            panelMenu.SetActive(false);

        pantallaVideo.SetActive(true);
        reproduciendoVideo = true;
        reproduciendoSegundoVideo = false;

        videoPlayer.loopPointReached += AlTerminarVideo;

        if (videoIntro != null)
        {
            ReproducirClip(videoIntro);
        }
        else if (videoBackrooms != null)
        {
            reproduciendoSegundoVideo = true;
            ReproducirClip(videoBackrooms);
        }
        else
        {
            CargarJuego();
        }
    }

    /// <summary>
    /// Asignar al botón "Salir" en el Canvas (opcional).
    /// </summary>
    public void SalirDelJuego()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ReproducirClip(VideoClip clip)
    {
        videoPlayer.clip = clip;
        videoPlayer.playbackSpeed = velocidadReproduccion;
        videoPlayer.Play();
    }

    private void AlTerminarVideo(VideoPlayer source)
    {
        if (!reproduciendoSegundoVideo && videoBackrooms != null)
        {
            reproduciendoSegundoVideo = true;
            ReproducirClip(videoBackrooms);
            return;
        }

        CargarJuego();
    }

    private void CargarJuego()
    {
        if (cargandoEscena) return;
        cargandoEscena = true;

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= AlTerminarVideo;
            videoPlayer.Stop();
        }

        SceneManager.LoadScene(nombreEscenaJuego);
    }
}
