using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject Staref, Diamondef;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, 100f)*Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(gameObject.tag == "Star")
            {
                GameManager.Instance.Getstar();
                Instantiate(Staref, transform.position, Quaternion.identity);
                
            }
           if(gameObject.tag == "Diamond")
            {
                GameManager.Instance.GetDiamond();
                Instantiate(Diamondef, transform.position, Quaternion.identity);
               
            }
           
        }
    Destroy(gameObject);
    }
    
}