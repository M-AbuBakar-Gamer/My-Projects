using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public GameObject birdPrefab;
    public int numberOfBirds = 10;
    public float minSpawnHeight = 30f;
    public float maxSpawnHeight = 60f;
    public Terrain terrain;

    void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain not assigned to BirdManager.");
            return;
        }

        SpawnBirds();
    }

    void SpawnBirds()
    {
        for (int i = 0; i < numberOfBirds; i++)
        {
            Vector3 spawnPosition = GetRandomTerrainPoint();
            float spawnHeight = Random.Range(minSpawnHeight, maxSpawnHeight);
            spawnPosition.y = spawnHeight;

            GameObject bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);

            // Do not set the terrain as the parent initially
            // bird.transform.parent = terrain.transform;

            Bird birdComponent = bird.GetComponent<Bird>();
            if (birdComponent != null)
            {
                birdComponent.SetTerrainDimensions(terrain.terrainData.size.x, terrain.terrainData.size.z);
                birdComponent.SetTerrain(terrain);
            }

            // Set the new terrain as the parent after updating bird properties
            bird.transform.parent = terrain.transform;
        }
    }


    Vector3 GetRandomTerrainPoint()
    {
        float randomX = Random.Range(0f, terrain.terrainData.size.x);
        float randomZ = Random.Range(0f, terrain.terrainData.size.z);
        float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0f, randomZ));

        return new Vector3(randomX, terrainHeight, randomZ);
    }

    // Example method to handle terrain destruction
    public void DestroyTerrain()
    {
        // Destroy the terrain and all its children (including birds)
        Destroy(terrain.gameObject);
    }
}