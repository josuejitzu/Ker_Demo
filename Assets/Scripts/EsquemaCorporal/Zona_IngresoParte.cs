using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EsquemaCorporal

{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class Zona_IngresoParte : MonoBehaviour
    {
        private SphereCollider trigger;
        private Rigidbody rigid;
       [SerializeField]public ParteCuerpoID parteCuerpo;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawSphere(this.transform.position, this.transform.localScale.x);
        }

        private void OnValidate()
        {
            trigger = this.GetComponent<SphereCollider>();
            rigid = this.GetComponent<Rigidbody>();

            this.transform.name = "Zona_Ingreso_" + parteCuerpo.ToString();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("mano"))
            {
                Debug.Log("Mano: " + other.transform.name + " entro en Zona_" + parteCuerpo.ToString());
                Coreografia_Cuerpo.instancia.VerificarParteCuerpo(parteCuerpo);

            }
        }
    }
}
