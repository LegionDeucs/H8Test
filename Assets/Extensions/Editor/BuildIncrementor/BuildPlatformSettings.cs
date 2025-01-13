using UnityEditor;
using UnityEngine;
using System;
using MyCore.EditorTools;

namespace MyCore.BuildIncrementor
{
    [Serializable]
    public class Version
    {
        [ReadOnly]
        public int Major = 1;
        [ReadOnly]
        public int Minor;
        [ReadOnly]
        public int Patch;

        public override string ToString()
        {
            return Major + "." + Minor + "." + Patch;
        }
    }

    [Serializable]
    public class BuildPlatformSettings
    {
        private const int MinVersion = 0, MaxVersion = 999;


        public Version Version;
        [ReadOnly]
        public string BuildVersion = "1.0.0";
        [ReadOnly]
        public int BuildNumber = 1;

        public virtual int MajorVersion
        {
            get { return Version.Major; }
            set
            {
                Version.Major = Mathf.Clamp(value, MinVersion, MaxVersion);
                Version.Minor = 0;
                Version.Patch = 0;
                UpdateBuildVersion();
            }
        }

        public virtual int MinorVersion
        {
            get { return Version.Minor; }
            set
            {
                Version.Minor = Mathf.Clamp(value, MinVersion, MaxVersion);
                Version.Patch = 0;
                UpdateBuildVersion();
            }
        }

        public virtual int PatchVersion
        {
            get { return Version.Patch; }
            set
            {
                Version.Patch = Mathf.Clamp(value, MinVersion, MaxVersion);
                UpdateBuildVersion();
            }
        }

        public virtual void IncreaseBuild()
        {
            BuildNumber++;
            UpdateBuildVersion();
        }

        public virtual void UpdateBuildVersion()
        {
            BuildVersion = Version.ToString();
        }
    }
}
