﻿using System;

using DigitR.Core.NeuralNetwork;
using DigitR.Core.NeuralNetwork.Algorithms;
using DigitR.Core.NeuralNetwork.Factories;
using DigitR.Core.NeuralNetwork.InputProvider;
using DigitR.Core.NeuralNetwork.Primitives;

using DigitR.NeuralNetwork.Cnn.Algorithms.BackPropagation.Common;
using DigitR.NeuralNetwork.Cnn.Algorithms.Extensions;

namespace DigitR.NeuralNetwork.Cnn.Algorithms.BackPropagation.Steps.Implementation
{
    internal class BackPropagateOutputLayerStep : IPropagationStep
    {
        private readonly double learningSpeed;
        private readonly IActivationAlgorithm<double, double> activationAlgorithm;

        public BackPropagateOutputLayerStep(
            double learningSpeed,
            IActivationAlgorithm<double, double> activationAlgorithm)
        {
            this.learningSpeed = learningSpeed;
            this.activationAlgorithm = activationAlgorithm;
        }

        public void Process(IMultiLayerNeuralNetwork<double> network, IInputTrainingPattern<double[]> pattern)
        {
            ILayer<INeuron<double>, IConnectionFactory<double, double>> outputLayer = network.GetLayer(layer => layer.IsLast);

            for (int neuronIndex = 0; neuronIndex < outputLayer.Neurons.Length; neuronIndex++)
            {
                INeuron<double> currentNeuron = outputLayer.Neurons[neuronIndex];
                BackPropagateNeuronInfo currentNeuronInfo = currentNeuron.GetNeuronInfo<BackPropagateNeuronInfo>();

                if (!double.IsNaN(currentNeuronInfo.LocalGradient))
                {
                    throw new Exception("Local gradient is null.");
                }
                if (currentNeuron.Outputs.Count == 0)
                {
                    throw new Exception("Wrong number of inputs.");
                }

                double errorSignal = pattern.Label[neuronIndex] - currentNeuron.Output;
                double currentLocalGradient = errorSignal * activationAlgorithm.CalculateFirstDerivative(currentNeuron.Output);
                
                currentNeuronInfo.LocalGradient = currentLocalGradient;

                foreach (IConnection<double, double> connection in currentNeuron.Inputs)
                {
                    double correction = learningSpeed * currentLocalGradient * connection.Neuron.Output;
                    StoreWeightCorrection(connection, correction);
                }
            }
        }

        private static void StoreWeightCorrection(IConnection<double, double> connection, double correction)
        {
            if (connection.Weight.AdditionalInfo == null)
            {
                connection.Weight.AdditionalInfo = new BackPropagateWeightInfo { WeightCorrection = correction };
            }
            else
            {
                connection.Weight.GetInfo<BackPropagateWeightInfo>().WeightCorrection += correction;
            }
        }
    }
}
