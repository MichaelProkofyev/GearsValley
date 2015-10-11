using UnityEngine;
using System.Collections;

public class HeroMovement : MonoBehaviour {


	public Animator animatorController;
	public Transform bulletPrefab;
	public Transform firePoint;
	public float fireRate;
	public NavMeshAgent navAgent;
	public Transform sprite; 

	private bool behindCover;
	private bool sliding;
	private float nextFire = 0f;
	private float slideDuration = 0.5f;
	private float speed = 5;
	private bool automaticFire = false;
	private float autoShootingBulletCount;
	private GameObject[] visibleEnemies;
	private int obstacleLayerMask;
	private CameraController cameraController;


	// Use this for initialization
	void Start () {
		cameraController = Camera.main.GetComponent<CameraController>();
		navAgent = GetComponent<NavMeshAgent> ();
		obstacleLayerMask = LayerMask.NameToLayer ("Obstacle");
		behindCover = false;
		fireRate = 1 / fireRate;
	}
	
	void Update () {            
		transform.rotation = Quaternion.identity;
		UpdateSprite ();
	}

	public void SetFirePoint (Transform newFirePoint) {
		firePoint.position = newFirePoint.position;
		firePoint.rotation = newFirePoint.rotation;
		behindCover = true;
		StopSliding();
	}

	public void UnsetFirePoint () {
		behindCover = false;
	}

	public void SwitchFireMode (bool automaticFire) {
		this.automaticFire = automaticFire;
//		if (automaticFire) {
//			autoShootingBulletCount = 0;
//		}
		autoShootingBulletCount = 0;
	}
	
	public void Shoot() {
		bool shootingTooFast =  Time.time < nextFire;
		if (shootingTooFast || sliding) return;



		if (!behindCover) {
			visibleEnemies = GameObject.FindGameObjectsWithTag("Enemy");
			float closestEnemyDistance = Mathf.Infinity;
			GameObject closestEnemy = null;
			foreach (GameObject enemy in visibleEnemies) {
				if (!Physics.Raycast(transform.position, enemy.transform.position, Mathf.Infinity,  ~obstacleLayerMask)) {
					Debug.DrawLine(transform.position, enemy.transform.position);
					float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
					if (enemyDistance < closestEnemyDistance) {
						closestEnemy = enemy;
					}
				}
			}
			if (!closestEnemy) return;
			Vector3 between = closestEnemy.transform.position - transform.position;
			firePoint.position = transform.position + between.normalized*0.25f;
			firePoint.LookAt(closestEnemy.transform);


		}


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
		cameraController.StartShake();



	}

	public void Slide () {
		if (!behindCover) {
			sliding = true;
			Debug.Log ("SLIDING");
			navAgent.speed = speed * 2;
			Invoke("StopSliding", slideDuration);	
		}
	}

	void StopSliding () {
		navAgent.speed = speed;
		sliding = false;
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
