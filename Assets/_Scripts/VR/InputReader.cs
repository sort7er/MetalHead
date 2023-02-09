using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputReader : MonoBehaviour
{

    List<InputDevice> inputDevices = new List<InputDevice>();

    void Start()
    {
        InitializeInputReader();
    }

    private void Update()
    {
        if(inputDevices.Count < 2)
        {
            InitializeInputReader();
        }
    }

    private void InitializeInputReader()
    {
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, inputDevices);

        foreach (var inputDevice in inputDevices)
        {
            inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            //Debug.Log(inputDevice.name + " " + triggerValue);
        }
    }

}
