using UnityEngine;

class Projectile : MonoBehaviour, ITeleportHandler, IEndLevelHandler
{
	[SerializeField] Rigidbody2D rigidBody;
	[SerializeField] new Renderer renderer;
	[SerializeField] new Collider2D collider;
	[SerializeField] Animator animator;
	[SerializeField] TrailRenderer trail;
	[Space]
	[SerializeField] float destroyDelay = 0.2f;
	[SerializeField] float acceleration = 15;

	[SerializeField] GameObject mazzleEffectPrefab;
	[SerializeField] GameObject dieEffectPrefab;
	[SerializeField] GameObject hitEffectPrefab;

	EffectManager _effectManager; 

	float _lifeTimeLeft;
	bool _destroyed = false;
	float _magnetism = 0;
	float _damage;
	float _push;
	float _speed;

	void Awake()
	{
		_effectManager = FindObjectOfType<EffectManager>();
	}

	public void Setup(Vector2 position, Vector2 startVelocity, float damage, float push, float magnetism, float duration)
	{
		float angel = Utility.GetAngle(startVelocity);
		Quaternion rotation = Quaternion.Euler(0, 0, angel - 90);
		transform.SetPositionAndRotation(position, rotation);

		_damage = damage;
		_push = push;
		_magnetism = magnetism;
		_lifeTimeLeft = duration;
		_speed = startVelocity.magnitude;
		rigidBody.velocity = startVelocity;
		_effectManager.Play(mazzleEffectPrefab, position, angel);
	}
	public void OnLevelEnded() => Die();
	void Update()
	{
		_lifeTimeLeft -= Time.deltaTime;

		if (_lifeTimeLeft <= 0)
			animator.SetTrigger("Die");
	}

	void FixedUpdate()
	{
		if (_magnetism != 0)
		{
			Vector2 force = MagnetTarget.GetForceVectorAt(transform.position);
			if (force != Vector2.zero)
			{
				Vector2 magnetForce = force * _magnetism;
				rigidBody.velocity += magnetForce * Time.fixedDeltaTime;
			}
		}

		float currentSpeed = rigidBody.velocity.magnitude;
		if (currentSpeed != _speed && currentSpeed != 0)
		{
			float speed = Mathf.MoveTowards(currentSpeed, _speed, acceleration * Time.fixedDeltaTime);
			rigidBody.velocity *= (speed / currentSpeed);
		}

		transform.rotation = Quaternion.Euler(0, 0, Utility.GetAngle(rigidBody.velocity) - 90);
	}
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + (Vector3)rigidBody.velocity);
	}
	public void OnTeleport()
	{
		trail.Clear();
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.TryGetComponent(out ImpactReceiver target))
		{
			Vector2 collisionPoint = other.ClosestPoint(transform.position);
			if (target.TryImpact(rigidBody.velocity, collisionPoint, _damage, _push))
			{
				_effectManager.Play(hitEffectPrefab, collisionPoint, Utility.GetAngle(rigidBody.velocity));
				Destroy();
			}
		}
	}
	public void Die()
	{
		if (_destroyed) return;
		_effectManager.Play(dieEffectPrefab, transform.position, Utility.GetAngle(rigidBody.velocity));
		Destroy();
	}
	void Destroy()
	{
		_destroyed = true;
		collider.enabled = false;
		renderer.enabled = false;
		rigidBody.velocity = Vector2.zero;
		Destroy(gameObject, destroyDelay);
	}
}
