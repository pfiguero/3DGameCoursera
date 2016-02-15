using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BotonEmpezar : MonoBehaviour {

    private GameManager gameManager;
    public InputField campoNombrePersonaje;

	// Use this for initialization
	void Start () {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

	}

    public void clickEmpezar() {

        gameManager.nombrePersonaje = campoNombrePersonaje.text;
        SceneManager.LoadScene(1);

    }
}
