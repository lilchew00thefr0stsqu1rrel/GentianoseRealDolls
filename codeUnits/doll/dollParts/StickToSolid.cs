using UnityEngine;

namespace GentianoseRealDolls
{

    public class StickToSolid : MonoBehaviour
    {

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        private Rigidbody rb;

        // Update is called once per frame
        void Update()
        {

        }



        private void OnCollisionEnter(Collision collision)
        {
            if (collision != null && !collision.collider.transform.root.GetComponent<Doll>() 
                && !collision.collider.transform.root.GetComponent<Poop>())
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }

}