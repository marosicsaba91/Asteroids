using TMPro;
using UnityEngine;
using UnityEngine.UI;

class GameUIManager : MonoBehaviour, IStartLevelHandler
{
	[SerializeField] TMP_Text levelText;
	[SerializeField] TMP_Text healthText;
	[SerializeField] RectTransform maxHealthFrame;
	[SerializeField] RectTransform currentHealthBar;
	[SerializeField] Button fullScreenButton;
	[SerializeField] Button muteButton;

	[SerializeField] AnimationCurve healthToLength;
	[SerializeField] float healthChangeSpeed = 10f;
	[SerializeField] float maxHealthChangeSpeed = 500f;

	GameManager _gameManager;
	Player _player;

	void Awake()
	{
		_gameManager = FindObjectOfType<GameManager>();
		_player = FindObjectOfType<Player>();
		 
		fullScreenButton.onClick.AddListener(FullScreen);
		muteButton.onClick.AddListener(Mute);
		AudioListener.pause = true;
	}

	public void OnStartLevel(int level) => UpdateUI();

	public void UpdateUI()
	{
		levelText.text = _gameManager.Level.ToString("00");

		float targetHealth = _player.CurrentHealth;
		float targetMaxHealth = _player.MaxHealth;
		healthText.text = targetHealth.ToString("0") + " / " + targetMaxHealth.ToString("0");
	}

	float _shownHealth;
	float _shownMaxHealth;

	void Update()
	{
		ChangeHealthBar();
	}

	void ChangeHealthBar()
	{
		float targetHealth = _player.CurrentHealth;
		float targetMaxHealth = _player.MaxHealth;
		if (_shownHealth != targetHealth || _shownMaxHealth != targetMaxHealth)
		{
			_shownHealth = Mathf.MoveTowards(_shownHealth, targetHealth, Time.deltaTime * healthChangeSpeed);

			float healthRate = Mathf.Clamp01(_shownHealth / _shownMaxHealth);
			Vector2 anchors = currentHealthBar.anchorMax;
			anchors.x = healthRate;
			currentHealthBar.anchorMax = anchors;

			_shownMaxHealth = Mathf.MoveTowards(_shownMaxHealth, targetMaxHealth, Time.deltaTime * maxHealthChangeSpeed);

			Vector2 sizeDelta = maxHealthFrame.sizeDelta;
			sizeDelta.x = healthToLength.Evaluate(_shownMaxHealth);
			maxHealthFrame.sizeDelta = sizeDelta;
		}
	}

	Resolution windowedResolution = new() { width = 1080, height = 720 };

	void Mute() 
	{
		AudioListener.pause = !AudioListener.pause;
	}

	void FullScreen()
	{
		if (Screen.fullScreen)
		{
			// TO WINDOWED
			Screen.SetResolution(windowedResolution.width, windowedResolution.height, false);
		}
		else
		{
			// TO FULLSCREEN
			windowedResolution = Screen.resolutions[0];
			Resolution fullScreenResolution = Screen.currentResolution;
			Screen.SetResolution(fullScreenResolution.width, fullScreenResolution.height, true);
		}
	}
}
