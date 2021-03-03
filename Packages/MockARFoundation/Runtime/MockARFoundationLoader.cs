using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.XR.Management;
#endif

using MockARFoundation.Internal;

namespace MockARFoundation
{

#if UNITY_EDITOR
    [XRSupportedBuildTarget(BuildTargetGroup.Standalone, new BuildTarget[] { BuildTarget.StandaloneOSX, BuildTarget.StandaloneWindows64 })]
#endif
    public class MockARFoundationLoader : XRLoaderHelper
    {
        static List<XRSessionSubsystemDescriptor> s_SessionSubsystemDescriptors = new List<XRSessionSubsystemDescriptor>();
        static List<XRCameraSubsystemDescriptor> s_CameraSubsystemDescriptors = new List<XRCameraSubsystemDescriptor>();
        // static List<XRInputSubsystemDescriptor> s_InputSubsystemDescriptors = new List<XRInputSubsystemDescriptor>();

        public override bool Initialize()
        {
            // if (!Application.isPlaying) return false;
#if UNITY_EDITOR
            CreateSubsystem<XRSessionSubsystemDescriptor, XRSessionSubsystem>(s_SessionSubsystemDescriptors, MockSessionSubsystem.ID);
            CreateSubsystem<XRCameraSubsystemDescriptor, XRCameraSubsystem>(s_CameraSubsystemDescriptors, MockCameraSubsystem.ID);
            // CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(s_InputSubsystemDescriptors, "ARKit-Input");
#endif

            // Debug.Log("MockARFoundation Initialize");

            var sessionSubsystem = GetLoadedSubsystem<XRSessionSubsystem>();
            if (sessionSubsystem == null)
            {
                Debug.LogError("Failed to load session subsystem.");
            }
            return sessionSubsystem != null;
        }

        public override bool Start()
        {
            return true;
        }

        public override bool Stop()
        {
            return true;
        }

        public override bool Deinitialize()
        {
            if (!Application.isPlaying)
            {
                // return base.Deinitialize();
            };

#if UNITY_EDITOR
            DestroySubsystem<XRCameraSubsystem>();
            // DestroySubsystem<XRInputSubsystem>();
            DestroySubsystem<XRSessionSubsystem>();
#endif

            // Debug.Log("MockARFoundation Deinitialize");
            return base.Deinitialize();
        }
    }
}
