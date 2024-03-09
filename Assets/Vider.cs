
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClickDetector : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component not found on object: " + gameObject.name);
        }
    }

    private void OnEnable()
    {
        grabInteractable.onSelectEntered.AddListener(OnGrab);
    }

    private void OnDisable()
    {
        grabInteractable.onSelectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(XRBaseInteractor interactor)
    {
        DisableChildrenByName("liquide1");
        DisableChildrenByName("liquide2");
        DisableChildrenByName("liquide3");
        DisableChildrenByName("liquide4");
    }
    void DisableChildrenByName(string name)
    {
        Transform child = transform.Find(name);
        
        if (child != null)
        {
            child.gameObject.SetActive(false);
        }
    }
}