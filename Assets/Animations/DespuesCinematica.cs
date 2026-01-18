using UnityEngine;
using UnityEngine.Playables;

public class DespuesCinematica : MonoBehaviour
{
    public PlayableDirector Cine;
    private float duracion;
    private float t;

    [Header("Objetos de Escena")]
    public GameObject CineObj;       
    public GameObject GameplayObj;   

    [Header("Componentes a Congelar")]
    public PlayerMovement playerMovement; 
    public GameObject hudCanvas;         

    void Start()
    {
        duracion = (float)Cine.duration;

        if (playerMovement != null) playerMovement.enabled = false;
        if (hudCanvas != null) hudCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (CineObj != null) CineObj.SetActive(true);
    }

    void Update()
    {
        t += Time.deltaTime;

        if (t >= duracion)
        {
            FinalizarCinematica();
        }
    }

    void FinalizarCinematica()
    {
        if (CineObj != null) CineObj.SetActive(false);

        // 3. Activamos el gameplay y liberamos al jugador
        if (GameplayObj != null) GameplayObj.SetActive(true);
        if (playerMovement != null) playerMovement.enabled = true;
        if (hudCanvas != null) hudCanvas.SetActive(true);

        Debug.Log("Cinemática terminada: Partida iniciada.");

        Destroy(this);
    }
}