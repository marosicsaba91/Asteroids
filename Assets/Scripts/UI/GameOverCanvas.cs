using UnityEngine;
using UnityEngine.UI;
class GameOverCanvas : MonoBehaviour, IGameOverHandler
{
	[SerializeField] Animator gameOverScreenAnimator;
	[SerializeField] Button restartButton;
	void OnValidate()
	{
		if (gameOverScreenAnimator == null)
			gameOverScreenAnimator = GetComponent<Animator>();
		if (restartButton == null)
			restartButton = GetComponentInChildren<Button>();
	}
	void Awake()
	{
		restartButton.onClick.AddListener(CloseUI);
	}
	public void RestartGame() // CALLED FROM ANIM METHOD
	{
		GameManager asteroidsGameManager = FindObjectOfType<GameManager>();
		asteroidsGameManager.DoRestartGame();
	}
	public void OnGameOver() =>
		gameOverScreenAnimator.SetBool("IsOpen", true);

	void CloseUI() =>
		gameOverScreenAnimator.SetBool("IsOpen", false);
}
