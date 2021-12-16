using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using UnityEngine.SceneManagement;

public class InputManagerScriptLeftHand : MonoBehaviour
{
    [SerializeField]
    XRNode node = XRNode.LeftHand;

    public Transform raycastOrigin;
    public LayerMask targetLayer;
    

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    
    void Start()
    {
        
    }

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(node, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
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
        
        List<InputFeatureUsage> features = new List<InputFeatureUsage>();
        device.TryGetFeatureUsages(features);

        bool triggerButtonAction = false;
       if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonAction) && triggerButtonAction)
        {
            FireRaycast();
        }
        
        bool primaryButton = false;
        InputFeatureUsage<bool> usage = CommonUsages.primaryButton;
    }

    void FireRaycast()
    {
        RaycastHit hit;

        if(Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLayer))
        {
            SceneManager.LoadScene(hit.transform.tag + "Scene");
        }
    }
}
