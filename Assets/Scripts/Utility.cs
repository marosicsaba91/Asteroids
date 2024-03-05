using UnityEngine;
using System.Linq;

public class Utility
{
	public static T[] FindInterfaces<T>() =>
		Object.FindObjectsOfType<MonoBehaviour>().Where(b => b is T).Cast<T>().ToArray();

	public static float GetAngle(Vector2 vector)
	{
		float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
		return angle;
	}
}