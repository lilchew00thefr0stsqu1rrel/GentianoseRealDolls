
using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace GentianoseRealDolls
{
    public class GlobalDependenciesContainer : Dependency
    {


        [SerializeField] private FollowCamera followCamera;
        private static GlobalDependenciesContainer instance;

        // Не имеют смысла в главном меню, но общие между картами
        [SerializeField] private Party party;
        [SerializeField] private Dashboard dashboard;

        [SerializeField] private AllDollCharacters allDollCharacters;

        [SerializeField] private AllDollSleeps allDollSleeps;
        [SerializeField] private CurrentSceneData currentScene;
        [SerializeField] private CombatDashboard combatDashboard;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected override void BindAll(MonoBehaviour mono)
        {
            //Bind<Dashboard>(dashboard, mono);
            Bind<FollowCamera>(followCamera, mono);
            Bind<Party>(party, mono);
            Bind<Dashboard>(dashboard, mono);
            Bind<AllDollCharacters>(allDollCharacters, mono);
            Bind<AllDollSleeps>(allDollSleeps, mono);
            Bind<CurrentSceneData>(currentScene, mono);
            Bind<CombatDashboard>(combatDashboard, mono);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            FindAllObjectToBind();
        }
    }
    }

