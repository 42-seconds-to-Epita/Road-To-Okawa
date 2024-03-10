using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControlsManager : MonoBehaviour
{
    public GameObject handR;


    public GameObject lockObject;
    public GameObject doorObject;
    public AudioClip lockKeyAudio;

    public GameObject WaterBucket;
    public AudioClip waterAudio;

    public GameObject wasteBucket;

    private void Start()
    {
        InvokeRepeating("UpdateHalfSeconds", 0, 0.5f);
    }

    void UpdateHalfSeconds()
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

                if (Vector3.Distance(handR.transform.position, WaterBucket.transform.position) < 0.35f)
                {
                    if (WaterBucket.GetComponent<WaterBucket>().UpdateLevel())
                    {
                        WaterBucket.GetComponent<AudioSource>().PlayOneShot(waterAudio);
                    }
                }

                if (Vector3.Distance(handR.transform.position, wasteBucket.transform.position) < 0.35f)
                {
                    Transform child = wasteBucket.transform.Find("waste");

                    if (child != null)
                    {
                        child.gameObject.SetActive(false);
                    }
                }

                foreach (IXRHoverInteractable xrHoverInteractable in handR.GetComponentInChildren<XRRayInteractor>()
                             .interactablesHovered)
                {
                    xrHoverInteractable.transform.gameObject.GetComponent<Container>().FillContainer();
                }
            }
        }
    }
}