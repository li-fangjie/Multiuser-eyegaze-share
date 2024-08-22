using Photon.Pun;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace MRTK.Tutorials.MultiUserCapabilities
{

    public class PhotonUser : MonoBehaviour
    {
        List<Color> colorList = new List<Color>()
        {
             Color.red,
             Color.green,
             Color.yellow,
             Color.magenta,
             new Color(255F, 0F, 255F),
             new Color(0F, 255F, 255F),
             new Color(255F, 255F, 0F),
             new Color(128F, 0F, 128F),
             new Color(128F, 0F, 0F)
        };

        private GameObject temp_user_name;
        private int user_num;

        private PhotonView pv;
        private string username;

        private void Start()
        {
            pv = GetComponent<PhotonView>();

            if (!pv.IsMine) return;
            temp_user_name = GameObject.Find("User" + PhotonNetwork.NickName);
            if (temp_user_name != null)
            {
                user_num = int.Parse(PhotonNetwork.NickName);
                user_num = user_num + 1;
                username = "User" + user_num.ToString();
            }
            else
            {
                username = "User" + PhotonNetwork.NickName;
            }
            pv.RPC("PunRPC_SetNickName", RpcTarget.AllBuffered, username);
        }

        [PunRPC]
        public void PunRPC_SetNickName(string nName)
        {
            gameObject.name = nName;
            if (nName == "User1")
            {
                gameObject.transform.Find("Sphere").GetComponent<Renderer>().enabled = false;

            }
            if (nName == "User2")
            {
                gameObject.transform.Find("Sphere").GetComponent<Renderer>().enabled = false;
            }
            if (nName == "User3")
            {
                gameObject.transform.Find("Sphere").GetComponent<Renderer>().enabled = false;
            }
            if (nName == "User4")
            {
                gameObject.transform.Find("Sphere").GetComponent<Renderer>().enabled = false;
            }
            if (nName == "User5")
            {
                gameObject.transform.Find("Sphere").GetComponent<Renderer>().enabled = false;
            }
        }

        [PunRPC]
        private void PunRPC_ShareAzureAnchorId(string anchorId)
        {
            GenericNetworkManager.Instance.azureAnchorId = anchorId;

            Debug.Log("\nPhotonUser.PunRPC_ShareAzureAnchorId()");
            Debug.Log("GenericNetworkManager.instance.azureAnchorId: " + GenericNetworkManager.Instance.azureAnchorId);
            Debug.Log("Azure Anchor ID shared by user: " + pv.Controller.UserId);
        }

        public void ShareAzureAnchorId()
        {
            if (pv != null)
                pv.RPC("PunRPC_ShareAzureAnchorId", RpcTarget.AllBuffered,
                    GenericNetworkManager.Instance.azureAnchorId);
            else
                Debug.LogError("PV is null");
        }
    }
}
