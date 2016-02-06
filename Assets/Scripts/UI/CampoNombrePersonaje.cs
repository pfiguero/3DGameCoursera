using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CampoNombrePersonaje : MonoBehaviour {

    public Button botonEmpezar;
    public InputField campoNombrePersonaje;

    public void cambioTexto() {

        if (!campoNombrePersonaje.text.Trim().Equals(""))
        {
            botonEmpezar.interactable = true;
        }
        else
        {
            botonEmpezar.interactable = false;
        }

    }
}
