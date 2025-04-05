using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{

    public static MoneyManager Instance;

    public int startingMoney = 2;
    int currentMoney;

    public TMP_Text moneyText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentMoney = startingMoney;
        UpdateMoneyText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool BuyTower(int cost)
    {  
        if (cost <= currentMoney)
        {
            currentMoney -= cost;
            UpdateMoneyText();
            return true;
        }
        
        return false;
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    public void GainMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyText();
    }

    public void UpdateMoneyText()
    {
        if (moneyText)
        {
            moneyText.text = currentMoney.ToString();
        }
    }
}
