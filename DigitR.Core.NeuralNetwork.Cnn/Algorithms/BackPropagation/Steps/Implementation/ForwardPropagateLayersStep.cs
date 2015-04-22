﻿using System;
using System.Linq;
using DigitR.Core.InputProvider;
using DigitR.Core.NeuralNetwork.Algorithms;
using DigitR.Core.NeuralNetwork.Primitives;

namespace DigitR.Core.NeuralNetwork.Cnn.Algorithms.BackPropagation.Steps.Implementation
{
    internal class ForwardPropagateLayersStep : IPropagationStep
    {
        private readonly IActivationAlgorithm<double, double> activationAlgorithm;

        public ForwardPropagateLayersStep(IActivationAlgorithm<double, double> activationAlgorithm)
        {
            if (activationAlgorithm == null)
            {
                throw new ArgumentNullException("activationAlgorithm");
            }
            this.activationAlgorithm = activationAlgorithm;
        }

        public void Process(IMultiLayerNeuralNetwork<double[], double[]> network, IInputTrainingPattern<double[], double[]> pattern)
        {
            foreach (ILayer<object> layer in network.Layers.Where(layer => !layer.IsFirst))
            {
                foreach (INeuron<double> neuron in layer.Neurons)
                {
                    double inducedLocalArea = neuron.Inputs
                        .Sum(connection => connection.Weight.Value * connection.Neuron.Output);

                    neuron.Output = activationAlgorithm.Calculate(inducedLocalArea);
                    neuron.AditionalInfo = new BackPropagateNeuronInfo
                    {
                        LocalGradient = Double.NaN,
                        LastInducesLocalAreaValue = inducedLocalArea
                    };
                }
            }
        }
    }
}
