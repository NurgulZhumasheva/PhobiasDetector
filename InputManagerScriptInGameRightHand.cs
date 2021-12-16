using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using UnityEngine.SceneManagement;

public class InputManagerScriptInGameRightHand : MonoBehaviour
{
    [SerializeField]
    XRNode node = XRNode.RightHand; // the script works for right hand controller

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device; // adding controller to the script

    void Start()
    {
        
    }

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

        bool secondaryButtonAction = false; // if the button is trigerred
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonAction) && secondaryButtonAction)
        { // if the red pointer hits the button
            SceneManager.LoadScene("IntroScene"); // move to intro scene with menu on TV
        }
        
        bool secondaryButton = false; // specifying the button
        InputFeatureUsage<bool> usage = CommonUsages.secondaryButton;
    }

}
