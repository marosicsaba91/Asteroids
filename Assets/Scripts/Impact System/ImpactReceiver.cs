using UnityEngine; 

public class ImpactReceiver : MonoBehaviour
{
	[SerializeField] new Collider2D collider2D;
	[SerializeField] Rigidbody2D rigidBody2D;
	[SerializeField] float inertia = 1;
	[SerializeField] float initialMaxHealth;
	[SerializeField] float minimumDamage = 1;
	[SerializeField] bool destroyOnDeath = false;

	public bool enableImpact = true;

	public float Inertia => inertia;
	public float HealthRate => currentHealth / maxHealth;

	void Awake()
	{
		maxHealth = initialMaxHealth;
		currentHealth = initialMaxHealth;
	}

	public float maxHealth;
	public float currentHealth; 

	void OnValidate()
	{
		if (collider2D == null)
			collider2D = GetComponent<Collider2D>();
		if (rigidBody2D == null)
			rigidBody2D = GetComponent<Rigidbody2D>();
	}

	public bool TryImpact(Vector2 direction, Vector2 position, float damage, float push)
	{
		if (!enableImpact) return false;
		if (damage < minimumDamage)
			damage = 0;

		Impact(direction, position, damage, push);
		return true;
	}

	void Impact(Vector2 direction, Vector2 position, float damage, float push)
	{
		if (currentHealth <= 0) return;
		if (rigidBody2D != null)
			rigidBody2D.AddForceAtPosition((push * rigidBody2D.mass) * direction.normalized, position, ForceMode2D.Impulse);

		float lastHealth = currentHealth;

		currentHealth -= damage;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		if (lastHealth != currentHealth)
		{
			damage = Mathf.Min(damage, lastHealth); 
			// HealthChanged?.Invoke();
		}

		IImpactHandler[] handlers = GetComponentsInChildren<IImpactHandler>();

		foreach (IImpactHandler handler in handlers)
			handler.OnImpact(this, direction, position, damage, push / inertia);

		if (currentHealth == 0)
		{
			// LostAllHealth?.Invoke();
			foreach (IImpactHandler handler in handlers)
				handler.OnAllHealthLost(this);

			if (destroyOnDeath)
				Destroy(gameObject);
		}
	}

	public Vector2 ClosestPoint(Vector2 p) => collider2D.ClosestPoint(p);
	public void Kill() => Impact(Vector2.zero, rigidBody2D.position, float.MaxValue, 0);
}