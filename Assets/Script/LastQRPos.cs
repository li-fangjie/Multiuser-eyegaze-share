using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastQRPos : MonoBehaviour
{
    private static GameObject Tracked;
    private static Vector3 AnchorCube_position;
    private static GameObject QRCode;
    public GameObject Tracker;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Tracked = GameObject.Find("QRCube");
        if (Tracked != null)
        {
            AnchorCube_position = Tracked.transform.position;
            Tracker.transform.position = AnchorCube_position;

        }
    }
}
