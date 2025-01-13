using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace MyCore.BuildIncrementor
{
    public class IncreaseBuildNumber : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            BuildVersionSettings.Instance.CurrentPlatform.IncreaseBuild();
            BuildVersionSettings.Instance.Save();
        }
    }
}