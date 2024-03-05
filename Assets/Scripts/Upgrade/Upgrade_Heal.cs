
using UnityEngine;

[CreateAssetMenu(menuName = "Asteroids/Upgrade/Heal", fileName = "Upgrade Heal")]
class Upgrade_Heal : UpgradeSetup
{
	[Space]
	[SerializeField, Min(0)] float healAmount = 0;
	[SerializeField, Min(0)] float healMultiplier = 0;

	[SerializeField, Min(0)] float raiseMaxHealthAmount = 0;
	[SerializeField, Min(0)] float raiseMaxHealthMultiplier = 0;

	public override void ApplyUpgrade()
	{
		Player player = FindObjectOfType<Player>();

		if (healAmount != 0)
			player.CurrentHealth += healAmount;
		if (healMultiplier != 0)
			player.CurrentHealth += player.MaxHealth * healMultiplier;

		if (raiseMaxHealthAmount != 0)
			player.MaxHealth += raiseMaxHealthAmount;
		if (raiseMaxHealthMultiplier != 0)
			player.MaxHealth += player.MaxHealth * raiseMaxHealthMultiplier;
	}

	public override string GetDescription()
	{
		if (healAmount != 0)
			return $"Heal {healAmount} HP";
		if (healMultiplier != 0)
			return $"Heal {healMultiplier * 100:0}% of your max HP";
		if (raiseMaxHealthAmount != 0)
			return $"Increase max HP by {raiseMaxHealthAmount}";
		if (raiseMaxHealthMultiplier != 0)
			return $"Increase max HP by {raiseMaxHealthMultiplier * 100:0}%";

		return "Heal...";
	}
}