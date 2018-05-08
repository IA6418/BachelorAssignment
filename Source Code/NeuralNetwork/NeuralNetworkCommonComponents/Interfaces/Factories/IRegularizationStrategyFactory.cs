﻿using NeuralNetwork.CommonComponents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.CommonComponents.Interfaces
{
    public interface IRegularizationStrategyFactory
    {
        IRegularizationStrategy CreateRegularizationStrategy(RegularizationType regularizationType);
    }
}
