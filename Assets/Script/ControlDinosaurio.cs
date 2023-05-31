using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDinosaurio : MonoBehaviour
{
    public float velocidadSalto = 5f;
    public float gravedadExtra = 10f;
    private bool enElSuelo = true;
    private bool agachado = false;
    private bool muerto = false;
    private Rigidbody2D rb;
    private Animator animator;

    public NeuralNetwork network;

    public bool dead = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        

        
       
    }



    void Update()
    {
        if (muerto) return;
        if(network != null){

               Debug.Log(network.ihWeights[0][0]+"====1");
        }
     
       
        float[] inputs = new float[4];
        
        float distancia = transform.position.x - GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>().position.x;

        inputs[0] = GameManager.gm.currentSpeed; // Velocidad
        inputs[1] = distancia;
        inputs[2] = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>().position.y;
        inputs[3] = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>().position.x;

        float[] outputs = network.FeedForward(inputs);

        if(!dead){

            if (enElSuelo && outputs[0] > 0.5f)
            {
                rb.AddForce(new Vector2(0f, velocidadSalto), ForceMode2D.Impulse);
                enElSuelo = false;
                animator.SetInteger("PlayerAnimation", 1);
            }

            if (outputs[1] > 0.5f)
            {
                agachado = true;
            
            }

            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                agachado = false;
            
            }


            if(enElSuelo){


                if(agachado){

                    animator.SetInteger("PlayerAnimation", 4); 

                }else{

                    animator.SetInteger("PlayerAnimation", 0);

                }
                

            }else{


                animator.SetInteger("PlayerAnimation", 1);

            }
        }else{

             animator.SetInteger("PlayerAnimation", 3);
        }

    }


     void FixedUpdate()
    {
        if (muerto) return;

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (gravedadExtra - 1) * Time.fixedDeltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Floor"))
        {
            enElSuelo = true;
      
        }


        if (col.gameObject.CompareTag("Enemy"))
        {
                GameManager.gm.removeList(gameObject);
                dead = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                
       
        }

    }
}
