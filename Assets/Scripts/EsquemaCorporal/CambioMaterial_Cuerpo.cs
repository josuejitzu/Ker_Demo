using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CambioMaterial_Cuerpo : MonoBehaviour
{
    public Material materialResaltar;
    public MeshRenderer[] meshesParteCuerpo;
    public List<Material> materialesDeCuerpos = new List<Material>();


    private void OnValidate()
    {
        materialesDeCuerpos.Clear();
        //meshesParteCuerpo = GetComponentsInChildren<MeshRenderer>();

        //foreach (MeshRenderer mesh in meshesParteCuerpo)
        //{
        //    materialesDeCuerpos.Add(mesh.sharedMaterial);

           
        //}
    }

    /// <summary>
    /// Toma todos los materiales que tenga la List materialesDeCuerpos y encinede o apaga
    /// el emission channel para mostrar, es llamado por Secuencia_ID_Cuerpos en una courutina
    /// </summary>
    /// <param name="resaltar"></param>
    public void ResaltarParte(bool resaltar)
    {
        

        if (resaltar)
        {
            
            for (int i = 0; i < meshesParteCuerpo.Length; i++)
            {
               
                meshesParteCuerpo[i].material.SetFloat("_EmissionValor", 1.0f);

                
                
            }

        }else
        {
            for (int i = 0; i < meshesParteCuerpo.Length; i++)
            {
                //meshesParteCuerpo[i].material = materialesDeCuerpos[i];
                 meshesParteCuerpo[i].material.SetFloat("_EmissionValor", 0.0f);

            }
        }


    }

}
