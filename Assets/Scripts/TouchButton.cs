using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TouchButton : XRBaseInteractable
{
    [Header("Button Data")]
    [SerializeField]
    private int buttonValue;
    private Material originMaterial;
    [SerializeField]
    private Material hoverMaterial;
    [SerializeField]
    private NumberPad numberPad;

    protected override void Awake()
    {
        base.Awake();
        originMaterial = gameObject.GetComponent<Renderer>().material;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        SetMaterial(hoverMaterial);
        if (gameObject.CompareTag("Back Button"))
        {
            numberPad.BackButtonPressed();
        }
        else if(gameObject.CompareTag("Clear Button"))
        {
            numberPad.ClearButtonPressed();
        }
        else
        {
            numberPad.ButtonPressed(buttonValue);
        }


    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        SetMaterial(originMaterial);
    }

    private void SetMaterial(Material newMaterial)
    {
        gameObject.GetComponent<Renderer>().material = newMaterial;
    }

}
