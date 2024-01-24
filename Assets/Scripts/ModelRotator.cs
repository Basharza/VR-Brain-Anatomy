using UnityEngine;

public class ModelRotator : MonoBehaviour
{
    public float rotationSpeed = 2.0f;

    private bool isRotating = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
           
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
           
            isRotating = false;
        }

        if (isRotating)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            transform.Rotate(Vector3.up, -mouseDelta.x * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, mouseDelta.y * rotationSpeed, Space.World);
            lastMousePosition = Input.mousePosition;
        }
    }
}
