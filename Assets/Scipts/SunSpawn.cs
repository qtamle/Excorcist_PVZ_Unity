using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSpawn : MonoBehaviour
{
    public GameObject sunObject;

    private void Start()
    {
        Invoke("SpawnSun", Random.Range(6, 12));
    }

    void SpawnSun()
    {
        GameObject mySun =  Instantiate(sunObject, new Vector3(Random.Range(-5.19f, 5.19f), 7, 0), Quaternion.identity);
        mySun.GetComponent<Sun>().dropToYpos = Random.Range(2f, -3f);
        Invoke("SpawnSun", Random.Range(6, 12));
    }
}
