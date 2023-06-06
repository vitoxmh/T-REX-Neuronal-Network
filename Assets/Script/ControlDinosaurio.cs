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
    public LayerMask mascaraObjetivo;
    public float distanciaMaxima = 15f;
    public float angle;


    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
   
    }



    void Update()
    {
        if (muerto) return;
       
     
       
        float[] inputs = new float[6];
        
    
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

                float distanciaEnemy2 = 1f;

                float angleInRadians = Mathf.Deg2Rad * angle;
                Vector2 direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

                // Lanzar el rayo
                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, direction, distanciaMaxima);

                Debug.DrawRay(transform.position, direction * distanciaMaxima, Color.blue); 


               

                 if (hit2.collider.tag == "Enemy")
                {

                         distanciaEnemy2 = hit2.distance;

                }


                if(enemigo != null){

                    float distancia = 1f;;

   
                    Debug.DrawRay(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector3.right * distanciaMaxima, Color.red);

                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector3.right, distanciaMaxima);

                    if (hit.collider != null)
                    {
                        
                        if (hit.collider.tag == "Enemy")
                        {

                            Debug.Log("Enemigo:"+hit.distance);
                            distancia = hit.distance;

                        }

                    }


                    inputs[0] = GameManager.gm.currentSpeed; // Velocidad
                    inputs[1] = distancia;
                    inputs[2] = enemigo.GetComponent<Transform>().position.y;
                    inputs[3] = enemigo.GetComponent<Transform>().position.x;

                    if(enemigo.GetComponent<Enemy>().type == 1){

                             inputs[4] = 1f;
                    }else{

                            inputs[4] = 0f;

                    }
                   

                    inputs[5] = distanciaEnemy2;
            
                    float[] outputs = network.FeedForward(inputs);


                    if (enElSuelo && outputs[0] > 0.8f)
                    {
                        rb.AddForce(new Vector2(0f, velocidadSalto), ForceMode2D.Impulse);
                        enElSuelo = false;
                        animator.SetInteger("PlayerAnimation", 1);
     


                    }else if (outputs[1] > 0.8f)
                    {
                        agachado = true;
                    
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
               
                 Destroy(gameObject);

        }

    }
}
