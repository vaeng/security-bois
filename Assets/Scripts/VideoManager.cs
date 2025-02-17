using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public float splashDuration = 5f;

    void Start()
    {
        // Video auf Loop setzen
        videoPlayer.isLooping = false;
        
        // Startet den Coroutine zum Wechseln nach 8 Sekunden
        StartCoroutine(WaitAndLoadScene());
    }

    IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(splashDuration); // Wartet 8 Sekunden
        SceneManager.LoadScene("mainMenu"); // Wechselt zur MainMenu-Szene
    }
}