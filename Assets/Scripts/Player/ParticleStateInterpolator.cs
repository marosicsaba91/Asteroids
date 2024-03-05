using System;
using UnityEngine;
using UnityEngine.Serialization;


class ParticleStateInterpolator : MonoBehaviour
{
	[Serializable]
	public struct State
	{
		public Vector3 position;
		public Vector3 scale;
		public float alpha;
		public float emission;
		public float startSpeed;
		public float startScale;

		public static State Lerp(State one, State other, float t) => new()
		{
			position = Vector3.Lerp(one.position, other.position, t),
			scale = Vector3.Lerp(one.scale, other.scale, t),
			alpha = Mathf.Lerp(one.alpha, other.alpha, t),
			emission = Mathf.Lerp(one.emission, other.emission, t),
			startScale = Mathf.Lerp(one.startScale, other.startScale, t),
			startSpeed = Mathf.Lerp(one.startSpeed, other.startSpeed, t),
		};
	}


	[SerializeField] new ParticleSystem particleSystem;

	[Space]
	[SerializeField, FormerlySerializedAs("minDriveState")] State minState = new();
	[SerializeField, FormerlySerializedAs("maxDriveState")] State maxState = new();

	[SerializeField] float changeSpeed = 1;

	[Space]
	[SerializeField, Range(0, 1)] float testValue = 0.5f;
	[SerializeField, Range(0, 1)] float startValue = 0.5f;

	float _currentValue;
	float _targetValue;

	void OnValidate()
	{
		if (particleSystem == null)
			particleSystem = GetComponentInChildren<ParticleSystem>();

		SetValueQuick(testValue);
	}

	void Start()
	{
		_currentValue = startValue;
		_targetValue = startValue;
		SetValueQuick(startValue);
	}

	void Update() 
	{
		if (_currentValue != _targetValue)
		{
			_currentValue = Mathf.MoveTowards(_currentValue, _targetValue, changeSpeed * Time.deltaTime);
			SetValueQuick(_currentValue);
		}
	}

	public void SetValue(float value)
	{
		_targetValue = Mathf.Clamp01(value);
	}

	public void SetValueQuick(float value)
	{
		if (particleSystem == null) return;

		value = Mathf.Clamp01(value); 
		State state = State.Lerp(minState, maxState, value);

		// Transform
		transform.localPosition = state.position;
		transform.localScale = state.scale;

		// Alpha
		float alpha = state.alpha;
		ParticleSystem.ColorOverLifetimeModule colorModule = particleSystem.colorOverLifetime;
		Gradient gradient = colorModule.color.gradient;
		gradient.alphaKeys = new GradientAlphaKey[] { new (alpha, 0), new (alpha, 1) };
		colorModule.color = gradient;

		// Emission
		ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
		emissionModule.rateOverTime = state.emission;

		// Speed
		ParticleSystem.MainModule mainModule = particleSystem.main;
		mainModule.startSize = state.startScale;
		mainModule.startSpeed = state.startSpeed;
	}
}
