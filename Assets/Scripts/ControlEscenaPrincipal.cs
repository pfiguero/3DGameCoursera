using UnityEngine;
using System.Collections;

public class ControlEscenaPrincipal : MonoBehaviour {

	private GameManager gameManager;
	private AudioSource sonidoCasaPfiguero1;

	// Use this for initialization
	void Start () {
		GameObject g = GameObject.Find ("GameManager");
		if (g != null)
			gameManager = g.GetComponent<GameManager> ();
		g = GameObject.Find ("casaPfiguero1");
		if (g != null) {
			sonidoCasaPfiguero1 = g.GetComponent<AudioSource> (); // first audio source.
			if( sonidoCasaPfiguero1 != null )
				sonidoCasaPfiguero1.time = gameManager.offsetSong;

		}
	
	}
	
	void OnEnable()
	{
		CambioEscena.OnCerrarEscena += CerrarEscena;
	}


	void OnDisable()
	{
		CambioEscena.OnCerrarEscena -= CerrarEscena;
	}

	// Update is called once per frame
	void Update () {
	
	}

	void CerrarEscena () {
		gameManager.offsetSong = sonidoCasaPfiguero1.time;
	}
}
