using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private int countNeuhborgs;
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private GameObject firstChunk;
    [SerializeField] public Vector2 mapSize;
    [SerializeField] private float chukSize;
    private List<ChunkData> chunks = new List<ChunkData>();
    private int x = 1, z = 1;
    private List<ChunkData> chunkNeiborgs = new List<ChunkData>();

    private void Start()
    {
        AddFirstChunk();
        CreateChunks();
    }

    private void AddFirstChunk()
    {
        var first = new ChunkData();
        first.z = 0;
        first.x = 0;
        first.isInstance = true;
        first.thisObject = firstChunk;
        chunks.Add(first);
    }

    private void CreateChunks()
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                var newChunk = new ChunkData();
                newChunk.x = i;
                newChunk.z = j;
                chunks.Add(newChunk);
            }
        }

        GetNeugbors();
    }

    private void GetNeugbors()
    {
        chunkNeiborgs.Clear();
        for (int xx = x - countNeuhborgs; xx <= x + countNeuhborgs; xx++)
        {
            for (int yy = z - countNeuhborgs; yy <= z + countNeuhborgs; yy++)
            {
                // if (x == xx && z == yy) continue;

                foreach (var chunk in chunks)
                {
                    if (xx == chunk.x && yy == chunk.z)
                    {
                        chunkNeiborgs.Add(chunk);
                    }
                }
            }
        }

        SpawnChunk(chunkNeiborgs);
    }

    private void SpawnChunk(List<ChunkData> list)
    {
        for (int xx = x - countNeuhborgs; xx <= x + countNeuhborgs; xx++)
        {
            for (int yy = z - countNeuhborgs; yy <= z + countNeuhborgs; yy++)
            {
                foreach (var chunk in list)
                {
                    if (xx == chunk.x && yy == chunk.z)
                    {
                        if (chunk.isInstance) continue;
                        var chank = Instantiate(chunkPrefab, new Vector3(xx, 0, yy) * chukSize, quaternion.identity);
                        chunk.thisObject = chank;
                        chunk.isInstance = true;
                    }
                }
            }
        }

        RemoveNonNeuborgh();
    }

    private void RemoveNonNeuborgh()
    {
        List<ChunkData> instatce = new List<ChunkData>();
        foreach (var chunk in chunks)
        {
            if (chunk.isInstance)
            {
                instatce.Add(chunk);
            }
        }

        foreach (var chunk in instatce)
        {
            if (!chunkNeiborgs.Contains(chunk))
            {
                chunk.isInstance = false;
                Destroy(chunk.thisObject);
            }
        }
    }

    private int _oldX = 1000000;
    private int _oldZ = 1000000;

    private void Update()
    {
        var position = transform.position;
        x = (int)((position.x + chukSize / 2) / chukSize);
        z = (int)((position.z + chukSize / 2) / chukSize);
        if (x != _oldX || z != _oldZ)
        {
            DownloadChunk();
        }

        _oldX = x;
        _oldZ = z;
    }

    private void DownloadChunk()
    {
        GetNeugbors();
    }
}