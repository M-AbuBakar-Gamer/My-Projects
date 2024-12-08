using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CarController : MonoBehaviour
{
    public float movespeed;
    bool faceleft, firstTab;
    // Start is called before the first frame updateate is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGamestarted)
        {
            Move();
            checkInput();
        }
        if(transform.position.y <= -4)
        { 
            GameManager.Instance.Gameover();
        }
       
    }
    void Move()
    {
        transform.position += transform.backword * movespeed * Time.deltaTime;
    }
    void checkInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeDir();
        }
    }
    void ChangeDir()
    {
        if (faceleft) 
        {
            faceleft = false;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            faceleft = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
