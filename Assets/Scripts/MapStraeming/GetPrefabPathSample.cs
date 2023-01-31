using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GetPrefabPathSample : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private void Start()
    {
        var path = AssetDatabase.GetAssetPath(prefab);
        Debug.Log("path "+ path.ToString());
    }
}
