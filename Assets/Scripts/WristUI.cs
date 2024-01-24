using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristUI : MonoBehaviour
{
    public InputActionAsset inputActions;

    private Canvas _wristUICanvas;
    public GameObject SecondCam;
    public InteractionHandler InteractionHandler;
    public RotateObjectWithThumbstick rotate;

    private InputAction _menu;
    private InputAction _cam;
    private InputAction _reset;
    private InputAction _rotate;
    private void Start()
    {
        _wristUICanvas = GetComponent<Canvas>();
        _menu = inputActions.FindActionMap("XRI RightHand").FindAction("Menu");
        _menu.Enable();
        _menu.performed += ToggleMenu;

        
        _cam = inputActions.FindActionMap("XRI RightHand").FindAction("CAM2");
        _cam.Enable();
        _cam.performed += ToggleCam;

        _rotate = inputActions.FindActionMap("XRI LeftHand").FindAction("Rotate");
        _rotate.Enable();
        _rotate.performed += Togglerotate;

        _reset = inputActions.FindActionMap("XRI RightHand").FindAction("Reset");
        _reset.Enable();
        _reset.performed += ToggleReset;
    }

    private void OnDestroy()
    {
        _menu.performed -= ToggleMenu;
        _cam.performed -= ToggleCam;
        _reset.performed -= ToggleReset;
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        _wristUICanvas.enabled = !_wristUICanvas.enabled;
    }
    public void ToggleCam(InputAction.CallbackContext context)
    {
        SecondCam.SetActive(!SecondCam.activeSelf);
    }
    public void Togglerotate(InputAction.CallbackContext context)
    {
        rotate.verticalrotation = !rotate.verticalrotation;
        //rotate.verticalrotation = rotate.verticalrotation == true ? false : true;
    }
    public void ToggleReset(InputAction.CallbackContext context)
    {
        InteractionHandler.VolReset();
    }

}
