using System.Linq;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.XR.Management;

namespace WebcamARFoundation
{
    public class ARKitStreamBuildProcessor : XRBuildHelper<WebcamARFoundationSetting>
    {
        public override string BuildSettingsKey => "com.github.asus4.webcam-arfoundation.setting";

        public override void OnPreprocessBuild(BuildReport report)
        {
            base.OnPreprocessBuild(report);
        }

        public override void OnPostprocessBuild(BuildReport report)
        {
            base.OnPostprocessBuild(report);
        }
    }
}
