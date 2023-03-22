using KBCore.Refs;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts
{
    public class SceneZoneManager : MonoBehaviour
    {
        [SerializeField, Child]
        public ZoneRegionSceneManager[] zoneRegionSceneManagers;
        [SerializeField]
        public Vector3 spawnLocation;
        [SerializeField]
        public Camera mainCamera;
        [SerializeField]
        public int MaxDistance;

        //private void Awake()
        //{
        //    mainCamera = Camera.main;
        //}

        public void DeactivateDistanceZoneRegions(Vector3 playerPosition)
        {
            if (MaxDistance > 0)
            {
                if (zoneRegionSceneManagers.Length > 0)
                {
                    foreach (ZoneRegionSceneManager zoneRegionSceneManager in zoneRegionSceneManagers)
                    {
                        Vector3 zoneRegionPosition = zoneRegionSceneManager.transform.position + zoneRegionSceneManager.ZoneRegionScene.zoneSize;

                        float xDistance = Mathf.Abs(zoneRegionPosition.x - playerPosition.x);
                        float zDistance = Mathf.Abs(zoneRegionPosition.z - playerPosition.z);

                        if (xDistance + zDistance > MaxDistance)
                        {
                            if (zoneRegionSceneManager.ZoneRegionScene.IsLoaded)
                            {
                                UnloadScene(zoneRegionSceneManager);
                            }
                        }
                        else
                        {
                            if (!zoneRegionSceneManager.ZoneRegionScene.IsLoaded)
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
                    var zoneRegionPosition = x.transform.position;
                    var zoneRegionSize = x.ZoneRegionScene.zoneSceneBounds.size * 0.5f;
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
                    LoadScene(zoneRegion);
                }
            }
        }

        public void LoadScene(ZoneRegionSceneManager zoneRegionSceneManager)
        {
            if (zoneRegionSceneManager)
            {
                Scene scene = SceneManager.GetSceneByName(zoneRegionSceneManager.ZoneRegionScene.zoneScene);

                if (!zoneRegionSceneManager.ZoneRegionScene.IsLoaded)
                {
                    SceneManager.LoadSceneAsync(zoneRegionSceneManager.ZoneRegionScene.zoneScene, LoadSceneMode.Additive);
                    zoneRegionSceneManager.ZoneRegionScene.IsLoaded = true;
                }
                
            }
        }

        public void UnloadScene(ZoneRegionSceneManager zoneRegionSceneManager)
        {
            if (zoneRegionSceneManager)
            {
                Scene scene = SceneManager.GetSceneByName(zoneRegionSceneManager.ZoneRegionScene.zoneScene);

                SceneManager.UnloadSceneAsync(zoneRegionSceneManager.ZoneRegionScene.zoneScene);
                zoneRegionSceneManager.ZoneRegionScene.IsLoaded = false;
            }
        }

        private void OnValidate()
        {
            this.ValidateRefs();
            mainCamera = Camera.main;
            zoneRegionSceneManagers = zoneRegionSceneManagers.OrderBy(s => s.zoneScene).ToArray();
        }
    }
}