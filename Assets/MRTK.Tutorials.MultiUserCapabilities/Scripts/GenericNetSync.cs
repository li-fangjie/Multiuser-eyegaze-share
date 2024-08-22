using Photon.Pun;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.MixedReality.Toolkit;
using System.Text;
using System.Linq;
using TMPro;
using UnityEngine.Windows.Speech;
using System.Drawing.Text;
using System.Threading;

namespace MRTK.Tutorials.MultiUserCapabilities
{
    public class GenericNetSync : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private bool isUser = default;
        public static GameObject Cursor;
        private static Vector3 Cursor_position;
        private static Quaternion Cursor_rotation;

        //Add GameObject EyeGaze
        //Use for user 1 local eye gaze data
        public static GameObject eyeGazeInstance;
        //// For new pointer
        public static GameObject eyeGazeCursor;
        private static Vector3 eyeGaze_LocalPosition;
        private static Quaternion eyeGaze_LocalRotation;

        private Camera mainCamera;

        private Vector3 networkLocalPosition;
        private Quaternion networkLocalRotation;

        private Vector3 startingLocalPosition;
        private Quaternion startingLocalRotation;

        KeywordRecognizer keywordRecognizer;
        private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();
        public static GameObject controlledObject;
        private static GameObject userID;

        // For new pointer 
        private static GameObject userid;

        public static bool setRecording = false;
        public static int count = 0;

