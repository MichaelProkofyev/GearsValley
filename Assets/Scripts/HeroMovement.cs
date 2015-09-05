using UnityEngine;
using System.Collections;

public class HeroMovement : MonoBehaviour {


	public Animator animatorController;
	public Transform bulletPrefab;
	public Transform firePoint;
	public float fireRate;
	public NavMeshAgent navAgent;
	public Transform sprite; 

	private bool canFire;
	private float nextFire = 0f;
	private float slideDuration = 0.5f;
	private float speed = 5;
	private bool automaticFire = false;
	private float autoShootingBulletCount;


	// Use this for initialization
	void Start () {
		navAgent = GetComponent<NavMeshAgent> ();
		canFire = false;
		fireRate = 1 / fireRate;
	}
	
	void Update () {            
		transform.rotation = Quaternion.identity;
		UpdateSprite ();
	}

	public void SetFirePoint (Transform newFirePoint) {
		firePoint = newFirePoint;
		canFire = true;
		StopSliding();
	}

	public void UnsetFirePoint () {
		canFire = false;
	}

	public void SwitchFireMode (bool automaticFire) {
		this.automaticFire = automaticFire;
//		if (automaticFire) {
//			autoShootingBulletCount = 0;
//		}
		autoShootingBulletCount = 0;
	}
	
	public void Shoot() {
		bool shootingNotTooFast = canFire && Time.time > nextFire;
		if (!shootingNotTooFast) return;


		Quaternion bulletRotation = firePoint.rotation;
		if (automaticFire) {
			float randAngleDiff = Mathf.Lerp (0.0f, 30.0f, autoShootingBulletCount/25);
			bulletRotation = Quaternion.Euler(bulletRotation.eulerAngles.x, bulletRotation.eulerAngles.y + Random.Range(-randAngleDiff, randAngleDiff), bulletRotation.eulerAngles.z);
			autoShootingBulletCount++;
		}


		nextFire = Time.time + fireRate;
		Instantiate (bulletPrefab, 
		             new Vector3(firePoint.position.x + Random.Range(-0.25f, 0.25f),  firePoint.position.y, firePoint.position.z + Random.Range(-0.25f, 0.25f)),
		             bulletRotation);

	}

	public void Slide () {
		if (!canFire) {
			Debug.Log ("SLIDING");
			navAgent.speed = speed * 2;
			Invoke("StopSliding", slideDuration);	
		}
	}

	void StopSliding () {
		navAgent.speed = speed;
	}

	public void MoveTo (Vector3 dest) {
		navAgent.SetDestination(dest);
	}


	void UpdateSprite ()
	{
		float scale = Mathf.Lerp(0.7f, 1f, 1f - (navAgent.speed/5 - 1));
		sprite.localScale = new Vector3 (scale, scale, sprite.localScale.z);
//		if (navAgent.velocity == Vector3.zero) {
//			animatorController.SetBool ("running", false);
//		} else {
//			animatorController.SetBool ("running", true);
//		}
	}

	void FaceLeft()    
	{	
		transform.localRotation = Quaternion.Euler(0, 0, 180);
	}
	
	void FaceRight()    
	{	
		transform.localRotation = Quaternion.identity;
	}
}
