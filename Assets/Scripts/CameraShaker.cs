using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraShaker : MonoBehaviour {

	private float decreaseFactor = 1.0f;
	private float shake = 0f;
	private float shakeAmount = 0.1f;
	private Camera mainCamera;
	private Vector3 basePosition;

	private void Awake()
	{
		basePosition = transform.position;
		mainCamera = this.GetComponent<Camera> ();
	}

	private void Update()
	{
		if (shake > 0)
		{
			mainCamera.transform.position = basePosition + (Random.insideUnitSphere * shakeAmount);
			shake -= Time.deltaTime * decreaseFactor;

		} else
		{
			mainCamera.transform.position = basePosition;
			shake = 0.0f;
		}
	}

	public void Shake(float value)
	{
		shake = value;
	}
}
