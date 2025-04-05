using UnityEngine;
using UnityEngine.UI;
public class TowerButtonBehavior : MonoBehaviour
{
	public int towerIndex;
	
	int towerCost;
	
	Button towerButton;

	void Start()
	{
		towerButton = GetComponent<Button>();
		towerCost = TowerBuilder.Instance.GetSelectedTowerCost(towerIndex);
	}
	
	void Update()
	{
		if (!MoneyManager.Instance)
			return;
			
		if (MoneyManager.Instance.GetCurrentMoney() >= towerCost)
		{
			towerButton.interactable = true;
		}
		else
		{
			towerButton.interactable = false;
		}
	}
}