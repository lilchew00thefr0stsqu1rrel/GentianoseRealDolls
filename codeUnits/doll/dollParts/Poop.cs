using GentianoseRealDolls;
using UnityEngine;

public class Poop : MonoBehaviour
{
    [SerializeField] private Material m_Material;
    [SerializeField] private MeshFilter m_MeshFilter;
    [SerializeField] private MeshRenderer m_MeshRenderer;
    [SerializeField] private int m_PoopSize;
    public int Size => m_PoopSize;
    [SerializeField] private int m_DollID;
    public int DollID => m_DollID;
    private Vector3 m_Scale;
    public Vector3 Scale => m_Scale;
    [SerializeField] private Rigidbody m_Rigidbody;
    private float m_Mass;
    public float Mass => m_Mass;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    public void InitPoop(DollAsset da)
    {
        m_MeshFilter.sharedMesh = da.PoopMesh.sharedMesh;
        m_MeshRenderer.material = da.PoopMaterial; 
        m_PoopSize = da.PoopSize;
        m_DollID = da.DollID;
        m_Scale = da.PoopScale;
        m_MeshRenderer.transform.localScale = m_Scale;
        m_Mass = da.PoopMass;
        m_Rigidbody.mass = m_Mass;
    }
    public void SetShape(int shapeID, Material material, Mesh mesh, int size, Vector3 scale, float mass)
    {
        m_MeshFilter.sharedMesh = mesh;
        m_MeshRenderer.material = material;
        m_PoopSize = size;
        m_DollID = shapeID;
        m_Scale = scale;
        m_MeshRenderer.transform.localScale = m_Scale;
        m_Mass = mass;
        m_Rigidbody.mass = m_Mass;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
