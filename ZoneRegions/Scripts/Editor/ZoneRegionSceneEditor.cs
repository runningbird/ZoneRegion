using UnityEditor;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts
{
    [CustomEditor(typeof(ZoneRegionScene))]
    [CanEditMultipleObjects]
    public class ZoneRegionSceneEditor : Editor
    {
        private ZoneRegionScene zoneRegionScene = null;

        protected void OnEnable()
        {
            zoneRegionScene = (ZoneRegionScene)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _ = DrawDefaultInspector();

            EditorGUI.BeginChangeCheck();
            //zoneRegionScene.zoneSize = EditorGUILayout.Vector3Field("Zone Size", zoneRegionScene.zoneSize);

            bool somethingChanged = EditorGUI.EndChangeCheck();
            if (somethingChanged)
            {
                EditorUtility.SetDirty(zoneRegionScene);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}