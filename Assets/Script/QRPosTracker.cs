using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QRPosTracker : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameObject Tracked;
    private static Vector3 AnchorCube_position;
    private static Quaternion AnchorCube_rotation;
    private static GameObject QRCode;
    public GameObject Tracker;
    // Start is called before the first frame update
    void Start()
    {
        Tracked = GameObject.Find("Monitor_Plane");
        if (Tracked != null)
        {
            AnchorCube_position = Tracked.transform.position;
            AnchorCube_rotation = Tracked.transform.rotation;
            Tracker.transform.position = AnchorCube_position;
            Tracker.transform.rotation = AnchorCube_rotation;

        }
    }

    // Update is called once per frame
    void Update()
    {
        Tracked = GameObject.Find("Monitor_Plane");
        if (Tracked != null)
        {
            AnchorCube_position = Tracked.transform.position;
            AnchorCube_rotation = Tracked.transform.rotation;
            Tracker.transform.position = AnchorCube_position;
            Tracker.transform.rotation = AnchorCube_rotation;

        }

    }
}
