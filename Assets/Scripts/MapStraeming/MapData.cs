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

        public List<string> folderName = new List<string>
        {
            "Buildings", "Characters", "Characters/Chr_Attach", "Environment","DeadBodies",
            "FX", "FX/Misc", "FX/Misc/Materials", "FX/Prefabbed", "Generic", "Item",
            "Props", "Vehicles", "!Vehicles/Attach!", "Weapons", "Weapons/Guns",
            "Weapons/Melee", "Weapons/Misc", "Weapons/Modular", "Weapons/Vehicle_Weapons"
        };

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

        public GameObject InstanceMapObject(MapObject mapObject, GameObject parentInstance)
        {
            GameObject temp = null;
            foreach (var folder in folderName)
            {
                var mapObjectObjectName = mapObject.pathAsset;
                var assetPath = mapObjectObjectName + folder +"/"+ mapObject.ObjectName + ".prefab";
                temp = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));

                if (temp) break;
            }

            if (temp == null)
                return null;
            temp = Instantiate(temp, parentInstance.transform);

            temp.transform.position = mapObject.ObjectPosition;
            temp.transform.rotation = mapObject.ObjectRotation;
            temp.transform.localScale = mapObject.ObjectScale;
            temp.name = mapObject.ObjectName;
            return temp;
        }
    }
}