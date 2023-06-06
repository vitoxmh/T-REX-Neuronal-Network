using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager gm;
     
    public int input;
    public int layer;
    public int output;
  
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
    public NeuralNetwork bestDinoNeuronal2;
    public bool gameOver = false;
    public Text textTime;
    public Text nDinosario;
    public Text generationText;
    public float TimeGame = 0f;
    public float mutationRate;
    public float bestLive = 0f;

   
    



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
   
        
          dinos = new List<GameObject>();

          if(generation == 1){


            inicioGeneracion();


          }else{

             newGeneration();
          }

    }


    private void inicioGeneracion(){

        for(int i = 0; i <= especimen; i++){
            

            GameObject nDino = Instantiate(dinosaruio, new Vector3(-5.917905f,-4.2f,0f), Quaternion.identity);
            nDino.GetComponent<ControlDinosaurio>().network =  new NeuralNetwork(input,layer,output);
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

             TimeGame += Time.deltaTime;


                nDinosario.text = "Dinosaurios:"+dinos.Count;
                generationText.text = "Generation:"+generation;

                int seconds = Mathf.RoundToInt(TimeGame);

                textTime.text = "TIME:" + seconds.ToString("000");

                if(dinos.Count >= 2){

                    if(dinos[0].GetComponent<ControlDinosaurio>().liveTime > bestLive){

                        bestDinoNeuronal  = (NeuralNetwork)dinos[0].GetComponent<ControlDinosaurio>().network.Clone();
                        bestDinoNeuronal2  = (NeuralNetwork)dinos[1].GetComponent<ControlDinosaurio>().network.Clone();
                        bestLive = dinos[0].GetComponent<ControlDinosaurio>().liveTime;

                    }
                  

                   

                }

                if(dinos.Count == 0){

                    bestLive = 0f;
                    reset();



                }

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
            
            NeuralNetwork t =  cruzar(bestDinoNeuronal,bestDinoNeuronal2);

            nDino.GetComponent<ControlDinosaurio>().network = Mutate(t);

            dinos.Add(nDino);
           
        }

         generation++;   
    }



    private NeuralNetwork Mutate(NeuralNetwork network)
    {
        NeuralNetwork mutatedNetwork = new NeuralNetwork(input,layer,output);

        for (int i = 0; i < network.numInputs; i++)
        {
            for (int j = 0; j < network.numHidden; j++)
            {
                if(Random.Range(0f, 1f) < mutationRate){

                    mutatedNetwork.ihWeights[i][j] =  Random.Range(0f, 1f);

                }else{

                    mutatedNetwork.ihWeights[i][j] =  network.ihWeights[i][j];
                }
                
            }
        }


        for (int i = 0; i < network.numHidden; i++)
        {
            for (int j = 0; j < network.numOutputs; j++)
            {
                
                if(Random.Range(0f, 1f) < mutationRate){

                    mutatedNetwork.hoWeights[i][j] = Random.Range(0f, 1f);

                }else{

                    mutatedNetwork.hoWeights[i][j] = network.hoWeights[i][j];
                }
            }
        }


        for (int i = 0; i < network.numHidden; i++)
        {
             if(Random.Range(0f, 1f) < mutationRate){

                  mutatedNetwork.hiddenBias[i] = Random.Range(0f, 1f);

             }else{

                  mutatedNetwork.hiddenBias[i] = network.hiddenBias[i];

             }
          
        }


        for (int i = 0; i < network.numOutputs; i++)
        {   
             if(Random.Range(0f, 1f) < mutationRate){

                mutatedNetwork.outputBias[i] = Random.Range(0f, 1f);

             }else{

                mutatedNetwork.outputBias[i] = network.outputBias[i];
             }
            
        }

        return mutatedNetwork;
    }



    private NeuralNetwork cruzar(NeuralNetwork newN,NeuralNetwork newN2){


        NeuralNetwork hijo = new NeuralNetwork(input,layer,output);

       
        for (int i = 0; i < newN.numInputs; i++)
        {
            for (int j = 0; j < newN.numHidden; j++)
            {
                if(Random.Range(0f, 1f) < 0.5f){

                    hijo.ihWeights[i][j] =  newN.ihWeights[i][j];

                }else{

                    hijo.ihWeights[i][j] =  newN2.ihWeights[i][j];
                }
                
            }
        }


        for (int i = 0; i < newN.numHidden; i++)
        {
            for (int j = 0; j < newN.numOutputs; j++)
            {
                

                
                if(Random.Range(0f, 1f) < 0.5f){

                    hijo.hoWeights[i][j] = newN.hoWeights[i][j];

                }else{

                    hijo.hoWeights[i][j] = newN2.hoWeights[i][j];
                }
            }
        }


        for (int i = 0; i < newN.numHidden; i++)
        {
             if(Random.Range(0f, 1f) < 0.5f){

                  hijo.hiddenBias[i] = newN.hiddenBias[i];

             }else{

                  hijo.hiddenBias[i] = newN2.hiddenBias[i];

             }
          
        }


        for (int i = 0; i < newN.numOutputs; i++)
        {   
             if(Random.Range(0f, 1f) < 0.5f){

                hijo.outputBias[i] = newN.outputBias[i];

             }else{

                hijo.outputBias[i] = newN2.outputBias[i];
             }
            
        }
             
        return hijo;

    }



    public void reset(){

        currentSpeed = initialSpeed;
        

        GameObject[] listEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < listEnemy.Length; i++)
        {

      
            Destroy(listEnemy[i]);

        }



        GameObject[] listFloor = GameObject.FindGameObjectsWithTag("FloorMove");

        for (int i = 0; i < listFloor.Length; i++)
        {

      
            listFloor[i].GetComponent<InfiniteScroll>().currentSpeed = currentSpeed;

        }

        newGeneration();
        
      
       

    }

}
