using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
public partial class SceneStaticData : ISerializationCallbackReceiver
{
    [SerializeField] private SceneAsset _sceneAsset;
    public void OnAfterDeserialize()
    {

    }
    public void OnBeforeSerialize()
    {
        if (_sceneAsset != null)
        {
            var newName = _sceneAsset.name;
            var newBuildIndex = SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(_sceneAsset));
            if (newName != _sceneName || newBuildIndex != _buildIndex)
            {
                _sceneName = newName; _buildIndex = newBuildIndex;
                SaveAsset();
            }
        }
    }
    private void SaveAsset()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
    }
}
#endif