using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorHandle : XRBaseInteractable
{
    private Vector3 doorStart;
    private Vector3 doorEnd;
    private Vector3 worldDragDirection; //convert local door direct to world direct
  

    [Header("Door Handle Data")]
    [SerializeField]
    private Transform doorTransform;
    [SerializeField]
    private float dragDistance;
    [SerializeField]
    private Vector3 localDragDirection; //local direction of the door can move

    [SerializeField]
    private int doorWeight = 20;

    private void Start()
    {
        worldDragDirection = transform.TransformDirection(localDragDirection).normalized;

        doorStart = doorTransform.position;
        doorEnd = doorStart + worldDragDirection * dragDistance;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (isSelected && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed)
        {
            var handlePosition = firstInteractorSelecting.GetAttachTransform(this);
            Vector3 pullVector = handlePosition.position - transform.position;
            float forceInDirectionOfDrag = Vector3.Dot(pullVector, worldDragDirection);
            bool dragToEnd = forceInDirectionOfDrag > 0.0f;
            float absoluteForce = Mathf.Abs(forceInDirectionOfDrag);
            float speed = absoluteForce / Time.deltaTime / doorWeight;
            doorTransform.position = Vector3.MoveTowards(doorTransform.position, dragToEnd ? doorEnd : doorStart, speed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 worldDirection = transform.TransformDirection(localDragDirection);
        worldDragDirection.Normalize();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + worldDragDirection * dragDistance);
    }
}
