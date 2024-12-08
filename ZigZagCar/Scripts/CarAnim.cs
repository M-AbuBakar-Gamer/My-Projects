using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnim : MonoBehaviour
{
    [SerializeField] Vector3 finalPosition;
    Vector3 InitialPos;
    private void Awake()
    {
        InitialPos = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, 0.2f);
        transform.Rotate(new Vector3(0f,40f,0)*Time.deltaTime);
    }

    private void OnDisable()
    {
        transform.position = InitialPos;
    }
}
