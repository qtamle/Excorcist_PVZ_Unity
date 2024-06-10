using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public int damage;
    public float explosionRadius = 2f; // Bán kính của vụ nổ
    public float speed = 0.8f;
    public bool freeze;

    private void Update()
    {
        transform.position += new Vector3(speed * Time.fixedDeltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            Explode(zombie); // Kích hoạt hàm vụ nổ khi bắn trúng zombie
            Destroy(gameObject);
        }
    }

    void Explode(Zombie target)
    {
        // Tìm tất cả các zombie trong bán kính nổ
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        // Duyệt qua từng zombie trong bán kính nổ và gây sát thương cho chúng
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<Zombie>(out Zombie zombie))
            {
                zombie.Hit(damage, freeze);
            }
        }
    }
}
