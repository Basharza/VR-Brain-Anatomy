using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class InteractionHandler : MonoBehaviour
{
    public InteractionController controller;
    public GameObject TOPDIAGONALUI;
    public GameObject BOTDIAGONALUI;
    public GameObject Cus1Slider;
    public GameObject Cus2Slider;
    public GameObject ZoomSlider;
    public GameObject Gobackbutton;
    public GameObject rotatebutton;
    public GameObject CAM2;
    
    

    public void setplane(string instance)
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        if (ray != null)
        {
            //top
            if (instance.Equals("Instance1"))
            {
                controller.setPlane(ray.GetPlanes().ElementAt(0));
                ray.GetPlanes().ElementAt(0).gameObject.SetActive(true);

                ray.GetPlanes().ElementAt(1).gameObject.SetActive(false);
                ray.GetPlanes().ElementAt(2).gameObject.SetActive(false);
                ray.GetPlanes().ElementAt(3).gameObject.SetActive(false);
              //  ray.GetPlanes().ElementAt(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
              //  CAM2.transform.parent = ray.GetPlanes().ElementAt(0);
                // TOPDIAGONALUI.SetActive(false);
                // BOTDIAGONALUI.SetActive(false);
                Cus1Slider.SetActive(false);
                Cus2Slider.SetActive(false);
                Debug.Log("BUTTON PRESSED");
                controller.log.text = "BUTTON PRESSED";
            }
            //bottom
            if (instance.Equals("Instance2"))
            {
                controller.setPlane(ray.GetPlanes().ElementAt(1));
                ray.GetPlanes().ElementAt(1).gameObject.SetActive(true);
                ray.GetPlanes().ElementAt(0).gameObject.SetActive(false);
                ray.GetPlanes().ElementAt(2).gameObject.SetActive(false);
                ray.GetPlanes().ElementAt(3).gameObject.SetActive(false);
              // CAM2.transform.parent = ray.GetPlanes().ElementAt(1);
             //   CAM2.transform.position = new Vector3(ray.GetPlanes().ElementAt(1).position.x, ray.GetPlanes().ElementAt(1).position.y, ray.GetPlanes().ElementAt(1).position.z);
           //   CAM2.transform.rotation = ray.GetPlanes() .ElementAt(1).rotation;
                //  TOPDIAGONALUI.SetActive(false);
                //  BOTDIAGONALUI.SetActive(false);
                Cus1Slider.SetActive(false);
                Cus2Slider.SetActive(false);

            }
            if (instance.Equals("Instance3"))
            {
                controller.setPlane(ray.GetPlanes().ElementAt(2));
                ray.GetPlanes().ElementAt(2).gameObject.SetActive(true);
                ray.GetPlanes().ElementAt(1).gameObject.SetActive(false);
                ray.GetPlanes().ElementAt(0).gameObject.SetActive(false);
                ray.GetPlanes().ElementAt(3).gameObject.SetActive(false);
                // TOPDIAGONALUI.SetActive(true);
                //  BOTDIAGONALUI.SetActive(false);
                Cus1Slider.SetActive(true);
                Cus2Slider.SetActive(false);

            }
            if (instance.Equals("Instance4"))
            {
                controller.setPlane(ray.GetPlanes().ElementAt(3));
                ray.GetPlanes().ElementAt(3).gameObject.SetActive(true);
                ray.GetPlanes().ElementAt(1).gameObject.SetActive(false);
                ray.GetPlanes().ElementAt(2).gameObject.SetActive(false);
                ray.GetPlanes().ElementAt(0).gameObject.SetActive(false);
                //  TOPDIAGONALUI.SetActive(false);
                //  BOTDIAGONALUI.SetActive(true);
                Cus1Slider.SetActive(false);
                Cus2Slider.SetActive(true);
            }
         
        }
        else {
            Debug.Log("RAY NULL");
        }
      
    }
    public void VolReset()
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();

        ray.GetPlanes().ElementAt(0).gameObject.transform.localPosition = new Vector3(0f, -0.2f, 0f);

        ray.GetPlanes().ElementAt(1).gameObject.transform.localPosition = new Vector3(26.1f, -0.869f, -8.8f);

        ray.GetPlanes().ElementAt(2).gameObject.transform.localPosition = new Vector3(0f, -50f, 0f);

        ray.GetPlanes().ElementAt(3).gameObject.transform.localPosition = new Vector3(26.1f, -50f, -8.8f);



        ray.GetPlanes().ElementAt(0).gameObject.SetActive(false);
        ray.GetPlanes().ElementAt(1).gameObject.SetActive(false);
        ray.GetPlanes().ElementAt(2).gameObject.SetActive(false);
        ray.GetPlanes().ElementAt(3).gameObject.SetActive(false);

       
        setplane("Instance1");
        setplane("Instance2");
        setplane("Instance3");
        setplane("Instance4");
        Cus1Slider.SetActive(false);
        Cus2Slider.SetActive(false);

    }
    public void TOPLEFT()
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        if (ray != null)
        {
            Transform plane = ray.GetPlanes().ElementAt(2);
            CAM2.transform.parent = plane;

            plane.localEulerAngles = new Vector3(40f, plane.position.y, plane.position.z);
        }
            
    }
    public void TOPRIGHT()
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        if (ray != null)
        {
            Transform plane = ray.GetPlanes().ElementAt(2);
            plane.localEulerAngles = new Vector3(-40f, plane.position.y, plane.position.z);
        }

    }
    public void BOTTOMLEFT()
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        if (ray != null)
        {
            Transform plane = ray.GetPlanes().ElementAt(3);
            plane.localEulerAngles = new Vector3(-40f, plane.position.y, plane.position.z);
        }

    }
    public void BOTTOMRIGHT()
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        if (ray != null)
        {
            Transform plane = ray.GetPlanes().ElementAt(3);
            plane.localEulerAngles = new Vector3(40f, plane.position.y, plane.position.z);
        }

    }
    public void Cus1(UnityEngine.UI.Slider slider) {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        Transform plane = ray.GetPlanes().ElementAt(2);
       
        plane.localEulerAngles = new Vector3(slider.value, plane.position.y, plane.position.z);
    }
    public void Cus2(UnityEngine.UI.Slider slider)
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        Transform plane = ray.GetPlanes().ElementAt(3);
        plane.localEulerAngles = new Vector3(slider.value, plane.position.y, plane.position.z);
    }
    public void rotateup2()
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        Transform plane = ray.GetPlanes().ElementAt(3);

       
        float rotationAmount = 5f;

    
        UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(rotationAmount, 0, 0);


        plane.localRotation *= rotation;

    }
    public void rotatedown2() {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        Transform plane = ray.GetPlanes().ElementAt(3);


        float rotationAmount = -5f;


        UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(rotationAmount, 0, 0);


        plane.localRotation *= rotation;

    }

    public void rotateup1()
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        Transform plane = ray.GetPlanes().ElementAt(2);


        float rotationAmount = 5f;


        UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(rotationAmount, 0, 0);


        plane.localRotation *= rotation;

    }
    public void rotatedown1()
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        Transform plane = ray.GetPlanes().ElementAt(2);


        float rotationAmount = -5f;


        UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(rotationAmount, 0, 0);


        plane.localRotation *= rotation;

    }
    public void zoom(UnityEngine.UI.Slider slider)
    {
        Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
        cam.fieldOfView = slider.value;
    }

    public void UnlockRotation()
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        if (ray != null)
        {
            GameObject[] instances = ray.GetInstances();
            foreach(GameObject instance in instances)
            {
                instance.SetActive(false);
            }
            Cus1Slider.SetActive(false);
            Cus2Slider.SetActive(false);
            // ray.getTarget().GetComponent<Rotate>().enabled = true;
            ray.getTarget().GetComponent<RotateObjectWithThumbstick>().enabled = true;
            Gobackbutton.SetActive(true);
            rotatebutton.SetActive(false);
            ZoomSlider.SetActive(true);
            ray.GetPlanes().ElementAt(3).gameObject.SetActive(false);
            ray.GetPlanes().ElementAt(1).gameObject.SetActive(false);
            ray.GetPlanes().ElementAt(2).gameObject.SetActive(false);
            ray.GetPlanes().ElementAt(0).gameObject.SetActive(false);
        }


        }

    public void GoBack()
    {
        RayMarching ray = GameObject.Find("Camera").GetComponent<RayMarching>();
        if (ray != null)
        {
            GameObject[] instances = ray.GetInstances();
            foreach (GameObject instance in instances)
            {
                instance.SetActive(true);
            }
            Cus1Slider.SetActive(false);
            Cus2Slider.SetActive(false);
            ray.getTarget().GetComponent<Rotate>().enabled = false;
            ray.getTarget().GetComponent<RotateObjectWithThumbstick>().enabled = false;
            ray.getTarget().GetComponent<Transform>().localEulerAngles = new Vector3(180f,0,0);
            Gobackbutton.SetActive(false);
            rotatebutton.SetActive(true);
            ZoomSlider.SetActive(false);
            ray.GetPlanes().ElementAt(3).gameObject.SetActive(false);
            ray.GetPlanes().ElementAt(1).gameObject.SetActive(false);
            ray.GetPlanes().ElementAt(2).gameObject.SetActive(false);
            ray.GetPlanes().ElementAt(0).gameObject.SetActive(false);
        }
        GameObject.Find("Camera").GetComponent<Camera>().fieldOfView = 51.9f;
    }

    public void ToggleCAM2()
    {
        if (CAM2.activeSelf)
        {
            CAM2.SetActive(false);
        }
        else
        {
            CAM2.SetActive(true);
        }
    }
}
