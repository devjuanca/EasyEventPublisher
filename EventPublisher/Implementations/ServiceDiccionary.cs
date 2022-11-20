using EasyEventPublisher.Interfaces;

namespace EasyEventPublisher.Implementations;

internal class ServiceDiccionary : IServiceDiccionary
{
    public Dictionary<Type, Type> ServiceKeyPairValues { get; set; } = new();
}
