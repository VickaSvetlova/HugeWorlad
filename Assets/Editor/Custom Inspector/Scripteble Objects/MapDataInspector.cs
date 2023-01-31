using MapStraeming;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapData))]
public class MapDataInspector : Editor
{
    public GameObject parentMap;
    public GameObject parentInstance;
    public MapData _mapData;

    private void OnEnable()
    {
        _mapData = (MapData)target;
    }

    public override void OnInspectorGUI()
    {
        parentMap = (GameObject)EditorGUILayout.ObjectField(parentMap, typeof(GameObject), true);

        EditorGUILayout.LabelField("Number of object : " + _mapData.MapObjects.Count.ToString());
        if (GUILayout.Button("Record Map Object"))
        {
            Transform[] objects = parentMap.GetComponentsInChildren<Transform>();
            _mapData.MapObjects.Clear();

            foreach (var transform in objects)
            {
                if (transform.name == parentMap.name)
                    continue;

                MapObject temp = new MapObject(transform);
                _mapData.AddObject(temp);
            }
        }

        if (GUILayout.Button("Instantiate map object"))
        {
            foreach (var mapObject in _mapData.MapObjects)
            {
                _mapData.InstanceMapObject(mapObject);
            }
        }
    }
}