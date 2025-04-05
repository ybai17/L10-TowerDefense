using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    public float panSpeed = 20f;

    public float paddingThickness = 20f;

    public float scrollSpeed = 2000f;

    Vector2 panLimitX = new Vector2(-20, 20);
    Vector2 panLimitZ = new Vector2(-20, 20);

    Vector2 zoomLimitY = new Vector2(5, 20);

    bool controlCamera = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            controlCamera = !controlCamera;

        if (!controlCamera)
            return;

        Vector3 pos = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - paddingThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= paddingThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - paddingThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= paddingThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        //scroll to zoom in/out
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * Time.deltaTime * scrollSpeed;

        pos.x = Mathf.Clamp(pos.x, panLimitX.x, panLimitX.y);
        pos.z = Mathf.Clamp(pos.z, panLimitZ.x, panLimitZ.y);
        pos.y = Mathf.Clamp(pos.y, zoomLimitY.x, zoomLimitY.y);

        transform.position = pos;
    }
}
