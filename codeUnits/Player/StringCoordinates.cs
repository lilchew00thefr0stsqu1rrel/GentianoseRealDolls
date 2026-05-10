using GentianoseRealDolls;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StringCoordinates : MonoBehaviour
{

    [SerializeField]
    private int[] levelsAsScenes = new int[]
    {
        1, 2, 3
    };
    public int[] LevelsAsScenes => levelsAsScenes;
    public Vector3 GetPositionFromString(string posString)
    {
        int city = int.Parse(posString[..2]);
        Vector3 pos = Vector3.zero;

        if (posString.Length == 14)
        {
            int x = int.Parse(posString.Substring(2, 4));
            int y = int.Parse(posString.Substring(6, 4));
            int z = int.Parse(posString.Substring(10, 4));
            pos = new Vector3(x, y, z);
        }
        if (posString.Length == 26)
        {
            float x = float.Parse(posString.Substring(2, 8));
            float y = float.Parse(posString.Substring(10, 8));
            float z = float.Parse(posString.Substring(18, 8));
            pos = new Vector3(x, y, z);
        }

        print("City: " + city + "  Legend: 1: Rusikova, 2: Kukly");

        return pos;
    }

    public int GetLocationFromString(string posString)
    {
         return  int.Parse(posString[..2]);
    }
}
