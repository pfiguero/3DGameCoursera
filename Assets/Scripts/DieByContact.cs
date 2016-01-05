using UnityEngine;
using System.Collections;

public class DieByContact : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name.Contains ("Die")) {
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
