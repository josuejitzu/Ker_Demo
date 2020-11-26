using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EasyButtons;

namespace EsquemaCorporal
{
    public class Coreografia_Cuerpo : Seccion_EsquemaCorporal
    {
        public static Coreografia_Cuerpo instancia;
        public Animator puppet_Anim;
        public GameObject boton_empezar;
        public GameObject texto1,texto2;

        [Space(10)]
        [SerializeField] private ParteCuerpoID parteCuerpo_Activa;
        public bool parteIdentificada;
        [SerializeField]private int enParte;
        public List<ParteCuerpo_DeCoreografia> partesDeCuerpo = new List<ParteCuerpo_DeCoreografia>();
        [SerializeField] private string instrucciones;

        [Header("Tiempo")]
        [SerializeField] private float tiempoTotalCompletacion;
        [SerializeField] private float tiempoPieza;
        [SerializeField] private float tiempoCronometro;
        [SerializeField] private bool empezarCronometro;
        [SerializeField] private TMP_Text cronometro_text;
        [SerializeField] private GameObject panelInstrucciones;

        public ParteCuerpoID ParteCuerpo_Activa
        {
            get => parteCuerpo_Activa;
            set
            {
                parteCuerpo_Activa = value;
                ParteCuerpo_DeCoreografia parte = new ParteCuerpo_DeCoreografia();
                parte.parteCuerpo = parteCuerpo_Activa;
            }
        }


        void Start()
        {
            instancia = this;

            puppet_Anim.gameObject.SetActive(false);
            foreach(ParteCuerpo_DeCoreografia p in partesDeCuerpo)
            {
                if(p.zonaTrigger)
                    p.parteCuerpo = p.zonaTrigger.parteCuerpo;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (empezarCronometro)
            {
                tiempoCronometro += Time.deltaTime;
                cronometro_text.text = $" {Mathf.Floor(tiempoCronometro / 60).ToString("00")}:{(tiempoCronometro % 60.0f).ToString("00.0")}";
            }
        }

        /// <summary>
        /// Empieza la seccion correspondiente
        /// </summary>
        [Button]
        public override void EmpezarSeccion()
        {
            panelInstrucciones.SetActive(true);
            puppet_Anim.gameObject.SetActive(true);
        }

        /// <summary>
        /// Llamado por BotonUI
        /// </summary>
        public void UsuarioEnPosicion()
        {
            StartCoroutine(EmpezarSeccion_Secuencia());

        }
        IEnumerator EmpezarSeccion_Secuencia()
        {
            
            texto1.SetActive(false);
            boton_empezar.SetActive(false);
            texto2.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            //panelInstrucciones.SetActive(false);
            yield return new WaitForSeconds(1.0f);
            empezarCronometro = true;
            puppet_Anim.SetBool("secuencia1", true);
        }


        /// <summary>
        /// Llamado por alguna Zona_IngresoParte.cs
        /// </summary>
        /// <param name="parteCuerpo"></param>
        public void VerificarParteCuerpo(ParteCuerpoID parteCuerpo)
        {
            if (parteIdentificada)
                return;

            if (partesDeCuerpo[enParte].parteCuerpo == parteCuerpo)
            {
                partesDeCuerpo[enParte].correcta = true;
            }

            partesDeCuerpo[enParte].zonaTrigger.gameObject.SetActive(false);
            partesDeCuerpo[enParte].ingresado = true;

            empezarCronometro = false;
            partesDeCuerpo[enParte].tiempoCompletacion = tiempoCronometro;
            parteIdentificada = true;//para que no pueda triggerearse esto multiple veces antes de cambiar de mano

            //enParte = enParte < partesDeCuerpo.Count ? enParte++ : enParte;
            if(enParte < partesDeCuerpo.Count-1)
            {
                enParte++;
                Debug.Log("Esperar siguiente parte:" + enParte);
            }else
            {
                Debug.Log("Coreografia completada");
                SeccionTerminada();
            }
           
        }

        /// <summary>
        /// Se Coloca la parte que va a compararse, tambien se resetea e inicia el cronometro
        /// </summary>
        /// <param name="_parte">parte del cuerpo que queremos guardar</param>
        public void SetParteCuerpo_Activa(ParteCuerpoID _parte)
        {
            //Por el momento esto no es necesario
            foreach (ParteCuerpo_DeCoreografia parte in partesDeCuerpo)
            {
                if (parte.parteCuerpo == _parte)
                {
                    //parteCuerpo_Activa = parte.parteCuerpo;
                    parte.zonaTrigger.gameObject.SetActive(true);
                }
            }

            //Se supone que cuando el muñeco llega a la posicion es cuando contamos el tiempo
            //resetear cronometro por si esta ya iniciado
            tiempoCronometro = 0f;
            empezarCronometro = true;
            parteIdentificada = false;
           // enParte++;
        }

        /// <summary>
        /// Esto es llamado por ParteEnPosicion.cs cuando la parte termino su animacion
        /// Debe activarse cuando el usuario no logro imitar el movimiento del muñeco
        /// </summary>
        /// <param name="_parte">parte del cuerpo ParteCuerpoID</param>
        public void SetParteCuerpo_Inactiva(ParteCuerpoID _parte)
        {
            //buscamos el slot correspondiente a la parte
            foreach(ParteCuerpo_DeCoreografia parte in partesDeCuerpo)
            {
                if(parte.parteCuerpo == _parte)
                {
                    if (parte.ingresado)
                        break;

                    empezarCronometro = false;
                    parte.tiempoCompletacion = tiempoCronometro;
                    parte.correcta = false;
                    parte.zonaTrigger.gameObject.SetActive(false);
                    parte.ingresado = false;

                    parteIdentificada = true;//para que VerificarParteCuerpo no pueda ser activada

                    break;
                }
            }
        }



        public override void SeccionTerminada()
        {
            puppet_Anim.SetBool("secuencia1", false);
            puppet_Anim.gameObject.SetActive(false);
            base.SeccionTerminada();

        }
    }

    [System.Serializable]
    public class ParteCuerpo_DeCoreografia
    {
        public ParteCuerpoID parteCuerpo;
        public float tiempoCompletacion;
        public bool correcta;
        public Zona_IngresoParte zonaTrigger;
        public bool ingresado;
    }
}
