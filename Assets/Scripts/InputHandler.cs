using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	
	private int clickableLayerMask;
	private HeroMovement player;
	private Vector2 touchStartPosition;
	private bool shooting = false;
	private bool prepShooting = false;
	private float swipeDuration = .25f;
	private float touchStartTime;


	void Awake () {
		clickableLayerMask = LayerMask.GetMask("Clickable");
		Function(4);
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<HeroMovement>();
//		clickableLayerMask = 1 << 8;//LayerMask.NameToLayer ("Clickable");
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_IPHONE
		if (Application.isMobilePlatform) {
			HandleTouches ();
			if (shooting) {
				player.Shoot();	
			} else {
				if (prepShooting) {
					bool holdTouchDetected = touchStartTime + swipeDuration < Time.time;
					if (holdTouchDetected) {
						player.SwitchFireMode(true);
						shooting = true;
						Debug.Log ("Started shooting");
					}						
				}
			}	
		}
#endif

#if UNITY_EDITOR_OSX
		HandleKeys ();
#endif
	}

	void HandleKeys () {
		if (Input.GetButton("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray,out raycastHit, Mathf.Infinity, clickableLayerMask)) {
				player.MoveTo(raycastHit.collider.gameObject.transform.position);
			} else {
				if (!shooting) {
					shooting = true;
					player.SwitchFireMode(true);
				}
				player.Shoot();
			}
		}

		if (Input.GetButtonUp("Fire1")) {
			shooting = false;
		}

		if (Input.GetButton("Jump")) {
			player.Slide();
		}
	}

	void Function (float x = Mathf.Infinity, int y = 10) {
		Debug.Log("X: " + x + "Y: " + y);
	}

	void HandleTouches ()	{
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];

			switch (touch.phase) {
			case TouchPhase.Began:
				touchStartPosition = touch.position;
				prepShooting = true;
				touchStartTime = Time.time;
				break;
			case TouchPhase.Ended:
				prepShooting = false;
				if (shooting) {
					shooting = false;
					Debug.Log ("Stopped shooting");
				} else {
					float swipeDistVertical = Mathf.Abs(touch.position.y - touchStartPosition.y);
					if (swipeDistVertical >= 200) {
						player.Slide();
					} else {
						if (swipeDistVertical < 20) {
							HandleTap (touchStartPosition);
						} else {
							Debug.Log ("Unhandled swipe with dist: " + swipeDistVertical);
						}
					}
				}
				break;
			}

		}
	}

	void HandleTap (Vector2 tapPosition) {
		Vector3 wp = Camera.main.ScreenToWorldPoint(tapPosition);
		Vector3 touchPos = new Vector3(wp.x, 0, wp.z);

		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(touchPos, 0.1F);

		Collider[] hitColliders = Physics.OverlapSphere(touchPos, .1f, clickableLayerMask);
		if (hitColliders.Length != 0) {
			player.MoveTo(hitColliders[0].gameObject.transform.position);
		}else {
			Debug.Log ("SHOOTING ONCE");
			player.SwitchFireMode(false);
			player.Shoot();
		}
	}
}
