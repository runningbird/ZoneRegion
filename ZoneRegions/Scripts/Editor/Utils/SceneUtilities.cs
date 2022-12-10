using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts
{
#if (UNITY_EDITOR)

    public static class SceneUtilities
    {
        public static void LoadSceneAdditively(string sceneName)
        {
            if (EditorApplication.isPlaying) return;

            string[] sceneAsset = AssetDatabase.FindAssets(sceneName);

            if (sceneAsset == null) return;

            foreach (string asset in sceneAsset)
            {
                string path = AssetDatabase.GUIDToAssetPath(asset);

                if (!string.IsNullOrEmpty(path))
                {
                    var assetLoaded = AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset));

                    if (assetLoaded != null)
                    {
                        if (assetLoaded.name == sceneName)
                        {
                            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                        }
                    }
                }
            }
        }

        public static void UnloadScene(string sceneName)
        {
            if (EditorApplication.isPlaying) return;
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.isDirty)
            {
                EditorSceneManager.SaveScene(scene);
            }

            EditorSceneManager.CloseScene(SceneManager.GetSceneByName(sceneName), true);
        }

        public static string GetScenePath(string sceneName)
        {
            string scenePath = string.Empty;

            string[] sceneAsset = AssetDatabase.FindAssets(sceneName);

            foreach (string asset in sceneAsset)
            {
                string path = AssetDatabase.GUIDToAssetPath(asset);

                if (!string.IsNullOrEmpty(path))
                {
                    var assetLoaded = AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset));
                    if (assetLoaded != null)
                    {
                        scenePath = path;
                    }
                }
            }

            return scenePath;
        }
    }

#endif
}