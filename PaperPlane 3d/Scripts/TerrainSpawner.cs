using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    public Terrain terrainPrefab;
    public Transform player;

    private List<Terrain> instantiatedTerrains = new List<Terrain>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GenerateTerrainOnLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            GenerateTerrainOnRight();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            GenerateTerrainOnFront();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called");
        
        // Check if the entering object has the "Left" tag
        if (other.CompareTag("Left"))
        {
            GenerateTerrainOnLeft();
        }
        else
        if (other.CompareTag("Right"))
        {
            GenerateTerrainOnRight();
        }
        else
        if (other.CompareTag("Front"))
        {
            GenerateTerrainOnFront();
        }
    }

    public void GenerateTerrainOnLeft()
    {
        SpawnTerrainRelativeToPlayer(Vector3.left);
    }

    public void GenerateTerrainOnRight()
    {
        SpawnTerrainRelativeToPlayer(Vector3.right);
    }

    public void GenerateTerrainOnFront()
    {
        SpawnTerrainRelativeToPlayer(Vector3.forward);
    }

    public void SpawnTerrainRelativeToPlayer(Vector3 direction)
    {
        if (player != null)
        {
            Terrain currentTerrain = GetCurrentTerrain(player.position);

            if (currentTerrain != null)
            {
                Vector3 targetPosition = currentTerrain.transform.position + direction * terrainPrefab.terrainData.size.z;

                if (IsPositionFree(targetPosition))
                {
                    Terrain newTerrain = InstantiateTerrain(targetPosition);

                    // Add the following line to create a new terrain collider
                    newTerrain.gameObject.AddComponent<TerrainCollider>();

                    AddToInstantiatedTerrains(newTerrain);
                }
                else
                {
                    Debug.LogWarning("Terrain already exists at the target position. Skipping instantiation.");
                }
            }
        }
        else
        {
            Debug.LogError("Player reference is not set. Make sure to assign the player's transform.");
        }
    }

    private Terrain InstantiateTerrain(Vector3 position)
    {
        Terrain newTerrain = Instantiate(terrainPrefab);
        newTerrain.transform.position = position;
        newTerrain.terrainData = Instantiate(terrainPrefab.terrainData); // Create a new instance of TerrainData
        return newTerrain;
    }

    



private Terrain GetCurrentTerrain(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(position, Vector3.down), out hit))
        {
            return hit.collider.GetComponent<Terrain>();
        }

        return null;
    }

    private bool IsPositionFree(Vector3 position)
    {
        foreach (Terrain terrain in instantiatedTerrains)
        {
            if (terrain != null && Vector3.Distance(position, terrain.transform.position) < terrain.terrainData.size.x)
            {
                return false;
            }
        }

        return true;
    }

    private void AddToInstantiatedTerrains(Terrain terrain)
    {
        instantiatedTerrains.Add(terrain);
    }
}
