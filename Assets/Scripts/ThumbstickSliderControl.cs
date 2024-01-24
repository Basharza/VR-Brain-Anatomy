using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.UI;
 
public class ThumbstickSliderControl : MonoBehaviour
{
    public Slider slider;
    private InputAction thumbstickAction;
    public InputActionAsset inputActions;
    [SerializeField] private float sensitivity = 0.1f;

    private void OnEnable()
    {
        thumbstickAction = inputActions.FindActionMap("XRI RightHand").FindAction("ThumbstickMovement");
        thumbstickAction.Enable();
    }

    private void OnDisable()
    {
        thumbstickAction.Disable();
    }

    private void Update()
    {
        Vector2 thumbstickValue = thumbstickAction.ReadValue<Vector2>();
        float horizontalInput = thumbstickValue.x;

        float currentValue = slider.value;
        slider.value = Mathf.Clamp01(currentValue + horizontalInput * sensitivity);
    }
}
