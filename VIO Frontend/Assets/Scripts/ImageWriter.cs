using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;

public class ImageWriter : MonoBehaviour
{
    public ARCameraManager cameraManager;
    public int outputWidth;
    public int outputHeight;

    private void Start()
    {
        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            XRCpuImage.ConversionParams conversionParams = new XRCpuImage.ConversionParams
            {
                inputRect = new RectInt(0, 0, image.width, image.height),
                outputDimensions = new Vector2Int(image.width, image.height),
                outputFormat = TextureFormat.RGB24,
                transformation = XRCpuImage.Transformation.None
            };
            int size = image.GetConvertedDataSize(conversionParams);
            var buffer = new NativeArray<byte>(size, Allocator.Temp);
            int width = image.width;
            int height = image.height;
            image.Convert(
                conversionParams,
                new System.IntPtr(buffer.GetUnsafePtr()),
                buffer.Length
            );
            image.Dispose();

            //Process here
            long timestamp = System.Diagnostics.Stopwatch.GetTimestamp();
            var bytes = ImageConversion.EncodeNativeArrayToPNG(
                buffer,
                UnityEngine.Experimental.Rendering.GraphicsFormatUtility.GetGraphicsFormat(conversionParams.outputFormat, false),
                (uint)width,
                (uint)height
            );
            File.WriteAllBytes(Application.persistentDataPath + "/" + timestamp + ".png", bytes.ToArray());
            buffer.Dispose();
        }
    }
}
