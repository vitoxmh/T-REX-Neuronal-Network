using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScroll : MonoBehaviour
{
    public float initialSpeed = 2f; // Velocidad inicial del piso
    public float acceleration = 0.1f; // Aceleración gradual del piso
    public float maxSpeed = 10f; // Velocidad máxima del piso
    public float tileSize = 10f; // Tamaño del sprite del piso
    public Vector3 startPosition;
    public float currentSpeed;
    public string nameFloor;

    private void Start()
    {
        
        currentSpeed = initialSpeed;
    }

    private void Update()
    {
        if(!GameManager.gm.gameOver){
            // Aumentar la velocidad con el tiempo hasta alcanzar la velocidad máxima
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);

            // Desplazar el piso hacia la izquierda a la velocidad actual
            transform.position = new Vector3(transform.position.x - (currentSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }

        // Si el piso se ha desplazado una distancia igual a su tamaño, reiniciar su posición a la posición inicial
        if (transform.position.x <= tileSize)
        {   

            float x = GameObject.Find(nameFloor).GetComponent<Transform>().position.x + GameObject.Find(nameFloor).GetComponent<BoxCollider2D>().size.x;
            Vector3 po = new Vector3(x,transform.position.y,transform.position.z);
            transform.position = po;
        }
    }
}