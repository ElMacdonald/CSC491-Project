using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CurrencyHolderTemp : MonoBehaviour
{
    public int currentCurrency = 0;
    public TextMeshProUGUI currencyDisplay;
    public bool purchased;
    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currencyDisplay.text = currentCurrency.ToString();
    }

   

    public bool Spend(int cost)
    {
        if(currentCurrency >= cost)
        {
            currentCurrency -= cost;
            return true;
        }
        return false;
    }

    public void Gain(int cost)
    {
        currentCurrency += cost;
    }
}
