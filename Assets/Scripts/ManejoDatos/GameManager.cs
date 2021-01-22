using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public string nombrePersonaje;
	public bool empezarTiempo;
	public float tiempo;

	// general
	public bool yaHuboEarthquake = false;
	public float tiempoEarthquake = 180f;

	// para pfiguero-Escenario
	public float pitch = 1.0f;
	public float pitchDuringEarthquake = 0.5f;
	public float offsetSong = 0f;

	// Use this for initialization
	void Start()
	{
		DontDestroyOnLoad(this);
		nombrePersonaje = "Sin nombre";
		empezarTiempo = false;
		tiempo = tiempoEarthquake;
	}

	void OnEnable()
	{
		TerrainController.OnEarthquake += EarthquakeEvent;
	}


	void OnDisable()
	{
		TerrainController.OnEarthquake -= EarthquakeEvent;
	}


	void OnLevelWasLoaded()
	{
		/*
				if (SceneManager.GetActiveScene().buildIndex == 1)
				{
					empezarTiempo = true;
				}
		*/
	}

	void Update()
	{

		if (empezarTiempo)
		{
			tiempo -= Time.deltaTime;
			if (tiempo <= 0f)
			{
				tiempo = tiempoEarthquake;
			}
		}

	}

	public void EarthquakeEvent()
	{
		if (!yaHuboEarthquake)
		{
			yaHuboEarthquake = true;
			empezarTiempo = true;
			pitch = pitchDuringEarthquake;
		}

	}
}
