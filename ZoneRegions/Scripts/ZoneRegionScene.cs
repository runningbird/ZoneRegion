using UnityEngine;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts
{
    //[CreateAssetMenu(fileName = "ZoneRegionScene", menuName = "RunningbirdStudios/ZoneSystem/ZoneRegionScene", order = 1)]
    public class ZoneRegionScene : ScriptableObject
    {
        public string zoneScene;
        public Vector3 zoneScenePosition;
        public string topNeighbor;
        public string bottomNeighbor;
        public string leftNeighbor;
        public string rightNeighbor;
        public Bounds zoneSceneBounds;
        public Vector3 zoneSize;
        public bool IsLoaded;
    }
}