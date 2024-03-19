using UnityEngine;

public class ImpactEffectPlayer : MonoBehaviour, IImpactHandler
{
	[SerializeField] float minimumDamage = 0.5f;
	[SerializeField] float minimumPush = 0;
	[SerializeField] bool disableOnDeath = true;


	[SerializeField] GameObject[] impactEffectPrefabs;
	[SerializeField] GameObject[] deathEffectPrefabs;

	EffectManager _effectManager;

	void Awake()
	{
		_effectManager = FindObjectOfType<EffectManager>();
	}

	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push)
	{
		if (minimumDamage > damage) return;
		if (minimumPush > push) return;
		if (impactEffectPrefabs.Length == 0) return;

		GameObject effect = impactEffectPrefabs[Random.Range(0, impactEffectPrefabs.Length)];
		_effectManager.Play(effect, position, Utility.GetAngle(direction));
	}

	public void OnAllHealthLost(ImpactReceiver receiver)
	{ 
		if (disableOnDeath)
			gameObject.SetActive(false);

		if (deathEffectPrefabs.Length == 0) return;

		GameObject effect = deathEffectPrefabs[Random.Range(0, deathEffectPrefabs.Length)];
		if (effect != null)
			_effectManager.Play(effect, transform.position, 0);
	}
}
