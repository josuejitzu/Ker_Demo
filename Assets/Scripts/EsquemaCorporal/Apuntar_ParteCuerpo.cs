using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;

namespace EsquemaCorporal
{
    public class Apuntar_ParteCuerpo : Seccion_EsquemaCorporal
    {

        public GameObject panelInstrucciones;
        public TMP_Text mensajeSeñalar_text;
        public GameObject puppetPartes;

        [Space(10)]
        public List<ParteSeñalar_Settings> partesASeñalar = new List<ParteSeñalar_Settings>();
        // Start is called before the first frame update
        public int enParte = 0;
        [SerializeField] private int señalarParteNum;
        [Space(10)]
        [Header("Controles")]
        public Apuntador_ParteDelCuerpo apuntadorL;
        public Apuntador_ParteDelCuerpo apuntadorR;

        [Space(20)]
        public TMP_Text cronometro_text;
        public float tiempoCronometro = 0f;
        public bool empezarCronometro;

        [Space(20)]
        public Cuerpo_Control cuerpoControl;


        [SerializeField] private List<int> partesBuscadas = new List<int>();

        private void OnEnable()
        {
            Apuntador_ParteDelCuerpo.ParteSeñalada += ParteSeñalada;
        }
        private void OnDisable()
        {
            Apuntador_ParteDelCuerpo.ParteSeñalada -= ParteSeñalada;

        }
        private void Start()
        {
            panelInstrucciones.SetActive(false);
            Debug.Log(partesASeñalar.Count);
        }
        

        void Update()
        {
            if(empezarCronometro)
            {
                tiempoCronometro += Time.deltaTime;
                cronometro_text.text =$" {Mathf.Floor( tiempoCronometro/60).ToString("00")}:{(tiempoCronometro % 60.0f).ToString("00.0")}";
            }
        }

        public void CambiarNombreLetrero()
        {
            mensajeSeñalar_text.text = partesASeñalar[señalarParteNum].mensaje;
        }


        /// <summary>
        /// Llamada por los controles con Apuntador_ParteDelCuerpo, pero por evento
        /// </summary>
        /// <param name="parte">gameobject que utilizamos para encontrar a donde procede</param>
        public  void ParteSeñalada(GameObject parte)
        {
            empezarCronometro = false;

            partesASeñalar[señalarParteNum].tiempoTranscurrido = tiempoCronometro;
            partesASeñalar[señalarParteNum].señalado = true;
            if(partesASeñalar[señalarParteNum].parteCuerpo == parte)
               partesASeñalar[señalarParteNum].esCorrecto = true;

            partesBuscadas.Add(señalarParteNum);

            SiguienteParte();

        }
        public void SiguienteParte()
        {
            tiempoCronometro = 0f;
            if (enParte < partesASeñalar.Count-1)
            {
                enParte += 1;
                señalarParteNum = RandomParteASeñalar();
                CambiarNombreLetrero();
                empezarCronometro = true;
            }
            else
            {
                SeccionTerminada();   
            }
        }

        public void ComenzarSeñalamiento()
        {
            StartCoroutine(ComenzarSeñalamiento_secuencia());
        }

        IEnumerator ComenzarSeñalamiento_secuencia()
        {
            panelInstrucciones.SetActive(true);

            yield return new WaitForSeconds(3.0f);

            apuntadorL.ToggleApuntador(true);
            apuntadorR.ToggleApuntador(true);
            CambiarNombreLetrero();
            empezarCronometro = true;


        }

        public int RandomParteASeñalar()
        {
            
            int r = UnityEngine.Random.Range(0,partesASeñalar.Count);

            while(partesBuscadas.Contains(r))
            {
                r =  UnityEngine.Random.Range(0, partesASeñalar.Count);
            }
           // partesBuscadas.Add(r);
            return r;
        }

        public override void  EmpezarSeccion()
        {
            ComenzarSeñalamiento();
        }

        public override void  SeccionTerminada()
        {
            panelInstrucciones.SetActive(false);
            puppetPartes.SetActive(false);
            apuntadorL.ToggleApuntador(false);
            apuntadorR.ToggleApuntador(false);
           // cuerpoControl.EmpezarSeccion();

            this.GetComponent<ControlSecciones>().SeccionTerminada();
        }
    }

    [System.Serializable]
    public class ParteSeñalar_Settings
    {
        public string mensaje;
        public float tiempoTranscurrido;
        public bool señalado;
        public bool esCorrecto;
        public GameObject parteCuerpo;
    }
}
