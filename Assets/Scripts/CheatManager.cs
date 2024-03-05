using UnityEngine;

class CheatManager : MonoBehaviour
{
	void Update()
	{
		if (!Application.isEditor) return;

		if (Input.GetKeyDown(KeyCode.X))
			FindObjectOfType<AsteroidManager>().KillAllAsteroids();

		if (Input.GetKeyDown(KeyCode.Alpha2))
			FindObjectOfType<EnemyManager>().SpawnSmall();

		if (Input.GetKeyDown(KeyCode.Alpha1))
			FindObjectOfType<EnemyManager>().SpawnBig();

		if (Input.GetKeyDown(KeyCode.Alpha0))
			FindObjectOfType<EnemyManager>().KillAllEnemies();
	}

}