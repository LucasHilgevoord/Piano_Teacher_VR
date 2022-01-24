using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        WebCamTexture camTexture = new WebCamTexture();
        RawImage rend = GetComponent<RawImage>();
        rend.material.mainTexture = camTexture;
        

        foreach (WebCamDevice device in devices)
        {
            if (device.name == "HTC Vive")
            {
                camTexture.deviceName = device.name;
                camTexture.Play();
                Debug.Log("play");
            }
        }


    }
}
