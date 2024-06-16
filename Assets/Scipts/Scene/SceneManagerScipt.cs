using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScipt : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Map 1");
    }
}