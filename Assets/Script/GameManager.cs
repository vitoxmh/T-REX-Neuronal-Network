using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager gm;

    public float initialSpeed = 2f; // Velocidad inicial del piso
    public float acceleration = 0.1f; // Aceleración gradual del piso
    public float maxSpeed = 10f; // Velocidad máxima del piso
    public float tileSize = 10f; // Tamaño del sprite del piso
    public float currentSpeed;
    public GameObject[] enemy;
    public GameObject dinosaruio;
    public int especimen;
    public List<GameObject> dinos;
    public int generation = 1;
    public NeuralNetwork bestDinoNeuronal;
    public bool gameOver = false;
    



     private void Awake()
    {
        if(gm == null)
        {
            gm = this;
            
        }
        else if (gm != null)
        {
            Destroy(gameObject);
        }
        

    }


    public void removeList(GameObject dino){

        dinos.Remove(dino);

    }

    void Start()
    {
   
          GameObject enemy =  GameObject.FindWithTag("Enemy");
          enemy.GetComponent<Enemy>().currentSpeed = currentSpeed;
          enemy.GetComponent<Enemy>().initialSpeed = initialSpeed;
          enemy.GetComponent<Enemy>().maxSpeed = maxSpeed;
          enemy.GetComponent<Enemy>().acceleration =acceleration;
          dinos = new List<GameObject>();

          for(int i = 0; i <= especimen; i++){
            

            GameObject nDino = Instantiate(dinosaruio, new Vector3(-5.917905f,-4.2f,0f), Quaternion.identity);
            nDino.GetComponent<ControlDinosaurio>().network =  new NeuralNetwork(4,2,2);
            dinos.Add(nDino);   

          }

    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(bestDinoNeuronal);
        if(!gameOver){
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);
        }

        if(dinos.Count >= 1){

           bestDinoNeuronal  = (NeuralNetwork)dinos[0].GetComponent<ControlDinosaurio>().network.Clone();

        }

        if(dinos.Count == 0){

               GameManager.gm.gameOver = true;
               //reset();

        }
         
    }


    public void newEnemy(){


        int nEnemy = Random.Range(0, enemy.Length);
        float nPosition = enemy[nEnemy].GetComponent<Enemy>().positionX;
        GameObject newEnemy = Instantiate(enemy[nEnemy], new Vector3(10.24f,nPosition,0), Quaternion.identity);

        newEnemy.GetComponent<Enemy>().currentSpeed = currentSpeed;
        newEnemy.GetComponent<Enemy>().acceleration = acceleration;
        newEnemy.GetComponent<Enemy>().maxSpeed = maxSpeed;
        newEnemy.GetComponent<Enemy>().initialSpeed = initialSpeed;
        


    }



    public void newGeneration(){



        for(int i = 0; i <= especimen; i++){
            
              
            GameObject nDino = Instantiate(dinosaruio, new Vector3(-5.917905f,-4.2f,0f), Quaternion.identity);
            
          
        NeuralNetwork t =  cruzar(bestDinoNeuronal);
         
            dinosaruio.GetComponent<ControlDinosaurio>().network = t;


            dinos.Add(nDino);
           

        }


         generation++;   
    }




    private NeuralNetwork cruzar(NeuralNetwork newN){


        NeuralNetwork hijo = new NeuralNetwork(4,2,2);

        hijo.numInputs = newN.numInputs;
        hijo.numHidden = newN.numHidden;
        hijo.numOutputs = newN.numOutputs;
        hijo.ihWeights = newN.ihWeights;
        hijo.hoWeights = newN.hoWeights;
        hijo.hiddenBias = newN.hiddenBias;
        hijo.outputBias = newN.hiddenBias;

        hijo.ihWeights[0][0] = UnityEngine.Random.Range(-1f, 1f);
        hijo.ihWeights[1][1] = UnityEngine.Random.Range(-1f, 1f);
        hijo.hoWeights[0][0] = UnityEngine.Random.Range(-1f, 1f);
        hijo.hiddenBias[0] = UnityEngine.Random.Range(-1f, 1f);
      
        return hijo;

    }



    public void reset(){

        currentSpeed = initialSpeed;
        
        newGeneration();


        GameObject floor1 =  GameObject.Find("Floor1");
        GameObject floor2 =  GameObject.Find("Floor2");
        floor1.GetComponent<Transform>().position = new Vector3(1f,GameObject.Find("Floor1").GetComponent<Transform>().position.y, 1 );
        floor1.GetComponent<InfiniteScroll>().currentSpeed = currentSpeed;
        floor2.GetComponent<Transform>().position = new Vector3(25.576f,GameObject.Find("Floor2").GetComponent<Transform>().position.y, 1 );
        floor2.GetComponent<InfiniteScroll>().currentSpeed = currentSpeed;

    }

}
