using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextoNombrePersonaje : MonoBehaviour {

    public GameManager gameManager;
    public Text nombrePersonaje;
    public string textoNombrePersonaje;

	// Use this for initialization
	void Start () {

        textoNombrePersonaje = "Pepito";

        if (GameObject.Find("GameManager"))
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            if (gameManager)
            {
                textoNombrePersonaje = gameManager.nombrePersonaje;
            }
        }
	}

    void Update() {

        nombrePersonaje.text = textoNombrePersonaje;

    }
}
