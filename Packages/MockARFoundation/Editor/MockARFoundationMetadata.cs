using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.XR.Management.Metadata;

namespace MockARFoundation
{
    class MockARFoundationPackage : IXRPackage
    {

        class MockARFoundationLoaderMetadata : IXRLoaderMetadata
        {
            public string loaderName { get; set; }
            public string loaderType { get; set; }
            public List<BuildTargetGroup> supportedBuildTargets { get; set; }
        }

        class MockARFoundationPackageMetadata : IXRPackageMetadata
        {
            public string packageName { get; set; }
            public string packageId { get; set; }
            public string settingsType { get; set; }
            public List<IXRLoaderMetadata> loaderMetadata { get; set; }
        }

        private static IXRPackageMetadata s_Metadata = new MockARFoundationPackageMetadata()
        {
            packageName = "Mock AR Foundation",
            packageId = "com.github.asus4.mock-arfoundation",
            settingsType = typeof(MockARFoundationSetting).FullName,
            loaderMetadata = new List<IXRLoaderMetadata>()
            {
                new MockARFoundationLoaderMetadata()
                {
                    loaderName = "MockARFoundationLoader",
                    loaderType = typeof(MockARFoundationLoader).FullName,
                    supportedBuildTargets = new List<BuildTargetGroup>()
                    {
                        BuildTargetGroup.Standalone,
                    }
                }
            },
        };

        public IXRPackageMetadata metadata => s_Metadata;

        public bool PopulateNewSettingsInstance(ScriptableObject obj)
        {
            return true;
        }
    }
}
