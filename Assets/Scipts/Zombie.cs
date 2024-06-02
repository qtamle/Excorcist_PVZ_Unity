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

    private void Start()
    {
        health = type.health;
        range = type.range;
        speed = type.speed;
        damage = type.damage;
        eatCooldown = type.eatCooldown;

        GetComponent<SpriteRenderer>().sprite = type.sprite;

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
            transform.position -= new Vector3 (speed, 0,0);
    }

    public void Hit(int damage, bool freeze)
    {
        health -= damage;

        if (freeze)
        {
            Freeze();
        }
        if (health <= 0)
        {
            GetComponent<SpriteRenderer>().sprite = type.deathSprite;
            Destroy(gameObject, 1);

        }
    }

    void Freeze()
    {
        CancelInvoke("UnFreeze");

        GetComponent<SpriteRenderer>().color = Color.blue;

        speed = type.speed / 2;
        
        Invoke("UnFreeze", 5);
    
    }

    void UnFreeze()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        speed = type.speed;
    
    }
}
