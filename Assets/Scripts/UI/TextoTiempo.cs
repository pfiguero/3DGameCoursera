using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextoTiempo : MonoBehaviour {

    public GameManager gameManager;
    public Text tiempo;

    // Use this for initialization
    void Start()
    {

        if (GameObject.Find("GameManager"))
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    void Update()
    {
        if (gameManager)
        {
            tiempo.text = Mathf.FloorToInt(gameManager.tiempo).ToString();
        }
        else 
        {
            tiempo.text = "Sin Comenzar";
        }
    }
}
