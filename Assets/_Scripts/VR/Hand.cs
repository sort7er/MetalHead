using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class Hand : MonoBehaviour
{
    public GameObject handPrefab;
    public InputDeviceCharacteristics controllerCharacteristics;

    private InputDevice targetDevice;
    private Animator handAnim;

    private void Start()
    {
        InitializeHand();
    }

    private void Update()
    {
        if(!targetDevice.isValid)
        {
            InitializeHand();
        }
        else
        {
            UpdateHand();
        }
    }

    private void InitializeHand()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if(devices.Count > 0 )
        {
            targetDevice = devices[0];

            GameObject spawnedHand = Instantiate(handPrefab, transform);
            handAnim = spawnedHand.GetComponent<Animator>();
        }
    }

    private void UpdateHand()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnim.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnim.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnim.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnim.SetFloat("Grip", 0);
        }
    }

    public void UsingRay(bool state)
    {
        if(handAnim!= null)
        {
            handAnim.SetBool("UsingRay", state);
        }
    }
}
