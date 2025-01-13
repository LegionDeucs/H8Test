using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private SceneStaticData loadingScene;
    [SerializeField] private SceneStaticData metaScene;
    [SerializeField] private List<SceneStaticData> levelScenes;

    public void LoadLoadingScene(Action callback = null)
    {
        LoadSceneAsynk(loadingScene, LoadSceneMode.Single, callback);
    }

    public void LoadLevelScene(int levelIndex, Action callback = null)
    {
        LoadSceneAsynk(levelScenes[(int)Mathf.Repeat(levelIndex, levelScenes.Count)], LoadSceneMode.Single, callback);
    }

    public void LoadMetaScene(Action callback)
    {
        LoadSceneAsynk(metaScene, LoadSceneMode.Additive, callback);
    }

    public void UnloadMetaScene(Action callback)
    {
        UnloadSceneAsynk(metaScene, callback);
    }

    private void LoadSceneAsynk(SceneStaticData sceneData, LoadSceneMode sceneLoadMode, Action callback = null)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneData.BuildIndex, sceneLoadMode);

        asyncOperation.completed += AsyncOperation_completed;
        void AsyncOperation_completed(AsyncOperation operation)
        {
            operation.completed -= AsyncOperation_completed;
            callback?.Invoke();
        }
    }

    private void UnloadSceneAsynk(SceneStaticData sceneData, Action callback)
    {
        var asyncOperation = SceneManager.UnloadSceneAsync(sceneData.BuildIndex);

        asyncOperation.completed += AsyncOperation_completed;
        void AsyncOperation_completed(AsyncOperation operation)
        {
            operation.completed -= AsyncOperation_completed;
            callback?.Invoke();
        }
    }
}
