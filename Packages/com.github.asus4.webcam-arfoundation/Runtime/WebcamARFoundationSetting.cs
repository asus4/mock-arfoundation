using UnityEngine;
using UnityEngine.XR.Management;

using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;


namespace WebcamARFoundation
{

    [System.Serializable]
    [XRConfigurationData("Webcam ARFoundation", "WebcamARFoundation.WebcamARFoundationSetting")]
    public class WebcamARFoundationSetting : ScriptableObject
    {
        [System.Serializable]
        public struct Resolution
        {
            public int width;
            public int height;
            public int refreshRate;

            public override string ToString()
            {
                return $"{width} x {height} @{refreshRate}Hz";
            }

            public static implicit operator UnityEngine.Resolution(Resolution r)
            {
                return new UnityEngine.Resolution()
                {
                    width = r.width,
                    height = r.height,
                    refreshRate = r.refreshRate,
                };
            }

            public static implicit operator Resolution(UnityEngine.Resolution r)
            {
                return new Resolution()
                {
                    width = r.width,
                    height = r.height,
                    refreshRate = r.refreshRate,
                };
            }
        }

        public static WebcamARFoundationSetting Instance { get; private set; }

        [WebcamName] public string preferredCamera = "";
        public Resolution preferredResolution = new Resolution()
        {
            width = 1920,
            height = 1080,
            refreshRate = 30,
        };

        private void OnEnable()
        {
            Instance = this;
        }

        public WebCamTexture GetWebCamTexture()
        {
            return new WebCamTexture(preferredCamera, preferredResolution.width, preferredResolution.height, preferredResolution.refreshRate);
        }

    }
}
