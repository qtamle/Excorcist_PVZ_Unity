using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantSlot : MonoBehaviour
{
    public Sprite plantSprite;
    public GameObject plantObject;
    public Image icon;
    public int price;
    public int coolDown;
    public TextMeshProUGUI priceText;
    public Image cooldownOverlay;

    private Gamemanager gms;
    private bool isCoolingDown = false;
    private Coroutine coroutine;

    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<Gamemanager>();
        GetComponent<Button>().onClick.AddListener(BuyPlant);
        OnValidate();
    }

    private void BuyPlant()
    {
        if (isCoolingDown)
        {
            Debug.Log("Đang cool down vui lòng chờ");
            return;
        }

        if (gms.suns >= price && !gms.currentPlant)
        {
            gms.suns -= price;
            gms.BuyPlant(plantObject, plantSprite);
            if (coolDown > 0)
            {
                isCoolingDown = true;
                coroutine = StartCoroutine(CoolDownTimer());
            }
        }
        else
        {
            Debug.Log("Không đủ suns hoặc có plant đang được chọn");
        }
    }

    private IEnumerator CoolDownTimer()
    {
        float remainingCoolDown = coolDown;
        cooldownOverlay.fillAmount = 1f; 

        while (remainingCoolDown > 0)
        {
            remainingCoolDown -= Time.deltaTime;
            float fill = remainingCoolDown / coolDown; 
            cooldownOverlay.fillAmount = fill;
            //priceText.text = $"Cooldown: {Mathf.CeilToInt(remainingCoolDown)}";
            yield return null;
        }

        cooldownOverlay.fillAmount = 0f;
        priceText.text = price.ToString();
        isCoolingDown = false;
    }

    private void OnValidate()
    {
        if (plantSprite)
        {
            icon.enabled = true;
            icon.sprite = plantSprite;
            if (!isCoolingDown)
            {
                priceText.text = price.ToString();
            }
        }
        else
        {
            icon.enabled = false;
        }
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}
