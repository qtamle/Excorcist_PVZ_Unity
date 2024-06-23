using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletController : MonoBehaviour
{
    public float BulletSpeed = 10f;
    public int damage = 100;
    public bool isFrozen = false; 
    public bool fromBomb = false;


    private AudioSource audioSource;
    public AudioClip lighting;
    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * BulletSpeed;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ghost") || collision.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            Zombie ghost = collision.gameObject.GetComponent<Zombie>();
            if (ghost != null)
            {
                ghost.Hit(damage, isFrozen, fromBomb);
                if (audioSource != null && lighting != null)
                {
                    audioSource.PlayOneShot(lighting);
                }
            }
        }
        Destroy(gameObject, 10);
    }
}
