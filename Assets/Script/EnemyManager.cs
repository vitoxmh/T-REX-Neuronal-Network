using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float intervaloTiempo = 5f;

    private void Start()
    {
       
        StartCoroutine(EjecutarMetodo());
    }

    IEnumerator EjecutarMetodo()
    {
        while (true)
        {
            
            MetodoAEjecutar();

           
            yield return new WaitForSeconds(intervaloTiempo);
        }
    }

    void MetodoAEjecutar()
    {
         GameManager.gm.newEnemy();
    }
}
