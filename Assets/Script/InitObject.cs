using Microsoft.MixedReality.SampleQRCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitObject : MonoBehaviour
{
    private static GameObject AnchorCube;
    private static Vector3 AnchorCube_pos;
    private static Quaternion AnchorCube_rotation;
    private static GameObject QRCode;
    public GameObject Table;
    // Start is called before the first frame update
    void Start()
    {
        AnchorCube = GameObject.Find("LastQRCode");
        if (AnchorCube != null)
        {
            AnchorCube_pos = AnchorCube.transform.position;
            AnchorCube_rotation = AnchorCube.transform.rotation;
            Table.transform.position = AnchorCube_pos;
            Table.transform.rotation = AnchorCube_rotation;

        }
    }

    // Update is called once per frame
    void Update()
    {
        AnchorCube = GameObject.Find("LastQRCode");
        if (AnchorCube != null)
        {
            AnchorCube_pos = AnchorCube.transform.position;
            AnchorCube_rotation = AnchorCube.transform.rotation;
            Table.transform.position = AnchorCube_pos;
            Table.transform.rotation = AnchorCube_rotation;

        }
    }
}
