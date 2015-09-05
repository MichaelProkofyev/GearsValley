using UnityEngine;
using System.Collections;

public class ShootingTrigger : MonoBehaviour {


	public Transform firePoint;
	private HeroMovement player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<HeroMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay (Collider other) {
		Debug.Log ("Trigger stay");
		if (other == player.GetComponent<BoxCollider> ()) {
			if (player.navAgent.velocity == Vector3.zero) {
				player.SetFirePoint(firePoint);
			}
			else {
				player.UnsetFirePoint();
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		//		if (other == target.GetComponent<BoxCollider>()) {
		//			target.SetFirePoint(firePoint);
		//			Debug.Log("Entered: " + this.gameObject.name);
		//
		//		}
	}
	
	void OnTriggerExit(Collider other) {
		//		if (other == target.GetComponent<BoxCollider> ()) {
		//			target.UnsetFirePoint();
		//			Debug.Log("Exited: " + this.gameObject.name);
		//		}
	}
}
