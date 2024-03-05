using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
class UpgradeManager : MonoBehaviour, IStartLevelHandler, IEndLevelHandler, IGameOverHandler
{
	[SerializeField] int upgradesToSpawn;
	[SerializeField] List<UpgradeSetup> allUpgrades;
	[SerializeField] List<Vector3> spawnPoints;
	[SerializeField] float startDelay;
	[SerializeField] float spawnDuration;

	readonly List<UpgradeSetup> _notUsedUpgrades = new();
	readonly List<UpgradeSetup> _usedUpgrades = new();
	readonly List<UpgradeSetup> _currentlyUsedUpgrades = new();
	
	public int UpgradeCount { get; set; }

	bool _enableSpawn;

	void Start()
	{
		UpgradeCount = upgradesToSpawn;

		foreach (UpgradeSetup upgradeSetup in allUpgrades)
		{
			int count = upgradeSetup.count;
			for (int i = 0; i < count; i++)
				_notUsedUpgrades.Add(upgradeSetup);
		}
	}

	public void OnStartLevel(int level) => _enableSpawn = true;
	public void OnLevelEnded()
	{
		if (_enableSpawn)
			StartCoroutine(SpawnUpgradesRoutine());
	}
	public void OnGameOver() => _enableSpawn = false;

	IEnumerator SpawnUpgradesRoutine()
	{
		_currentlyUsedUpgrades.Clear();

		yield return new WaitForSeconds(startDelay);
		for (int i = 0; i < UpgradeCount; i++)
		{
			Vector3 point = spawnPoints[i];
			int index = Random.Range(0, _notUsedUpgrades.Count);
			UpgradeSetup upgradeSetup = _notUsedUpgrades[index];
			if (_currentlyUsedUpgrades.Contains(upgradeSetup))
			{
				i--;
				continue;
			}

			yield return new WaitForSeconds(spawnDuration);
			_currentlyUsedUpgrades.Add(upgradeSetup);
			_usedUpgrades.Add(upgradeSetup);
			if (upgradeSetup.removeOnUse)
				_notUsedUpgrades.RemoveAt(index);

			Upgrade upgrade = Instantiate(upgradeSetup.prefab, point, Quaternion.identity);
			upgrade.Setup(upgradeSetup);
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		foreach (Vector3 point in spawnPoints)
		{
			Gizmos.DrawSphere(point, 0.25f);
		}
	}

	Coroutine _clearUpgradesCoroutine;

	public void Upgrade(UpgradeSetup upgradeSetup)
	{
		upgradeSetup.ApplyUpgrade();
		if (_clearUpgradesCoroutine != null)
			return;
		_clearUpgradesCoroutine = StartCoroutine(ClearUpgrades());
	}

	IEnumerator ClearUpgrades()
	{
		yield return null;

		Upgrade[] upgrades = FindObjectsOfType<Upgrade>();
		foreach (Upgrade upgrade in upgrades)
			upgrade.Lock();

		foreach (Upgrade upgrade in upgrades)
		{
			if (upgrade == null) continue;
			yield return new WaitForSeconds(spawnDuration);
			upgrade.Disappear();
			upgrade.Disappear();
		}

		_usedUpgrades.Clear();
		FindObjectOfType<GameManager>().NextLevel();
		_clearUpgradesCoroutine = null;
	}
}