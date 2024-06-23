using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public float bulletSpeed = 10f;

    private int bulletsShot = 0;
    private int maxBullets = 2;

    private Coroutine scaleCoroutine;
    public void FireBullet()
    {
        if (bulletsShot < maxBullets)
        {
            // Tạo một viên đạn từ prefab tại vị trí của lá bùa
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Đặt vận tốc cho đạn (xuyên qua tất cả)
                rb.velocity = transform.right * bulletSpeed;
            }

            bulletsShot++;

            // Kiểm tra nếu đã bắn đủ số viên đạn cần thiết
            if (bulletsShot >= maxBullets)
            {
                // Hủy bỏ lá bùa sau khi bắn đủ viên đạn
                Destroy(gameObject);
            }
        }
    }
}
