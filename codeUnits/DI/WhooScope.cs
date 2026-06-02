using GentianoseRealDolls;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class WhooScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        Debug.Log("WhooScope Started");

        builder.RegisterComponentInHierarchy<InteractableObject>();
        builder.RegisterComponentInHierarchy<SarvaToilet>();
        builder.RegisterComponentInHierarchy<Mechanism>();
        builder.RegisterComponentInHierarchy<Shop>();

        builder.RegisterComponentInHierarchy<GiveResource>();
        builder.RegisterComponentInHierarchy<ZifferGate>();
        builder.Register<ThrowBeastsToScene>(Lifetime.Scoped);
        builder.Register<EnemyWildlifeSpawner>(Lifetime.Scoped);
    }
}
