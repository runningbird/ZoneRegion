using Assets.RunningbirdStudios.ZoneRegions.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts
{
#if(UNITY_EDITOR)

    public class ZoneRegionEditor : EditorWindow
    {
        public List<Scene> Scenes;

        //public List<ZoneRegionSceneManager> zoneRegionSceneManagers;
        public NavMeshData ZoneRegionsNavMesh;

        private Vector2 scrollPos;
        private int mainToolbarInt = 0;
        private int linkWidth = 10;

        private ZoneRegionEditor()
        {
            EditorSceneManager.activeSceneChanged += this.EditorSceneManager_activeSceneChanged;
        }

        private void EditorSceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            ZoneRegionUtilities.Instance();
        }

        [MenuItem("Window/Runningbird Studios/ZoneRegion-Utilities")]
        private static void ShowWindow()
        {
            GetWindow<ZoneRegionEditor>(false, "Zone Region UTILITIES", true);
            ZoneRegionUtilities.Instance();
        }

        private void OnGUI()
        {
            DrawLogo();
            Texture2D ZoneIcon;
            ZoneIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/RunningbirdStudios/MMOKIT/ZoneRegions/Gui/zone-region-manager-icon.png");

            if (Application.isPlaying) return;
            string[] toolbarStrings = { "Zones", "NavMesh", "Terrain Tools" };
            mainToolbarInt = GUILayout.Toolbar(mainToolbarInt, toolbarStrings);
            ZoneRegionUtilities.Instance();
            switch (mainToolbarInt)
            {
                case 0:
                    {
                        ShowZoneRegionSceneManagers();
                        break;
                    }
                case 1:
                    {
                        ShowNavMeshBuild();
                        break;
                    }
                case 2:
                    {
                        ShowTerrainTools();
                        break;
                    }
            }
        }

        private void ShowTerrainTools()
        {
            Terrain[] terrains = GameObject.FindObjectsOfType<Terrain>(true);
            if (terrains != null && terrains.Length > 0)
            {
                GUILayout.Label("Make Terrains unique");
                if (GUILayout.Button("Rename Terrains"))
                {
                    UpdateTerrainNames(terrains);
                }
            }
            else
            {
                GUILayout.Label("No Terrains Found in scene");
            }
        }

        private void DrawLogo()
        {
            Texture2D logo;
            logo = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/RunningbirdStudios/Base/Gui/runningbirdstudios_logo.psd");

            //if (logo != null)
            //{
            GUILayout.Space(60);
            GUI.DrawTexture(new Rect(5, 8, 240, 48), logo, ScaleMode.ScaleToFit);
            //}
        }

        private void ShowZoneRegionSceneManagers()
        {
            if (ZoneRegionUtilities.SceneZoneRegion != null && ZoneRegionUtilities.ZoneRegionSceneManagers.Count > 0)
            {
                if (GUILayout.Button("Load All Scenes"))
                {
                    LoadAllScenesForZoneREgions();
                }
                if (GUILayout.Button("Unload All Scenes"))
                {
                    UnLoadSAllcenesForZoneRegions();
                }
                GUILayout.Label("Zone Region Scene Managers");
                EditorGUILayout.BeginHorizontal();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                foreach (ZoneRegionSceneManager zoneRegionSceneManager in ZoneRegionUtilities.ZoneRegionSceneManagers)
                {
                    if (GUILayout.Button(zoneRegionSceneManager.zoneScene.ToString()))
                    {
                        Selection.SetActiveObjectWithContext(zoneRegionSceneManager.gameObject, null);
                    }
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Label("No Zone Region Scene Managers found");
                Terrain[] terrains = FindObjectsOfType<Terrain>(true); FindObjectsOfType<Terrain>();
                if (terrains.Length > 0)
                {
                    GUILayout.Label("Terrains found in scene:" + terrains.Length.ToString());
                    if (GUILayout.Button("Create Zone Regions"))
                    {
                        CreateScenesForTerrains(terrains);
                    }
                }
                else if (terrains.Length == 1)
                {
                    GUILayout.Label("Only one Terrain Found in scene, Please either create more tiles or split current terrain");
                }
                else
                {
                    GUILayout.Label("No Terrains Found in scene, please add some terrains");
                }
            }
        }

        private void ShowNavMeshBuild()
        {
            if (ZoneRegionUtilities.ZoneRegionSceneManagers.Count > 0)
            {
                if (GUILayout.Button("Build NavMesh for all Zone Regions"))
                {
                    BuildNavMeshForZoneRegions();
                }

                GUILayout.Label("NavMesh Link Width");
                string linkWidthString = GUILayout.TextField(linkWidth.ToString());
                if (int.TryParse(linkWidthString, out int result))
                {
                    linkWidth = result;
                }
                if (GUILayout.Button("Build NavMesh Offlinks for all Zone Regions"))
                {
                    BuildNavMeshLinkForZoneRegions();
                }
            }
            else
            {
                GUILayout.Label("No Zone Regions Found in scene");
            }
        }

        private void SetZoneRegions()
        {
            if (ZoneRegionUtilities.ZoneRegionSceneManagers == null)
            {
                ZoneRegionUtilities.ZoneRegionSceneManagers = new List<ZoneRegionSceneManager>();
                ZoneRegionUtilities.ZoneRegionSceneManagers = FindObjectsOfType<ZoneRegionSceneManager>().ToList();
            }
        }

        private void UnLoadSAllcenesForZoneRegions()
        {
            foreach (ZoneRegionSceneManager manager in ZoneRegionUtilities.ZoneRegionSceneManagers)
            {
                SceneUtilities.UnloadScene(manager.zoneScene);
            }
        }

        private void LoadAllScenesForZoneREgions()
        {
            foreach (ZoneRegionSceneManager manager in ZoneRegionUtilities.ZoneRegionSceneManagers)
            {
                SceneUtilities.LoadSceneAdditively(manager.zoneScene);
            }
        }

        private void BuildNavMeshLinkForZoneRegions()
        {
            try
            {
                if (ZoneRegionUtilities.ZoneRegionSceneManagers.Count > 0)
                {
                    UnLoadSAllcenesForZoneRegions();

                    foreach (ZoneRegionSceneManager zoneRegionSceneManager in ZoneRegionUtilities.ZoneRegionSceneManagers)
                    {
                        SceneUtilities.LoadSceneAdditively(zoneRegionSceneManager.zoneScene);

                        Scene zoneScene = SceneManager.GetSceneByName(zoneRegionSceneManager.zoneScene);
                        if (zoneScene != null)
                        {
                            NavMeshUtilities.CreateOffMeshLinks(zoneScene, linkWidth);
                        }
                        SceneUtilities.UnloadScene(zoneRegionSceneManager.zoneScene);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void BuildNavMeshForZoneRegions()
        {
            try
            {
                if (ZoneRegionUtilities.ZoneRegionSceneManagers.Count > 0)
                {
                    UnLoadSAllcenesForZoneRegions();

                    foreach (ZoneRegionSceneManager zoneRegionSceneManager in ZoneRegionUtilities.ZoneRegionSceneManagers)
                    {
                        SceneUtilities.LoadSceneAdditively(zoneRegionSceneManager.zoneScene);

                        Scene zoneScene = SceneManager.GetSceneByName(zoneRegionSceneManager.zoneScene);
                        if (zoneScene != null)
                        {
                            NavMeshUtilities.BakeNavMesh(zoneScene);
                        }
                        SceneUtilities.UnloadScene(zoneRegionSceneManager.zoneScene);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void GenerateWorldNavMeshData()
        {
            ZoneRegionsNavMesh = new NavMeshData();
        }

        private void CreateScenesForTerrains(Terrain[] terrains)
        {
            if (terrains.Length > 0)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                string sceneFolderPath = CreateFolders(activeScene);
                int index = 0;

                foreach (Terrain terrain in terrains)
                {
                    EditorUtility.DisplayProgressBar("ZoneRegion", "Creating Zone Regions", index);
                    Scene scene = CreateScene(terrain, sceneFolderPath);
                    index++;
                }
                EditorUtility.ClearProgressBar();
            }
        }

        private Scene CreateScene(Terrain terrain, string sceneFolderPath)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);

            string newScenePath = GetNewFileName(sceneFolderPath, terrain.name);

            newScene.name = terrain.name;
            if (terrain.gameObject.transform.parent)
            {
                terrain.gameObject.transform.parent = null;
            }

            CreateZoneRegionManager(newScene, activeScene, terrain);

            SceneManager.MoveGameObjectToScene(terrain.gameObject, newScene);
            EditorSceneManager.SaveScene(newScene, newScenePath);
            SceneManager.SetActiveScene(activeScene);
            EditorSceneManager.SaveScene(activeScene);
            return newScene;
        }

        public void CreateZoneRegionManager(Scene scene, Scene activeScene, Terrain terrain)
        {
            GameObject zoneRegion = new GameObject(scene.name);
            ZoneRegionSceneManager regionManager = zoneRegion.AddComponent<ZoneRegionSceneManager>();
            ZoneRegionScene zoneRegionScene = new ZoneRegionScene();
            regionManager.ZoneRegionScene = zoneRegionScene;

            SetZoneRegionScene(zoneRegionScene, scene, terrain);

            GameObject ZoneRegions = GameObject.Find("ZoneRegions");
            if (!ZoneRegions)
            {
                ZoneRegions = new GameObject("ZoneRegions");
                ZoneRegions.AddComponent<SceneZoneManager>();
            }
            SceneManager.MoveGameObjectToScene(ZoneRegions, activeScene);

            SceneManager.MoveGameObjectToScene(zoneRegion, activeScene);

            zoneRegion.transform.parent = ZoneRegions.transform;

            //SceneManager.UnloadSceneAsync(scene);
        }

        private void SetZoneRegionScene(ZoneRegionScene zoneRegionScene, Scene scene, Terrain terrain)
        {
            zoneRegionScene.name = scene.name;
            zoneRegionScene.zoneScene = scene.name;
            zoneRegionScene.zoneSceneBounds = terrain.terrainData.bounds;
            zoneRegionScene.zoneScenePosition = terrain.transform.position;
            if (terrain.topNeighbor) zoneRegionScene.topNeighbor = terrain.topNeighbor.name;
            if (terrain.bottomNeighbor) zoneRegionScene.bottomNeighbor = terrain.bottomNeighbor.name;
            if (terrain.leftNeighbor) zoneRegionScene.leftNeighbor = terrain.leftNeighbor.name;
            if (terrain.rightNeighbor) zoneRegionScene.rightNeighbor = terrain.rightNeighbor.name;
            EditorUtility.SetDirty(zoneRegionScene);
        }

        private string CreateFolders(Scene activeScene)
        {
            //int indexOfName = activeScene.path.LastIndexOf('/');
            //string pathName = activeScene.path.Substring(0, indexOfName);
            string subFolder = "Assets/Scenes/" + activeScene.name;
            string subSceneFolder = subFolder + "/ZoneRegions";

            if (!AssetDatabase.IsValidFolder(subFolder))
            {
                string sceneFolderPathGuid = AssetDatabase.CreateFolder("Assets/Scenes", activeScene.name);
                string sceneFolderPath = AssetDatabase.GUIDToAssetPath(sceneFolderPathGuid);

                subSceneFolder = sceneFolderPath + "/ZoneRegions";

                if (!AssetDatabase.IsValidFolder(subSceneFolder))
                {
                    string zoneRegionsPathGuid = AssetDatabase.CreateFolder(sceneFolderPath, "ZoneRegions");
                    return AssetDatabase.GUIDToAssetPath(zoneRegionsPathGuid);
                }
            }

            return subSceneFolder;
        }

        private string GetNewFileName(string ZoneRegionsFolderPath, string terrainName)
        {
            string newFileName = ZoneRegionsFolderPath + "/" + terrainName + ".unity";
            string scenePath = AssetDatabase.GenerateUniqueAssetPath(newFileName);
            return newFileName;
        }

        private void UpdateTerrainNames(Terrain[] terrains)
        {
            if (terrains != null && terrains.Length > 0)
            {
                foreach (Terrain t in terrains)
                {
                    t.name = t.terrainData.name;
                }
            }
        }
    }

#endif
}