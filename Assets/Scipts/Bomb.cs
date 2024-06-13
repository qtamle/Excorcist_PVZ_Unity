using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float radius = 2.0f;
    public float damage = 100f;
    public float coolDown = 1.0f;
    public LayerMask targetMask;

    public GameObject sunPrefab; // Thêm tham chiếu tới prefab của Sun

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
                targetZombie.Hit(damage, false, true); // true vì bị giết bởi bomb
                targetZombie.OnZombieKilled += HandleZombieKilled;
            }
        }

        if (gameManager != null)
        {
            gameManager.NotifyPlantRemoved(transform.position);
        }

        Destroy(gameObject);
    }

    void HandleZombieKilled(bool killedByBomb, Vector3 position)
    {
        if (killedByBomb && Random.value <= 0.3f)
        {
            SpawnSun(position);
        }
    }

    void SpawnSun(Vector3 position)
    {
        GameObject mySun = Instantiate(sunPrefab, position, Quaternion.identity);
        mySun.GetComponent<Sun>().dropToYpos = position.y - 1;
    }
}
