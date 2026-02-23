using SpaceShooter;

public class ActiveCamera : SingletonBase<ActiveCamera>
{
    private FollowCamera m_Cam;

    private new void Awake()
    {
        base.Awake();

        m_Cam = GetComponent<FollowCamera>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
