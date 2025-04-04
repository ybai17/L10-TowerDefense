using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public GameObject[] towers;

    int selectedTowerIndex;
    bool selectedTower = false;

    public static TowerBuilder Instance { get; private set;}

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            //need to ensure there is only 1 instance, a la the singleton pattern
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // SelectTower(1);
    }

    public void SelectTower(int index)
    {
        if (index >= 0 && index < towers.Length)
        {
            selectedTowerIndex = index;
            selectedTower = true;
        }
        else 
        {
            Debug.LogWarning("Invalid tower index");
            selectedTower = false;
        }
    }

    public GameObject GetSelectedTower()
    {
        return towers[selectedTowerIndex];
    }

    public bool HasTowerSelected()
    {
        return selectedTower;
    }

    public void ClearSelection()
    {
        selectedTower = false;
        selectedTowerIndex = -100;
    }
}
