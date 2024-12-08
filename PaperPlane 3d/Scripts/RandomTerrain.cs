using System.Collections;
using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public Terrain[] terrains;
    public float minHeight = 0f;
    public float maxHeight = 10f;
    public float frequency = 0.1f;
    public GameObject[] treePrefabs;
    public GameObject birdPrefab;
    public int minBirds = 3;
    public int maxBirds = 5;
    public float birdMinHeight = 1f;
    public float birdMaxHeight = 5f;

    private int terrainIndex = 0;
    private bool terrainGenerationComplete = false;

    public float sectionLength = 100f;  // Length of each terrain section
    public float delayBetweenSections = 15f;  // Delay between spawning trees in each section

    private bool spawningTrees = false;

    void Start()
    {
        StartCoroutine(GenerateTerrainAsync());
    }

    IEnumerator GenerateTerrainAsync()
    {
        while (terrainIndex < terrains.Length)
        {
            GenerateRandomTerrain(terrains[terrainIndex]);
            yield return null; // Yield to the next frame
        }

        terrainGenerationComplete = true;

        // Spawn trees and birds only after terrain generation is complete
        SpawnTreesOnTerrain();
        SpawnBirdsOnTerrain();
    }

    void GenerateRandomTerrain(Terrain terrain)
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain reference is missing!");
            return;
        }

        TerrainData terrainData = terrain.terrainData;

        int resolution = terrainData.heightmapResolution;
        float[,] heights = new float[resolution, resolution];

        float xOffset = Random.Range(0f, 1000f);
        float zOffset = Random.Range(0f, 1000f);

        // Use Perlin noise for smoother mountain shapes
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                float xCoord = (i + xOffset) * frequency;
                float zCoord = (j + zOffset) * frequency;
                float perlinValue = Mathf.PerlinNoise(xCoord, zCoord);

                float mountainHeight = perlinValue * (maxHeight - minHeight) + minHeight;

                // Gradual slopes for the mountains
                float distanceToCenter = Mathf.Sqrt(Mathf.Pow(i - resolution / 2, 2) + Mathf.Pow(j - resolution / 2, 2));
                mountainHeight *= Mathf.Clamp01(1 - distanceToCenter / (resolution / 2));

                heights[i, j] = mountainHeight;
            }
        }

        terrainData.SetHeights(0, 0, heights);
        terrainIndex++;
    }

    void SpawnBirdsOnTerrain()
    {
        if (!terrainGenerationComplete)
        {
            Debug.LogError("Terrain generation is not complete yet!");
            return;
        }

        foreach (Terrain terrain in terrains)
        {
            if (terrain == null || birdPrefab == null)
            {
                Debug.LogError("Terrain reference or bird prefab is missing!");
                continue;
            }

            TerrainData terrainData = terrain.terrainData;

            Vector3 terrainCenter = terrain.transform.position + new Vector3(terrainData.size.x / 2f, 0f, terrainData.size.z / 2f);

            for (int i = 0; i < Random.Range(minBirds, maxBirds + 1); i++)
            {
                float birdX = Random.Range(terrainCenter.x - terrainData.size.x / 2f, terrainCenter.x + terrainData.size.x / 2f);
                float birdZ = Random.Range(terrainCenter.z - terrainData.size.z / 2f, terrainCenter.z + terrainData.size.z / 2f);
                float birdY = terrain.SampleHeight(new Vector3(birdX, 0f, birdZ)) + Random.Range(birdMinHeight, birdMaxHeight);

                Instantiate(birdPrefab, new Vector3(birdX, birdY, birdZ), Quaternion.identity);
            }
        }
    }

    IEnumerator SpawnTreesInSection()
    {
        if (spawningTrees)
        {
            yield break;
        }

        spawningTrees = true;

        foreach (Terrain terrain in terrains)
        {
            if (terrain == null || treePrefabs == null || treePrefabs.Length == 0)
            {
                Debug.LogError("Terrain reference or tree prefabs are missing!");
                continue;
            }

            TerrainData terrainData = terrain.terrainData;

            int numTrees = Random.Range(200, 300);

            for (float sectionStartZ = 0f; sectionStartZ < terrainData.size.z; sectionStartZ += sectionLength)
            {
                float sectionEndZ = Mathf.Min(sectionStartZ + sectionLength, terrainData.size.z);

                SpawnTreesInSection(terrain, sectionStartZ, sectionEndZ, numTrees);

                yield return new WaitForSeconds(delayBetweenSections);
            }
        }

        spawningTrees = false;
    }

    void SpawnTreesInSection(Terrain terrain, float startZ, float endZ, int numTrees)
    {
        for (int i = 0; i < numTrees; i++)
        {
            float x = Random.Range(0f, 1f);
            float z = Random.Range(startZ / terrain.terrainData.size.z, endZ / terrain.terrainData.size.z);

            Vector3 localPosition = new Vector3(x * terrain.terrainData.size.x, 0f, z * terrain.terrainData.size.z);
            Vector3 worldPosition = terrain.transform.TransformPoint(localPosition);
            worldPosition.y = terrain.SampleHeight(worldPosition);

            GameObject selectedTreePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            Instantiate(selectedTreePrefab, worldPosition, randomRotation);
        }
    }
    void SpawnTreesOnTerrain()
    {
        if (!terrainGenerationComplete)
        {
            Debug.LogError("Terrain generation is not complete yet!");
            return;
        }

        StartCoroutine(SpawnTreesInSection());
    }
}
