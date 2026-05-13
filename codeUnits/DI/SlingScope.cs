using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GentianoseRealDolls
{
    public class SlingScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InteractableObject>();
            builder.RegisterComponentInHierarchy<SarvaToilet>();
            builder.RegisterComponentInHierarchy<Mechanism>();
            builder.RegisterComponentInHierarchy<Bed>();

            builder.RegisterComponentInHierarchy<ZifferGate>();
            builder.Register<ThrowBeastsToScene>(Lifetime.Scoped);
        }
    }
}
