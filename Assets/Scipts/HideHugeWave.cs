using UnityEngine;
using UnityEngine.UI;

public class HideHugeWave : MonoBehaviour
{
    public Image imageToHide;

    void Start()
    {
        // Hide the image when the game starts
        if (imageToHide != null)
        {
            imageToHide.gameObject.SetActive(false);
        }
    }
}
