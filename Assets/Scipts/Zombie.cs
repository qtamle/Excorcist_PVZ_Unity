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
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, plantMask);

        if (hit.collider)
        {
            targetPlant = hit.collider.GetComponent<Plant>();
            Eat();
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
        speed = type.speed / 2;
        Invoke("UnFreeze", 8);
    }

    void UnFreeze()
    {
        GetComponent<SpriteRenderer>().color = originalColor;
        speed = type.speed;
        isFrozen = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem va chạm với lá bùa không
        if (collision.CompareTag("LuaBua"))
        {
            Talisman luaBua = collision.GetComponent<Talisman>();
            if (luaBua != null)
            {
                luaBua.FireBullet();
            }
        }
    }

}