using Microsoft.MixedReality.QR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopQRcode : MonoBehaviour
{
    private static GameObject QRCodemanager;
    // Start is called before the first frame update
    void Start()
    {
        QRCodemanager = GameObject.Find("QRCodesManager");

    }

    public void stopQrcode_detection()
    {
        QRCodemanager.SetActive(false);
    }

}
