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
            dinos.Add(nDino);   

          }

    }

    // Update is called once per frame
    void Update()
    {
         
         currentSpeed += acceleration * Time.deltaTime;
         currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);
         
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

}
