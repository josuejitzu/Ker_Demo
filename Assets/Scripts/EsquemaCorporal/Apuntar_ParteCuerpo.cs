using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;

namespace EsquemaCorporal
{
    public class Apuntar_ParteCuerpo : MonoBehaviour
    {

        public GameObject panelInstrucciones;
        public TMP_Text mensajeSeñalar_text;

        public List<ParteSeñalar_Settings> partesASeñalar = new List<ParteSeñalar_Settings>();
        // Start is called before the first frame update
        public int enParte = 0;
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
        // Update is called once per frame
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
            mensajeSeñalar_text.text = partesASeñalar[enParte].mensaje;
        }

        public  void ParteSeñalada(GameObject parte)
        {
            empezarCronometro = false;

            partesASeñalar[enParte].tiempoTranscurrido = tiempoCronometro;
            partesASeñalar[enParte].señalado = true;
            if(partesASeñalar[enParte].parteCuerpo == parte)
               partesASeñalar[enParte].esCorrecto = true;

            SiguienteParte();

        }
        public void SiguienteParte()
        {
            tiempoCronometro = 0f;
            if (enParte < partesASeñalar.Count-1)
            {
                enParte += 1;
                CambiarNombreLetrero();
                empezarCronometro = true;
            }
            else
            {
                cuerpoControl.EmpezarArmado();
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
