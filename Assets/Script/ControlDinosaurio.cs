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
    public float currentSpeed;
    public float liveTime = 0;
    public bool gamer = false;


    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
   
    }



    void Update()
    {
        if (muerto) return;
       
     
       
        float[] inputs = new float[4];
        
        

       

      

        if(!dead){
            
            liveTime += Time.deltaTime;


            if(gamer){


                if (enElSuelo && Input.GetKeyDown("space"))
                {
                    rb.AddForce(new Vector2(0f, velocidadSalto), ForceMode2D.Impulse);
                    enElSuelo = false;
                    animator.SetInteger("PlayerAnimation", 1);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    agachado = true;
                
                }

                else if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    agachado = false;
                
                }

            }else{

                GameObject enemigo = GameObject.FindGameObjectWithTag("Enemy");

                if(enemigo != null){

                    float distancia = transform.position.x - enemigo.GetComponent<Transform>().position.x;

                    inputs[0] = GameManager.gm.currentSpeed; // Velocidad
                    inputs[1] = distancia;
                    inputs[2] = enemigo.GetComponent<Transform>().position.y;
                    inputs[3] = enemigo.GetComponent<Transform>().position.x;

                    float[] outputs = network.FeedForward(inputs);


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

                }

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
             
             // Desplazar el piso hacia la izquierda a la velocidad actual
              
            
            
        }
    }


     void FixedUpdate()
    {
        if (muerto) return;

        if (rb.velocity.y < 0 && !dead)
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
               
                if(GameManager.gm.dinos.Count == 1){

                    dead = true;

                }else{

                     
                    Destroy(gameObject);

                }

        }

    }
}
