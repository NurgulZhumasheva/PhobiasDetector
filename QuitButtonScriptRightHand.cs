using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using UnityEngine.SceneManagement;

public class QuitButtonScriptRightHand : MonoBehaviour
{ 
    [SerializeField]
    XRNode node = XRNode.RightHand; // the script works for right hand controller

    public Transform raycastOrigin;
    public LayerMask targetLayer;


    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device; // adding controller to the script

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(node, devices); // getting to know all devices
        device = devices.FirstOrDefault(); // controle the experience using the first device that's been grabbed or use default
    }

    void OnEnable()
    {
        if (!device.isValid) // making sure that the device is enabled/valid/working
        {
            GetDevice();
        }
    }

    void Update()
    {
        if (!device.isValid)
        {
            GetDevice();
        }

        List<InputFeatureUsage> features = new List<InputFeatureUsage>(); // knowing features of a device
        device.TryGetFeatureUsages(features);

        bool triggerButtonAction = false; // if the button is trigerred
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonAction) && triggerButtonAction)
        {
            FireRaycast();
        }

        bool primaryButton = false; // specifying that only primary button on a joysctick should work to change the scene
        InputFeatureUsage<bool> usage = CommonUsages.primaryButton;
    }

    void FireRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLayer))
        { // if the red pointer hits the button
            Application.Quit();
            Debug.Log("Quit!"); // Quit the game
        }
    }
}


}
