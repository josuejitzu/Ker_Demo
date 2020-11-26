﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OutlineObjetos : MonoBehaviour
{
    [Tooltip("Deben ser objetos con mesh que tengan el script de Outline.cs")]
    public Outline[] objetosOutline;

    [SerializeField] private OVRGrabber manoGrabber;

   
    /// <summary>
    /// Activa el Outline por si necesitamos que se ilumine el objeto
    /// </summary>
    /// <param name="activar"></param>
    public void ToggleOutline(bool activar)
    {
        foreach(Outline objOutline in objetosOutline)
        {
            objOutline.enabled = activar;
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<OVRGrabber>())
        {
            ToggleOutline(true);
            

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<OVRGrabber>())
        {
            ToggleOutline(false);

        }
    }
}

