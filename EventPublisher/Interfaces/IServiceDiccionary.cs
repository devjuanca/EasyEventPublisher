using System;
using System.Collections.Generic;

namespace EasyEventPublisher.Interfaces
{

    internal interface IServiceDiccionary
    {
        Dictionary<Type, Type> ServiceKeyPairValues { get; set; }
    }
}
