using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EasyButtons;

namespace EsquemaCorporal
{
    public class Apuntador_ParteDelCuerpo : MonoBehaviour
    {
        public OVRInput.Controller control;
        public LineRenderer lineRenderer;
        public bool puedeApuntar;
        public LayerMask layerPartesCuerpo;

        public delegate void ParteSeñaladaEvent(GameObject parte);
        public static event ParteSeñaladaEvent ParteSeñalada;
        // Start is called before the first frame update
        public GameObject parteSeñalando;
        public float distancia = 2.0f;

        void Start()
        {
            if(lineRenderer)
            {
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, this.transform.position + new Vector3(0, 0, 1));


            }

        }

        // Update is called once per frame
        void Update()
        {
            if(puedeApuntar)
            {
                if(Physics.Raycast(this.transform.position,this.transform.forward,out var hit,100.0f,layerPartesCuerpo))
                {
                    
                    if (hit.transform.CompareTag("parteCuerpo") && !parteSeñalando)
                    {
                        parteSeñalando = hit.transform.gameObject;
                        parteSeñalando.GetComponent<CambioMaterial_Cuerpo>().ResaltarParte(true);
                    }
                    //ParteSeñalada?.Invoke(hit.transform.gameObject);
                    //puedeApuntar = false;


                }
                else
                {
                    if(parteSeñalando)
                    {
                        parteSeñalando.GetComponent<CambioMaterial_Cuerpo>().ResaltarParte(false);
                        parteSeñalando = null;
                    }
                }


                if (lineRenderer)
                {

                    lineRenderer.SetPosition(0, this.transform.position);
                    lineRenderer.SetPosition(1, this.transform.position + (this.transform.forward * distancia));

                }

                Debug.DrawRay(this.transform.position, this.transform.forward * 3);

            }
            else
            {
                if (parteSeñalando)
                {
                    parteSeñalando.GetComponent<CambioMaterial_Cuerpo>().ResaltarParte(false);
                    parteSeñalando = null;
                }
            }

           
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger,control))
            {
                EnviarParteSeñalada();
            }
        }

        [Button]
        [ContextMenu("Enviar Parte Señalada")]
        public void EnviarParteSeñalada()
        {
            if (!parteSeñalando)
                return;
            
            ParteSeñalada?.Invoke(parteSeñalando);

        }

        public void ToggleApuntador(bool apuntador)
        {
            puedeApuntar = apuntador;
            lineRenderer.enabled = apuntador;
            
        }
    }
}
