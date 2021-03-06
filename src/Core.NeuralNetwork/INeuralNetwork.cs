﻿using DigitR.Core.NeuralNetwork.Algorithms;
using DigitR.Core.NeuralNetwork.InputProvider;

namespace DigitR.Core.NeuralNetwork
{
    /// <summary>
    /// Provides an interface of neural network.
    /// </summary>
    public interface INeuralNetwork<TData>
    {
        /// <summary>
        /// Provides a determination logic according to input pattern 
        /// and current state of this instance of neuran network.
        /// </summary>
        /// <param name="inputPattern">The input pattern for determine.</param>
        /// <param name="processingAlgorithm"></param>
        /// <returns>The result successful flag.</returns>
        TData[] Process(
            IInputPattern<TData[]> inputPattern, 
            IProcessingAlgorithm<INeuralNetwork<TData>, IInputPattern<TData[]>, TData> processingAlgorithm);
    }
}
