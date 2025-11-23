using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    private readonly HashSet<IXRSelectInteractor> activeInteractors = new HashSet<IXRSelectInteractor>();
    private Transform combinedAttachPoint;
    private bool bothHandsGrabbing = false;

    protected override void Awake()
    {
        base.Awake();
        // Create a single persistent attach point
        combinedAttachPoint = new GameObject("TwoHandAttachPoint").transform;
        combinedAttachPoint.SetParent(transform, false);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        activeInteractors.Add(args.interactorObject);

        // If only one hand, let the first hand attach normally but don’t move the object yet
        if (activeInteractors.Count == 1)
        {
            base.OnSelectEntered(args);
            bothHandsGrabbing = false;
        }

        // If both hands are grabbing
        if (activeInteractors.Count == 2)
        {
            bothHandsGrabbing = true;

            // Compute midpoint and rotation between both hands
            var interactorList = new List<IXRSelectInteractor>(activeInteractors);
            Transform firstHand = interactorList[0].transform;
            Transform secondHand = interactorList[1].transform;

            Vector3 midpoint = (firstHand.position + secondHand.position) / 2f;
            Quaternion rotation = Quaternion.LookRotation(firstHand.forward, Vector3.up);

            combinedAttachPoint.SetPositionAndRotation(midpoint, rotation);

            // Assign the persistent attach point
            attachTransform = combinedAttachPoint;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        activeInteractors.Remove(args.interactorObject);

        // If fewer than 2 hands, stop two-hand mode
        if (activeInteractors.Count < 2)
            bothHandsGrabbing = false;

        base.OnSelectExited(args);
    }

    private void Update()
    {
        // Continuously update attach point while both hands are grabbing
        if (bothHandsGrabbing && activeInteractors.Count == 2)
        {
            var interactorList = new List<IXRSelectInteractor>(activeInteractors);
            Transform firstHand = interactorList[0].transform;
            Transform secondHand = interactorList[1].transform;

            Vector3 midpoint = (firstHand.position + secondHand.position) / 2f;
            Quaternion rotation = Quaternion.LookRotation(firstHand.forward, Vector3.up);

            combinedAttachPoint.SetPositionAndRotation(midpoint, rotation);
        }
    }
}