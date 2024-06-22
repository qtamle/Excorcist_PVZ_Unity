using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieTypes type;
    public float speed;
    public float health;
    public float range;
    public float damage;
    public float eatCooldown;

    private bool canEat = true;
    public Plant targetPlant;
    public LayerMask plantMask;
    public AudioClip snowAudio;

    private AudioSource audioSource;
    private bool isFrozen = false;

    public delegate void ZombieKilledHandler(bool killedByBomb, Vector3 position);
    public event ZombieKilledHandler OnZombieKilled;

    public GameObject sunPrefab; // Thêm tham chiếu tới prefab của Sun
    private Color originalColor;

    private Color hitColor;

    public bool EnhancedGhost = false;

    public bool isRugbyGhost = false;

    private bool hasDestroyedTarget = false;

    private void Start()
    {
        health = type.health;
        range = type.range;
        speed = type.speed;
        damage = type.damage;
        eatCooldown = type.eatCooldown;

        GetComponent<SpriteRenderer>().sprite = type.sprite;
        audioSource = GetComponent<AudioSource>();
        originalColor = GetComponent<SpriteRenderer>().color;

        hitColor = new Color(241f / 255f, 117f / 255f, 134f / 255f);

        if (isRugbyGhost)
        {
            speed = 0.05f; 
            damage = 100f; 
        }
        else
        {
            speed = type.speed;
        }

    }
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, plantMask);

        if (hit.collider && !hasDestroyedTarget)
        {
            hasDestroyedTarget = true;
            targetPlant = hit.collider.GetComponent<Plant>();
            if (targetPlant != null)
            {
                targetPlant.Hit(damage);
            }
            if (health > 0)
            {
                speed = 0.01f;
                damage = 2f;
            }
        }

        if (EnhancedGhost && health <= 10)
        {
            damage *= 1.5f;
            speed *= 1.5f;
            EnhancedGhost = false;
        }
    }


    void Eat()
    {
        if (!canEat || !targetPlant)
            return;
        canEat = false;
        Invoke("ResetEatCooldown", eatCooldown);
        targetPlant.Hit(damage);
    }

    void ResetEatCooldown()
    {
        canEat = true;
    }

    private void FixedUpdate()
    {
        if (!targetPlant)
            transform.position -= new Vector3(speed, 0, 0);
    }

    public void Hit(float damage, bool freeze, bool fromBomb = false)
    {
        health -= damage;
        if (!freeze && !isFrozen)
        {
            StartCoroutine(ChangeColorGhost());
        }
        if (freeze && !isFrozen && audioSource != null && snowAudio != null)
        {
            audioSource.PlayOneShot(snowAudio);
            Freeze();
        }
        if (health <= 10 && !EnhancedGhost)
        {
            EnhancedGhost = true;
        }
        if (health <= 0)
        {
            GetComponent<SpriteRenderer>().sprite = type.deathSprite;
            if (OnZombieKilled != null)
            {
                OnZombieKilled.Invoke(fromBomb, transform.position);
            }
            Destroy(gameObject, 0.5f);
        }
    }


    private IEnumerator ChangeColorGhost()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = isFrozen ? Color.blue : originalColor;
    }

    void Freeze()
    {
        CancelInvoke("UnFreeze");

        GetComponent<SpriteRenderer>().color = Color.blue;
        isFrozen = true;

        if (isRugbyGhost)
        {
            speed = 0.01f;
        }
        else if (!EnhancedGhost || health > 10)
        {
            speed = type.speed / 2;
        }

        Invoke("UnFreeze", 8);
    }

    void UnFreeze()
    {
        GetComponent<SpriteRenderer>().color = originalColor;
        isFrozen = false;

        if (isRugbyGhost)
        {
            speed = 0.05f;
        }
        else if (EnhancedGhost && health > 0 && health <= 10)
        {
            speed = type.speed * 1.5f;
        }
        else
        {
            speed = type.speed;
        }
    }

}
