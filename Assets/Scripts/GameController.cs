using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public enum LocomotionMetaphor
{
    RealWalking,
    Teleport,
    Trackpad,
    ArmsSwining
}

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject CamVR;
    [SerializeField] private XRRayInteractor LeftRayInteractor;
    [SerializeField] private XRRayInteractor RightRayInteractor;

    [SerializeField] private GameObject WaypointStart;
    [SerializeField] private GameObject Waypoint1;
    [SerializeField] private GameObject Waypoint2;
    [SerializeField] private GameObject Waypoint3;

    [SerializeField] private GameObject EnvPractice;
    [SerializeField] private GameObject EnvTask;

    [SerializeField] public LocomotionMetaphor LocoMode;
    private LocomotionMetaphor PrevLocoMode;

    [SerializeField] public string UserID;
    [SerializeField] private bool SaveData;

    // Start is called before the first frame update
    void Start()
    {
        LeftRayInteractor.enabled = false;
        RightRayInteractor.enabled = false;

        EnvPractice.SetActive(true);
        EnvTask.SetActive(false);

        SetLocomotionMode(LocoMode);
        PrevLocoMode = LocoMode;
        CamVR.GetComponent<WaypointReach>().SetLocomotionMetaphor(LocoMode);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("t"))
        {
            EnvPractice.SetActive(false);
            EnvTask.SetActive(true);
        }

        if (Input.GetKeyDown("p"))
        {
            EnvPractice.SetActive(true);
            EnvTask.SetActive(false);
        }

        if (PrevLocoMode!=LocoMode)
        {
            SetLocomotionMode(LocoMode);
            PrevLocoMode = LocoMode;
            CamVR.GetComponent<WaypointReach>().SetLocomotionMetaphor(LocoMode);
        }

    }

    private void SetLocomotionMode(LocomotionMetaphor CurLocoMode)
    {
        if (CurLocoMode == LocomotionMetaphor.RealWalking)
        {
            this.GetComponent<LocoTrackpad>().enabled = false;
            this.GetComponent<LocoTeleport>().enabled = false;
            this.GetComponent<LocoArms>().enabled = false;
        }
        if (CurLocoMode == LocomotionMetaphor.Teleport)
        {
            this.GetComponent<LocoTrackpad>().enabled = false;
            this.GetComponent<LocoTeleport>().enabled = true;
            this.GetComponent<LocoArms>().enabled = false;
        }
        if (CurLocoMode == LocomotionMetaphor.Trackpad)
        {
            this.GetComponent<LocoTrackpad>().enabled = true;
            this.GetComponent<LocoTeleport>().enabled = false;
            this.GetComponent<LocoArms>().enabled = false;
        }
        if (CurLocoMode == LocomotionMetaphor.ArmsSwining)
        {
            this.GetComponent<LocoTrackpad>().enabled = false;
            this.GetComponent<LocoTeleport>().enabled = false;
            this.GetComponent<LocoArms>().enabled = true;
        }
    }

    private void OnApplicationQuit()
    {
        if (SaveData)
        {
            CamVR.GetComponent<SaveUserData>().SaveToJson(LocoMode, UserID);
        }
    }

}
