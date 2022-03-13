using System;
using UnityEngine;
using Unity.Collections;
using UnityEngine.Rendering;
#if MODULE_URP_ENABLED
using UnityEngine.Rendering.Universal;
#endif
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;



namespace MockARFoundation.Internal
{
    [Preserve]
    public sealed class MockCameraSubsystem : XRCameraSubsystem
    {
        public const string ID = "Mock-subsystem";

#if !UNITY_2020_2_OR_NEWER
        protected override Provider CreateProvider() => new MockProvider();
#endif




        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
#if UNITY_EDITOR
            XRCameraSubsystemCinfo cameraSubsystemCinfo = new XRCameraSubsystemCinfo
            {
                id = ID,
#if UNITY_2020_2_OR_NEWER
                providerType = typeof(MockCameraSubsystem.MockProvider),
                subsystemTypeOverride = typeof(MockCameraSubsystem),
#else
                implementationType = typeof(MockCameraSubsystem),
#endif

                supportsAverageBrightness = false,
                supportsAverageColorTemperature = true,
                supportsColorCorrection = false,
                supportsDisplayMatrix = true,
                supportsProjectionMatrix = true,
                supportsTimestamp = true,
                supportsCameraConfigurations = true,
                supportsCameraImage = true,
                supportsAverageIntensityInLumens = true,
                supportsFocusModes = true,
                supportsFaceTrackingAmbientIntensityLightEstimation = true,
                supportsFaceTrackingHDRLightEstimation = true,
                supportsWorldTrackingAmbientIntensityLightEstimation = true,
                supportsWorldTrackingHDRLightEstimation = false,
            };

            if (!XRCameraSubsystem.Register(cameraSubsystemCinfo))
            {
                Debug.LogErrorFormat("Cannot register the {0} subsystem", ID);
            }
            else
            {
                Debug.LogFormat("Registered the {0} subsystem", ID);
            }
#endif // UNITY_EDITOR
        }

        class MockProvider : Provider
        {
            static readonly int _TEXTURE_MAIN = Shader.PropertyToID("_MainTex");

            private IMockCamera mockCamera;
            private Material m_CameraMaterial;
            public override Material cameraMaterial => m_CameraMaterial;

            public override bool permissionGranted => true;

            private static string ShaderName
            {
                get
                {
                    var pipeline = GraphicsSettings.renderPipelineAsset;
                    if (pipeline == null)
                    {
                        return "Unlit/WebcamBackground";
                    }
#if MODULE_URP_ENABLED
                    else if (pipeline is UniversalRenderPipelineAsset)
                    {
                        return "Unlit/WebcamBackground";
                    }
#endif
                    Debug.LogError($"{pipeline} is not supported in ARKit");
                    return "Unlit/WebcamBackground";
                }
            }

            public MockProvider()
            {
                // m_CameraMaterial = CreateCameraMaterial(ShaderName);
            }

            public override void Start()
            {
                base.Start();
                if (m_CameraMaterial == null)
                {
                    m_CameraMaterial = CreateCameraMaterial(ShaderName);
                }

                var setting = MockARFoundationSetting.Instance;
                mockCamera = setting.GetMockCamera();
            }

            public override void Destroy()
            {
                mockCamera?.Dispose();
                mockCamera = null;

                if (m_CameraMaterial != null)
                {
                    UnityEngine.Object.Destroy(m_CameraMaterial);
                    m_CameraMaterial = null;
                }
                base.Destroy();
            }

            public override Feature currentCamera => Feature.AnyCamera;

            public override Feature requestedCamera
            {
                get => Feature.AnyCamera;
                set
                {
                    // Debug.Log($"requestedCamera: {value}")
                }
            }

            public override bool TryGetFrame(XRCameraParams cameraParams, out XRCameraFrame cameraFrame)
            {
                if (!Application.isPlaying || !mockCamera.isPrepared)
                {
                    cameraFrame = default(XRCameraFrame);
                    return false;
                }

                const XRCameraFrameProperties properties =
                    XRCameraFrameProperties.Timestamp
                    // | XRCameraFrameProperties.ProjectionMatrix
                    | XRCameraFrameProperties.DisplayMatrix;


                Matrix4x4 displayMatrix = GetDisplayTransform(
                    (float)mockCamera.texture.width / mockCamera.texture.height,
                    (float)Screen.width / Screen.height
                );

                cameraFrame = (XRCameraFrame)new CameraFrame()
                {
                    timestampNs = DateTime.Now.Ticks,
                    averageBrightness = 0,
                    averageColorTemperature = 0,
                    colorCorrection = default(Color),
                    projectionMatrix = Matrix4x4.identity,
                    displayMatrix = displayMatrix,
                    trackingState = TrackingState.Tracking,
                    nativePtr = new IntPtr(0),
                    properties = properties,
                    averageIntensityInLumens = 0,
                    exposureDuration = 0,
                    exposureOffset = 0,
                    mainLightIntensityLumens = 0,
                    mainLightColor = default(Color),
                    ambientSphericalHarmonics = default(SphericalHarmonicsL2),
                    cameraGrain = default(XRTextureDescriptor),
                    noiseIntensity = 0,
                };

                // Debug.Log(cameraFrame);
                return true;
            }

            public override bool autoFocusEnabled => true;

            public override bool autoFocusRequested
            {
                get => true;
                set
                {
                    // Debug.Log($"autoFocusRequested: {value}");
                }
            }

            public override Feature currentLightEstimation => Feature.AnyLightEstimation;
            public override Feature requestedLightEstimation
            {
                get => Feature.AnyLightEstimation;
                set
                {
                    // Debug.Log($"requestedLightEstimation: {value}");
                }
            }

            public override NativeArray<XRTextureDescriptor> GetTextureDescriptors(XRTextureDescriptor defaultDescriptor, Allocator allocator)
            {
                if (!Application.isPlaying || !mockCamera.isPrepared)
                {
                    return new NativeArray<XRTextureDescriptor>(0, allocator);
                }

                var arr = new NativeArray<XRTextureDescriptor>(1, allocator);
                arr[0] = new TextureDescriptor(mockCamera.texture, _TEXTURE_MAIN);

                return arr;
            }

            private static Matrix4x4 GetDisplayTransform(float srcAspect, float dstAspect)
            {
                Vector3 scale;
                Vector3 offset;

                if (srcAspect > dstAspect)
                {
                    float s = dstAspect / srcAspect;
                    offset = new Vector3((1f - s) / 2f, 0, 0);
                    scale = new Vector3(s, 1, 1);
                }
                else
                {
                    float s = srcAspect / dstAspect;
                    offset = new Vector3(0, (1f - s) / 2f, 0);
                    scale = new Vector3(1, s, 1);
                }
                return Matrix4x4.TRS(
                    offset,
                    Quaternion.identity,
                    scale
                );
            }
        }
    }
}
