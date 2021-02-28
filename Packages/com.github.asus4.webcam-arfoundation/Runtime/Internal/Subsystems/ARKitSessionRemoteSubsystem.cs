﻿using System;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace ARKitStream.Internal
{
    [Preserve]
    public class ARKitSessionRemoteSubsystem : XRSessionSubsystem
    {
        public const string ID = "ARKit-Remote-Session";
        // protected override Provider CreateProvider() => new ARKitRemoteProvider();

#if !UNITY_2020_2_OR_NEWER
        protected override Provider CreateProvider() => new ARKitRemoteProvider();
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_EDITOR
            XRSessionSubsystemDescriptor.RegisterDescriptor(new XRSessionSubsystemDescriptor.Cinfo
            {
                id = ID,
#if UNITY_2020_2_OR_NEWER
                providerType = typeof(ARKitSessionRemoteSubsystem.ARKitRemoteProvider),
                subsystemTypeOverride = typeof(ARKitSessionRemoteSubsystem),
#else
                subsystemImplementationType = typeof(ARKitSessionRemoteSubsystem),
#endif
                supportsInstall = false,
                supportsMatchFrameRate = false
            });
            Debug.Log($"Register {ID} subsystem");
#endif // UNITY_EDITOR
        }

        class ARKitRemoteProvider : Provider
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
                    var receiver = ARKitReceiver.Instance;
                    if (receiver != null)
                    {
                        return receiver.trackingState;
                    }
                    return TrackingState.None;
                }
            }


        }
    }
}
