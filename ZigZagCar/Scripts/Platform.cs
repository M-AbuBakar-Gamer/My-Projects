using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject destroyeffect;
    public GameObject Diamond, Star;
   
    // Start is called before the first frame update
    void Start()
    {
        Vector3 temPos = transform.position;
        temPos.y += 1f;
        int rand = Random.Range(1, 40);
        int rand2 = Random.Range(0, 150);
        if(rand < 4)
        {
            Instantiate(Star, temPos, Star.transform.rotation);
        }
        if (rand2 == 50)
        {
            Instantiate(Diamond, temPos, Diamond.transform.rotation);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Invoke("Falldown",.1f);
            Invoke("destroy", .5f);
        }
    }

    void Falldown()
    {
        if (gameObject != null)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            Destroy(gameObject, .5f);
            
        }
    }
    void destroy()
    {
        Instantiate(destroyeffect, transform.position, Quaternion.identity);
    }

}
