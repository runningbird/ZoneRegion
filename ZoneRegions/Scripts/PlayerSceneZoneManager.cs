using System.Linq;
using UnityEngine;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts
{
    public class PlayerSceneZoneManager : MonoBehaviour
    {
        private SceneZoneManager sceneZoneManager;

        public ZoneRegionSceneManager CurrentZoneRegion;
        public ZoneRegionSceneManager PrevousZoneRegion;
        public int MaxDistance;

        private void Start()
        {
            if (MaxDistance == 0)
            {
                MaxDistance = (int)Camera.main.farClipPlane * 2;
            }

            sceneZoneManager = FindObjectOfType<SceneZoneManager>();
            if (sceneZoneManager)
            {
                if (sceneZoneManager.MaxDistance == 0)
                {
                    sceneZoneManager.MaxDistance = MaxDistance;
                }
                sceneZoneManager.SetClosestZoneRegionActivce(gameObject.transform.position);
            }
        }

        private void Update()
        {
            ZoneRegionsUpdate();
        }

        private void ZoneRegionsUpdate()
        {
            if (sceneZoneManager)
            {
                sceneZoneManager.DeactivateDistanceZoneRegions(gameObject.transform.position);

                CurrentZoneRegion = sceneZoneManager.GetNearestZoneRegion(gameObject.transform.position);

                if (CurrentZoneRegion != PrevousZoneRegion)
                {
                    PrevousZoneRegion = CurrentZoneRegion;
                }
            }
        }
    }
}