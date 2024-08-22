using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MRTK.Tutorials.MultiUserCapabilities
{
    public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        public static PhotonRoom Room;

        [SerializeField] private GameObject photonUserPrefab = default;

        // private PhotonView pv;
        private Player[] photonPlayers;
        private int playersInRoom;
        private int myNumberInRoom;

        public GameObject monitorPrefab;
        private static GameObject AnchorPos;
        private Vector3 monitorLocation = Vector3.zero;

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom++;

            //Make first person to join room a masterClient. Also if there is a single User in room make player a masterclinet.
            //We can add a button later that allows users to choose to be masterclinets or localplayers.
            if (playersInRoom == 1)
            {
                Player newMaster = newPlayer;
                PhotonNetwork.SetMasterClient(newMaster);
            }
        }

        private void Awake()
        {
            if (Room == null)
            {
                Room = this;
            }
            else
            {
                if (Room != this)
                {
                    Destroy(Room.gameObject);
                    Room = this;
                }
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void Start()
        {
             // Allow prefabs not in a Resources folder
            if (PhotonNetwork.PrefabPool is DefaultPool pool)
            {
                if (photonUserPrefab != null) pool.ResourceCache.Add(photonUserPrefab.name, photonUserPrefab);
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom = photonPlayers.Length;
            myNumberInRoom = playersInRoom;
            PhotonNetwork.NickName = myNumberInRoom.ToString();

            StartGame();
        }

        private void StartGame()
        {
            CreatPlayer();

            if (!PhotonNetwork.IsMasterClient)
            {
                //Print number of users in room, for debuggin purposes,
                //it should keep updating as new players join room, else SetMasterClient isn't working
                Debug.Log("Number of players in room is:" + playersInRoom);

                return;
            }
            if (TableAnchor.Instance != null) CreateInteractableObjects();
        }

        private void CreatPlayer()
        {

            var player = PhotonNetwork.Instantiate(photonUserPrefab.name, Vector3.zero, Quaternion.identity);
            Debug.Log(player.transform.position);
            Debug.Log(player.transform.rotation);

        }

        private void CreateInteractableObjects()
        {
            //We need to add another variable to obtain the eye gaze relative to the monitor. This will be used later to re-create the 
            //relative position of the eye gaz
            AnchorPos = GameObject.Find("LastQRCode");
            var position = AnchorPos.transform.position;
            var go = PhotonNetwork.InstantiateRoomObject(monitorPrefab.name, position, AnchorPos.transform.rotation);
        }

        // private void CreateMainLunarModule()
        // {
        //     module = PhotonNetwork.Instantiate(roverExplorerPrefab.name, Vector3.zero, Quaternion.identity);
        //     pv.RPC("Rpc_SetModuleParent", RpcTarget.AllBuffered);
        // }
        //
        // [PunRPC]
        // private void Rpc_SetModuleParent()
        // {
        //     Debug.Log("Rpc_SetModuleParent- RPC Called");
        //     module.transform.parent = TableAnchor.Instance.transform;
        //     module.transform.localPosition = moduleLocation;
        // }
    }
}
