using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooter : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootOrigin;
    public float cooldown;

    private bool canShoot;

    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private Animator animator;
    private void Start()
    {
        Invoke("ResetCooldown", cooldown);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
        if (hit.collider)
        {
            target = hit.collider.gameObject;

            SetShootAnimation(true);
            Shoot();
            
        }
        else
        {
            SetShootAnimation(false);
        }

    }

    void ResetCooldown()
    {
        canShoot = true;
    }

    void Shoot()
    {
        if (!canShoot)
        {
            return;
        }
        canShoot = false;
        Invoke("ResetCooldown", cooldown);
        GameObject myBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
    }

    void SetShootAnimation(bool isShooting)
    {
        if (animator != null)
        {
            animator.SetBool("Shoot", isShooting);
        }
    }
}
