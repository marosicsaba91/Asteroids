using UnityEngine;

class GameManager : MonoBehaviour
{	public int Level { get; private set; }
	public bool IsQuitting { get; private set; } = false;
	void Awake()
	{ 
		Application.quitting += () => IsQuitting = true;
		Level = 1;
	}
	void Start()
	{
		StartLevel();
	}
	public void NextLevel()
	{
		Level++;
		StartLevel();
	}
	void StartLevel()
	{
		if (IsQuitting) return;
		foreach (IStartLevelHandler handler in Utility.FindInterfaces<IStartLevelHandler>())
			handler.OnStartLevel(Level);
	}
	public void EndLevel()
	{
		if (IsQuitting) return;
		foreach (IEndLevelHandler handler in Utility.FindInterfaces<IEndLevelHandler>())
			handler.OnLevelEnded();
	}
	public void GameOver() 
	{
		foreach (IGameOverHandler handler in Utility.FindInterfaces<IGameOverHandler>())
			handler.OnGameOver();
	}
	public void DoRestartGame()
	{
		string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
	}
}
