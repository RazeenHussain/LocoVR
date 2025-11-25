using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LocoArms : MonoBehaviour
{
    [SerializeField] private XRRayInteractor LeftRayInteractor;

    [SerializeField] private GameObject User;
    [SerializeField] private GameObject CamVR;
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject ForwardDirection;


    private Vector3 PositionPreviousLeftController;
    private Vector3 PositionPreviousRightController;
    private Vector3 PositionPreviousPlayer;
    private Vector3 PositionCurrentLeftController;
    private Vector3 PositionCurrentRightController;
    private Vector3 PositionCurrentPlayer;

    [SerializeField] private float Speed = 30f;
    [SerializeField] private float Threshold = 0.025f;
    private float HandSpeed;

    // Start is called before the first frame update
    void Start()
    {
        LeftRayInteractor.enabled = false;

        PositionPreviousPlayer = User.transform.position;
        PositionPreviousLeftController = LeftHand.transform.position;
        PositionPreviousRightController = RightHand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Arms Swinging
        ForwardDirection.transform.eulerAngles = new Vector3(0, CamVR.transform.eulerAngles.y, 0);
        PositionCurrentPlayer = User.transform.position;
        PositionCurrentLeftController = LeftHand.transform.position;
        PositionCurrentRightController = RightHand.transform.position;
        var DisplacementPlayer = Vector3.Distance(PositionCurrentPlayer, PositionPreviousPlayer);
        var DisplacementLeftHand = Vector3.Distance(PositionCurrentLeftController, PositionPreviousLeftController);
        var DisplacementRightHand = Vector3.Distance(PositionCurrentRightController, PositionPreviousRightController);
        HandSpeed = ((DisplacementLeftHand - DisplacementPlayer) + (DisplacementRightHand - DisplacementPlayer));
        //Debug.Log(HandSpeed + "   " + DisplacementPlayer + "   " + DisplacementLeftHand + "   " + DisplacementRightHand);
        if (Time.timeSinceLevelLoad > 1f && HandSpeed > Threshold)
        {
            User.transform.position += ForwardDirection.transform.forward * HandSpeed * Time.deltaTime * Speed;
        }
        PositionPreviousPlayer = PositionCurrentPlayer;
        PositionPreviousLeftController = PositionCurrentLeftController;
        PositionPreviousRightController = PositionCurrentRightController;

    }
}
