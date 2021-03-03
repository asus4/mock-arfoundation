using UnityEngine;
using UnityEngine.XR.Management;

using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using MockARFoundation.Internal;

namespace MockARFoundation
{

    [System.Serializable]
    [XRConfigurationData("Mock AR Foundation", "MockARFoundation.MockARFoundationSetting")]
    public class MockARFoundationSetting : ScriptableObject
    {
        public enum Source
        {
            WebCamera,
            Video,
        }

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

        [SerializeField]
        private Source source = Source.WebCamera;

        [SerializeField, WebcamName]
        private string preferredCamera = "";

        [SerializeField]
        private Resolution preferredResolution = new Resolution()
        {
            width = 1920,
            height = 1080,
            refreshRate = 30,
        };

        [SerializeField]
        private string videoPath;

        public Source CurrentSource => source;

        public static MockARFoundationSetting Instance { get; private set; }

        private void OnEnable()
        {
            Instance = this;
        }

        public IMockCamera GetMockCamera()
        {
            switch (source)
            {
                case Source.WebCamera:
                    return new WebCamMockCamera(preferredCamera, preferredResolution);
                case Source.Video:
                    return new VideoMockCamera(videoPath);
            }
            throw new System.NotImplementedException();
        }
    }
}
