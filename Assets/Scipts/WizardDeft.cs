using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardDeft : MonoBehaviour
{
    private Gamemanager gameManager;

    public LayerMask deftMask;
    public float range;

    private Animator animator;
    private GameObject target;
    private AudioSource audioSource;

    public AudioClip windSound;

    private bool isReadySoundPlayed = false;

    private void Start()
    {
        gameObject.layer = 9;
        gameManager = FindObjectOfType<Gamemanager>();
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, deftMask);

        if (hit.collider != null)
        {
            target = hit.collider.gameObject;
            Stop(false);
            Ready(true);
            if (target != null)
            {
                Idle(false);
                Dance(true);
            }
        }
        else
        {
            Stop(true);
            Idle(true);
            Ready(false);
        }
    }

    private void Idle(bool idle)
    {
        if (animator != null)
        {
            animator.SetBool("idle", idle);
            if (idle)
            {
                Dance(false);
            }
        }
    }

    private void Ready(bool ready)
    {
        if (animator != null)
        {
            animator.SetBool("ready", ready);
            if (ready && !isReadySoundPlayed)
            {
                PlayReadySound();
                isReadySoundPlayed = true;
            }
            else if (!ready)
            {
                isReadySoundPlayed = false;
            }
        }
    }

    private void Dance(bool dance)
    {
        if (animator != null)
        {
            animator.SetBool("dance", dance);
        }
    }

    private void Stop(bool stop)
    {
        if (animator != null)
        {
            animator.SetBool("stop", stop);
        }
    }

    private void PlayReadySound()
    {
        if (audioSource != null && windSound != null)
        {
            audioSource.PlayOneShot(windSound);
        }
    }
}
