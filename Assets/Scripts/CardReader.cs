using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReader : XRSocketInteractor
{
    [Header("Card Reader Variable")]
    [SerializeField]
    private Vector3 hoverEntry;
    [SerializeField]
    private Vector3 hoverExit;
    [SerializeField]
    private Vector3 traveledVector;
    [SerializeField]
    private Transform keycardTransform;
    [SerializeField]
    private bool isSwipe = false;
    [SerializeField]
    private Material greenEmission;
    [SerializeField]
    private Material redEmission;
    [SerializeField]
    private float allowUprightErrorRange = 0.2f;

    [Header("Card Reader Data")]
    [SerializeField]
    private GameObject greenLight;
    [SerializeField]
    private GameObject redLight;
    [SerializeField]
    private GameObject doorLockingPad;
    [SerializeField]
    private GameObject doorHandle;
    [SerializeField]
    private float distanceSwipe = -0.15f;

    protected override void Awake()
    {
        base.Awake();
        greenEmission = greenLight.GetComponent<Renderer>().material;
        redEmission = redLight.GetComponent<Renderer>().material;
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        base.CanSelect(interactable);
        return false;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        keycardTransform = args.interactableObject.transform;
        hoverEntry = keycardTransform.position;
        isSwipe = true;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        hoverExit = args.interactableObject.transform.position;
        traveledVector = hoverExit - hoverEntry;
        Debug.Log("Lenght traveledVector: " + traveledVector.y);

        if(isSwipe && traveledVector.y < distanceSwipe)
        {
            doorLockingPad.SetActive(false);
            doorHandle.SetActive(true);
            greenEmission.EnableKeyword("_EMISSION");
            redEmission.DisableKeyword("_EMISSION");
        }
        else
        {
            greenEmission.DisableKeyword("_EMISSION");
            redEmission.EnableKeyword("_EMISSION");
        }
        keycardTransform = null;
    }

    private void Update()
    {
        if(keycardTransform != null)
        {
            Vector3 keycardUp = keycardTransform.forward;
            float dot = Vector3.Dot(keycardUp, Vector3.up);
            Debug.Log("Dot: " + dot);

            if(dot > 1 - allowUprightErrorRange)
            {
                isSwipe = false;
                greenEmission.DisableKeyword("_EMISSION");
                redEmission.EnableKeyword("_EMISSION");
            }
        }
    }
}
