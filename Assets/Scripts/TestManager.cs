using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestManager : MonoBehaviour{
    public GameObject panelInicial;
    public GameObject panelTest;
    public GameObject panelFinal;

    void Start() {
        ActivarPanelInicial();
    }


    /*----------------------------------------------------
    ----------------FUNCIONES PARA BOTONES----------------
    ------------------------------------------------------*/
    public void ActivarPanelInicial() {
        panelInicial.SetActive(true);
        panelTest.SetActive(false);
        panelFinal.SetActive(false);
    }

    public void ActivarPanelTest() {
        panelTest.SetActive(true);
        panelInicial.SetActive(false);
        panelFinal.SetActive(false);
    }

    public void ActivarpanelFinal() {
        panelFinal.SetActive(true);
        panelTest.SetActive(false);
        panelInicial.SetActive(false);
    }
}
