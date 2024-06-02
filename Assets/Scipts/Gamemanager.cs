using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gamemanager : MonoBehaviour
{
    public GameObject currentPlant;
    public Sprite currentPlantSprite;
    public Transform tiles;
    public LayerMask tileMask;
    private Tile tileComponent; // Thêm biến này để lưu trữ Tile hiện tại

    public int suns;

    public TextMeshProUGUI sunText;

    public LayerMask sunMask;

    public void BuyPlant(GameObject plant, Sprite sprite)
    {
        currentPlant = plant;
        currentPlantSprite = sprite;
    }

    private void Update()
    {
        sunText.text = suns.ToString();

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);

        foreach (Transform tile in tiles)
            tile.GetComponent<SpriteRenderer>().enabled = false;

        if (hit.collider && currentPlant)
        {
            Tile tileComponent = hit.collider.GetComponent<Tile>(); // Lấy component Tile từ collider
            if (tileComponent != null && !tileComponent.HasPlant()) // Kiểm tra xem ô có cây không
            {
                hit.collider.GetComponent<SpriteRenderer>().sprite = currentPlantSprite;
                hit.collider.GetComponent<SpriteRenderer>().enabled = true;

                if (Input.GetMouseButton(0))
                {
                    Instantiate(currentPlant, hit.collider.transform.position, Quaternion.identity);
                    tileComponent.SetHasPlant(true); // Cập nhật trạng thái có cây của ô
                    currentPlant = null;
                    currentPlantSprite = null;
                }
            }
        }
        RaycastHit2D sunHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, sunMask);

        if (sunHit.collider)
        {
            if (Input.GetMouseButtonDown(0))
            {
                suns += 25;
                Destroy(sunHit.collider.gameObject);
            }
        }
    }

    // Thêm phương thức để cập nhật trạng thái hasPlant của ô
    public void RemovePlantFromTile(Tile tile)
    {
        if (tile != null)
        {
            tile.SetHasPlant(false);
            Debug.Log("Đã cập nhật trạng thái ô không có cây");
        }
    }

    // Phương thức để nhận thông báo từ ShovelRemove
    public void NotifyPlantRemoved(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, tileMask);
        if (hit.collider)
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile != null)
            {
                tile.SetHasPlant(false);
            }
        }
    }
}
