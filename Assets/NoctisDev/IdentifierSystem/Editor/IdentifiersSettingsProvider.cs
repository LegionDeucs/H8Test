using System.IO;
using UnityEditor;
using UnityEngine;

namespace NoctisDev.IdentifierSystem
{
    public static class IdentifiersSettingsProvider
    {
        private const string DIRECTORY_PATH = "Assets/Plugins/IdentifiersSystem";
        private const string SETTINGS_PATH = DIRECTORY_PATH + "/IdentifierSettings.asset";
        
        public static IdentifiersSettings GetSettings()
        {
            IdentifiersSettings settings = AssetDatabase.LoadAssetAtPath<IdentifiersSettings>(SETTINGS_PATH);

            if (settings == null)
            {
                settings = CreateNewSettings();
            }

            return settings;
        }

        private static IdentifiersSettings CreateNewSettings()
        {
            IdentifiersSettings settings = ScriptableObject.CreateInstance<IdentifiersSettings>();
                
            if (!Directory.Exists(DIRECTORY_PATH)) 
                Directory.CreateDirectory(DIRECTORY_PATH);

            AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
            AssetDatabase.SaveAssets();
            return settings;
        }
    }
}