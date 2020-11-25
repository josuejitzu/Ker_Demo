using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;
using TMPro;
using UnityEngine.XR;

public class MasterLevel : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject usuario;
    public OVRInput.Controller control = OVRInput.Controller.LTouch;
    public OVRInput.Button botonMenu = OVRInput.Button.Start;

    [Header("Mano")]
    public OVRHand mano;
    public bool pinchIndex;
    public Mano_Control _manoControl_L;
    public Mano_Control _manoControl_R;

    [Header("Escenas")]
    public Escenas_DB escenasDB;
    public List<Escena> escenas = new List<Escena>();

    [Space(10)]
    public OVRScreenFade fade;
    public bool menuEnInicio;

    [Space(10)]
    [Header("XR Settings")]
    public float eyeResolution = 1.5f;
    public bool fixedFoveatedRendering = true;
    public OVRManager.FixedFoveatedRenderingLevel nivelFFR = OVRManager.FixedFoveatedRenderingLevel.High;
    public Transform objetoDebug;

    private int intervalo = 7;
    
    private void OnValidate()
    {
        if (escenas.Count == escenasDB.escenas.Length)
        {
            for (int i = 0; i < escenasDB.escenas.Length; i++)
            {
                escenas[i].nombreID = escenasDB.escenas[i].nombreID;
                escenas[i].escena = escenasDB.escenas[i].escena;
                if (escenas[i].boton)
                {
                    if (escenasDB.escenas[i].thumbnail)
                    {
                        escenas[i].boton.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = escenasDB.escenas[i].titulo;
                        escenas[i].thumbnail = escenasDB.escenas[i].thumbnail;
                        escenas[i].boton.transform.GetChild(0).GetComponent<Image>().sprite = escenas[i].thumbnail;
                    }
                    //Desactivar el boton de la escena actual
                    if (SceneManager.GetActiveScene().name == escenasDB.escenas[i].EscenaNombre || escenasDB.escenas[i].escena == null)
                    {
                        
                        escenas[i].boton.interactable = false;
                        
                    }
                    else if (escenas[i].boton)
                    {
                        escenas[i].boton.interactable = true;
                    }

                }

                
            }
        }
    }

    private void Start()
    {
        //if(!menuEnInicio)
        //     ToggleMenu();

        XRSettings.eyeTextureResolutionScale = eyeResolution;

        //OVRPlugin.fixedFoveatedRenderingLevel = OVRPlugin.FixedFoveatedRenderingLevel.HighTop;
        if (fixedFoveatedRendering)
        {
            OVRManager.fixedFoveatedRenderingLevel = nivelFFR; // it's the maximum foveation level
            OVRManager.useDynamicFixedFoveatedRendering = true;
        }
        


    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            CambiarEscena(escenasDB.escenas[0].EscenaNombre);
        }

        if(OVRInput.GetDown(botonMenu,control))
        {
            ToggleMenu();
        }

        //OVRHand.TrackingConfidence estabilidadMano = mano.GetFingerConfidence(OVRHand.HandFinger.Middle);


        //Transform posPuntero = mano.PointerPose;
        //Debug.DrawRay(posPuntero.localPosition, posPuntero.forward, Color.red);
        //objetoDebug.transform.position = posPuntero.position;
    }

    private void FixedUpdate()
    {
        pinchIndex = mano.GetFingerIsPinching(OVRHand.HandFinger.Middle);
        if (Time.frameCount % intervalo == 0)
        {

            if (pinchIndex)
            {
                ToggleMenu();
            }
        }
    }
    public void CambiarEscena(Button _boton)
    {
        foreach(Escena _escena in escenas)
        {
            if (_boton == _escena.boton) 
            {
                CambiarEscena(_escena.escena);
                break;
            }
        }
    }
    
    void CambiarEscena(string nombre)
    {
        Debug.Log($"Cambiando a escena: {nombre}");
        StartCoroutine(CambiarEscena_Rutina(nombre));
    }
    IEnumerator CambiarEscena_Rutina(string nombreEscena)
    {
        fade.FadeOut();
        
        yield return new WaitForSeconds(fade.fadeTime);
        SceneManager.LoadScene(nombreEscena);
    }

    [ContextMenu("ToggleMenu")]
    public void ToggleMenu()
    {
        if (panelMenu)
        {
            panelMenu.SetActive(!panelMenu.activeInHierarchy);

            panelMenu.transform.position = new Vector3(usuario.transform.position.x, panelMenu.transform.position.y, usuario.transform.position.z);

            Quaternion rotacion = usuario.transform.rotation;
            rotacion.x = 0;
            rotacion.z = 0;
            
            panelMenu.transform.rotation = rotacion;

            if (_manoControl_L && _manoControl_R)
            {
                _manoControl_R.ToggleLineRenderer(panelMenu.activeInHierarchy);
                _manoControl_L.ToggleLineRenderer(panelMenu.activeInHierarchy);
            }
        }
        
    }

}

