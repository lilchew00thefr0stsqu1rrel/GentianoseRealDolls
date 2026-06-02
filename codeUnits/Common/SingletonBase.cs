
using UnityEngine;

[DisallowMultipleComponent]
public abstract class SingletonBase<T> : MonoBehaviour where T:MonoBehaviour
{
    [Header("Singleton")]
    [SerializeField] private bool m_DoNotDestroyOnLoad;

    public static T Instance { get; private set; }

    public void Init()
    {
        if (Instance != null)
        {
            Debug.LogWarning("MonoSingleton: object of type already exists, instance will be destroyed = " + typeof(T).Name);
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
    }


    protected virtual void Awake()
    {
        Init();

        if (m_DoNotDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}



