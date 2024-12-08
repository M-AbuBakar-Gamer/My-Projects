using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlacer : MonoBehaviour
{
    public Terrain terrain;
    public GameObject[] treePrefabs;  // Use an array for tree prefabs

    void Start()
    {
        PlaceTrees();
    }

    void PlaceTrees()
    {
        // Randomly determine the number of trees
        int numberOfTrees = Random.Range(2500, 5000);

        for (int i = 0; i < numberOfTrees; i++)
        {
            // Random position on the terrain
            float randomX = Random.Range(0f, terrain.terrainData.size.x);
            float randomZ = Random.Range(0f, terrain.terrainData.size.z);

            // Get the height at the correct position
            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            // Adjust for the terrain's position
            y += terrain.transform.position.y;

            // Instantiate a random tree prefab at the random position
            GameObject treePrefab = GetRandomTreePrefab();
            GameObject instantiatedTree = Instantiate(treePrefab, new Vector3(randomX, y, randomZ), Quaternion.identity);

            // Set the terrain as the parent of the instantiated tree
            instantiatedTree.transform.parent = terrain.transform;
        }
    }

    GameObject GetRandomTreePrefab()
    {
        // Randomly choose a tree prefab from the array
        int randomIndex = Random.Range(0, treePrefabs.Length);
        return treePrefabs[randomIndex];
    }
}
