using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EsquemaCorporal
{
    
    public class Cuerpo_Control : Seccion_EsquemaCorporal
    {
        public static Cuerpo_Control instancia;

        [Header("Tiempo")]
        [SerializeField] private float tiempoTotalCompletacion;
        [SerializeField] private float tiempoPieza;
        [SerializeField] private float tiempoCronometro;
        [SerializeField] private bool empezarCronometro;
        [SerializeField] private TMP_Text cronometro_text;
        [SerializeField] private GameObject panelInstrucciones;

        [Space(10)]
        public GameObject meshOutlineCuerpo;
        public GameObject parteParaArmar;
        
        [SerializeField] private bool inicioArmado;
        [SerializeField] private bool terminoArmado;

        [Space(10)]
        public List<ParteDeCuerpo> partesDelCuerpo = new List<ParteDeCuerpo>();


        private void Awake()
        {
            instancia = this;
#if UNITY_EDITOR ///PARA mejorar rendimiento porque Debug.Log usa mucho proceso y lo ejecuta aun en las builds de cualquier plataforma
            Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif

        }

        void Start()
        {
            panelInstrucciones.SetActive(false);
            parteParaArmar.SetActive(false);
        }
        private void Update()
        {
           
        }

        public void EmpezarArmado()
        {
            ///mostrar letrero con instrucciones
           
            StartCoroutine(EmpezarArmado_Secuencia());
            
        }

        IEnumerator EmpezarArmado_Secuencia()
        {
            ///Desaparecer mesh de señalizacion de partes

            ///Activar partes de cuerpo para armar
            panelInstrucciones.SetActive(true);
            meshOutlineCuerpo.SetActive(true);
            parteParaArmar.SetActive(true);
            yield return new WaitForSeconds(3.0f);

            empezarCronometro = true;
            inicioArmado = true;
            //Iniciar cronometro

            //Desaparecer letrero??
        }

        /// <summary>
        /// Es llamado externamente por alguna parte del cuerpo con PuntoUnion.cs para saber si es   colocada o no
        /// 
        /// </summary>
        /// <param name="parteID"></param>
        /// <param name="_esCorrecta"></param>
        public void ParteUnida(ParteCuerpoID parteID, PiezaConectadaCorrecta _esCorrecta)
        {
            if (terminoArmado)
                return;

            foreach(ParteDeCuerpo parteCuerpo in partesDelCuerpo)
            {
                if(parteCuerpo.parteDelCuerpo == parteID  )
                {
                    parteCuerpo.conPiezaCorrecta = _esCorrecta;
                    parteCuerpo.tiempo = tiempoCronometro;

                    Debug.Log($"Pieza {parteID } colocada, es {_esCorrecta}");
                    RevizarEstadoPiezas();
                    break;
                }
            }
        }

        void RevizarEstadoPiezas()
        {
            if (!inicioArmado)
                return;

            int buenas = 0;

            foreach(ParteDeCuerpo parteCuerpo in partesDelCuerpo)
            {
                if(parteCuerpo.conPiezaCorrecta == PiezaConectadaCorrecta.Correcta)
                {
                    buenas++;
                }
                else
                {
                    if (buenas > 0)
                        buenas--;
                }

            }

            if(buenas == partesDelCuerpo.Count)
            {
                terminoArmado = true;
                Debug.Log("Todas las partes del cuerpo colocadas correctamentes");
                SeccionTerminada();
            }
        }

        public override void EmpezarSeccion()
        {
            EmpezarArmado();
        }

        //public void SeccionTerminada()
        //{
        //    this.GetComponent<ControlSecciones>().SeccionTerminada();
        //}
    }

    [System.Serializable]
    public class ParteDeCuerpo
    {
        public ParteCuerpoID parteDelCuerpo;
        public PiezaConectadaCorrecta conPiezaCorrecta;
        public float tiempo;

    }
}