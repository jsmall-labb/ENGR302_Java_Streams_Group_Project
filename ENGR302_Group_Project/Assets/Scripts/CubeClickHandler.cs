using UnityEngine;
using UnityEngine.InputSystem;

public class CubeClickHandler : MonoBehaviour
{
    void Start()
    {
        // Make sure the cube has a collider for mouse detection
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"No collider found on {gameObject.name}. Adding BoxCollider for mouse detection.");
            gameObject.AddComponent<BoxCollider>();
        }
    }
    
    void Update()
    {
        // Check for mouse click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckForCubeClick();
        }
    }
    
    void CheckForCubeClick()
    {
        // Get mouse position and create a ray from camera
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        
        // Check if ray hits this cube
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // If the hit object is this cube, log the message
            if (hit.collider.gameObject == gameObject)
            {
            	// Show your menu (UNCOMMENT WHEN MERGED)
				// yourMenuGameObject.SetActive(true);
				// Debug.Log($"Menu opened by cube '{gameObject.name}'!");
                Debug.Log($"Cube '{gameObject.name}' was clicked!");
            }
        }
    }
}