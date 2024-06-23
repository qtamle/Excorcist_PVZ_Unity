using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float health;
    private Gamemanager gameManager;
    private Color originalColor;

    private void Start()
    {
        gameObject.layer = 9;
        gameManager = FindObjectOfType<Gamemanager>();
        originalColor = GetComponent<SpriteRenderer>().color;
    }

    public void Hit(float damage)
    {
        health -= damage;
        StartCoroutine(ChangeColorWizard());
        if (health <= 0)
        {
            if (gameManager != null)
            {
                gameManager.NotifyPlantRemoved(transform.position);
            }
            Destroy(gameObject);
        }
    }
    private IEnumerator ChangeColorWizard()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
}
