// Copyright © 2022 Runningbird Studios.  All Rights Reserved.
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts
{
    public class SceneZoneManager : MonoBehaviour
    {
        public ZoneRegionSceneManager[] zoneRegionSceneManagers;
        public Vector3 spawnLocation;

        public Camera mainCamera;

        public Color GizmoColor;

        public int MaxDistance;

        private void Awake()
        {
            mainCamera = Camera.main;
            SetZoneRegionSceneManagers();
        }

        private void OnDrawGizmos()
        {
            if (zoneRegionSceneManagers.Length == 0)
            {
                SetZoneRegionSceneManagers();
            }
        }

        private void SetZoneRegionSceneManagers()
        {
            zoneRegionSceneManagers = GameObject.FindObjectsOfType<ZoneRegionSceneManager>();
        }

        public void DeactivateDistanceZoneRegions(Vector3 playerPosition)
        {
            if (MaxDistance > 0)
            {
                if (zoneRegionSceneManagers.Length > 0)
                {
                    foreach (ZoneRegionSceneManager zoneRegionSceneManager in zoneRegionSceneManagers)
                    {
                        Vector3 zoneRegionPosition = zoneRegionSceneManager.ZoneRegionScene.zoneScenePosition;

                        float xDistance = Mathf.Abs(zoneRegionSceneManager.ZoneRegionScene.zoneScenePosition.x - playerPosition.x);
                        float zDistance = Mathf.Abs(zoneRegionSceneManager.ZoneRegionScene.zoneScenePosition.z - playerPosition.z);

                        if (xDistance + zDistance > MaxDistance)
                        {
                            if (zoneRegionSceneManager.IsLoaded)
                            {
                                UnloadScene(zoneRegionSceneManager);
                            }
                        }
                        else
                        {
                            if (!zoneRegionSceneManager.IsLoaded)
                            {
                                LoadScene(zoneRegionSceneManager);
                            }
                        }
                    }
                }
            }
        }

        public ZoneRegionSceneManager GetNearestZoneRegion(Vector3 position)
        {
            if (zoneRegionSceneManagers.Length > 0)
            {
                return zoneRegionSceneManagers.OrderBy(x =>
                {
                    var zoneRegionPosition = x.ZoneRegionScene.zoneScenePosition;
                    var zoneRegionSize = x.ZoneRegionScene.zoneSize * 0.5f;
                    var terrainCenter = new Vector3(zoneRegionPosition.x + zoneRegionSize.x, position.y, zoneRegionPosition.z + zoneRegionSize.z);
                    return Vector3.Distance(terrainCenter, position);
                }
                ).First();
            }
            return null;
        }

        public void SetClosestZoneRegionActivce(Vector3 position)
        {
            if (position != null)
            {
                if (zoneRegionSceneManagers.Length > 0)
                {
                    ZoneRegionSceneManager zoneRegion = GetNearestZoneRegion(position);
                    //LoadUnloadScene(zoneRegion.ZoneRegionScene, true);
                    SceneManager.LoadScene(zoneRegion.ZoneRegionScene.name, LoadSceneMode.Additive);
                    zoneRegion.ZoneRegionScene.IsLoaded = true;
                }
            }
        }

        public void LoadScene(ZoneRegionSceneManager zoneRegionSceneManager)
        {
            if (zoneRegionSceneManager)
            {
                Scene scene = SceneManager.GetSceneByName(zoneRegionSceneManager.ZoneRegionScene.name);

                if (zoneRegionSceneManager)
                {
                    SceneManager.LoadSceneAsync(zoneRegionSceneManager.ZoneRegionScene.name, LoadSceneMode.Additive);
                    zoneRegionSceneManager.IsLoaded = true;
                }
            }
        }

        public void UnloadScene(ZoneRegionSceneManager zoneRegionSceneManager)
        {
            if (zoneRegionSceneManager)
            {
                Scene scene = SceneManager.GetSceneByName(zoneRegionSceneManager.ZoneRegionScene.name);

                SceneManager.UnloadSceneAsync(zoneRegionSceneManager.ZoneRegionScene.name);
                zoneRegionSceneManager.IsLoaded = false;
            }
        }
    }
}