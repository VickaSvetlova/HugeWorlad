using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MapStraeming
{
    [CreateAssetMenu(fileName = "MapData", menuName = "SO/MapData"), Serializable]
    public class MapData : ScriptableObject
    {
        public List<MapObject> MapObjects;

        private void Awake()
        {
            if (MapObjects == null)
            {
                MapObjects = new List<MapObject>();
            }
        }

        public void AddObject(MapObject mapObject)
        {
            MapObjects.Add(mapObject);
        }

        public GameObject InstanceMapObject(MapObject mapObject)
        {
            var mapObjectObjectName = mapObject.pathAsset + mapObject.ObjectName + ".prefab";
            GameObject temp =
                (GameObject)AssetDatabase.LoadAssetAtPath(mapObjectObjectName,
                    typeof(GameObject));
            if (temp == null)
                return null;
            temp = Instantiate(temp);
            temp.transform.position = mapObject.ObjectPosition;
            temp.transform.rotation = mapObject.ObjectRotation;
            temp.transform.localScale = mapObject.ObjectScale;
            temp.name = mapObject.ObjectName;
            return temp;
        }
    }
}