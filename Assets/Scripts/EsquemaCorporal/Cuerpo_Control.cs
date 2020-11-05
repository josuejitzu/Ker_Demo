using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EsquemaCorporal
{
    
    public class Cuerpo_Control : MonoBehaviour
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
        public List<ParteDeCuerpo> partesDelCuerpo = new List<ParteDeCuerpo>();

        private void Awake()
        {
            instancia = this;
        }

        void Start()
        {

        }
        private void Update()
        {
            if (empezarCronometro)
            {
                tiempoCronometro += Time.deltaTime;
                cronometro_text.text = $" {Mathf.Floor(tiempoCronometro / 60).ToString("00")}:{(tiempoCronometro % 60.0f).ToString("00.0")}";
            }
        }

        public void EmpezarArmado()
        {
            ///mostrar letrero con instrucciones
            panelInstrucciones.SetActive(true);
            StartCoroutine(EmpezarArmado_Secuencia());
            
        }

        IEnumerator EmpezarArmado_Secuencia()
        {
            ///Desaparecer mesh de señalizacion de partes
            
            ///Activar partes de cuerpo para armar

            yield return new WaitForSeconds(1.0f);

            //Iniciar cronometro

            //Desaparecer letrero??
        }

        public void ParteUnida(ParteCuerpoID parteID, PiezaConectadaCorrecta _esCorrecta)
        {
            foreach(ParteDeCuerpo parteCuerpo in partesDelCuerpo)
            {
                if(parteCuerpo.parteDelCuerpo == parteID  )
                {
                    parteCuerpo.conPiezaCorrecta = _esCorrecta;
                    Debug.Log("Pieza colocada");
                    break;
                }
            }
        }

    }

    [System.Serializable]
    public class ParteDeCuerpo
    {
        public ParteCuerpoID parteDelCuerpo;
        public PiezaConectadaCorrecta conPiezaCorrecta;
        public float tiempo;

    }
}