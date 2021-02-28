﻿using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;
using UnityXRHumanBody = UnityEngine.XR.ARSubsystems.XRHumanBody;
using UnityTrackableId = UnityEngine.XR.ARSubsystems.TrackableId;

namespace ARKitStream.Internal
{
    [Preserve]
    public class ARKitHumanBodyRemoteSubsystem : XRHumanBodySubsystem
    {
        public const string ID = "ARKit-HumanBody-Remote";

#if !UNITY_2020_2_OR_NEWER
        protected override Provider CreateProvider() => new ARKitRemoteProvider();
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
#if UNITY_EDITOR
            XRHumanBodySubsystemCinfo humanBodySubsystemCinfo = new XRHumanBodySubsystemCinfo()
            {
                id = ID,
#if UNITY_2020_2_OR_NEWER
                providerType = typeof(ARKitHumanBodyRemoteSubsystem.ARKitRemoteProvider),
                subsystemTypeOverride = typeof(ARKitHumanBodyRemoteSubsystem),
#else
                implementationType = typeof(ARKitHumanBodyRemoteSubsystem),
#endif
                supportsHumanBody2D = true,
                supportsHumanBody3D = true,
                supportsHumanBody3DScaleEstimation = true,
            };

            if (XRHumanBodySubsystem.Register(humanBodySubsystemCinfo))
            {
                Debug.LogFormat("Registerd the {0} subsystem", ID);
            }
            else
            {
                Debug.LogErrorFormat("Cannot register the {0} subsystem", ID);
            }
#endif // UNITY_EDITOR
        }



        class ARKitRemoteProvider : XRHumanBodySubsystem.Provider
        {
            TrackableChangesModifier<UnityXRHumanBody> modifier = new TrackableChangesModifier<UnityXRHumanBody>();

            public ARKitRemoteProvider() { }

            public override void Start() { }

            public override void Stop() { }

            public override void Destroy()
            {
                modifier.Dispose();
            }


            public override bool pose2DRequested
            {
                get => true;
                set => Debug.Log($"pose2DRequested: {value}");
            }
            public override bool pose2DEnabled => true;


            public override bool pose3DRequested
            {
                get => true;
                set => Debug.Log($"pose3DRequested: {value}");
            }
            public override bool pose3DEnabled => true;

            public override bool pose3DScaleEstimationRequested
            {
                get => true;
                set => Debug.Log($"pose3DScaleEstimationEnabled: {value}");
            }

            public override TrackableChanges<UnityXRHumanBody> GetChanges(UnityXRHumanBody defaultHumanBody, Allocator allocator)
            {
                var info = ARKitReceiver.Instance?.HumanBody;
                if (info == null)
                {
                    return new TrackableChanges<UnityXRHumanBody>();
                }

                var added = info.added.Select(o => (UnityXRHumanBody)o).ToList();
                var updated = info.updated.Select(o => (UnityXRHumanBody)o).ToList();
                var removed = info.removed.Select(id => (UnityTrackableId)id).ToList();

                return modifier.Modify(added, updated, removed, allocator);
            }

            public override void GetSkeleton(UnityTrackableId trackableId, Allocator allocator, ref NativeArray<XRHumanBodyJoint> skeleton)
            {
                var info = ARKitReceiver.Instance?.HumanBody;
                if (info == null)
                {
                    return;
                }

                byte[] bytes;
                if (!info.joints.TryGetValue(trackableId, out bytes))
                {
                    Debug.LogWarning($"Skeleton ID: {trackableId} not found");
                    return;
                }

                NativeArrayExtension.CopyFromRawBytes(bytes, allocator, ref skeleton);
            }

        }

    }
}
