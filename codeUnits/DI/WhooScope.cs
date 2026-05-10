using GentianoseRealDolls;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class WhooScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        Debug.Log("WhooScope Started");
        builder.RegisterComponentInHierarchy<GiveResource>();
        builder.RegisterComponentInHierarchy<ZifferGate>();
    }
}
