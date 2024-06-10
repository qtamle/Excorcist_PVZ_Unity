using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutCanon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float cooldownTime = 5f; // Thời gian cooldown
    private bool canShoot = true; // Biến để kiểm tra có thể bắn hay không
    public LayerMask targetLayer; // LayerMask cho đối tượng mà đạn sẽ va chạm và bị hủy

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Shoot();
                StartCoroutine(Cooldown());
            }
        }
    }

    IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldownTime);
        canShoot = true;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(10f, 0f); // Bắn đạn về phía bên phải
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == targetLayer)
        {
            Destroy(collision.gameObject); // Hủy đối tượng khi va chạm với LayerMask được chỉ định
        }
    }
}
