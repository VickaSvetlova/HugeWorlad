using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct MapObject
{
    public string ObjectName;
    public Vector3 ObjectPosition;
    public Quaternion ObjectRotation;
    public string pathAsset;
    public Vector3 ObjectScale;

    public MapObject(Transform mapObjectRef)
    {
        ObjectName = mapObjectRef.name.Split(' ')[0];
        ObjectPosition = mapObjectRef.position;
        ObjectRotation = mapObjectRef.rotation;
        ObjectScale = mapObjectRef.localScale;
        pathAsset = "Assets/Prefabs/MapStreaming/";
    }
}