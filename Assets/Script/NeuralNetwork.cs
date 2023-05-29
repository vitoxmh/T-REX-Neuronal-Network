using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    private int numInputs;
    private int numHidden;
    private int numOutputs;
    private float[][] ihWeights; // pesos entrada -> oculta
    private float[][] hoWeights; // pesos oculta -> salida
    private float[] hiddenBias;
    private float[] outputBias;
    private float[] hiddenLayer;
    private float[] outputLayer;

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
                ihWeights[i][j] = Random.Range(-1f, 1f);
            }
        }
        for (int i = 0; i < numHidden; i++)
        {
            for (int j = 0; j < numOutputs; j++)
            {
                hoWeights[i][j] = Random.Range(-1f, 1f);
            }
        }
        for (int i = 0; i < numHidden; i++)
        {
            hiddenBias[i] = Random.Range(-1f, 1f);
        }
        for (int i = 0; i < numOutputs; i++)
        {
            outputBias[i] = Random.Range(-1f, 1f);
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
            hiddenLayer[i] = Sigmoid(sum + hiddenBias[i]);
        }
        for (int i = 0; i < numOutputs; i++)
        {
            float sum = 0f;
            for (int j = 0; j < numHidden; j++)
            {
                sum += hiddenLayer[j] * hoWeights[j][i];
            }
            outputLayer[i] = Sigmoid(sum + outputBias[i]);
        }
        return outputLayer;
    }

    private float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }
}

