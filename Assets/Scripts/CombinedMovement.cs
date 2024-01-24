using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CombinedMovement : MonoBehaviour
{
    public enum MovementMethod { Thumbstick, Headset };
    public MovementMethod movementMethod = MovementMethod.Thumbstick;
    public float thumbstickSpeed = 1.0f; // Movement speed for thumbstick-based movement
    public float headsetSpeed = 1.0f; // Movement speed for headset-based movement
    public XRController leftController; // Reference to the left-hand controller
    public XRController rightController; // Reference to the right-hand controller

    private void Update()
    {
        Vector3 movementDirection = Vector3.zero;

        if (movementMethod == MovementMethod.Thumbstick)
        {
            // Thumbstick-based movement
            Vector2 thumbstickValue = GetThumbstickInput(leftController.inputDevice);
            movementDirection = new Vector3(thumbstickValue.x, 0f, thumbstickValue.y) * thumbstickSpeed;
        }
        else if (movementMethod == MovementMethod.Headset)
        {
            // Headset-based movement
            Vector3 headsetForward = InputTracking.GetLocalPosition(XRNode.Head) + InputTracking.GetLocalRotation(XRNode.Head) * Vector3.forward;
            movementDirection = new Vector3(headsetForward.x, 0f, headsetForward.z).normalized * headsetSpeed;
        }

        // Apply movement to the XR Rig
        transform.Translate(movementDirection * Time.deltaTime);
    }

    private Vector2 GetThumbstickInput(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstickValue))
        {
            return thumbstickValue;
        }
        return Vector2.zero;
    }
}
