using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonFollowScript : MonoBehaviour
{
    public Transform visualTarget;
    public Vector3 localAxis;
    public float resetSpeed = 5f;
    public float followAngleThr = 45f;
    
    public AudioSource waterAudioSource;
    public AudioClip waterAudio;

    private bool freeze = false;

    private Vector3 initalPos;

    private Vector3 offset;
    private Transform pokeAttachTransform;

    private XRBaseInteractable interactable;
    private bool isFollowing = false;

    void Start()
    {
        initalPos = visualTarget.localPosition;

        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);
    }

    public void Follow(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is not XRPokeInteractor)
        {
            return;
        }

        XRPokeInteractor interactor = (XRPokeInteractor)args.interactorObject;
        pokeAttachTransform = interactor.transform;
        offset = visualTarget.position - pokeAttachTransform.position;

        float pokeAngle = Vector3.Angle(offset, visualTarget.TransformDirection(localAxis));

        if (pokeAngle < followAngleThr)
        {
            isFollowing = true;
            freeze = false;
        }
    }

    public void Reset(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is not XRPokeInteractor)
        {
            return;
        }

        isFollowing = false;
        freeze = false;
    }

    public void Freeze(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRPokeInteractor)
        {
            freeze = true;
            
            waterAudioSource.PlayOneShot(waterAudio);
        }
    }

    void Update()
    {
        if (freeze)
        {
            return;
        }
        
        if (!isFollowing)
        {
            visualTarget.localPosition =
                Vector3.Lerp(visualTarget.localPosition, initalPos, Time.deltaTime * resetSpeed);
            return;
        }

        Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
        Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);
        visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
    }
}