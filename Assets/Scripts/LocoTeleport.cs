using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.InputSystem;

public class LocoTeleport : MonoBehaviour
{
    [SerializeField] private XRRayInteractor RayInteractor;
    [SerializeField] private TeleportationProvider Provider;
    [SerializeField] private InputActionReference LeftControllerTeleport;

    private void Start()
    {
        RayInteractor.enabled = false;
        LeftControllerTeleport.action.started += OnTeleportActivate;
        LeftControllerTeleport.action.canceled += OnTeleportCancel;
    }

    private void Update()
    {
        if (LeftControllerTeleport.action.inProgress)
        {
            return;
        }

        if (!RayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            RayInteractor.enabled = false;
            return;
        }

        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point,
            //destinationRotation =,
            //matchOrientation =,
            //requestTime =,
        };

        Provider.QueueTeleportRequest(request);

    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        RayInteractor.enabled = true;
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        RayInteractor.enabled = false;
    }

}
