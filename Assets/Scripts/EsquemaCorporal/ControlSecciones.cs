using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSecciones : MonoBehaviour
{
    [SerializeField]
    public List<Seccion_EsquemaCorporal> secciones = new List<Seccion_EsquemaCorporal>();
    
    [SerializeField] private int enSeccion;
    public GameObject panelInstrucciones;
    public GameObject panelInstrucciones_Final;
    private void OnValidate()
    {
        // secciones = G<GameObject>();
       
        //for (int i = 0; i < interfazEjemplo.Length; i++)
        //{
        //    secciones.Add(interfazEjemplo[i].);
        //}
        Debug.Log(secciones.Count);
    }


    public void Start()
    {
    }

    /// <summary>
    /// Llamado por UI en boton empezar del usuario
    /// </summary>
    public void IniciarActividades()
    {
        panelInstrucciones.SetActive(false);
        secciones[enSeccion].EmpezarSeccion();

    }
    public void SeccionTerminada()
    {
        Debug.Log($"Seccion:{secciones[enSeccion]} terminada, siguiente...");
        
        if(enSeccion < secciones.Count-1)
        {
            enSeccion++;

        }else
        {
            FinActividad();
            return;
        }

        secciones[enSeccion].EmpezarSeccion();
        Debug.Log($"Siguiente seccion: {secciones[enSeccion]} terminada");


    }
    public void FinActividad()
    {
        panelInstrucciones_Final.SetActive(true);
    }
}
