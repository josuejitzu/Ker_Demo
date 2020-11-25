using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EsquemaCorporal

{
    public class Secuencia_ID_Cuerpo : Seccion_EsquemaCorporal
    {
        // Start is called before the first frame update
        public GameObject panelPartesCuerpo;
        public TMP_Text nombreParte;

        [Space(10)]
        public float inicioDeSecuencia = 2.0f;

       
        public Parte_secuencia[] partesCuerpo;
        int enParte=0;

        [Space(20)]
        public Apuntar_ParteCuerpo apuntarPartes;
        void Start()
        {
           
        }

        public void SiguienteParte()
        {
            if (enParte < partesCuerpo.Length)
            {
                StartCoroutine(SecuenciaOutline(partesCuerpo[enParte].tiempoApagado));
            }else
            {
                
                SeccionTerminada();
            }

        }

        public IEnumerator SecuenciaOutline(float tiempo)
        {
            //partesCuerpo[enParte].parteCuerpo_outline.ToggleOutline(true);
            partesCuerpo[enParte].parteCuerpo_outline.ResaltarParte(true);
            nombreParte.text = partesCuerpo[enParte].nombreParte;
            yield return new WaitForSeconds(tiempo);

            partesCuerpo[enParte].parteCuerpo_outline.ResaltarParte(false);
            //partesCuerpo[enParte].parteCuerpo_outline.ToggleOutline(false);
            enParte += 1;
            SiguienteParte();

        }

        public override void EmpezarSeccion()
        {
            panelPartesCuerpo.SetActive(true);
            Invoke("SiguienteParte", inicioDeSecuencia);
        }

        public override void SeccionTerminada()
        {
            Debug.Log("Seccion Identificacion partes temrinada");
            panelPartesCuerpo.SetActive(false);
            this.GetComponent<ControlSecciones>().SeccionTerminada();
            //apuntarPartes.EmpezarSeccion();
        }
    }

    [System.Serializable]
    public class Parte_secuencia
    {
        public CambioMaterial_Cuerpo parteCuerpo_outline;
        public float tiempoApagado;
        public string nombreParte;
    }
}
