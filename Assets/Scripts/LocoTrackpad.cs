using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LocoTrackpad : MonoBehaviour
{

    [SerializeField] private InputActionReference LeftControllerTrackpad;
    [SerializeField] private XRRayInteractor LeftRayInteractor;
    [SerializeField] private GameObject User;
    [SerializeField] private GameObject CamVR;
    [SerializeField] private GameObject ForwardDirection;

    // Start is called before the first frame update
    void Start()
    {
        LeftRayInteractor.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
          // Trackpad Left Controller
        Vector2 trackpadValue = LeftControllerTrackpad.action.ReadValue<Vector2>();
        //float horizontalMovement = -1 * trackpadValue.x;
        float verticalMovement = trackpadValue.y;
        ForwardDirection.transform.eulerAngles = new Vector3(0, CamVR.transform.eulerAngles.y, 0);
        if (Mathf.Abs(verticalMovement) > 0.25)
        {
            User.transform.position += ForwardDirection.transform.forward * verticalMovement * (Time.deltaTime);
        }
        //Debug.Log("X: "+horizontalMovement+ " Y: "+verticalMovement);


    }
}
