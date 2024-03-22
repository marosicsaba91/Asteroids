using System.Collections.Generic;
using UnityEngine;

class EnemyManager : MonoBehaviour, IStartLevelHandler, IEndLevelHandler
{
	[SerializeField] List<Vector2> spawnPoints;
	[SerializeField] GameObject enemyPrefabsSmall;
	[SerializeField] GameObject enemyPrefabsBig;
	[SerializeField] AnimationCurve nextEnemySpawnDurationOverTime1;
	[SerializeField] AnimationCurve nextEnemySpawnDurationOverTime2;
	[SerializeField] AnimationCurve probabilityOfBigEnemyOverTime;
	[SerializeField] AnimationCurve healthMultiplierOverTime;

	public bool paused = false;
	float _time = 0;
	float _nextEnemySpawnTime;
	void Start()
	{
		_time = 0;
		CalculateNextSpanTime();
	}
	public void OnLevelEnded() => paused = true;
	public void OnStartLevel(int level) => paused = false;


	void Update()
	{
		if (paused) return;

		_time += Time.deltaTime;

		if (_time >= _nextEnemySpawnTime)
		{
			SpawnEnemy();
			CalculateNextSpanTime();
		} 
	}

	void CalculateNextSpanTime()
	{
		float t1 = nextEnemySpawnDurationOverTime1.Evaluate(_time);
		float t2 = nextEnemySpawnDurationOverTime2.Evaluate(_time);
		_nextEnemySpawnTime = _time + Random.Range(t1, t2);
	}

	void SpawnEnemy()
	{
		float p = probabilityOfBigEnemyOverTime.Evaluate(_time);
		GameObject enemyPrefab = Random.value < p ? enemyPrefabsBig : enemyPrefabsSmall;

		Spawn(enemyPrefab);
	}

	public void SpawnSmall() => Spawn(enemyPrefabsSmall);
	public void SpawnBig() => Spawn(enemyPrefabsBig);

	void Spawn(GameObject enemyPrefab)
	{
		Vector2 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
		GameObject newGO = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

		float healthMultiplier = healthMultiplierOverTime.Evaluate(_time);
		ImpactReceiver impactReceiver = newGO.GetComponent<ImpactReceiver>();
		impactReceiver.maxHealth *= healthMultiplier;
		impactReceiver.currentHealth = impactReceiver.maxHealth;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		foreach (Vector2 spawnPoint in spawnPoints)
		{
			Gizmos.DrawSphere(spawnPoint, 0.3f);
		}
	}

	public void KillAllEnemies()
	{
		foreach (Enemy enemy in FindObjectsOfType<Enemy>())
			enemy.GetComponent<ImpactReceiver>().Kill();
	}
}