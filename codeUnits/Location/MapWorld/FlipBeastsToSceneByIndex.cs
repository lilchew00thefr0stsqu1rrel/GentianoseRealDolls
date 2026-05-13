using UnityEngine;


namespace GentianoseRealDolls
{
    public class FlipBeastsToSceneByIndex : MonoBehaviour
    {
        //TODO: Dependency
        [SerializeField] private Dashboard dashboard;

        [SerializeField] private TeleportBeasts teleportBeasts;
        [SerializeField] private Party party;

        public void Flip(int index)
        {
            switch (index)
            {
                case 0:
                    SceneHelper.EnterHouse(1);
                    break;
                case 1:
                    SceneHelper.ExitHouse();
                    break;
            }

            gameObject.SetActive(false);
        }

        public void Telep(string posString)
        {
            dashboard.CloseInventory();
            teleportBeasts.Teleport(posString, party.AreThereSleepingBeasts);
        }
    }

}
