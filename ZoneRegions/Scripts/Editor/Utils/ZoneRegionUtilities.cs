// Copyright © 2022 Runningbird Studios.  All Rights Reserved.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts.Utils
{
    public class ZoneRegionUtilities
    {
        private static ZoneRegionUtilities instance;

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
            if (ZoneRegionSceneManagers.Count == 0)
            {
                ZoneRegionSceneManagers = new List<ZoneRegionSceneManager>(GameObject.FindObjectsOfType<ZoneRegionSceneManager>());
            }
        }

        public static void LoadSceneZoneRegion()
        {
            if (SceneZoneRegion == null)
            {
                SceneZoneRegion = GameObject.Find("ZoneRegions");
            }
        }

        public static List<ZoneRegionSceneManager> ZoneRegionSceneManagers;
        public static NavMeshData ZoneRegionsNavMesh;
        public static GameObject SceneZoneRegion;
    }
}