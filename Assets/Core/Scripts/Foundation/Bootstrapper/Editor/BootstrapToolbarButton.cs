using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
[DefaultExecutionOrder(-100)]
public static class BootstrapToolbarButton
{
    private const string ENABLED_TEXT = "Start from boot scene";
    private const string DISABLED_TEXT = "Start from current scene";
    private const string BOOT_SCENE_ICON_NAME = "UnityLogo";

    private static bool _startFromBootScene;

    static BootstrapToolbarButton()
    {
        EditorApplication.update += Init;
    }

    private static void Init()
    {
        EditorApplication.update -= Init;
    }

    private static void OnToolbarGUI()
    {
        Texture image = EditorGUIUtility.IconContent(BOOT_SCENE_ICON_NAME).image;
        _startFromBootScene = GUILayout.Toggle(
            _startFromBootScene,
            new GUIContent(null, image, _startFromBootScene ? DISABLED_TEXT : ENABLED_TEXT),
            "Command"
        );

        BootstrapSceneLoader.StartFromBoot = _startFromBootScene;
    }

    [DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        _startFromBootScene = BootstrapSceneLoader.StartFromBoot;
    }
}
