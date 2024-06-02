using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float radius = 2.0f;
    public float damage = 100f;
    public float coolDown = 2.0f;
    public LayerMask targetMask;

    private Gamemanager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<Gamemanager>();
        StartCoroutine(CountdownAndExplode());
    }

    public void StartExplosionCountdown()
    {
        StartCoroutine(CountdownAndExplode());
    }

    private IEnumerator CountdownAndExplode()
    {
        yield return new WaitForSeconds(coolDown);
        Explode();
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        foreach (Collider2D collider in colliders)
        {
            Zombie targetZombie = collider.GetComponent<Zombie>();
            if (targetZombie != null)
            {
                targetZombie.Hit(damage, false);
            }
        }

        if (gameManager != null)
        {
            gameManager.NotifyPlantRemoved(transform.position);
        }

        Destroy(gameObject);
    }

}
