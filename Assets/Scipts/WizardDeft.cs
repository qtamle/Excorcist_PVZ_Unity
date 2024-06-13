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

    private void Start()
    {
        gameObject.layer = 9;
        gameManager = FindObjectOfType<Gamemanager>();
        animator = gameObject.GetComponent<Animator>();
    }


    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, deftMask);

        if (hit.collider)
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
        }

    }

    private void Idle (bool idle)
    {
        if (animator != null)
        {
            animator.SetBool("idle", idle);
            if (idle)
            {
                Ready(false);
                Dance(false);
            }
        }
    }

    private void Ready (bool ready)
    {
        if (animator != null)
        {
            animator.SetBool("ready", ready);
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
}
