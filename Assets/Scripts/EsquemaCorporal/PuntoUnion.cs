using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JitzuTools.DebugColor;

namespace EsquemaCorporal
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class PuntoUnion : MonoBehaviour
    {
        public ParteCuerpoID organoID;
        public TipoUnion _tipoUnion;

        public Transform posicionUnion;
        public Transform meshPadre;
        [SerializeField] private EstadoPieza estadoPieza;
        //[SerializeField] private EstadoAgarre estadoAgarre;
        [SerializeField] private PuntoUnion puntoUnionMaestro;
        [SerializeField] private PuntoUnion puntoUnionEsclavo;
        [SerializeField] private PiezaConectadaCorrecta esPiezaCorrecta;
        public PiezaConectadaCorrecta EsPiezaCorrecta { get => esPiezaCorrecta; set => esPiezaCorrecta = value; }

        [SerializeField] private GameObject meshReferenciaPosicion;
        public EstadoPieza EstadoPieza
        {
            get => estadoPieza;
            set
            {
                estadoPieza = value;


            }
        }

        [SerializeField] private bool debug;


        private void OnValidate()
        {
            GetComponent<Rigidbody>().useGravity = false;
            this.transform.name = "PuntoUnion_" + _tipoUnion.ToString() + "_" + organoID.ToString();
            if (posicionUnion) posicionUnion.transform.name = "Posicion Union";

            meshReferenciaPosicion.SetActive(debug);

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawSphere(this.transform.position, 0.05f);
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);

            if (posicionUnion)
                Gizmos.DrawSphere(posicionUnion.transform.position, 0.01f);
        }

        void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PuntoUnion>())
            {
                ChecarUnionPieza(other.GetComponent<PuntoUnion>());
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<PuntoUnion>())
            {
                ChecarUnionPieza(other.GetComponent<PuntoUnion>());

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PuntoUnion>())
            {
                DebugColor.Mensaje("Punto salio area");
                EstadoPieza = EstadoPieza.Vacio;
                if (EstadoPieza != EstadoPieza.Unida)
                    puntoUnionMaestro = null;

                if(meshPadre)
                     meshPadre.transform.parent = null;
                RevizarEsPiezaCorrecta();
                //EsPiezaCorrecta = PiezaConectadaCorrecta.SinPieza;
                this.GetComponent<SphereCollider>().enabled = true;

                //Parece ya no ser necesario
                //if (EstadoPieza == EstadoPieza.Unida)
                //{
                //    UnirPieza();
                //}
            }



        }


        /// <summary>
        /// Llamado por los triggers para revizar que debemos hacer con la pieza en el zona del trigger
        /// </summary>
        /// <param name="_puntoUnionEncontrado"></param>
        void ChecarUnionPieza(PuntoUnion _puntoUnionEncontrado)
        {
            //Esto lo comente por la logica de tomar
            //if (EstadoPieza == EstadoPieza.Unida && !debug)
            //    return;

            if (_tipoUnion == TipoUnion.Esclavo)
            {
                DebugColor.Mensaje($"Punto esclavo {_puntoUnionEncontrado.organoID} entro en zona de Union");


                puntoUnionMaestro = _puntoUnionEncontrado;
                EstadoPieza = EstadoPieza.EnArea;

                UnirPieza();

            }
            else if (_tipoUnion == TipoUnion.Maestro)
            {
                puntoUnionEsclavo = _puntoUnionEncontrado;
                EstadoPieza = EstadoPieza.EnArea;

            }

        }

        /// <summary>
        /// Llamado solamente si es Tipo Esclavo y esta en Trigger con la zona de Union
        /// </summary>
        public void UnirPieza()
        {
            if (meshPadre.GetComponent<OVRGrabbable>().GrabbedBy)
            {
                return;
            }
            if (EstadoPieza == EstadoPieza.Unida && puntoUnionMaestro)
            {
                meshPadre.transform.localPosition = puntoUnionMaestro.posicionUnion.localPosition;
                meshPadre.transform.localRotation = puntoUnionMaestro.posicionUnion.localRotation;

                return;
            }
            else if (EstadoPieza == EstadoPieza.Unida || !puntoUnionMaestro)
            {

                return;

            }




            meshPadre.transform.parent = puntoUnionMaestro.transform;
            meshPadre.transform.localPosition = puntoUnionMaestro.posicionUnion.localPosition;
            meshPadre.transform.localRotation = puntoUnionMaestro.posicionUnion.localRotation;
            EstadoPieza = EstadoPieza.Unida;

            //Debug.Log($"PosicionPunto Union local:{puntoUnionMaestro.posicionUnion.localPosition} " +
            //    $"PosicionPunto Union global:{puntoUnionMaestro.posicionUnion.position} " +
            //    $"PosicionMeshPadre{meshPadre.transform.localPosition} PosicionMeshPadre global{meshPadre.transform.position}");


            puntoUnionMaestro.EstadoPieza = EstadoPieza.Unida;
            puntoUnionMaestro.RevizarEsPiezaCorrecta();
            meshPadre.GetComponent<Rigidbody>().isKinematic = true;

        }

        /// <summary>
        /// Llamado si la pieza es Maestro, normalmente llamado por el esclavo avizando de la union
        /// </summary>
        public void RevizarEsPiezaCorrecta()
        {
            if (_tipoUnion != TipoUnion.Maestro || !puntoUnionEsclavo)
            {
                return;
            }

            if (puntoUnionEsclavo.organoID == organoID)
            {
                EsPiezaCorrecta = PiezaConectadaCorrecta.Correcta;

            }
            else if (puntoUnionEsclavo.organoID != organoID)
            {
                EsPiezaCorrecta = PiezaConectadaCorrecta.Incorrecta;

            }
            else
            {
                EsPiezaCorrecta = PiezaConectadaCorrecta.SinPieza;

            }

            Cuerpo_Control.instancia.ParteUnida(organoID, EsPiezaCorrecta);

        }
    }

    public enum EstadoPieza
    {
        Vacio,
        EnArea,
        Unida
    }

    public enum EstadoAgarre
    {
        Soltada,
        Agarrada
    }

    public enum TipoUnion
    {
        Maestro,
        Esclavo
    }

    public enum PiezaConectadaCorrecta
    {
        SinPieza,
        Correcta,
        Incorrecta

    }
}