using Assets.RunningbirdStudios.ZoneRegions.Scripts.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts
{
    [CustomEditor(typeof(ZoneRegionSceneManager))]
    [CanEditMultipleObjects]
    public class ZoneRegionSceneManagerEditor : Editor
    {
        private ZoneRegionSceneManager ZoneRegionSceneManager = null;
        private int linkWidth = 10;

        public bool HasFrameBounds()
        {
            return true;
        }

        protected void OnEnable()
        {
            ZoneRegionSceneManager = (ZoneRegionSceneManager)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Zone Region Scene Manager ");
            EditorGUILayout.LabelField("Zone Scene", ZoneRegionSceneManager.zoneScene);
            EditorGUILayout.LabelField("Zone Scene Position", ZoneRegionSceneManager.zoneScenePosition.ToString());
            EditorGUILayout.LabelField("Neighboring Zone Regions");
            EditorGUILayout.LabelField("Top Neighbor", ZoneRegionSceneManager.topNeighbor);
            EditorGUILayout.LabelField("Bottom Neighbor", ZoneRegionSceneManager.bottomNeighbor);
            EditorGUILayout.LabelField("Left Neighbor", ZoneRegionSceneManager.leftNeighbor);
            EditorGUILayout.LabelField("Right Neighbor", ZoneRegionSceneManager.rightNeighbor);
            EditorGUILayout.LabelField("IsLoaded", ZoneRegionSceneManager.ZoneRegionScene.IsLoaded.ToString());
            //EditorGUILayout.LabelField("IsUpdated", ZoneRegionSceneManager.IsUpdated.ToString());
            base.OnInspectorGUI();
            if (GUILayout.Button("Load Scene"))
            {
                SceneUtilities.LoadSceneAdditively(ZoneRegionSceneManager.ZoneRegionScene.name);
            }

            if (GUILayout.Button("Unload Scene"))
            {
                SceneUtilities.UnloadScene(ZoneRegionSceneManager.ZoneRegionScene.name);
            }

            if (GUILayout.Button("Bake NavMesh"))
            {
                Scene zoneScene = SceneManager.GetSceneByName(ZoneRegionSceneManager.ZoneRegionScene.name);
                if (zoneScene == null)
                {
                    SceneUtilities.LoadSceneAdditively(ZoneRegionSceneManager.ZoneRegionScene.name);
                }

                NavMeshUtilities.BakeNavMesh(zoneScene);
            }
            EditorGUILayout.LabelField("NavMeshLink Width");
            string linkWidthString = GUILayout.TextField(linkWidth.ToString());
            if (int.TryParse(linkWidthString, out int result))
            {
                linkWidth = result;
            }

            if (GUILayout.Button("Create OffMesh Links"))
            {
                Scene zoneScene = SceneManager.GetSceneByName(ZoneRegionSceneManager.ZoneRegionScene.name);
                if (zoneScene == null)
                {
                    SceneUtilities.LoadSceneAdditively(ZoneRegionSceneManager.ZoneRegionScene.name);
                }
                NavMeshUtilities.CreateOffMeshLinks(zoneScene, linkWidth);
            }

            bool somethingChanged = EditorGUI.EndChangeCheck();
            if (somethingChanged)
            {
                EditorUtility.SetDirty(ZoneRegionSceneManager);
            }
            //serializedObject.ApplyModifiedProperties();
        }

        public void OnSceneGUI()
        {
            var color = new Color(0, 0.8f, 0.4f, 1);
            Handles.color = color;
            Handles.Label(ZoneRegionSceneManager.transform.position + Vector3.up * 50, ZoneRegionSceneManager.name);
        }

        public Bounds OnGetFrameBounds()
        {
            ZoneRegionSceneManager script = (ZoneRegionSceneManager)target;
            
            Debug.Log("Focus on: " + target.name + " with bounds: " + script.ZoneRegionScene.zoneSceneBounds);
            return script.ZoneRegionScene.zoneSceneBounds;
        }

        public enum Side
        {
            Top,
            Bottom,
            Left,
            Right
        }
    }
}