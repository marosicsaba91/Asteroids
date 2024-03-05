using TMPro;
using UnityEngine;

public class Upgrade : MonoBehaviour, IImpactHandler
{
	[SerializeField] TMP_Text descriptionText;
	[SerializeField] SpriteRenderer upgradeImage;
	[SerializeField] ImpactReceiver impactReceiver;
	[SerializeField] Animator upgradeAnimator;

	UpgradeSetup _upgradeSetup;
	UpgradeManager _upgradeManager;

	void OnValidate()
	{
		if (descriptionText == null)
			descriptionText = GetComponentInChildren<TMP_Text>();
		if (upgradeImage == null)
			upgradeImage = GetComponentInChildren<SpriteRenderer>();
		if (impactReceiver == null)
			impactReceiver = GetComponent<ImpactReceiver>();
		if (upgradeAnimator == null)
			upgradeAnimator = GetComponent<Animator>();
	}

	public void Setup(UpgradeSetup upgradeSetup)
	{
		_upgradeManager = FindObjectOfType<UpgradeManager>();
		_upgradeSetup = upgradeSetup;
		descriptionText.text = upgradeSetup.GetDescription();
		upgradeImage.sprite = upgradeSetup.icon;

	}

	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push) { }

	public void OnAllHealthLost(ImpactReceiver receiver)
	{
		_upgradeManager.Upgrade(_upgradeSetup);
	}

	public void Disappear() => upgradeAnimator.SetBool("IsVisible", false);
	public void Lock() => impactReceiver.enableImpact = false;

	public void OnDisappear()
	{
		Destroy(gameObject);
	}
}