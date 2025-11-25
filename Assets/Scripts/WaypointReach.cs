using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class WaypointReach : MonoBehaviour
{
    [SerializeField] private GameObject Tablet;
    [SerializeField] private GameObject MarkerStart;
    [SerializeField] private GameObject MarkerWaypoint1;
    [SerializeField] private GameObject MarkerWaypoint2;
    [SerializeField] private GameObject MarkerWaypoint3;
    [SerializeField] private GameObject Viewer;

    [SerializeField] private GameObject EnvTask;

    [SerializeField] private Slider Q_Slider;
    [SerializeField] private TMPro.TextMeshProUGUI Q_Answer;
    [SerializeField] private TMPro.TextMeshProUGUI Instructions;

    [SerializeField] private XRRayInteractor RayInteractor;
    [SerializeField] private InputActionReference RightControllerTrigger;
    [SerializeField] private XRInteractorReticleVisual RightRerticle;

    private int state;
    [SerializeField] private int config;
    [SerializeField] private int CurrentConfigIndex;
    private int[] ConfigOrder = new int[5];
    private int[] OrderRW = { 1, 2, 3, 4, 5 };
    private int[] OrderTP = { 3, 5, 2, 1, 4 };
    private int[] OrderJS = { 4, 1, 5, 3, 2 };
    private int[] OrderAS = { 2, 3, 4, 5, 1 };

    private LocomotionMetaphor CurrentLocoMode;

    private float TimePassed;
    private bool TargetReached;
    private bool RecordPosition;
    private Vector3 TempPosition;
    private float TempTime;

    void Start()
    {
        CurrentConfigIndex = 0;
        TargetReached = false;

        RecordPosition = false;
        TempPosition = new Vector3(0f, 0f, 0f);
        
        state = 0;
        Tablet.SetActive(false);
        Q_Slider.value = 0;
        RayInteractor.enabled = false;
        RightRerticle.enabled = false;
        RayInteractor.lineType = XRRayInteractor.LineType.StraightLine;
        Instructions.text = "";

        MarkerStart.SetActive(false);
        MarkerWaypoint1.SetActive(false);
        MarkerWaypoint2.SetActive(false);
        MarkerWaypoint3.SetActive(false);
        Viewer.SetActive(false);
        TempTime = 0f;
        TimePassed = 0f;
    }

    void Update()
    {
        if (Tablet.activeInHierarchy)
        {
            Q_Answer.text = "Distance: " + Q_Slider.value.ToString("0.0") + "m";
        }

        if(state==0 && EnvTask.activeInHierarchy)
        {
            Viewer.SetActive(true);

            if (CurrentConfigIndex>=5)
            {
                EnvTask.SetActive(false);
                Instructions.text = "Session Completed";
                return;
            }

            Instructions.text = "Move to Starting Position\n(Yellow Disc)";

            SetWaypoints(ConfigOrder[CurrentConfigIndex]);
            state = 1;
            MarkerStart.SetActive(true);
            MarkerWaypoint1.SetActive(false);
            MarkerWaypoint2.SetActive(false);
            MarkerWaypoint3.SetActive(false);
        }

        if (state==6)
        {
            if (RightControllerTrigger.action.triggered)
            {
                if (RayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                {
                    RayInteractor.enabled = false;
                    RightRerticle.enabled = false;
                    RayInteractor.lineType = XRRayInteractor.LineType.StraightLine;

                    Debug.Log("Target : " + hit.point);
                    this.GetComponent<SaveUserData>().SessionData.EstimatedSP[CurrentConfigIndex] = new Vector2(hit.point.x,hit.point.z);

                    state = 0;
                    CurrentConfigIndex++;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        TimePassed += Time.deltaTime;

        TempTime += Time.deltaTime;


        if(RecordPosition && TempTime >= 1f )
        {
            TempPosition = this.transform.position;
            TempPosition.y = TempPosition.z;
            TempPosition.z = TimePassed;
            this.GetComponent<SaveUserData>().SessionData.UserPositions.Add(TempPosition);
            TempTime = 0f;
        }
    }

    public void SetLocomotionMetaphor(LocomotionMetaphor ToSet)
    {
        CurrentLocoMode = ToSet;
        if (CurrentLocoMode == LocomotionMetaphor.RealWalking)
        {
            ConfigOrder = OrderRW;
        }
        if (CurrentLocoMode == LocomotionMetaphor.Teleport)
        {
            ConfigOrder = OrderTP;
        }
        if (CurrentLocoMode == LocomotionMetaphor.Trackpad)
        {
            ConfigOrder = OrderJS;
        }
        if (CurrentLocoMode == LocomotionMetaphor.ArmsSwining)
        {
            ConfigOrder = OrderAS;
        }
    }

    private void SetWaypoints(int configuration)
    {
        switch (configuration)
        {
            case 1:
                MarkerStart.transform.position = new Vector3(-2.8f, MarkerStart.transform.position.y, 1.5f);
                MarkerWaypoint1.transform.position = new Vector3(0.9f, MarkerWaypoint1.transform.position.y, 1.5f);
                MarkerWaypoint2.transform.position = new Vector3(1f, MarkerWaypoint2.transform.position.y, -1.4f);
                MarkerWaypoint3.transform.position = new Vector3(-2.2f, MarkerWaypoint3.transform.position.y, -1f);
                Debug.Log("Configuration -> 1");
                break;
            case 2:
                MarkerStart.transform.position = new Vector3(-3f, MarkerStart.transform.position.y, -0.8f);
                MarkerWaypoint1.transform.position = new Vector3(3f, MarkerWaypoint3.transform.position.y, -1.5f);
                MarkerWaypoint2.transform.position = new Vector3(3.1f, MarkerWaypoint2.transform.position.y, 1.4f);
                MarkerWaypoint3.transform.position = new Vector3(-2f, MarkerWaypoint1.transform.position.y, 1.4f);
                Debug.Log("Configuration -> 2");
                break;
            case 3:
                MarkerStart.transform.position = new Vector3(1f, MarkerStart.transform.position.y, 1.5f);
                MarkerWaypoint1.transform.position = new Vector3(-2.7f, MarkerWaypoint1.transform.position.y, 1.4f);
                MarkerWaypoint2.transform.position = new Vector3(-1.9f, MarkerWaypoint2.transform.position.y, -1.1f);
                MarkerWaypoint3.transform.position = new Vector3(0.8f, MarkerWaypoint3.transform.position.y, -0.9f);
                Debug.Log("Configuration -> 3");
                break;
            case 4:
                MarkerStart.transform.position = new Vector3(3.1f, MarkerStart.transform.position.y, -1.4f);
                MarkerWaypoint1.transform.position = new Vector3(-2.7f, MarkerWaypoint1.transform.position.y, -0.8f);
                MarkerWaypoint2.transform.position = new Vector3(-2.5f, MarkerWaypoint2.transform.position.y, 1.5f);
                MarkerWaypoint3.transform.position = new Vector3(3.1f, MarkerWaypoint3.transform.position.y, 1.5f);
                Debug.Log("Configuration -> 4");
                break;
            case 5:
                MarkerStart.transform.position = new Vector3(0.7f, MarkerStart.transform.position.y, 1.5f);
                MarkerWaypoint1.transform.position = new Vector3(3.1f, MarkerWaypoint1.transform.position.y, 1.4f);
                MarkerWaypoint2.transform.position = new Vector3(3.1f, MarkerWaypoint2.transform.position.y, -1.4f);
                MarkerWaypoint3.transform.position = new Vector3(0.5f, MarkerWaypoint3.transform.position.y, -1.3f);
                Debug.Log("Configuration -> 5");
                break;
            default:
                Debug.Log("Invalid Configuration");
                break;
        }
        if (CurrentConfigIndex>=0 && CurrentConfigIndex<5)
        this.GetComponent<SaveUserData>().SessionData.Waypoints[CurrentConfigIndex * 4 + 0] = new Vector2(MarkerStart.transform.position.x, MarkerStart.transform.position.z);
        this.GetComponent<SaveUserData>().SessionData.Waypoints[CurrentConfigIndex * 4 + 1] = new Vector2(MarkerWaypoint1.transform.position.x, MarkerWaypoint1.transform.position.z);
        this.GetComponent<SaveUserData>().SessionData.Waypoints[CurrentConfigIndex * 4 + 2] = new Vector2(MarkerWaypoint2.transform.position.x, MarkerWaypoint2.transform.position.z);
        this.GetComponent<SaveUserData>().SessionData.Waypoints[CurrentConfigIndex * 4 + 3] = new Vector2(MarkerWaypoint3.transform.position.x, MarkerWaypoint3.transform.position.z);
    }

    public void Q_SubmitPress()
    {
        Debug.Log("Answer : " + Q_Slider.value.ToString("0.0"));
        /// write answer to file here 
        Tablet.SetActive(false);
        if (state == 3)
        {
            this.GetComponent<SaveUserData>().SessionData.EstimatedDistance[CurrentConfigIndex].x = Q_Slider.value;
            Instructions.text = "Move to Waypoint\n(Blue Disc)";
            Viewer.SetActive(true);
            MarkerWaypoint1.SetActive(false);
            MarkerWaypoint2.SetActive(true);
            RayInteractor.enabled = false;
            TargetReached = false;
            RecordPosition = true;
            TempTime = 0f;
            TimePassed = 0f;

            TempPosition = MarkerWaypoint1.transform.position;
            TempPosition.y = TempPosition.z;
            TempPosition.z = TimePassed;
            this.GetComponent<SaveUserData>().SessionData.UserPositions.Add(TempPosition);
        }
        if (state == 4)
        {
            this.GetComponent<SaveUserData>().SessionData.EstimatedDistance[CurrentConfigIndex].y = Q_Slider.value;
            Viewer.SetActive(true);
            Instructions.text = "Move to Waypoint\n(Purple Disc)";
            MarkerWaypoint2.SetActive(false);
            MarkerWaypoint3.SetActive(true);
            RayInteractor.enabled = false;
            TargetReached = false;
            RecordPosition = true;
            TempTime = 0f;
            TimePassed = 0f;

            TempPosition = MarkerWaypoint2.transform.position;
            TempPosition.y = TempPosition.z;
            TempPosition.z = TimePassed;
            this.GetComponent<SaveUserData>().SessionData.UserPositions.Add(TempPosition);
        }
        if (state == 5)
        {
            this.GetComponent<SaveUserData>().SessionData.EstimatedDistance[CurrentConfigIndex].z = Q_Slider.value;
            Viewer.SetActive(true);
            Instructions.text = "Select Starting Position\n(Where Yellow Disc Last Appeared)";
            MarkerWaypoint3.SetActive(false);
            RayInteractor.enabled = true;
            RightRerticle.enabled = true;
            RayInteractor.lineType = XRRayInteractor.LineType.ProjectileCurve;
            state = 6;
            TargetReached = false;
        }
        Q_Slider.value = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Start")
        {
            Debug.Log("Collision with Start");
            state = 2;
            RayInteractor.enabled = false;
            Viewer.SetActive(true);
            Instructions.text = "Move to Waypoint\n(Red Disc)";
            MarkerStart.SetActive(false);
            MarkerWaypoint1.SetActive(true);
            RecordPosition = true;
            TempTime = 0f;
            TimePassed = 0f;

            TempPosition = MarkerStart.transform.position;
            TempPosition.y = TempPosition.z;
            TempPosition.z = TimePassed;
            this.GetComponent<SaveUserData>().SessionData.UserPositions.Add(TempPosition);

        }
        if (collision.gameObject.name == "Target1" && !TargetReached)
        {
            Debug.Log("Collision with Target1 "+TimePassed.ToString("0.00")+"s");
            this.GetComponent<SaveUserData>().SessionData.TaskTime[CurrentConfigIndex].x = TimePassed;
            TargetReached = true;
            Tablet.SetActive(true);
            state = 3;
            RayInteractor.enabled = true;
            Viewer.SetActive(false);
            Instructions.text = "";
            RecordPosition = false;
        }
        if (collision.gameObject.name == "Target2" && !TargetReached)
        {
            Debug.Log("Collision with Target2 " + TimePassed.ToString("0.00") + "s");
            this.GetComponent<SaveUserData>().SessionData.TaskTime[CurrentConfigIndex].y = TimePassed;
            TargetReached = true;
            Tablet.SetActive(true);
            state = 4;
            RayInteractor.enabled = true;
            Viewer.SetActive(false);
            Instructions.text = "";
            RecordPosition = false;

        }
        if (collision.gameObject.name == "Target3" && !TargetReached)
        {
            Debug.Log("Collision with Target3 " + TimePassed.ToString("0.00") + "s");
            this.GetComponent<SaveUserData>().SessionData.TaskTime[CurrentConfigIndex].z = TimePassed;
            TargetReached = true;
            Tablet.SetActive(true);
            state = 5;
            RayInteractor.enabled = true;
            Viewer.SetActive(false);
            Instructions.text = "";
            RecordPosition = false;

            TempPosition = MarkerWaypoint3.transform.position;
            TempPosition.y = TempPosition.z;
            TempPosition.z = 0f;
            this.GetComponent<SaveUserData>().SessionData.UserPositions.Add(TempPosition);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Start")
        {
            TimePassed = 0f;
        }
        if (collision.gameObject.name == "Target1")
        {
            TimePassed = 0f;
        }
        if (collision.gameObject.name == "Target2")
        {
            TimePassed = 0f;
        }
        if (collision.gameObject.name == "Target3")
        {
            TimePassed = 0f;
        }
    }


}
