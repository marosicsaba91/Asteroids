
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField] Camera mainCamera;
	[SerializeField] float sizeChange = 5;

	float _startSize;

	public float targetSize;

	void OnValidate()
	{
		if (mainCamera == null)
			mainCamera = Camera.main;
	}

	void Awake()
	{
		_startSize = mainCamera.orthographicSize;
		targetSize = _startSize;
	}
	
	void FixedUpdate()
	{
		if (mainCamera.orthographicSize == targetSize) return;
		float size  = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.fixedDeltaTime * sizeChange);
		mainCamera.orthographicSize = size;
	}
}
