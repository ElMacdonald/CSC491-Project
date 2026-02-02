using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenuBhv : MonoBehaviour
{
    public int price;
    public bool purchased;
    public Sprite purchaseSprite;
    public string itemType;
    // Start is called before the first frame update
    void Start()
    {
        MakePriceText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakePurchase(int cost)
    {
        CurrencyHolderTemp ch = FindObjectOfType<CurrencyHolderTemp>();
        if(!purchased)
        {
            if(ch.Spend(cost))
            {
                purchased = true;
                Destroy(this.transform.Find("PriceText").gameObject);
            }
        }

        if (purchased)
        {
            Equip();
        }
    }

    void Equip()
    {
        Debug.Log("Equipped " + itemType);
        if(itemType == "hat")
        {
            GameObject.Find("Hat").GetComponent<Image>().sprite = purchaseSprite;
            // reset transparency to be visible
            Color hatColor = GameObject.Find("Hat").GetComponent<Image>().color;
            hatColor.a = 1f;
            GameObject.Find("Hat").GetComponent<Image>().color = hatColor;
        }else if(itemType == "shirt")
        {
            GameObject.Find("Body").GetComponent<Image>().sprite = purchaseSprite;
            Color bodyColor = GameObject.Find("Body").GetComponent<Image>().color;
            bodyColor.a = 1f;
            GameObject.Find("Body").GetComponent<Image>().color = bodyColor;
        }else if(itemType == "pants")
        {
            GameObject.Find("Legs").GetComponent<Image>().sprite = purchaseSprite;
            Color legsColor = GameObject.Find("Legs").GetComponent<Image>().color;
            legsColor.a = 1f;
            GameObject.Find("Legs").GetComponent<Image>().color = legsColor;
        }
    }

     // Instantiates price text on top of button if haven't purchased
     // Adds a colored background so text is visible
    void MakePriceText()
    {
        if(!purchased)
        {
            GameObject priceTextObj = new GameObject("PriceText");
            priceTextObj.transform.SetParent(this.transform);
            priceTextObj.AddComponent<RectTransform>();
            priceTextObj.AddComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = priceTextObj.GetComponent<TextMeshProUGUI>();
            priceText.text = price.ToString();
            priceText.fontSize = 36;
            priceText.alignment = TextAlignmentOptions.Center;
            RectTransform rt = priceTextObj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 50);
            rt.anchoredPosition = Vector2.zero;

            // Add background
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(priceTextObj.transform);
            bgObj.AddComponent<RectTransform>();
            bgObj.AddComponent<Image>();
            Image bgImage = bgObj.GetComponent<Image>();
            bgImage.color = new Color(0f, 0f, 0f, 0.75f); // semi-transparent black
            RectTransform bgRt = bgObj.GetComponent<RectTransform>();
            bgRt.sizeDelta = new Vector2(100, 50);
            bgRt.anchoredPosition = Vector2.zero;
        }
    }
}
