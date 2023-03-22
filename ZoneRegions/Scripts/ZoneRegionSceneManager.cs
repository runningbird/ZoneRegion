using KBCore.Refs;
using UnityEngine;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts
{
    public class ZoneRegionSceneManager : MonoBehaviour
    {
        private bool _isUpdated;

        public string zoneScene
        {
            get { return ZoneRegionScene.zoneScene; }
        }

        public Vector3 zoneScenePosition
        {
            get { return ZoneRegionScene.zoneScenePosition; }
        }
        public string topNeighbor
        {
            get { return ZoneRegionScene.topNeighbor; }
        }
        public string bottomNeighbor
        {
            get { return ZoneRegionScene.bottomNeighbor; }
        }
        public string leftNeighbor
        {
            get { return ZoneRegionScene.leftNeighbor; }
        }
        public string rightNeighbor
        {
            get { return ZoneRegionScene.rightNeighbor; }
        }
        public Bounds zoneSceneBounds
        {
            get { return ZoneRegionScene.zoneSceneBounds; }
        }

        public ZoneRegionScene ZoneRegionScene;      

        public bool IsLoaded
        {
            get { return ZoneRegionScene.IsLoaded; }
        }

        private void OnAwake()
        {

        }
        private void OnDrawGizmos()
        {
            if (ZoneRegionScene)
            {
                Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.25F);

                this.transform.position = ZoneRegionScene.zoneScenePosition;
                DrawGizmoCube();

                //SetZoneRegion();
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Display the explosion radius when selected
            Gizmos.color = new Color(1, 1, 0, 0.75F);

            DrawGizmoCube();
        }

        private void DrawGizmoCube()
        {
            this.transform.localScale = new Vector3(ZoneRegionScene.zoneSceneBounds.size.x, 2000, ZoneRegionScene.zoneSceneBounds.size.z);
            Vector3 center = new Vector3(ZoneRegionScene.zoneScenePosition.x + ZoneRegionScene.zoneSceneBounds.center.x, 0, ZoneRegionScene.zoneScenePosition.z + ZoneRegionScene.zoneSceneBounds.center.z);

            Gizmos.DrawCube(center, this.transform.localScale);
        }

        private void SetZoneRegion()
        {
            if (!_isUpdated)
            {
                if (ZoneRegionScene)
                {
                    //this.transform.position = ZoneRegionScene.zoneScenePosition;
                    //_isUpdated = true;
                }
            }
        }

    }
}