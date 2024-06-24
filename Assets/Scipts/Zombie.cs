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

    private Color colorAngry;

    public bool Witch = false;

    public bool Rugby = true;

    public bool hasCollided = false;

    public AudioClip GhosthitSound;

    public AudioClip ScreamAudio;

    public bool ScreamSound = true;

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
        colorAngry = new Color(156f / 255f, 0f / 255f, 255f / 255f);

        if (Rugby)
        {
            damage = 100f;
            speed = 0.1f;
        }
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
        if (audioSource != null && GhosthitSound != null)
        {
            audioSource.PlayOneShot(GhosthitSound);
        }

        if (Rugby && !hasCollided)
        {
            speed = type.speed;
            damage = type.damage;

            // Đánh dấu rằng zombie đã va chạm
            hasCollided = true;
        }
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

    public void Hit(float incomingDamage, bool freeze, bool fromBomb = false)
    {
        health -= incomingDamage;

        if (!freeze && !isFrozen && !(Witch && health <= 15))
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
            OnZombieKilled?.Invoke(fromBomb, transform.position);
            Destroy(gameObject, 0.5f);
        }

        if (Witch && health > 0 && health <= 15)
        {
            GetComponent<SpriteRenderer>().color = colorAngry; // Chuyển màu sang colorAngry
            IncreaseStats(incomingDamage);
            if (audioSource != null && ScreamAudio != null && ScreamSound)
            {
                audioSource.PlayOneShot(ScreamAudio);
                ScreamSound = false;
            }
        }
    }

    private void IncreaseStats(float damageAmount)
    {
        float increaseFactor = damageAmount / 15.0f;
        speed += type.speed * increaseFactor * 2f;
        damage += type.damage * increaseFactor * 5f;
    }

    private IEnumerator ChangeColorGhost()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);

        if (!Witch || health > 15) // Chỉ thay đổi màu nếu không phải Witch hoặc máu trên 15
        {
            spriteRenderer.color = isFrozen ? Color.blue : originalColor;
        }
        else // Giữ màu colorAngry nếu là Witch và máu dưới hoặc bằng 15
        {
            spriteRenderer.color = colorAngry;
        }
    }

    void Freeze()
    {
        CancelInvoke("UnFreeze");

        GetComponent<SpriteRenderer>().color = Color.blue;
        speed = type.speed / 2;
        isFrozen = true;

        Invoke("UnFreeze", 8);
    }

    void UnFreeze()
    {
        if (Witch && health <= 15)
        {
            GetComponent<SpriteRenderer>().color = colorAngry;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = originalColor;
        }

        if (Rugby && !hasCollided)
        {
            speed = 0.1f;
        }
        else
        {
            speed = type.speed;
        }

        isFrozen = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LuaBua"))
        {
            Talisman luaBua = collision.GetComponent<Talisman>();
            luaBua?.FireBullet();
        }
    }
}
