using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirObjeto : MonoBehaviour
{

    public float velocidad = 10.0f;
    public float xVal;
    public Rigidbody rigid;
    public Transform posEsclavo;
    
    public bool hacerSeguimiento;
    [Tooltip("Activar si estas de espalda, de lo contrario es como un espejo")]
    public bool espejoEn_Z;

    [SerializeField] private Vector3 posInicial;
    [SerializeField] private Vector3 posInicial_Esclavo;
    [SerializeField] private Vector3 distancia;
    [SerializeField] private Vector3 posOffset;
    [SerializeField] private float diferenciaZ;
    [SerializeField] private float diferenciaZ_Esclavo;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, posEsclavo.position);
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawSphere(posEsclavo.position, 0.1f);
    }
    void Start()
    {
        //rigid = this.GetComponent<Rigidbody>();
        distancia = posEsclavo.position - this.transform.position;
        posInicial = this.transform.position;
        posInicial_Esclavo = posEsclavo.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //xVal = Input.GetAxis("Horizontal");
        //rigid.velocity = new Vector3(xVal, 0, 0);

      
        diferenciaZ =  this.transform.position.z - posInicial.z;
        diferenciaZ_Esclavo =  distancia.z - diferenciaZ ;

        posOffset = this.transform.position + distancia;
        posOffset.z = espejoEn_Z ? posInicial_Esclavo.z + diferenciaZ : posInicial_Esclavo.z - diferenciaZ;
        posEsclavo.transform.position = posOffset;


        Debug.DrawRay(this.transform.position, posOffset, Color.green);      
        Debug.Log("Distancia de Controlador a Esclavo" + distancia);
        Debug.Log("Pos final:"+ posOffset);

       
    }
}
