using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooter : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootOrigin;
    public float cooldown;

    private bool canShoot = true;
    private bool isReady = false;
    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
        if (hit.collider)
        {
            target = hit.collider.gameObject;
            if (!isReady)
            {
                ReadyShot(true);
                isReady = true;
            }
            SetShootAnimation(true);
            Shoot();
        }
        else
        {
            if (isReady)
            {
                Stop(true);
                isReady = false;
            }
        }
    }

    void ResetCooldown()
    {
        canShoot = true;
    }

    void Shoot()
    {
        if (!canShoot)
            return;

        canShoot = false;
        Invoke("ResetCooldown", cooldown);
        Instantiate(bullet, shootOrigin.position, Quaternion.identity);
    }

    void SetShootAnimation(bool isShooting)
    {
        if (animator != null)
        {
            animator.SetBool("fire", isShooting);
        }
    }

    void ReadyShot(bool ready)
    {
        if (animator != null)
        {
            animator.SetBool("ready", ready);
        }
    }

    void Stop(bool stop)
    {
        if (animator != null)
        {
            animator.SetBool("stop", stop);
            if (stop)
            {
                Idle(true);
                ReadyShot(false);
                SetShootAnimation(false);
            }
            else
            {
                Idle(false);
            }
        }
    }

    void Idle(bool idle)
    {
        if (animator != null)
        {
            animator.SetBool("idle", idle);
        }
    }
}
