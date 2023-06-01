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
    public Text textTime;
    public Text nDinosario;
    public Text generationText;
    public float TimeGame = 0f;
    



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

             TimeGame += Time.deltaTime;


                nDinosario.text = "Dinosaurios:"+dinos.Count;
                generationText.text = "Generation:"+generation;

                int seconds = Mathf.RoundToInt(TimeGame);

                textTime.text = "TIME:" + seconds.ToString("000");

                if(dinos.Count >= 1){

                    bestDinoNeuronal  = (NeuralNetwork)dinos[0].GetComponent<ControlDinosaurio>().network.Clone();

                }

                if(dinos.Count == 0){

                    
                    gameOver = true;

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
            
            NeuralNetwork t =  cruzar(bestDinoNeuronal);
         
            nDino.GetComponent<ControlDinosaurio>().network = t;

            dinos.Add(nDino);
           
        }

         generation++;   
    }




    private NeuralNetwork cruzar(NeuralNetwork newN){


        NeuralNetwork hijo = new NeuralNetwork(4,2,2);

       
        for (int i = 0; i < newN.numInputs; i++)
        {
            for (int j = 0; j < newN.numHidden; j++)
            {
                hijo.ihWeights[i][j] =  newN.ihWeights[i][j];
            }
        }
        for (int i = 0; i < newN.numHidden; i++)
        {
            for (int j = 0; j < newN.numOutputs; j++)
            {
                hijo.hoWeights[i][j] = newN.hoWeights[i][j];
            }
        }
        for (int i = 0; i < newN.numHidden; i++)
        {
            hijo.hiddenBias[i] = newN.hiddenBias[i];
        }
        for (int i = 0; i < newN.numOutputs; i++)
        {
            hijo.outputBias[i] = newN.outputBias[i];
        }
       

        hijo.ihWeights[0][0] =  UnityEngine.Random.Range(-1f, 1f);
        hijo.ihWeights[1][1] =  UnityEngine.Random.Range(-1f, 1f);
        hijo.hoWeights[0][1] = UnityEngine.Random.Range(-1f, 1f);
        hijo.hiddenBias[0] = UnityEngine.Random.Range(-1f, 1f);
        hijo.outputBias[0] = UnityEngine.Random.Range(-1f, 1f);

      
        return hijo;

    }



    public void reset(){


    
      
       

    }

}
