using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    public Transform target;
    Vector3 distance;
    public float followspeed;
    [SerializeField][Range(0f, 1f)] float lerptime;
    [SerializeField] Color[] myColor;
    int ci = 0;
    float change = 0f;
    int length;
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        distance = target.position - transform.position;
        length = myColor.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.position.y >= 0)
        {
            Follow();
        }
        Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, myColor[ci], lerptime * Time.deltaTime);
        change = Mathf.Lerp(change, 1f, lerptime * Time.deltaTime);
        if (change > 0.9) 
        {
            change = 0;
            ci++;
            if (ci >= length)
            {
                ci = 0;
            }
        }
    }
    void Follow()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetpos = target.position - distance;
        transform.position = Vector3.Lerp(currentPos, targetpos, followspeed*Time.deltaTime);
    }
}
