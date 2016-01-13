using UnityEngine;
using System.Collections;

public class DieByContact : MonoBehaviour {

	public delegate void DyingAction();
	public static event DyingAction OnDying;

	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name.Contains ("Die")) {
			Destroy (gameObject);
			if (OnDying != null)
				OnDying ();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
