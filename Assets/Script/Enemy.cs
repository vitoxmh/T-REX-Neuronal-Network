using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float initialSpeed = 2f; // Velocidad inicial del piso
    public float acceleration = 0.1f; // Aceleración gradual del piso
    public float maxSpeed = 10f; // Velocidad máxima del piso
    public float currentSpeed;
    public float positionX;
    public int type;
    public Animator Animator;
    
 

    // Start is called before the first frame update
    void Start()
    {
       Animator = GetComponent<Animator>();
    }
      

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.gm.gameOver){

            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);

            // Desplazar el piso hacia la izquierda a la velocidad actual
            transform.position = new Vector3(transform.position.x - (currentSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }else{ 

            Animator.speed = 0f;
            

        }
    
    }
}
