using System;
using System.Collections.Generic;
using UnityEngine;
public class Asteroid : MonoBehaviour, IImpactHandler
{
	[SerializeField] Rigidbody2D rigidBody2D;
	[SerializeField] ImpactReceiver impactReceiver;
	[SerializeField] new Collider2D collider2D;

	[SerializeField] float asteroidWeight = 100;
	[SerializeField] float impactDamage;
	[SerializeField, Range(0, 1)] float damageVelocityMultiplier = 0.5f;
	[SerializeField] float damageMaxVelocity = 10f;
	[SerializeField, Range(0, 1)] float damageInertiaMultiplier = 0.5f;
	[SerializeField] float damageMaxInertia = 100f;
	[SerializeField] List<Asteroid> subAsteroids = new();

	bool _isInitialized = false;
	AsteroidManager _asteroidManager;

	public float AsteroidWeight => asteroidWeight;
	 
	public GameObject spawnEffectPrefab;

	public Collider2D Collider => collider2D;

	public void Kill() => impactReceiver.Kill();

	public void SetVelocity(Vector2 value) => rigidBody2D.velocity = value;

	public void SetAngularVelocity(float value) => rigidBody2D.angularVelocity = value; 

	void OnValidate()
	{
		if (rigidBody2D == null)
			rigidBody2D = GetComponentInChildren<Rigidbody2D>();
		if (impactReceiver == null)
			impactReceiver = GetComponentInChildren<ImpactReceiver>();
		if (collider2D == null)
			collider2D = GetComponentInChildren<Collider2D>();
	}
	void Awake()
	{
		_asteroidManager = FindObjectOfType<AsteroidManager>();
		_isInitialized = true;
	}

	void OnEnable()
	{
		_asteroidManager.RegisterAsteroid(this);
	}

	void OnDisable()
	{
		_asteroidManager.UnregisterAsteroid(this);
	}


	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push) { }

	public void OnAllHealthLost(ImpactReceiver receiver)
	{
		_asteroidManager.SpawnSubAsteroids(transform.position, rigidBody2D.velocity, rigidBody2D.angularVelocity, subAsteroids);
		Destroy(gameObject);
	}

	
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!_isInitialized) return;

		if (collision.gameObject.TryGetComponent(out ImpactReceiver impactReceiver))
		{
			ContactPoint2D contact = collision.contacts[0];
			Vector2 direction = contact.relativeVelocity.normalized;
			Vector2 position = contact.point;

			float push = 0;

			float damageVelocity = Mathf.Clamp(collision.relativeVelocity.magnitude, 0, damageMaxVelocity) / damageMaxVelocity;
			float damageInertia = Mathf.Clamp(impactReceiver.Inertia, 0, damageMaxInertia) / damageMaxInertia;
			
			float multiplier = Mathf.LerpUnclamped(1, damageVelocity, damageVelocityMultiplier);
			multiplier *= Mathf.LerpUnclamped(1, damageInertia, damageInertiaMultiplier);

			float damage = impactDamage * Mathf.Clamp01(multiplier);
			impactReceiver.TryImpact(direction, position, damage, push);
		}
	}

	void OnDrawGizmos()
	{
		// Center of Mass
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(rigidBody2D.worldCenterOfMass, 0.1f);

	}
}
