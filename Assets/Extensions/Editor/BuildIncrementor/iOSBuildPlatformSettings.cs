using System;
using UnityEditor;

namespace MyCore.BuildIncrementor
{
    [Serializable]
    public class iOSBuildPlatformSettings : BuildPlatformSettings
    {
        public override void UpdateBuildVersion()
        {
            base.UpdateBuildVersion();
#if UNITY_IOS
            PlayerSettings.bundleVersion = BuildVersion;
            PlayerSettings.iOS.buildNumber = BuildNumber.ToString();
#endif
        }
    }
}
