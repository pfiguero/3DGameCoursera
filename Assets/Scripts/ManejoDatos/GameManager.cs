using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public string nombrePersonaje;
    public bool empezarTiempo;
    public float tiempo;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        empezarTiempo = false;
        tiempo = 0;	
	}

    void OnLevelWasLoaded() {

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            empezarTiempo = true;
        }

    }

    void Update() {

        if (empezarTiempo)
        {
            tiempo += Time.deltaTime;
        }

    }
}
