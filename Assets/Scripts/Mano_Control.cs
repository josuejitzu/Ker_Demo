using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Mano_Control : MonoBehaviour
{
    public OVRHand mano;
    public LineRenderer pointerRenderer;
    public OVRHand.HandFinger dedoSeleccion = OVRHand.HandFinger.Index;
    [SerializeField] private bool pinchIndex;

    public OVRSkeleton manoEsqueleto;   
    private List<OVRBone> huesosMano;
    public List<ManoGesto> gestosManos;

    public float distanciaPuntero = 2.0f;
    public bool debug;
    public LayerMask layerMask = 5;
    Vector3 posFin;

    [SerializeField] private Button botonSeleccionado;

    [Space(15)]
    [Header("Movimiento Joystick")]
    public OVRInput.Controller control;
    public OVRInput.Axis2D joystick = OVRInput.Axis2D.PrimaryThumbstick;
    [Range(0.0f,10.0f)]
    public float velocidad = 2.0f;
    public Transform camaraCentro;
    public Transform camaraEye;
    [SerializeField] Vector2 joystickMovimiento;
    [SerializeField] Vector3 posicionJoystick;
    [SerializeField] Quaternion rotacionEuler;
    [SerializeField] Vector3 direccionCamara;
    [SerializeField] Vector3 nuevaPosicion;
   
    private void OnDrawGizmos()
    {
        //Debug.DrawRay(this.transform.position, Vector3.Normalize((this.transform.forward ) + (this.transform.up )) * distanciaPuntero, Color.red );

    }
    void Start()
    {
        huesosMano = new List<OVRBone>( manoEsqueleto.Bones);

    }
   

    // Update is called once per frame
    void Update()
    {
        joystickMovimiento = OVRInput.Get(joystick, control);
        if(joystickMovimiento.magnitude != 0)
        {
            posicionJoystick = new  Vector3(joystickMovimiento.x, 0, joystickMovimiento.y);
            direccionCamara = new Vector3(0, camaraEye.rotation.eulerAngles.y, 0);
            rotacionEuler = Quaternion.Euler(direccionCamara);
            nuevaPosicion = rotacionEuler * posicionJoystick;
            camaraCentro.position += nuevaPosicion *(Time.deltaTime * velocidad);

            //posicionJoystick = 2,0,1
            //direccionCamara = 0,45,0 en Euler
            //nuevaPosicion = Euler(direccionCamara) * posicionJoytick;
            //              = (0,45,0) * (2,0,1)
            //              = 2,0,1
        }
 
        posFin =  this.transform.position  + ( Vector3.Normalize ((this.transform.right * -1) + (this.transform.up * -1)) * distanciaPuntero);
        //posFin =   ((this.transform.right * -distanciaPuntero) + (this.transform.up * -distanciaPuntero));
      
        if (debug)
        {
            Debug.DrawRay(this.transform.position, (this.transform.right* -distanciaPuntero) + (this.transform.up * -distanciaPuntero) , Color.red);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EscribirGesto();
            }
        }
        ///Actualizar posiciones del Line Renderer
        pointerRenderer.SetPosition(0, this.transform.position);
        if(!botonSeleccionado)
             pointerRenderer.SetPosition(1, posFin);

        ///
        Ray rayo = new Ray(this.transform.position, (this.transform.right * -distanciaPuntero) + (this.transform.up * -distanciaPuntero));
        RaycastHit hit;
        if (Physics.Raycast(rayo,out hit, distanciaPuntero,layerMask))
        {
            if(hit.transform.GetComponent<Button>())
            {
                if(!botonSeleccionado) botonSeleccionado = hit.transform.GetComponent<Button>();
                pointerRenderer.SetPosition(1, hit.point);

            }
            

        }
        else
        {
            botonSeleccionado = null;
            pointerRenderer.SetPosition(1, posFin);
        }
       

    }

    private void FixedUpdate()
    {
        pinchIndex = mano.GetFingerIsPinching(dedoSeleccion);
        if(pinchIndex)
        {
            if (botonSeleccionado)
            {
                botonSeleccionado.onClick.Invoke();
                Debug.Log("Boton clickeado con el dedo");
            }
        }
    }
    public void ToggleLineRenderer(bool activar)
    {
        if (!pointerRenderer)
            return;

        pointerRenderer.enabled = activar;
    }
    void EscribirGesto()
    {
        ManoGesto nuevoGesto = new ManoGesto();
        nuevoGesto.nombreID = "Nuevo gesto";
        List<Vector3> posicionesHueso = new List<Vector3>();
        foreach(var hueso in huesosMano)
        {
            posicionesHueso.Add(manoEsqueleto.transform.InverseTransformPoint(hueso.Transform.position));
        }
        nuevoGesto.posiciones = posicionesHueso;
        gestosManos.Add(nuevoGesto);
    }
    void LeerGesto()
    {

    }
}


[System.Serializable]
public class ManoGesto
{
    public string nombreID;
    public List<Vector3> posiciones;
}