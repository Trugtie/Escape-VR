using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Serialization;

namespace FinalExample
{

    public class TouchButton : XRBaseInteractable
    {
        [Header("Visuals")]
        public Material normalMaterial;
        public Material touchedMaterial;

        [Header("Button Data")]
        public int buttonNumber;
        public NumberPad linkedNumberpad;

        private int m_NumberOfInteractor = 0;
        private Renderer m_RendererToChange;

        private void Start()
        {
            m_RendererToChange = GetComponent<MeshRenderer>();
        }

        public override bool IsHoverableBy(IXRHoverInteractor interactor)
        {
            return base.IsHoverableBy(interactor) && (interactor is XRDirectInteractor);
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);

            //we only want to change for the first interactor that touch (e.g. if we touch with a second hand when one is
            //already touching the button, no need to change the data again)
            if (m_NumberOfInteractor == 0)
            {
                m_RendererToChange.material = touchedMaterial;

                linkedNumberpad.ButtonPressed(buttonNumber);
            }

            m_NumberOfInteractor += 1;
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);

            m_NumberOfInteractor -= 1;

            //if we have multiple interactor touching that (e.g. 2 hands) then this will be called when only one is removed.
            //we want to make sure we have nothing left hovering before returning it to "normal" state.
            if (m_NumberOfInteractor == 0)
                m_RendererToChange.material = normalMaterial;
        }
    }
}