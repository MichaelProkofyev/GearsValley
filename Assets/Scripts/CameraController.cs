using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	
 	public float shake = 0f;

	float shakeAmount = .1f;
	float decreaseFactor = 8f;
	float maxOffset = .1f;
	float maxXValue;
	float minXValue;
	float maxZValue;
	float minZValue;
	Vector3 originalLocalPosition;


	void Start () {
		originalLocalPosition = Camera.main.transform.localPosition;
		maxXValue = originalLocalPosition.x + maxOffset;
		minXValue = originalLocalPosition.x - maxOffset;
		maxZValue = originalLocalPosition.z + maxOffset;
		minZValue = originalLocalPosition.z - maxOffset;
	}
	
	// Update is called once per frame
	void Update () {
		if (shake > 0) {
			Vector2 randCPoint = Random.insideUnitCircle * shakeAmount;

			Camera.main.transform.localPosition = new Vector3( Mathf.Clamp(Camera.main.transform.localPosition.x + randCPoint.x, minXValue, maxXValue),
			                                                  Camera.main.transform.localPosition.y,
			                                                  Mathf.Clamp(Camera.main.transform.localPosition.z + randCPoint.y, minZValue, maxZValue));

			shake -= Time.deltaTime * decreaseFactor;
		}
		else {
			Camera.main.transform.localPosition = originalLocalPosition;
		}

	}

	public void StartShake () {
		shake = 2.0f;
	}

}
