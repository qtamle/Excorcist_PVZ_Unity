using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnButtonClick : MonoBehaviour
{
    public string sceneName;
    public AudioMainMenu audioMainMenu;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAndPlaySFX(string sceneName, AudioClip sfxClip)
    {
        audioMainMenu.StopMusic();
        audioMainMenu.PlaySFX(sfxClip);
        SceneManager.LoadScene(sceneName);
    }
}
