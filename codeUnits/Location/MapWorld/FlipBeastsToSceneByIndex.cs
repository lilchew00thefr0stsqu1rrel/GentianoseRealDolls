using UnityEngine;


namespace GentianoseRealDolls
{
    public class FlipBeastsToSceneByIndex : MonoBehaviour
    {
        

        public void Flip(int index)
        {
            switch (index)
            {
                case 0:
                    SceneHelper.EnterHouse();
                    break;
                case 1:
                    SceneHelper.ExitHouse();
                    break;
            }

            gameObject.SetActive(false);
        }
        public void Telep(string posString)
        {
            SceneHelper.Teleport(posString);

            gameObject.SetActive(false);
        }
    }

}
