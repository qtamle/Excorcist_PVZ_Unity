using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Gift : MonoBehaviour
{
    private Audio audioManager;
    private AudioSource audioSource;
    public AudioClip soundWin;
    public ParticleSystem fireworksEffect; // Thêm trường này để tham chiếu đến Particle System

    private void Start()
    {
        // Lấy tham chiếu đến AudioManager từ SceneManager
        audioManager = FindObjectOfType<Audio>();
        audioSource = GetComponent<AudioSource>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }

        // Lấy tham chiếu đến Particle System nếu chưa có
        if (fireworksEffect == null)
        {
            fireworksEffect = GetComponent<ParticleSystem>();
        }

        // Tắt Particle System ban đầu
        if (fireworksEffect != null)
        {
            fireworksEffect.Stop();
        }
    }

    private void OnMouseDown()
    {
        StartCoroutine(LoadMainMenuAfterDelay(5f)); // Load scene MainMenu sau 5 giây

        if (audioSource != null && soundWin != null)
        {
            audioSource.PlayOneShot(soundWin);
        }

        // Đổi màu sắc của Particle System thành màu đỏ và kích hoạt nó
        if (fireworksEffect != null)
        {
            var mainModule = fireworksEffect.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.white);

            fireworksEffect.Play();
        }
    }

    IEnumerator LoadMainMenuAfterDelay(float delay)
    {
        // Dừng âm thanh từ AudioManager
        if (audioManager != null)
        {
            audioManager.StopMusic();
        }
        else
        {
            Debug.LogError("AudioManager reference is null!");
        }

        yield return new WaitForSeconds(delay);

        // Load scene MainMenu
        SceneManager.LoadScene("MainMenu");
    }
}
