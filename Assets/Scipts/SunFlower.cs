using System.Collections;
using UnityEngine;

public class SunFlower : MonoBehaviour
{
    public GameObject sunObject;
    public float cooldown;

    private Animator animator;
    private AudioSource audioSource;
    public AudioClip farmSound;
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("SpawnSpirit", cooldown, cooldown);
    }

    void SpawnSpirit()
    {
        animator.SetBool("spawn", true);
        if (audioSource != null && farmSound != null)
        {
            audioSource.PlayOneShot(farmSound);
        }
        StartCoroutine(SpawnSunCoroutine());
    }

    IEnumerator SpawnSunCoroutine()
    {
        yield return new WaitForSeconds(1f);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + Random.Range(-0.5f, 0.5f),
            transform.position.y + Random.Range(0f, 0f),
            0
        );
        GameObject mySun = Instantiate(sunObject, spawnPosition, Quaternion.identity);
        mySun.GetComponent<Sun>().dropToYpos = transform.position.y - 1;

        animator.SetBool("idle", true);
        animator.SetBool("spawn", false);
    }
}
