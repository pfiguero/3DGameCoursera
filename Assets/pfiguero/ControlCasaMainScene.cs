using UnityEngine;
using System.Collections;

public class ControlCasaMainScene : MonoBehaviour {

	private AudioSource source;
	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		GameObject g = GameObject.Find ("GameManager");
		if (g != null)
			gameManager = g.GetComponent<GameManager> ();
	}

	void OnEnable()
	{
		TerrainController.OnEarthquake += EarthquakeEvent;
	}


	void OnDisable()
	{
		TerrainController.OnEarthquake -= EarthquakeEvent;
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	void EarthquakeEvent() {
		source.pitch = gameManager.pitch;
	}
}
