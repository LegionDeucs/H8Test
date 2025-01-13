using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BootstrapSceneLoader
{
    private const string BOOT_SCENE_ENABLE_ENABLED_KEY = "StartFromBootSceneEnabled";

    public static bool StartFromBoot { get => EditorPrefs.GetBool(BOOT_SCENE_ENABLE_ENABLED_KEY, false); set => EditorPrefs.SetBool(BOOT_SCENE_ENABLE_ENABLED_KEY, value); }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadDefaultScene()
    {
        if (DontStartFromBoot())
            return;

        DisableCurrentOpenedSceneMonoBehaviors();
        EditorSceneManager.LoadScene(0);
    }

    private static void DisableCurrentOpenedSceneMonoBehaviors()
    {
        MonoBehaviour[] monoBehaviours = Object.FindObjectsOfType<MonoBehaviour>();
        foreach (var mono in monoBehaviours)
            mono.SetInactive();
    }

    private static bool DontStartFromBoot() =>
        !StartFromBoot || EditorSceneManager.GetActiveScene().buildIndex == 0;
}