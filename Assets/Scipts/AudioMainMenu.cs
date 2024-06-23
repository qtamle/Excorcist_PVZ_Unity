using UnityEngine;

public class AudioMainMenu : MonoBehaviour
{
    public AudioSource music;
    public AudioSource vfx;

    public AudioClip musicMain;
    public AudioClip sfxButton;

    private void Start()
    {
        music.clip = musicMain;
        music.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        vfx.clip = sfxClip;
        vfx.PlayOneShot(sfxClip);
    }

    public void StopMusic()
    {
        music.Stop();
    }
}
