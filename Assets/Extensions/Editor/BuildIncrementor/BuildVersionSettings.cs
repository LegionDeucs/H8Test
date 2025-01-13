using UnityEngine;
using UnityEditor;
using MyCore.EditorTools;

namespace MyCore.BuildIncrementor
{
    [InitializeOnLoad]
    public class BuildVersionSettings : ResourceSingleton<BuildVersionSettings>
    {
        [SerializeField]
        private AndroidBuildPlatformSettings android;
        [SerializeField]
        private iOSBuildPlatformSettings iOS;

        public BuildPlatformSettings CurrentPlatform
        {
            get
            {
#if UNITY_ANDROID
                return android;
#elif UNITY_IOS
                return IOS;
#else
                return new BuildPlatformSettings();
#endif
            }
        }

        public AndroidBuildPlatformSettings Android => android;
        public iOSBuildPlatformSettings IOS => iOS;

        private void OnValidate()
        {
            UpdatePlatformVersionNumber();
        }

        private void Reset()
        {
            UpdatePlatformVersionNumber();
        }

        protected override void OnInstanceLoaded()
        {
            UpdatePlatformVersionNumber();
        }

        private void UpdatePlatformVersionNumber()
        {
            android?.UpdateBuildVersion();
            iOS?.UpdateBuildVersion();
            Save();
        }
    }
}
