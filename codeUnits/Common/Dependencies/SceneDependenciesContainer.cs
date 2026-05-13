using SpaceShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GentianoseRealDolls
{

    public class SceneDependenciesContainer : Dependency
    {
        [SerializeField] private Level level;


        protected override void BindAll(MonoBehaviour monoBehaviourInScene)
        {
            Bind<Level>(level, monoBehaviourInScene);
        } 

        //private void Bind(MonoBehaviour mono)
    
    

        private void Awake()
        {
            FindAllObjectToBind();
        }
    }
}
