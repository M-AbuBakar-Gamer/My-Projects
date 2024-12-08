using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformspawner : MonoBehaviour
{
    public GameObject Platform;
    public Transform lastplatform;
    Vector3 laspos;
    Vector3 newpos;
    public bool stop;
    // Start is called before the first frame update
    void Start()
    {
        laspos = lastplatform.position;
        StartCoroutine(SpawnPlatform());
      
    }//Start 

    // Update is called once per frame
    void Update()
    {
        
    }
    void GenratePos()
    {
        newpos = laspos;
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            newpos.x += 2f;
        }
        else
        {
            newpos.z += 2f;
        }
        
    }
    IEnumerator SpawnPlatform()
    {
        while (!stop) 
        {
            GenratePos();
            Instantiate(Platform, newpos, Quaternion.identity);
            laspos = newpos;
            yield return new WaitForSeconds(.15f);
        }
    }
}
