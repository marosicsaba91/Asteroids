using UnityEngine;

// Global event handlers
public interface IStartLevelHandler
{
	void OnStartLevel(int level);
}
public interface IEndLevelHandler
{
	void OnLevelEnded();
}
public interface IGameOverHandler
{
	void OnGameOver();
}

// Local event handlers
public interface ITeleportHandler
{
	void OnTeleport();
}
public interface IImpactHandler
{
	void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push);
	void OnAllHealthLost(ImpactReceiver receiver);
}

public interface IGun
{
	float Damage { get; }
	float Duration { get; }
	float TargetSpeed { get; }
	float Push { get; }
	float Magnetism { get; }
	Transform Transform { get; }
}
