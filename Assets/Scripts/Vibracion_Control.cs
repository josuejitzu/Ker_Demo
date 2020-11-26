using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibracion_Control : MonoBehaviour
{

    public OVRInput.Controller control;
    public float frecuencia = 0.1f;
    public float amplitud = 0.1f;
    public string tag;
    private void OnTriggerEnter(Collider other)
    {
        
        if (!string.IsNullOrEmpty(tag) && other.CompareTag(tag))
        {
            OVRInput.SetControllerVibration(frecuencia, amplitud, control);

        }


    }
    private void OnTriggerStay(Collider other)
    {
        if (!string.IsNullOrEmpty(tag) && other.CompareTag(tag))
        {
            OVRInput.SetControllerVibration(frecuencia, amplitud, control);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(tag) && other.CompareTag(tag))
        {
            OVRInput.SetControllerVibration(0f, 0f, control);
        }

    }
}
