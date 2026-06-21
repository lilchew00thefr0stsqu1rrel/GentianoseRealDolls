using System.Threading.Tasks;
using UnityEngine;

public class EffectPart : MonoBehaviour
{
    private void OnEnable()
    {
        Hide();
    }

    private async void Hide()
    {
        await Task.Delay(3000);

        gameObject.SetActive(false);
    }
}
