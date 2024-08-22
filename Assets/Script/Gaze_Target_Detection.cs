using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gaze_Target_Detection : MonoBehaviour
{
    private static GameObject Gaze_Target1;
    private static GameObject Gaze_Target2;
    private static GameObject Gaze_Target3;
    private static GameObject Gaze_Target4;
    private static GameObject Gaze_Target5;
    private static Vector3 Position_Gaze_Target1;
    private static Vector3 Position_Gaze_Target2;
    private static Vector3 Position_Gaze_Target3;
    private static Vector3 Position_Gaze_Target4;
    private static Vector3 Position_Gaze_Target5;

    private static GameObject Gaze;
    private static Vector3 Gaze_Position;

    private static float offsets1_x;
    private static float offsets1_y;
    private static float offsets1_z;

    private static float offsets2_x;
    private static float offsets2_y;
    private static float offsets2_z;

    private static float offsets3_x;
    private static float offsets3_y;
    private static float offsets3_z;

    private static float offsets4_x;
    private static float offsets4_y;
    private static float offsets4_z;

    private static float offsets5_x;
    private static float offsets5_y;
    private static float offsets5_z;

    private static int time_count_center_cube = 0;
    private static int time_count_right_top_cube = 0;
    private static int time_count_right_bottom_cube = 0;
    private static int time_count_left_top_cube = 0;
    private static int time_count_left_bottom_cube = 0;

    private static string start_time;
    private static string start_date;

    private static Vector3 Head_direction;
    public GameObject Head;

    private static int cont_cube = 5;
    private static GameObject testsphere;
    
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("Target_center_followed")!= null)
        {
            Gaze_Target1 = GameObject.Find("Target_center_followed");
            Debug.Log("Find cube center");
            //time_count_center_cube++;
        }
        if (GameObject.Find("Target_right_top_followed") != null)
        {
            Gaze_Target2 = GameObject.Find("Target_right_top_followed");
            Debug.Log("Find cube right top");
        }
        if (GameObject.Find("Target_right_bottom_followed") != null)
        {
            Gaze_Target3 = GameObject.Find("Target_right_bottom_followed");
            Debug.Log("Find cube right bottom");
        }
        if (GameObject.Find("Target_left_top_followed") != null)
        {
            Gaze_Target4 = GameObject.Find("Target_left_top_followed");
            Debug.Log("Find cube left top");
        }
        if (GameObject.Find("Target_left_bottom_followed") != null)
        {
            Gaze_Target5 = GameObject.Find("Target_left_bottom_followed");
            Debug.Log("Find cube left bottom");
        }
        if (GameObject.Find("DefaultGazeCursor(Clone)") != null)
        {
            Gaze = GameObject.Find("DefaultGazeCursor(Clone)");
            Debug.Log("Find Gaze(gaze/target detection)");
        }
        else
        {
            Debug.Log("Didn't Find gaze(target detection)");
        }
        start_time = DateTime.Now.ToLocalTime().ToString("HHmmss");
        start_date = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Target_center_followed") != null)
        {
            Gaze_Target1 = GameObject.Find("Target_center_followed");
            Position_Gaze_Target1 = Gaze_Target1.transform.position;
            //Debug.Log("Find cube center");
            //Debug.Log("cube center x,y,z=" + Position_Gaze_Target1.x + Position_Gaze_Target1.y + Position_Gaze_Target1.z);
        }
        if (GameObject.Find("Target_right_top_followed") != null)
        {
            Gaze_Target2 = GameObject.Find("Target_right_top_followed");
            Position_Gaze_Target2 = Gaze_Target2.transform.position;
            //Debug.Log("Find cube right top");
        }
        if (GameObject.Find("Target_right_bottom_followed") != null)
        {
            Gaze_Target3 = GameObject.Find("Target_right_bottom_followed");
            Position_Gaze_Target3 = Gaze_Target3.transform.position;
            //Debug.Log("Find cube right bottom");
        }
        if (GameObject.Find("Target_left_top_followed") != null)
        {
            Gaze_Target4 = GameObject.Find("Target_left_top_followed");
            Position_Gaze_Target4 = Gaze_Target4.transform.position;
            //Debug.Log("Find cube left top");
        }
        if (GameObject.Find("Target_left_bottom_followed") != null)
        {
            Gaze_Target5 = GameObject.Find("Target_left_bottom_followed");
            Position_Gaze_Target5 = Gaze_Target5.transform.position;
            //Debug.Log("Find cube left bottom");
        }
        if (GameObject.Find("DefaultGazeCursor(Clone)") != null)
        {
            Gaze = GameObject.Find("DefaultGazeCursor(Clone)");
            Debug.Log("Find Gaze(gaze/target detection)");
            Gaze_Position = Gaze.transform.position;
            //Debug.Log("Gaze_Position x = " + Gaze_Position.x + " y = " + Gaze_Position.y + " z = " + Gaze_Position.z);
        }
        else
        {
            Debug.Log("Didn't Find gaze(target detection)");
        }

        //1
        if (Gaze_Position != null && Position_Gaze_Target1 != null && time_count_center_cube != -1)
        {
            offsets1_x = Math.Abs(Gaze_Position.x - Position_Gaze_Target1.x);
            offsets1_y = Math.Abs(Gaze_Position.y - Position_Gaze_Target1.y);
            offsets1_z = Math.Abs(Gaze_Position.z - Position_Gaze_Target1.z);
            //Debug.Log("offsets1 x = " + offsets1_x + " y = " + offsets1_y + " z = " + offsets1_z);
            if (offsets1_x <= 0.045 && offsets1_y <=0.045 && offsets1_z <= 0.025)
            {
                Debug.Log("In range(Target_center_followed)");
                time_count_center_cube++;
                Save_target_offset(Gaze, Head, Gaze_Target1);
                if (time_count_center_cube >= 40)
                {
                    Destroy(Gaze_Target1);
                    time_count_center_cube = -1;
                    cont_cube = cont_cube - 1;
                }
            }
        }

        //2
        if (Gaze_Position != null && Position_Gaze_Target2 != null && time_count_right_top_cube != -1)
        {
            offsets2_x = Math.Abs(Gaze_Position.x - Position_Gaze_Target2.x);
            offsets2_y = Math.Abs(Gaze_Position.y - Position_Gaze_Target2.y);
            offsets2_z = Math.Abs(Gaze_Position.z - Position_Gaze_Target2.z);
            //Debug.Log("offsets2 x = " + offsets2_x + " y = " + offsets2_y + " z = " + offsets2_z);
            if (offsets2_x <= 0.045 && offsets2_y <= 0.045 && offsets2_z <= 0.025)
            {
                Debug.Log("In range(Target_right_top_followed)");
                time_count_right_top_cube++;
                Save_target_offset(Gaze, Head, Gaze_Target2);
                if (time_count_right_top_cube >= 40)
                {
                    Destroy(Gaze_Target2);
                    time_count_right_top_cube = -1;
                    cont_cube = cont_cube - 1;
                }
            }
        }

        //3
        if (Gaze_Position != null && Position_Gaze_Target3 != null && time_count_right_bottom_cube != -1)
        {
            offsets3_x = Math.Abs(Gaze_Position.x - Position_Gaze_Target3.x);
            offsets3_y = Math.Abs(Gaze_Position.y - Position_Gaze_Target3.y);
            offsets3_z = Math.Abs(Gaze_Position.z - Position_Gaze_Target3.z);
            //Debug.Log("offsets3 x = " + offsets3_x + " y = " + offsets3_y + " z = " + offsets3_z);
            if (offsets3_x <= 0.045 && offsets3_y <= 0.045 && offsets3_z <= 0.025)
            {
                Debug.Log("In range(Target_right_bottom_followed)");
                time_count_right_bottom_cube++;
                Save_target_offset(Gaze, Head, Gaze_Target3);
                if (time_count_right_bottom_cube >= 40)
                {
                    Destroy(Gaze_Target3);
                    time_count_right_bottom_cube = -1;
                    cont_cube = cont_cube - 1;
                }
            }
        }

        //4
        if (Gaze_Position != null && Position_Gaze_Target4 != null && time_count_left_top_cube != -1)
        {
            offsets4_x = Math.Abs(Gaze_Position.x - Position_Gaze_Target4.x);
            offsets4_y = Math.Abs(Gaze_Position.y - Position_Gaze_Target4.y);
            offsets4_z = Math.Abs(Gaze_Position.z - Position_Gaze_Target4.z);
            //Debug.Log("offsets4 x = " + offsets4_x + " y = " + offsets4_y + " z = " + offsets4_z);
            if (offsets4_x <= 0.045 && offsets4_y <= 0.045 && offsets4_z <= 0.025)
            {
                Debug.Log("In range(Target_left_top_followed)");
                time_count_left_top_cube++;
                Save_target_offset(Gaze, Head, Gaze_Target4);
                if (time_count_left_top_cube >= 40)
                {
                    Destroy(Gaze_Target4);
                    time_count_left_top_cube = -1;
                    cont_cube = cont_cube - 1;
                }
            }
        }

        //5
        if (Gaze_Position != null && Position_Gaze_Target5 != null && time_count_left_bottom_cube != -1)
        {
            offsets5_x = Math.Abs(Gaze_Position.x - Position_Gaze_Target5.x);
            offsets5_y = Math.Abs(Gaze_Position.y - Position_Gaze_Target5.y);
            offsets5_z = Math.Abs(Gaze_Position.z - Position_Gaze_Target5.z);
            //Debug.Log("offsets5 x = " + offsets5_x + " y = " + offsets5_y + " z = " + offsets5_z);
            if (offsets5_x <= 0.045 && offsets5_y <= 0.045 && offsets5_z <= 0.025)
            {
                Debug.Log("In range(Target_left_bottom_followed)");
                time_count_left_bottom_cube++;
                Save_target_offset(Gaze, Head, Gaze_Target5);
                if (time_count_left_bottom_cube >= 40)
                {
                    Destroy(Gaze_Target5);
                    time_count_left_bottom_cube = -1;
                    cont_cube = cont_cube - 1;
                }
            }
        }
        if (cont_cube == 0)
        {
            testsphere = GameObject.Find("Sphere");
            testsphere.GetComponent<Renderer>().enabled = false;
        }
    }

    public static void Save_target_offset(GameObject gaze, GameObject head_, GameObject target)
    {
        float px = gaze.transform.position.x;
        float py = gaze.transform.position.y;
        float pz = gaze.transform.position.z;

        Quaternion temp_direction = head_.transform.rotation;
        Head_direction = temp_direction.eulerAngles;

        string[] name = target.name.Split('_');

        DateTime foo = DateTime.Now;
        long unixTime = ((DateTimeOffset)foo).ToUnixTimeMilliseconds();
        //unixTime = unixTime * (long)0.001;
        string unixTime_String = unixTime.ToString();
        //Debug.Log("UNIX_timeStamp" + unixTime_String);
        string timeStamp = DateTime.Now.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
        //Debug.Log("timeStamp" + timeStamp);
        string filepath_in_function2 = Application.persistentDataPath + "/Target_" + name[1] + "_" + name[2] + "_" + start_date + "_" + start_time + ".csv";
        Debug.Log("filepath" + filepath_in_function2);
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath_in_function2, true))
            {
                file.WriteLine(px + "," + py + "," + pz + "," + Head_direction.x + "," + Head_direction.y + "," + Head_direction.z + "," + timeStamp + "," + unixTime_String);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to save:  ", ex);
        }
    }

    public static void Save_head_direction(GameObject head)
    {
        Quaternion temp_direction = head.transform.rotation;
        Head_direction = temp_direction.eulerAngles;
        Debug.Log("head direction x = " + Head_direction.x + " y = " + Head_direction.y + " z = " + Head_direction.z);
        DateTime foo = DateTime.Now;
        long unixTime = ((DateTimeOffset)foo).ToUnixTimeMilliseconds();
        string unixTime_String = unixTime.ToString();
        string timeStamp = DateTime.Now.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
        string filepath_in_function3 = Application.persistentDataPath + "/Head_Direction_" + start_date + "_" + start_time + ".csv";
        Debug.Log("filepath" + filepath_in_function3);
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath_in_function3, true))
            {
                //file.WriteLine(px + "," + py + "," + pz + "," + rw + "," + rx+ "," + ry + "," + rz + "," + "," + cpx + "," + cpy + "," + cpz + "," + crw + "," + crx + "," + cry + "," + crz);
                file.WriteLine(Head_direction.x + "," + Head_direction.y + "," + Head_direction.z + "," + temp_direction.w + "," + temp_direction.x + "," + temp_direction.y + "," + temp_direction.z + "," + timeStamp + "," + unixTime_String);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to save:  ", ex);
        }
    }
}
