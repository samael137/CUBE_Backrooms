using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Administrador de la pantalla de Game Over y reinicio de partida.
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject panelGameOver;

    void Start()
    {
        // Asegurarse de que el menú empiece oculto al inicio de la escena
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
    }

    /// <summary>
    /// Muestra la pantalla de muerte y libera el cursor del ratón.
    /// </summary>
    public void MostrarGameOver()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }

        // Liberar el cursor para poder presionar los botones
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Reinicia la escena actual desde el principio.
    /// Asignar esta función al botón "Reiniciar" en el Canvas.
    /// </summary>
    public void ReiniciarPartida()
    {
        // Asegurar que el tiempo corra con normalidad
        Time.timeScale = 1f;

        // Recargar la escena activa
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Cierra el juego (o detiene el Play Mode en el editor).
    /// Asignar al botón "Salir".
    /// </summary>
    public void SalirDelJuego()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
