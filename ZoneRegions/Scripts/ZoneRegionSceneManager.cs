// Copyright © 2022 Runningbird Studios.  All Rights Reserved.
using UnityEngine;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts
{
    public class ZoneRegionSceneManager : MonoBehaviour
    {
        public string zoneScene;
        public Vector3 zoneScenePosition;
        public string topNeighbor;
        public string bottomNeighbor;
        public string leftNeighbor;
        public string rightNeighbor;
        public Bounds zoneSceneBounds;
        public ZoneRegionScene ZoneRegionScene;
        public bool IsUpdated;
        public bool IsLoaded;

        private void OnDrawGizmos()
        {
            if (ZoneRegionScene)
            {
                Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.25F);

                this.transform.position = zoneScenePosition;
                //Renderer renderer = GetComponent<Renderer>();
                this.transform.localScale = new Vector3(ZoneRegionScene.zoneSceneBounds.size.x, 2000, ZoneRegionScene.zoneSceneBounds.size.z);
                //Gizmos.DrawCube(renderer.bounds.center, this.transform.localScale);
                Vector3 center = new Vector3(zoneScenePosition.x + zoneSceneBounds.center.x, 0, zoneScenePosition.z + zoneSceneBounds.center.z);

                //Gizmos.DrawCube(this.transform.position, this.transform.localScale);
                Gizmos.DrawCube(center, this.transform.localScale);

                SetZoneRegion();
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Display the explosion radius when selected
            Gizmos.color = new Color(1, 1, 0, 0.75F);

            //this.transform.position = zoneScenePosition;
            this.transform.localScale = new Vector3(ZoneRegionScene.zoneSceneBounds.size.x, 2000, ZoneRegionScene.zoneSceneBounds.size.z);
            Vector3 center = new Vector3(zoneScenePosition.x + zoneSceneBounds.center.x, 0, zoneScenePosition.z + zoneSceneBounds.center.z);

            //Gizmos.DrawCube(this.transform.position, this.transform.localScale);
            Gizmos.DrawCube(center, this.transform.localScale);
        }

        private void Awake()
        {
            IsLoaded = false;
        }

        private void Start()
        {
            SetZoneRegion();
        }

        private void SetZoneRegion()
        {
            if (!IsUpdated)
            {
                if (ZoneRegionScene)
                {
                    zoneScene = ZoneRegionScene.zoneScene;
                    zoneScenePosition = ZoneRegionScene.zoneScenePosition;
                    topNeighbor = ZoneRegionScene.topNeighbor;
                    bottomNeighbor = ZoneRegionScene.bottomNeighbor;
                    leftNeighbor = ZoneRegionScene.leftNeighbor;
                    rightNeighbor = ZoneRegionScene.rightNeighbor;
                    zoneSceneBounds = ZoneRegionScene.zoneSceneBounds;
                    this.transform.position = ZoneRegionScene.zoneScenePosition;
                    IsUpdated = true;
                }
            }
        }
    }
}