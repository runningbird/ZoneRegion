using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts.Utils
{
    public class ZoneRegionUtilities
    {
        private static ZoneRegionUtilities instance;

        [SerializeField]
        public static List<ZoneRegionSceneManager> ZoneRegionSceneManagers;
        [SerializeField]
        public static NavMeshData ZoneRegionsNavMesh;
        [SerializeField]
        public static GameObject SceneZoneRegion;

        protected ZoneRegionUtilities()
        {
        }

        public static ZoneRegionUtilities Instance()
        {
            if (instance == null)
            {
                instance = new ZoneRegionUtilities();
                ZoneRegionSceneManagers = new List<ZoneRegionSceneManager>();
                ZoneRegionsNavMesh = new NavMeshData();
            }
            LoadZoneRegionSceneManagers();
            LoadSceneZoneRegion();

            return instance;
        }

        public static void LoadZoneRegionSceneManagers()
        {
            ZoneRegionSceneManagers = new List<ZoneRegionSceneManager>(GameObject.FindObjectsOfType<ZoneRegionSceneManager>());
        }

        public static void LoadSceneZoneRegion()
        {
           SceneZoneRegion = GameObject.Find("ZoneRegions");
        }
   
    }
}