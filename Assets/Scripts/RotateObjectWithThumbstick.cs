using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RotateObjectWithThumbstick : MonoBehaviour
{
    public XRController controller;
    public Transform objectToRotate;
    public float rotationSpeed = 45.0f;
    public bool verticalrotation = true;
    

    private void Update()
    {
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstickValue))
        {
            if (verticalrotation == false)
            {
                //float rotationAmount = thumbstickValue.x * rotationSpeed * Time.deltaTime;
                if (thumbstickValue.x > 0.5)
                {
                    float rotationAmount = 25.0f;
                    objectToRotate.Rotate(Vector3.up, rotationAmount);
                }
                if (thumbstickValue.x < -0.5)
                {
                    float rotationAmount = 25.0f;
                    objectToRotate.Rotate(Vector3.down, rotationAmount);
                }
            }
            else
            {
                  if (thumbstickValue.y > 0.5)
                {
                //float verticalRotationAmount = thumbstickValue.y * rotationSpeed * Time.deltaTime;
                     float verticalRotationAmount = 25.0f;

                     objectToRotate.Rotate(Vector3.forward, verticalRotationAmount);
                 }
                  if(thumbstickValue.y < -0.5)
                {
                    float verticalRotationAmount = 25.0f;

                    objectToRotate.Rotate(Vector3.back, verticalRotationAmount);
                }
            }
            //  if (thumbstickValue.y > 0.)
            // {
            //float verticalRotationAmount = thumbstickValue.y * rotationSpeed * Time.deltaTime;
            ////      float verticalRotationAmount = 25.0f;

            //      objectToRotate.Rotate(Vector3.right, verticalRotationAmount);
            //  }
        }
    }
}
