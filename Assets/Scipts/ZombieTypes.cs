using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New ZombieType", menuName ="Zombie")]

public class ZombieTypes : ScriptableObject
{
    public float health;

    public float damage;

    public Sprite sprite;

    public Sprite deathSprite;

    public float range = .5f;

    public float eatCooldown = 1f;

    public float speed;
}
