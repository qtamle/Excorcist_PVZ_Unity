using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public int damage;
    public float explosionRadius = 2f; // Bán kính của vụ nổ
    public float speed = 0.8f;
    public bool freeze;
    public GameObject explosionPrefab;
    public GameObject audioSourcePrefab; 
    public AudioClip hitSound;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (audioSourcePrefab == null)
        {
            Debug.LogError("AudioSource prefab chưa được gán.");
        }
    }

    private void Update()
    {
        transform.position += new Vector3(speed * Time.fixedDeltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            Explode(zombie);
            PlayHitSound(); 
            Destroy(gameObject);
        }
    }

    void Explode(Zombie target)
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, target.transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
        }

        // Tìm tất cả các zombie trong bán kính nổ
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        // Duyệt qua từng zombie trong bán kính nổ và gây sát thương cho chúng
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<Zombie>(out Zombie zombie))
            {
                zombie.Hit(damage, freeze);
            }
        }
    }

    void PlayHitSound()
    {
        if (hitSound != null && audioSourcePrefab != null)
        {
            GameObject audioObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(hitSound);
            Destroy(audioObject, hitSound.length); // Hủy đối tượng sau khi âm thanh đã phát xong
        }
    }
}
