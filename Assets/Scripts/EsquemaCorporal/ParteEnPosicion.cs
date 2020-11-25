using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EsquemaCorporal
{
    public class ParteEnPosicion : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        /// <summary>
        /// Llamado por el timeline de Animacion, cambia la variable ParteCuerpo_Activa en Coreografia_Cuerpo
        /// para que pueda compararse despues
        /// </summary>
        /// <param name="parte"></param>
        public void ParteEnPosicion_Llamada(ParteCuerpoID parte)
        {
            Debug.Log($"Parte: {parte} en posicion, esperando...");
            Coreografia_Cuerpo.instancia.SetParteCuerpo_Activa(parte);
            //Activar trigger para el jugador
        }

        /// <summary>
        /// Llamado por el timeline de Animacion
        /// </summary>
        /// <param name="parte"></param>
        public void ParteSalioDePosicion(ParteCuerpoID parte)
        {
            Debug.Log($"Parte: {parte} salio de posicion, esperando...");

        }
    }
}

