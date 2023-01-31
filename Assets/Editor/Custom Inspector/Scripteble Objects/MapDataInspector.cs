using System.Collections.Generic;
using MapStraeming;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CustomEditor(typeof(MapData))]
public class MapDataInspector : Editor
{
    public GameObject parentMap;
    public GameObject parentInstance;
    public MapData _mapData;
    public int countInstance;

    private void OnEnable()
    {
        _mapData = (MapData)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Original map : ");
        parentMap = (GameObject)EditorGUILayout.ObjectField(parentMap, typeof(GameObject), true);
        EditorGUILayout.LabelField("Number of object : " + _mapData.MapObjects.Count.ToString());
        EditorGUILayout.LabelField("Parent instance map: ");
        parentInstance = (GameObject)EditorGUILayout.ObjectField(parentInstance, typeof(GameObject), true);
        EditorGUILayout.LabelField("Object count instance: " + countInstance);

        if (GUILayout.Button("Record Map Object"))
        {
            Transform[] objects = parentMap.GetComponentsInChildren<Transform>();
            _mapData.MapObjects.Clear();

            var child = parentInstance.transform.childCount;
            if (child > 0)
            {
                for (int i = 0; i < child; i++)
                {
                    DestroyImmediate(parentInstance.transform.GetChild(i).gameObject);
                }
            }

            foreach (var transform in objects)
            {
                var objectName = transform.name;
                var mapName = parentMap.name;
                if (objectName == mapName)
                    continue;

                MapObject temp = new MapObject(transform);
                _mapData.AddObject(temp);
            }
        }

        if (GUILayout.Button("Instantiate map object"))
        {
            countInstance = 0;
            foreach (var mapObject in _mapData.MapObjects)
            {
                if (_mapData.InstanceMapObject(mapObject, parentInstance))
                {
                    countInstance++;
                }
            }
        }
    }
}