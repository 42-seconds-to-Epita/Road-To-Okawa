using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControlsManager : MonoBehaviour
{
    public GameObject handR;


    public GameObject lockObject;
    public GameObject doorObject;

    public AudioClip lockKeyAudio;

    void Update()
    {
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, leftHandDevices);

        if (leftHandDevices.Count == 1)
        {
            UnityEngine.XR.InputDevice device = leftHandDevices[0];
            bool triggerValue;
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton,
                    out triggerValue)
                && triggerValue)
            {
                if (Vector3.Distance(handR.transform.position, lockObject.transform.position) < 0.10f)
                {
                    doorObject.GetComponent<XRGrabInteractable>().enabled = true;
                    lockObject.GetComponentInChildren<AudioSource>().PlayOneShot(lockKeyAudio);
                }
            }
        }
    }
}