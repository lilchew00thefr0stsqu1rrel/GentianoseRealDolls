using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GentianoseRealDolls
{
    public class SlingScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InteractableObject>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<InteractableObject>();
            builder.RegisterComponentInHierarchy<Bed>();
            builder.RegisterComponentInHierarchy<SarvaToilet>();
            builder.RegisterComponentInHierarchy<ZifferGate>();
            builder.RegisterComponentInHierarchy<ExitHabitat>();
        }
    }
}
