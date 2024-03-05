using UnityEngine;

public class ImpactEffectPlayer : MonoBehaviour, IImpactHandler
{
	[SerializeField] Effect[] impactEffects;
	[SerializeField] Effect[] deathEffects;
	[SerializeField] float minimumDamage = 0.5f;
	[SerializeField] float minimumPush = 0;
	[SerializeField] bool disableOnDeath = true;

	EffectManager _effectManager;

	void Awake()
	{
		_effectManager = FindObjectOfType<EffectManager>();
	}

	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push)
	{
		if (minimumDamage > damage) return;
		if (minimumPush > push) return;
		if (impactEffects.Length == 0) return;

		Effect effect = impactEffects[Random.Range(0, impactEffects.Length)];
		_effectManager.Play(effect, position, Utility.GetAngle(direction));
	}

	public void OnAllHealthLost(ImpactReceiver receiver)
	{ 
		if (disableOnDeath)
			gameObject.SetActive(false);

		if (deathEffects.Length == 0) return;

		Effect effect = deathEffects[Random.Range(0, deathEffects.Length)];
		if (effect != null)
			_effectManager.Play(effect, transform.position, 0);
	}
}
