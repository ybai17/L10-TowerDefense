using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    public Material highlightMaterial;
    Material originalMaterial;
    Renderer renderer;

    public GameObject towerPrefab;
    
    GameObject tileTower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
        tileTower = null;
    }

    void OnMouseOver()
    {
        if (!TowerBuilder.Instance.HasTowerSelected())
            return;
        
        if (highlightMaterial)
            HighlightTile();
    }

    void OnMouseExit()
    {
        if (!TowerBuilder.Instance.HasTowerSelected())
            return;

        if (!tileTower)
        {
            if (originalMaterial)
                renderer.sharedMaterial = originalMaterial;
        }
    }

    void OnMouseDown()
    {
        if (!tileTower)
        {
            if (TowerBuilder.Instance.HasTowerSelected())
            {
                GameObject towerPrefab = TowerBuilder.Instance.GetSelectedTower();

                var tower = Instantiate(towerPrefab, transform.parent.position, transform.parent.rotation);
                tileTower = tower;

                TowerBuilder.Instance.ClearSelection();
            }
        }
    }

    void HighlightTile()
    {
        renderer.sharedMaterial = highlightMaterial;
    }
}
