using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScroll : MonoBehaviour
{
    public float velocidad = 5f;
    public float limite = -15f;
    public float inicio = 15f;

    void Update()
    {
        transform.position -= new Vector3(velocidad * Time.deltaTime, 0, 0);

        if (transform.position.x <= limite)
        {
            transform.position = new Vector3(inicio, transform.position.y, transform.position.z);
        }
    }
}