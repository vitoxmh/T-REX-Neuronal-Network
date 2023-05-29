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


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if(network == null){

            network = new NeuralNetwork(4,2,3);
        }
       
    }

    void Update()
    {
        if (muerto) return;

        float[] inputs = new float[4];
        
        float distancia = transform.position.x - GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>().position.x;

        inputs[0] = GameManager.gm.currentSpeed; // Velocidad
        inputs[1] = distancia;
        inputs[2] = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>().position.y;
        inputs[3] = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>().position.x;

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


        if(enElSuelo){


            if(agachado){

                 animator.SetInteger("PlayerAnimation", 4); 

            }else{

                 animator.SetInteger("PlayerAnimation", 0);

            }
            

        }else{


            animator.SetInteger("PlayerAnimation", 1);

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
                Destroy(gameObject);
      
        }


        /*else if (col.gameObject.CompareTag("Obstaculo"))
        {
            muerto = true;
            animator.SetBool("Muerto", true);
            // Aquí puedes agregar lógica adicional para el manejo de la muerte del dinosaurio
        }*/
    }
}
