using UnityEngine;
class Player : MonoBehaviour, IImpactHandler
{
	[SerializeField] ImpactReceiver spaceshipImpactReceiver;
	[SerializeField] SpaceshipGun gun;
	public SpaceshipGun Gun => gun;
	GameUIManager _gameUIManager;
	void Awake()
	{
		_gameUIManager = FindObjectOfType<GameUIManager>(); 
	}
	void OnValidate()
	{
		if (spaceshipImpactReceiver == null)
			spaceshipImpactReceiver = GetComponentInChildren<ImpactReceiver>();
		if (gun == null)
			gun = GetComponentInChildren<SpaceshipGun>();
	}
	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push) => _gameUIManager.UpdateUI();
	public void OnAllHealthLost(ImpactReceiver receiver) => FindObjectOfType<GameManager>().GameOver();

	public float MaxHealth
	{
		get => spaceshipImpactReceiver.maxHealth;
		set => spaceshipImpactReceiver.maxHealth = value;
	}
	public float CurrentHealth
	{
		get => spaceshipImpactReceiver.currentHealth;
		set => spaceshipImpactReceiver.currentHealth = value;
	}
}