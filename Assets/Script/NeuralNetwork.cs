using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NeuralNetwork : ICloneable 
{
    public int numInputs;
    public int numHidden;
    public int numOutputs;
    public float[][] ihWeights; // pesos entrada -> oculta
    public float[][] hoWeights; // pesos oculta -> salida
    public float[] hiddenBias;
    public float[] outputBias;
    public float[] hiddenLayer;
    public float[] outputLayer;

    public NeuralNetwork(int numInputs, int numHidden, int numOutputs)
    {
        this.numInputs = numInputs;
        this.numHidden = numHidden;
        this.numOutputs = numOutputs;


        ihWeights = new float[numInputs][];
        for (int i = 0; i < numInputs; i++)
        {
            ihWeights[i] = new float[numHidden];
        }
        hoWeights = new float[numHidden][];
        for (int i = 0; i < numHidden; i++)
        {
            hoWeights[i] = new float[numOutputs];
        }
        hiddenBias = new float[numHidden];
        outputBias = new float[numOutputs];
        hiddenLayer = new float[numHidden];
        outputLayer = new float[numOutputs];
        InitWeights();
    }

    private void InitWeights()
    {
        // Inicializar pesos y sesgos aleatorios
        for (int i = 0; i < numInputs; i++)
        {
            for (int j = 0; j < numHidden; j++)
            {
                ihWeights[i][j] = UnityEngine.Random.Range(-1f, 1f);
            }
        }
        for (int i = 0; i < numHidden; i++)
        {
            for (int j = 0; j < numOutputs; j++)
            {
                hoWeights[i][j] = UnityEngine.Random.Range(-1f, 1f);
            }
        }
        for (int i = 0; i < numHidden; i++)
        {
            hiddenBias[i] = UnityEngine.Random.Range(-1f, 1f);
        }
        for (int i = 0; i < numOutputs; i++)
        {
            outputBias[i] = UnityEngine.Random.Range(-1f, 1f);
        }
    }

    public float[] FeedForward(float[] inputs)
    {
        // Propagación hacia adelante
        for (int i = 0; i < numHidden; i++)
        {
            float sum = 0f;
            for (int j = 0; j < numInputs; j++)
            {
                sum += inputs[j] * ihWeights[j][i];
            }
            hiddenLayer[i] = ReLU(sum + hiddenBias[i]);
        }
        for (int i = 0; i < numOutputs; i++)
        {
            float sum = 0f;
            for (int j = 0; j < numHidden; j++)
            {
                sum += hiddenLayer[j] * hoWeights[j][i];
            }
            outputLayer[i] = ReLU(sum + outputBias[i]);
        }
        return outputLayer;
    }

    private float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }


    private float ReLU(float x)
    {
        return Math.Max(0f, x);
    }


    public object Clone()
    {
        return (NeuralNetwork)MemberwiseClone();
    }

}

