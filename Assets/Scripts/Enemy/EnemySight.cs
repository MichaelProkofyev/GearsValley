using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

	public float fieldOfViewAngle = 110f;
	public bool playerInSight;
	public Vector3 personalLastSighting;
	public LineRenderer leftFOVLine;
	public LineRenderer rightFOVLine;

	private GameObject player;
	private SphereCollider sphereCollider;


	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player");
		sphereCollider = GetComponent<SphereCollider>();

		leftFOVLine.enabled = true;
		rightFOVLine.enabled = true;

		rightFOVLine.sortingLayerName = "Foreground";
		leftFOVLine.sortingLayerName = "Foreground";

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		leftFOVLine.SetPosition(0, transform.position);
		rightFOVLine.SetPosition(0, transform.position);

		leftFOVLine.SetPosition(1, Quaternion.AngleAxis(-fieldOfViewAngle/2, Vector3.up) * transform.forward * sphereCollider.radius);
		rightFOVLine.SetPosition(1, Quaternion.AngleAxis(fieldOfViewAngle/2, Vector3.up) * transform.forward * sphereCollider.radius);

//		Vector3 rightVector = Quaternion.AngleAxis(fieldOfViewAngle/2, Vector3.up) * transform.forward;
//		Debug.DrawLine (transform.position, leftVector * sphereCollider.radius, Color.red);
//		Debug.DrawLine (transform.position, rightVector * sphereCollider.radius, Color.red);
		Debug.DrawLine (transform.position, transform.forward * sphereCollider.radius, Color.green);
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject == player) {
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			if (angle < fieldOfViewAngle * 0.5f) {
				playerInSight = true;
				personalLastSighting = other.gameObject.transform.position;

			} else {
				playerInSight = false;
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject == player) {
			playerInSight = false;
		}
	}
}
