using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public NeuralNetwork bestDino;


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
            nDino.GetComponent<ControlDinosaurio>().network =  new NeuralNetwork(4,2,3);
            dinos.Add(nDino);   

          }

    }

    // Update is called once per frame
    void Update()
    {
        


        currentSpeed += acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);

        if(dinos.Count == 0){

                Debug.Log("Termina el juego");

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



    public void newGeneration(GameObject bestDino){

        NeuralNetwork network = bestDino.GetComponent<ControlDinosaurio>().network;
        network.ihWeights[0][0] = Random.Range(-1f, 1f);
        network.ihWeights[1][1] = Random.Range(-1f, 1f);
        network.ihWeights[0][1] = Random.Range(-1f, 1f);
        network.ihWeights[1][0] = Random.Range(-1f, 1f);




    }

}
