using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class IMURender : MonoBehaviour
{
    public TMP_Text accelRender;
    public TMP_Text gyroRender;
    Gyroscope gyro;

    // Start is called before the first frame update
    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
        gyro.updateInterval = 0.005f;
    }

    public void GetStopWatchInfo()
    {
        accelRender.SetText(
            Application.persistentDataPath
        );
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.accelerationEventCount > 0)
        //{
        //    // Sync gyro updates with accelerometer updates
        //    float rot_x = gyro.rotationRateUnbiased.x;
        //    float rot_y = gyro.rotationRateUnbiased.y;
        //    float rot_z = gyro.rotationRateUnbiased.z;
        //    float grav_x = gyro.gravity.x;
        //    float grav_y = gyro.gravity.y;
        //    float grav_z = gyro.gravity.z;
        //    gyroRender.SetText("Rotation\nX: " + rot_x + "\nY: " + rot_y + "\nZ: " + rot_z + "\n\nGravity\nX: " + grav_x + "\nY: " + grav_y + "\nZ: " + grav_z);
        //}
        //else
        //{
        //    accelRender.SetText("No data available.");
        //}
    }
}
