using SpaceShooter;
using UnityEngine;

namespace GentianoseRealDolls
{
    [RequireComponent(typeof(BoxCollider))]
    public class Buoyancy : MonoBehaviour
    {
        [SerializeField] private float m_BuoyantForce;
        [SerializeField] private float m_FishFactor;
        [SerializeField] private float m_StockLiquid = -0.24f;

        private BoxCollider m_BoxCollider;

        private void Start()
        {
            m_BoxCollider = GetComponent<BoxCollider>();    
        }

        private void OnTriggerStay(Collider other)
        {
            Destructible destructible = other.transform.root.GetComponent<Destructible>();
            if (destructible != null)
            {
               

                if (other.transform.root.GetComponent<Doll>() != null)
                {

                    Rigidbody rb = other.transform.root.GetComponent<Rigidbody>();
                    if (rb != null && other.transform.position.y < transform.position.y - 0.05f)
                    {
                        print($"FA ");
                        rb.AddForce(Vector3.up * rb.mass * m_BuoyantForce);
                    }
                    print("Submerged");
                }
                if (destructible.UnitID == "20201")
                {

                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    if (rb != null && other.transform.position.y < transform.position.y)
                    {
                        rb.AddForce(Vector3.up * rb.mass * m_BuoyantForce * m_FishFactor);
                        print("FA");
                    }
                    print("Submerged");
                }
            }

        }


    }
}

