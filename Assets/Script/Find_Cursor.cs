using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.MixedReality.Toolkit;
using System.Text;
using System.Linq;
using TMPro;

#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class Find_Cursor : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameObject Cursor;
    private static Vector3 Cursor_position;
    private static Vector3 Head_position;
    private static Quaternion Cursor_rotation;
    private static Vector3 Head_direction;
    private static string start_time;
    private static string start_date;
    private GameObject VideoPlane_Sphere;//this sphere is the children of the video plane
    private static Vector3 Sphere_Worldpos;
    private static Vector3 Sphere_Localpos;

    public GameObject Head;
    public GameObject testsphere;
    public TMP_Text Text3D;
    public GameObject MyPrefab;
    

    //public TMP_Text TMP_showText;
    //public Camera Main_Camera;
    //private static Vector3 Camera_position;
    //private static Quaternion Camera_rotation;
    void Start()
    {
        VideoPlane_Sphere = GameObject.Find("VideoPlane_Sphere");
        Cursor = GameObject.Find("DefaultGazeCursor(Clone)");
        if (Cursor != null)
        {
            //Debug.Log("Find gaze");
            //TMP_showText.text = Cursor.transform.position.ToString();
        }
        else
        {
            //Debug.Log("Didn't Find gaze");
        }
        //Instantiate(MyPrefab, new Vector3(0, 0, 1), Quaternion.identity);
        start_time = DateTime.Now.ToLocalTime().ToString("HHmmss");
        start_date = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
        //Debug.Log("start_time" + start_time);
        //Debug.Log("start_date" + start_date);
    }

    // Update is called once per frame
    void Update()
    {
        VideoPlane_Sphere = GameObject.Find("VideoPlane_Sphere");
        //Cursor = GameObject.Find("DefaultGazeCursor(Clone)");
        if (Cursor != null)
        {
            //Debug.Log("Find gaze(sphere followed)");
            //TMP_showText.text = Cursor.transform.position.ToString();
            Cursor_position = Cursor.transform.position;
            Cursor_rotation = Cursor.transform.rotation;
            testsphere.transform.position = Cursor_position;
            testsphere.transform.rotation = Cursor_rotation;
            //Camera_position = Main_Camera.transform.position;
            //Camera_rotation = Main_Camera.transform.rotation;
            //Save_position_IO(Cursor_position.x, Cursor_position.y, Cursor_position.z, Cursor_rotation.w, Cursor_rotation.x, Cursor_rotation.y, Cursor_rotation.z,
            //    Camera_position.x, Camera_position.y, Camera_position.z, Camera_rotation.w, Camera_rotation.x, Camera_rotation.y, Camera_rotation.z);
            Save_gaze_data(Cursor_position.x, Cursor_position.y, Cursor_position.z, Cursor_rotation.w, Cursor_rotation.x, Cursor_rotation.y, Cursor_rotation.z);
            Save_head_data(Head);
        }
        else
        {
            //Debug.Log("Didn't Find gaze");
        }
        if(VideoPlane_Sphere!=null)
        {
            VideoPlane_Sphere.transform.position = Cursor_position;//world coordinate
            VideoPlane_Sphere.transform.rotation = Cursor_rotation;
            Save_VideoPlane_Sphere_data(VideoPlane_Sphere);
        }
        else
        {
            //Debug.Log("Didn't Find VideoPlane_Sphere");
        }
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
        //Debug.Log("timeStamp" + timeStamp);
        string filepath_in_function2 = Application.persistentDataPath + "/Eye_Gaze_Coordinates_" + start_date + "_" + start_time + ".csv";
        //Debug.Log("filepath" + filepath_in_function2);
        //string test_filePath = "U:/Users/yizhou.li@vanderbilt.edu/AppData/Local/Packages/Eyerecorder_pzq3xp76mxafg/LocalState/position_Cursor_IO.csv";
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

        //string test_filePath_invideo = "U:/Users/yizhou.li@vanderbilt.edu/Videos/position_Cursor_IO.csv";
        //try
        //{
        //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@test_filePath_invideo, true))
        //    {
        //        //file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx+ "," + ry + "," + rz + "," + "," + cpx + "," + cpy + "," + cpz + "," + crw + "," + crx + "," + cry + "," + crz);
        //        file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx + "," + ry + "," + rz);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new ApplicationException("Failed to save:  ", ex);
        //}
    }

    public static void Save_head_data(GameObject head)
    {
        Quaternion temp_direction = head.transform.rotation;
        Head_direction = temp_direction.eulerAngles;
        Head_position = head.transform.position;
        //Debug.Log("head direction x = " + Head_direction.x + " y = " + Head_direction.y + " z = " + Head_direction.z);
        DateTime foo = DateTime.Now;
        long unixTime = ((DateTimeOffset)foo).ToUnixTimeMilliseconds();
        string unixTime_String = unixTime.ToString();
        string timeStamp = DateTime.Now.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
        string filepath_in_function3 = Application.persistentDataPath + "/Head_Direction_" + start_date + "_" + start_time + ".csv";
        //Debug.Log("filepath" + filepath_in_function3);
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath_in_function3, true))
            {
                //file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx+ "," + ry + "," + rz + "," + "," + cpx + "," + cpy + "," + cpz + "," + crw + "," + crx + "," + cry + "," + crz);
                file.WriteLine(Head_direction.x + "," + Head_direction.y + "," + Head_direction.z + "," + Head_position.x + "," + Head_position.y + "," + Head_position.z + "," + temp_direction.w + "," + temp_direction.x + "," + temp_direction.y + "," + temp_direction.z + "," + timeStamp + "," + unixTime_String);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to save:  ", ex);
        }
    }

    public static void Save_VideoPlane_Sphere_data(GameObject Sphere)
    {
        DateTime foo = DateTime.Now;
        long unixTime = ((DateTimeOffset)foo).ToUnixTimeMilliseconds();
        string unixTime_String = unixTime.ToString();
        string timeStamp = DateTime.Now.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
        string filepath_in_function4 = Application.persistentDataPath + "/VideoPlane_Sphere_" + start_date + "_" + start_time + ".csv";
        Sphere_Worldpos = Sphere.transform.position;
        Sphere_Localpos = Sphere.transform.localPosition;
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath_in_function4, true))
            {
                //file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx+ "," + ry + "," + rz + "," + "," + cpx + "," + cpy + "," + cpz + "," + crw + "," + crx + "," + cry + "," + crz);
                file.WriteLine(Sphere_Worldpos.x + "," + Sphere_Worldpos.y + "," + Sphere_Worldpos.z + "," + Sphere_Localpos.x + "," + Sphere_Localpos.y + "," + Sphere_Localpos.z + "," + timeStamp + "," + unixTime_String);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to save:  ", ex);
        }
    }

}