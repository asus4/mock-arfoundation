using System;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace WebcamARFoundation.Internal
{
    [Preserve]
    public class WebcamSessionSubsystem : XRSessionSubsystem
    {
        public const string ID = "Webcam-Session";
        // protected override Provider CreateProvider() => new ARKitRemoteProvider();

#if !UNITY_2020_2_OR_NEWER
        protected override Provider CreateProvider() => new WebcamProvider();
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_EDITOR
            XRSessionSubsystemDescriptor.RegisterDescriptor(new XRSessionSubsystemDescriptor.Cinfo
            {
                id = ID,
#if UNITY_2020_2_OR_NEWER
                providerType = typeof(WebcamSessionSubsystem.WebcamProvider),
                subsystemTypeOverride = typeof(WebcamSessionSubsystem),
#else
                subsystemImplementationType = typeof(WebcamSessionSubsystem),
#endif
                supportsInstall = false,
                supportsMatchFrameRate = false
            });
            Debug.Log($"Register {ID} subsystem");
#endif // UNITY_EDITOR
        }

        class WebcamProvider : Provider
        {
            public override Promise<SessionAvailability> GetAvailabilityAsync()
            {
                var flag = SessionAvailability.Supported | SessionAvailability.Installed;
                return Promise<SessionAvailability>.CreateResolvedPromise(flag);
            }

            public override TrackingState trackingState
            {
                get
                {
                    return TrackingState.None;
                }
            }


        }
    }
}
