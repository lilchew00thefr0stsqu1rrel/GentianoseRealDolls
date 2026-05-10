
using VContainer;
using VContainer.Unity;
using UnityEngine;
using SpaceShooter;
using UnityEngine.SceneManagement;

namespace GentianoseRealDolls
{
    public class RootScope : LifetimeScope
    {
        IObjectResolver _container;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("Root Scope Started");
            Debug.Log("Scene: " + SceneManager.GetActiveScene().name);

            builder.Register<CurrentSceneData>(Lifetime.Singleton);
           
            builder.Register<AllDollCharacters>(Lifetime.Singleton);
            builder.Register<AllDollPositions>(Lifetime.Singleton);
            builder.Register<AllDollSleeps>(Lifetime.Singleton);

            builder.Register<StringCoordinates>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<TeleportBeasts>();

            // //// builder.RegisterComponentInHierarchy<MainMenu>();

            builder.RegisterComponentInHierarchy<Dashboard>();
        }
    }
}
