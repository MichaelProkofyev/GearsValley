using UnityEngine;
using System.Collections;

public class MoveBullet : MonoBehaviour {


	public float moveSpeed;

	// Use this for initialization
	void Start () {
		GetComponent<TrailRenderer> ().sortingLayerName = "Foreground";
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * Time.deltaTime * moveSpeed);
		Destroy (gameObject, 0.5f);
	}
}
