using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float health;
    private Gamemanager gameManager;

    private void Start()
    {
        gameObject.layer = 9;
        gameManager = FindObjectOfType<Gamemanager>(); 
    }

    public void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (gameManager != null)
            {
                gameManager.NotifyPlantRemoved(transform.position);
            }
            Destroy(gameObject);
        }
    }
}