        // To scale the pointer based on slider input
        public PinchSlider pinchSlider;
        public float minScale = 0.0f;
        public float maxScale = 1.0f;

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.localPosition);
                stream.SendNext(transform.localRotation);
            }
            else
            {
                networkLocalPosition = (Vector3)stream.ReceiveNext();
                networkLocalRotation = (Quaternion)stream.ReceiveNext();
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;

            Cursor = GameObject.Find("DefaultGazeCursor(Clone)");

            if (isUser)
            {
                if (TableAnchor.Instance != null) transform.parent = FindObjectOfType<TableAnchor>().transform;

                if (photonView.IsMine) GenericNetworkManager.Instance.localUser = photonView;
            }

            var trans = transform;
            startingLocalPosition = trans.localPosition;
            startingLocalRotation = trans.localRotation;

            networkLocalPosition = startingLocalPosition;
            networkLocalRotation = startingLocalRotation;
            userID = GameObject.Find("User1");
            
            if (userID != null )//and photonView.IsMine == false)
            {
                eyeGazeInstance = userID.transform.GetChild(0).gameObject;
                eyeGazeInstance.GetComponent<Renderer>().enabled = false;
                eyeGazeCursor = userID.transform.GetChild(1).gameObject;
                eyeGazeCursor.GetComponent<Renderer>().enabled = false;
                controlledObject = eyeGazeInstance;
            }


            // Initliaze slider
            // Ensure pinchSlider is assigned in the Inspector
            if (pinchSlider == null)
            {
                Debug.LogError("PinchSlider is not assigned!");
                return;
            }

            // Subscribe to the OnValueUpdated event of the PinchSlider
            pinchSlider.OnValueUpdated.AddListener(OnPinchSliderUpdated);

        }

        // Update is called once per frame
        private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
        {
            System.Action keywordAction;
            // if the keyword recognized is in our dictionary, call that Action.
            if (actions.TryGetValue(speech.text, out keywordAction))
            {
                Debug.Log(speech.text);
                keywordAction.Invoke();

            }
        }

        public void OnPinchSliderUpdated(SliderEventData eventData)
        {
            // Retrieve the slider value
            float sliderValue = eventData.NewValue;

            // Map the slider value to the desired scale range
            float scaleFactor = Mathf.Lerp(minScale, maxScale, sliderValue);

            // Apply the scale to the target object
            controlledObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        }

        public void Show_1()
        {
            userid = GameObject.Find("User1");
            eyeGazeCursor = userid.transform.GetChild(1).gameObject;
            eyeGazeCursor.GetComponent<Renderer>().enabled = true;
            controlledObject = eyeGazeCursor;
        }

        public void Hide()
        {
            userid = GameObject.Find("User1");
            eyeGazeInstance = userid.transform.GetChild(0).gameObject;
            eyeGazeCursor = userid.transform.GetChild(1).gameObject;
            eyeGazeInstance.GetComponent<Renderer>().enabled = false;
            eyeGazeCursor.GetComponent<Renderer>().enabled = false;
        }
        public void Show()
        {
            userid = GameObject.Find("User1");
            eyeGazeInstance = userid.transform.GetChild(0).gameObject;
            eyeGazeInstance.GetComponent<Renderer>().enabled = true;
            controlledObject = eyeGazeInstance;
        }
        // private void FixedUpdate()
        private void Update()
        {
            if (!photonView.IsMine)
            {
                var trans = transform;
                trans.localPosition = networkLocalPosition;
                trans.localRotation = networkLocalRotation;
            }

            if (photonView.IsMine && isUser)
            {
                var trans = transform;
                var mainCameraTransform = mainCamera.transform;
                trans.position = mainCameraTransform.position;
                trans.rotation = mainCameraTransform.rotation;
            }

            if (Cursor != null)
            {
                // if (userID != null )//and photonView.IsMine == false)
                // {
                //     //Added by Jumana 5/22, edited 5/30;
                //     eyeGazeInstance = userID.transform.GetChild(0).gameObject;
                //     //controlledObject = eyeGazeInstance;
                //     //only add for trainee build
                //     //eyeGazeCursor = userID.transform.GetChild(1).gameObject;
                //     eyeGazeInstance.GetComponent<Renderer>().enabled = false;
                //     eyeGazeCursor = userID.transform.GetChild(1).gameObject;
                //     //controlledObject = eyeGazeCursor;
                //     eyeGazeCursor.GetComponent<Renderer>().enabled = true;
                // }

                //Comment below for surgeon build only
               if (photonView.IsMine == true && photonView.name == "User1")
                {
                    controlledObject.transform.position = Cursor.transform.position;
                    controlledObject.transform.rotation = Cursor.transform.rotation;
                    eyeGaze_LocalPosition = controlledObject.transform.localPosition;
                    eyeGaze_LocalRotation = controlledObject.transform.localRotation;
                    Save_local_gaze_data(eyeGaze_LocalPosition.x, eyeGaze_LocalPosition.y, eyeGaze_LocalPosition.z, eyeGaze_LocalRotation.w, eyeGaze_LocalRotation.x, eyeGaze_LocalRotation.y, eyeGaze_LocalRotation.z);
                 } 
                Cursor_position = Cursor.transform.position;
                Cursor_rotation = Cursor.transform.rotation;
                if (GameObject.Find("User1") != null)
                {
                    Cursor_position = GameObject.Find("User1").transform.position;
                    Cursor_rotation = GameObject.Find("User1").transform.rotation;
                    Save_gaze_data(Cursor_position.x, Cursor_position.y, Cursor_position.z, Cursor_rotation.w, Cursor_rotation.x, Cursor_rotation.y, Cursor_rotation.z);
                }
            }

        }
        public void Start_Recording()
        {
            if (!setRecording)
                count++;
            setRecording = true;
        }
        public void Stop_Recording()
        {
            setRecording = false;
        }
        public static void Save_gaze_data(float px, float py, float pz, float rw, float rx, float ry, float rz)
        {
            //Debug.Log("gaze_position x = " + px + " y = " + py + " z = " + pz);
            DateTime foo = DateTime.Now;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeMilliseconds();
            //unixTime = unixTime * (long)0.001;
            string unixTime_String = unixTime.ToString();
            //Debug.Log("UNIX_timeStamp" + unixTime_String);
            string timeStamp = DateTime.Now.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
            string start_time = Time.time.ToString().Replace(".", "").Replace(":", "");
            string start_date = DateTime.Now.ToLocalTime().ToString("yyyyMMdd_HH");
            //Debug.Log("timeStamp" + timeStamp);
            if (setRecording)
            {
                string filepath_in_function2 = Application.persistentDataPath + "/Eye_Gaze_Coordinates_" + start_date + "_" + count + "_" + ".csv";
                //Debug.Log("filepath" + filepath_in_function2);
                //string test_filePath = "U:/Users/yizhou.li@vanderbilt.edu/AppData/Local/Packages/Eyerecorder_pzq3xp76mxafg/LocalState/position_Cursor_IO.csv";
                //Debug.Log("setRecording is " + setRecording);
                try
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath_in_function2, true))
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@test_filePath, true))
                    {
                        //file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx+ "," + ry + "," + rz + "," + "," + cpx + "," + cpy + "," + cpz + "," + crw + "," + crx + "," + cry + "," + crz);
                        file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx + "," + ry + "," + rz + "," + timeStamp + "," + unixTime_String);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to save:  ", ex);
                }
            }
        }
        public static void Save_data(float px, float py, float pz, float rw, float rx, float ry, float rz)
        {
            //Debug.Log("gaze_position x = " + px + " y = " + py + " z = " + pz);
            DateTime foo = DateTime.Now;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeMilliseconds();
            //unixTime = unixTime * (long)0.001;
            string unixTime_String = unixTime.ToString();
            //Debug.Log("UNIX_timeStamp" + unixTime_String);
            string timeStamp = DateTime.Now.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
            string start_time = Time.time.ToString().Replace(".", "").Replace(":", "");
            string start_date = DateTime.Now.ToLocalTime().ToString("yyyyMMdd_HH");
            //Debug.Log("timeStamp" + timeStamp);
            if (setRecording)
            {
                string filepath_in_function2 = Application.persistentDataPath + "/Data_Coordinates_" + start_date + "_" + count + "_" + ".csv";
                //Debug.Log("filepath" + filepath_in_function2);
                //string test_filePath = "U:/Users/yizhou.li@vanderbilt.edu/AppData/Local/Packages/Eyerecorder_pzq3xp76mxafg/LocalState/position_Cursor_IO.csv";
                //Debug.Log("setRecording is " + setRecording);
                try
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath_in_function2, true))
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@test_filePath, true))
                    {
                        //file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx+ "," + ry + "," + rz + "," + "," + cpx + "," + cpy + "," + cpz + "," + crw + "," + crx + "," + cry + "," + crz);
                        file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx + "," + ry + "," + rz + "," + timeStamp + "," + unixTime_String);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to save:  ", ex);
                }
            }
        }
        public static void Save_local_gaze_data(float px, float py, float pz, float rw, float rx, float ry, float rz)
        {
            //Debug.Log("gaze_position x = " + px + " y = " + py + " z = " + pz);
            DateTime foo = DateTime.Now;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeMilliseconds();
            //unixTime = unixTime * (long)0.001;
            string unixTime_String = unixTime.ToString();
            //Debug.Log("UNIX_timeStamp" + unixTime_String);
            string timeStamp = DateTime.Now.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
            string start_time = Time.time.ToString().Replace(".", "").Replace(":", "");
            string start_date = DateTime.Now.ToLocalTime().ToString("yyyyMMdd_HH");
            //Debug.Log("timeStamp" + timeStamp);
            if (setRecording)
            {
                string filepath_in_function2 = Application.persistentDataPath + "/Eye_Gaze_local_Coordinates_" + start_date + "_" + count + "_" + ".csv";
                //Debug.Log("filepath" + filepath_in_function2);
                //string test_filePath = "U:/Users/yizhou.li@vanderbilt.edu/AppData/Local/Packages/Eyerecorder_pzq3xp76mxafg/LocalState/position_Cursor_IO.csv";
                //Debug.Log("setRecording is " + setRecording);
                try
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath_in_function2, true))
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@test_filePath, true))
                    {
                        //file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx+ "," + ry + "," + rz + "," + "," + cpx + "," + cpy + "," + cpz + "," + crw + "," + crx + "," + cry + "," + crz);
                        file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx + "," + ry + "," + rz + "," + timeStamp + "," + unixTime_String);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to save:  ", ex);
                }
            }
        }
    }
}
