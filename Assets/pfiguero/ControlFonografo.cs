using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class ControlFonografo : MonoBehaviour {

	public AudioMixer audioMixer;
	public float timeoutBola = 8f;
	public float deltaPitch = .05f;
	private float minPitch = .2f;
	private float curPitch = 1f;

	// Use this for initialization
	void Start () {
		Invoke ("DecreasePitch", timeoutBola);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BolaDestruida() {
		CancelInvoke ();
		if (audioMixer != null) {
			curPitch = 1f;
			audioMixer.SetFloat ("pitch", curPitch);
		}
		Invoke ("DecreasePitch", timeoutBola);
	}

	void DecreasePitch() {
		Debug.Log ("Bajando pitch: " + curPitch);
		if (curPitch >= minPitch)
			curPitch -= deltaPitch;
		if (audioMixer != null) {
			audioMixer.SetFloat ("pitch", curPitch);
		}
		Invoke ("DecreasePitch", timeoutBola);
	}

	void OnEnable()
	{
		BallController.OnBolaDestruida += BolaDestruida;
	}


	void OnDisable()
	{
		BallController.OnBolaDestruida -= BolaDestruida;
	}
}
