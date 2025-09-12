using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform target;   // assign your main player camera here
    public float height = 2f;  // keep marker floating above

    void Update()
    {
        if (target != null)
        {
            // follow X/Z (room position), keep fixed height
            transform.position = new Vector3(target.position.x + 9f, height, target.position.z);
        }
    }
}