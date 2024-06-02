using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float dropToYpos;

    private float speed = .10f;

    private void Start()
    {
        Destroy(gameObject, Random.Range(10f,20f));
    }

    private void Update()
    {
        if (transform.position.y > dropToYpos)
        {
            transform.position -= new Vector3(0, speed * Time.fixedDeltaTime, 0);
        }
    }
}
