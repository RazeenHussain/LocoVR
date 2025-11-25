using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUserData : MonoBehaviour
{
    public ExpData SessionData = new ExpData();
    private void Start()
    {
        SessionData.Waypoints = new Vector2[20];
        SessionData.EstimatedDistance = new Vector3[5];
        SessionData.TaskTime = new Vector3[5];
        SessionData.EstimatedSP = new Vector2[5];

    }

    public void SaveToJson(LocomotionMetaphor LocoUsed, string FileName)
    {
        string UserData = JsonUtility.ToJson(SessionData);
        string FilePath = Application.streamingAssetsPath + "/"+LocoUsed.ToString()+"_"+FileName+".json";
        System.IO.File.WriteAllText(FilePath, UserData);
        Debug.Log("Data Saved To "+FilePath);
    }

}


[System.Serializable]
public class ExpData 
{
    public Vector2[] Waypoints; // Start, Waypoint1, Waypoint2, Waypoint3
    public Vector3[] EstimatedDistance; // Waypoint1, Waypoint2, Waypoint3
    public Vector3[] TaskTime; // Waypoint1, Waypoint2, Waypoint3
    public Vector2[] EstimatedSP; // X, Z

    public List<Vector3> UserPositions = new List<Vector3>(); // X, Z, time 
}

