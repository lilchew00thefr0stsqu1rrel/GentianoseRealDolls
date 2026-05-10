using GentianoseRealDolls;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuScope : LifetimeScope
{
    [SerializeField] private MainMenu m_MainMenu;
    protected override void Configure(IContainerBuilder builder)
    {
        Debug.Log("MainMenuScope Started");
        builder.Register<MainMenu>(Lifetime.Scoped);
    }
}
