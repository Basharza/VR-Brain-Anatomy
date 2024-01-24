using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class InteractionController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private RayMarching RayMarchingScript; // Ensure this is your correct class

    [SerializeField]
    private LayerMask sliceInteractionLayer;
    [SerializeField]
    private Transform slicePlanceTransform;

    [SerializeField]
    private XRController controller; // Reference to your XR Controller

    [SerializeField]
    private float slicingSpeed = 0.1f;

    private bool isPressed = false;
    private float initialControllerPosition;
    public TextMeshProUGUI log;
    public GameObject VrLeftController;

    void Update()
    {
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed))
        {
            if (pressed && !isPressed)
            {
                isPressed = true;
                initialControllerPosition = controller.transform.position.y;
              //  log.text = "pressed";

            }
            else if (!pressed && isPressed)
            {
              //  log.text = "pressed-1";
                isPressed = false;
              //  VrLeftController.GetComponent<LineRenderer>().enabled = false;
               // VrLeftController.GetComponent<XRInteractorLineVisual>().enabled = false;
            }
        }

        if (isPressed)
        {
           // log.text = "pressed0";
            if (slicePlanceTransform == null)
                return;

            float controllerDelta = controller.transform.position.y - initialControllerPosition;
         //   log.text = "pressed1";
            Ray ray = new Ray(controller.transform.position, controller.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
            {
             //   log.text = "pressed2";
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
             //   log.text = "pressed3";
             //   VrLeftController.GetComponent<LineRenderer>().enabled = true;
             //   VrLeftController.GetComponent<XRInteractorLineVisual>().enabled = true;
                slicePlanceTransform.position = new Vector3(slicePlanceTransform.position.x, hit.point.y, slicePlanceTransform.position.z);
            }

            float slicingDepth = -controllerDelta * slicingSpeed;
            initialControllerPosition = controller.transform.position.y;
        }
    }


    public void setPlane(Transform currentPlane)
    {
        slicePlanceTransform = currentPlane;
    }
}