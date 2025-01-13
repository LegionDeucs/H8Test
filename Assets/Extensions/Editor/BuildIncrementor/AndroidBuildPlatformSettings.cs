using UnityEditor;
using System;

namespace MyCore.BuildIncrementor
{
    [Serializable]
    public class AndroidBuildPlatformSettings : BuildPlatformSettings
    {
        public override void UpdateBuildVersion()
        {
            base.UpdateBuildVersion();
#if UNITY_ANDROID
            PlayerSettings.bundleVersion = BuildVersion;
            PlayerSettings.Android.bundleVersionCode = BuildNumber;
#endif
        }
    }
}
