using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MockARFoundation.Internal
{
    public class WebCamMockCamera : IMockCamera
    {
        public Texture texture => webCamTexture;
        public bool isPrepared => webCamTexture.width > 16;

        private WebCamTexture webCamTexture;

        public WebCamMockCamera(string preferredCameraName, Resolution preferredResolution)
        {
            string cameraName = GetWebCameraName(preferredCameraName);
            webCamTexture = new WebCamTexture(cameraName, preferredResolution.width, preferredResolution.height, preferredResolution.refreshRate);
            webCamTexture.Play();
        }

        public void Dispose()
        {
            webCamTexture?.Stop();
            webCamTexture = null;
        }

        private static string GetWebCameraName(string preferredName)
        {
            var devices = WebCamTexture.devices;

            // Found the exacty same name
            foreach (var device in devices)
            {
                if (device.name == preferredName)
                {
                    return preferredName;
                }
            }
            return devices[0].name;
        }
    }
}