using UnityEngine;
using System.Collections;

public class ControlEscenaPfiguero : MonoBehaviour {

	private GameObject bed;
	private GameObject phonographLocker;
	private AudioSource sonidoCasaPfiguero1;

	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = new GameManager();
		bed = GameObject.Find ("bed");
		phonographLocker = GameObject.Find ("phonographLocker");

		GameObject g = GameObject.Find ("phonograph");
		if (g != null)
			sonidoCasaPfiguero1 = g.GetComponent<AudioSource> ();	

		g = GameObject.Find ("GameManager");
		if (g != null)
			gameManager = g.GetComponent<GameManager> ();

		if (gameManager.yaHuboEarthquake) {
			bed.transform.position = new Vector3 (1533, 137.53f, 1375.79f);
			bed.transform.Rotate (new Vector3 (0f, 270, 0f));
			phonographLocker.transform.position = new Vector3 (1529.8f, 137.63f, 1375.44f);
			phonographLocker.transform.Rotate (new Vector3 (0, -90, 0));
			sonidoCasaPfiguero1.pitch = gameManager.pitch;
		} /*else {
			bed.transform.position = new Vector3 (1533, 137.63f, 1375.23f);
			bed.transform.Rotate (new Vector3 (0, -90, 0));
			phonographLocker.transform.position = new Vector3 (1529.8f, 137.63f, 1374.82f);
			phonographLocker.transform.Rotate (new Vector3 (0, -90, 0));
		}*/
		
	}
	
	void OnEnable()
	{
		CambioEscena.OnCerrarEscena += CerrarEscena;
	}


	void OnDisable()
	{
		if (gameManager != null && sonidoCasaPfiguero1 != null) {
			gameManager.offsetSong = sonidoCasaPfiguero1.time;
		}
		CambioEscena.OnCerrarEscena -= CerrarEscena;
	}

	// Update is called once per frame
	void Update () {

	}

	void CerrarEscena () {
		//gameManager.offsetSong = sonidoCasaPfiguero1.time;
	}
}
