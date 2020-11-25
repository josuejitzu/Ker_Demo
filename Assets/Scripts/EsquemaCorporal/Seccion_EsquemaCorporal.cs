using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Seccion_EsquemaCorporal : MonoBehaviour
{
   /// <summary>
   /// Empieza las instrucciones de la seccion correspondiente
   /// </summary>
    public virtual void EmpezarSeccion()
    {

    }

    /// <summary>
    /// Triggereado por la seccion cuando el usuario termino su parte
    /// </summary>
    public virtual void SeccionTerminada()
    {

        this.GetComponent<ControlSecciones>().SeccionTerminada();

    }
}
