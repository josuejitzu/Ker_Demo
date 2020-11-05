using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEngine.UI;
[CreateAssetMenu(fileName ="nueva Data base escenas",menuName ="Crear BD Escenas")]
public class Escenas_DB : ScriptableObject
{
    public Escena[] escenas;
}

[System.Serializable]
public class Escena 
{
    public string nombreID;
    public string escena;
    public string EscenaNombre { get { return escena; } set { } }
    public Button boton;
    public string titulo;
    public string descripcion;
    public Sprite thumbnail;
}