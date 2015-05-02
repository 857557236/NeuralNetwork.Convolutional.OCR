﻿using DigitR.Core.InputProvider;
using DigitR.Core.Output;

namespace DigitR.Core.NeuralNetwork
{
    /// <summary>
    /// 
    /// </summary>
    public interface INeuralNetworkProcessor
    {
        object NeuralNetwork
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputProvider"></param>
        /// <param name="outputProvider"></param>
        /// <returns></returns>
        bool Process(IInputProvider inputProvider, IOutputProvider outputProvider);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainingInputProvider"></param>
        /// <returns></returns>
        bool Train(IInputProvider trainingInputProvider);
    }
}