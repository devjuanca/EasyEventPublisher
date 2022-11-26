using EasyEventPublisher.Interfaces;
using System;
using System.Collections.Generic;

namespace EasyEventPublisher.Implementations
{

    internal class ServiceDiccionary : IServiceDiccionary
    {
        public Dictionary<Type, Type> ServiceKeyPairValues { get; set; } = new Dictionary<Type, Type>();
    }
}
