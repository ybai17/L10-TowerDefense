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
        if (highlightMaterial)
            HighlightTile();
    }

    void OnMouseExit()
    {
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
            if (towerPrefab)
            {
                var tower = Instantiate(towerPrefab, transform.parent.position, transform.parent.rotation);
                tileTower = tower;
            }
        }
    }

    void HighlightTile()
    {
        renderer.sharedMaterial = highlightMaterial;
    }
}
