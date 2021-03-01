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

#if !UNITY_EDITOR
        public static WebcamARFoundationSetting s_RuntimeInstance = null;
#endif

        public enum Requirement
        {
            Required,
            Optional,
        }

        [SerializeField] Requirement m_Requirement = Requirement.Required;

        void Awake()
        {
#if !UNITY_EDITOR
            s_RuntimeInstance = this;
#endif
        }

    }
}
