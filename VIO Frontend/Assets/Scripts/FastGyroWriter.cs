using System.Threading;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using TMPro;

/// <summary>
/// This class shows how to make an Update loop on a separate thread that is faster
/// than 60fps
/// </summary>
public class FastGyroWriter : MonoBehaviour
{
    Gyroscope gyro;
    public string filename = "data.csv";
    public TMP_Text debugRender;
    string filepath;
    // This is our thread that we'll use to do our super fast update loop with
    private Thread _updateThread;
    private bool threadActive;
    Vector3 accel, rot;
    long count;
    bool gyroUpdate;

    
    private void Start()
    {
        Time.fixedDeltaTime = 0.002f;
        Time.maximumDeltaTime = 0.005f;
        // Enable gyroscope (required, otherwise it defaults to disabled and this won't work) and sample at 200Hz
        gyro = Input.gyro;
        gyro.updateInterval = 0.002f;
        gyro.enabled = true;
        count = 0;
        //if(!Permission.HasUserAuthorizedPermission(Permission.)
        filepath = Application.persistentDataPath;
    }

    private void FixedUpdate()
    {
        if(!gyroUpdate)
        {
            accel = gyro.gravity + gyro.userAcceleration;
            rot = gyro.rotationRateUnbiased;
            count++;
            gyroUpdate = true;
        }
    }

    public void TestFileWrite()
    {
        long start, stop;
        using (StreamWriter csvWriter = new StreamWriter(filepath + "/" + filename))
        {
            csvWriter.WriteLine(
                "Timestamp," +
                "Acceleration_X," +
                "Acceleration_Y," +
                "Acceleration_Z," +
                "Rotation_X," +
                "Rotation_Y," +
                "Rotation_Z"
            );
            start = System.Diagnostics.Stopwatch.GetTimestamp();
            long time;
            Vector3 accel, rot;
            time = System.Diagnostics.Stopwatch.GetTimestamp();
            accel = gyro.gravity + gyro.userAcceleration;
            rot = gyro.rotationRateUnbiased;

            // Write acceleration and rotation information to csv
            csvWriter.WriteLine(
                time + "," +
                accel.x + "," +
                accel.y + "," +
                accel.z + "," +
                rot.x + "," +
                rot.y + "," +
                rot.z
            );
            stop = System.Diagnostics.Stopwatch.GetTimestamp();
        }
        debugRender.SetText(
            "Time: " + (stop - start)
        );
    }

    public void StartCollection()
    {
        // Start the thread that will be the SuperFastLoop
        threadActive = true;
        gyroUpdate = false;
        _updateThread = new Thread(SuperFastLoop);
        _updateThread.Start();
    }

    public void StopCollection()
    {
        // Stop the thread when disabled, or it will keep running in the background
        // _updateThread.Abort();
        threadActive = false;
        // Waits for the Thread to stop
        _updateThread.Join();

        debugRender.SetText(
            "Exists: " + File.Exists(filepath + "/"  + filename)
        );
    }

    private void OnDisable()
    {
        StopCollection();
    }

    private void Update()
    {
        accel = gyro.gravity + gyro.userAcceleration;
        rot = gyro.rotationRateUnbiased;
        count++;
    }

    private void SuperFastLoop()
    {
        long time, threadCount;
        Vector3 threadAccel, threadRot;

        using (StreamWriter csvWriter = new StreamWriter(filepath + "/" + filename, false))
        {
            csvWriter.WriteLine(
                "Timestamp," +
                "Acceleration_X," +
                "Acceleration_Y," +
                "Acceleration_Z," +
                "Rotation_X," +
                "Rotation_Y," +
                "Rotation_Z," +
                "Count"
            );
        }

        // This begins our Update loop
        while (threadActive)
        {
            if(gyroUpdate)
            {
                threadAccel = accel;
                threadRot = rot;
                threadCount = count;
                gyroUpdate = false;
                time = System.Diagnostics.Stopwatch.GetTimestamp();
                using (StreamWriter csvWriter = new StreamWriter(filepath + "/" + filename, true))
                {
                    // Write acceleration and rotation information to csv
                    csvWriter.WriteLine(
                        time + "," +
                        threadAccel.x + "," +
                        threadAccel.y + "," +
                        threadAccel.z + "," +
                        threadRot.x + "," +
                        threadRot.y + "," +
                        threadRot.z + "," +
                        threadCount
                    );
                }
                // This suspends the thread for 5 milliseconds, making this code execute 200 times per second
                //Thread.Sleep(5);
            }
        }
    }
}
