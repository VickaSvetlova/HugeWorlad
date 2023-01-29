using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private int countNeuhborgs;
    [SerializeField] private GameObject chunkPrefab;
    public GameObject firstChunk;
    [SerializeField] public Vector2 mapSize;
    [SerializeField] private float chukSize;
    private List<ChunkData> chunks = new List<ChunkData>();
    private int x, z;
    private List<ChunkData> chunkNeiborgs = new List<ChunkData>();

    public void PlayerPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private void Start()
    {
        AddFirstChunk();
        CreateChunks();
    }

    private void AddFirstChunk()
    {
        var first = new ChunkData();
        var pos = GetPositionCharacter();
        first.z = (int)pos.z;
        first.x = (int)pos.x;
        first.isInstance = true;
        first.thisObject = firstChunk;
        chunks.Add(first);
    }

    private Vector3 GetPositionCharacter()
    {
        var position = transform.position;
        x = (int)((position.x + chukSize / 2) / chukSize);
        z = (int)((position.z + chukSize / 2) / chukSize);
        return new Vector3(x, position.y, z);
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
        foreach (var chunk in list)
        {
            if (chunk.isInstance) continue;
            var chank = Instantiate(chunkPrefab, new Vector3(chunk.x, 0, chunk.z) * chukSize, quaternion.identity);
            chunk.thisObject = chank;
            chunk.isInstance = true;
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
        var pose = GetPositionCharacter();
        x = (int)pose.x;
        z = (int)pose.z;

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