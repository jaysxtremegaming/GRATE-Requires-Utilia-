using UnityEngine;

namespace Grate.Modules.Movement;

public class LocalGorillaVelocityTracker : MonoBehaviour
{
	private Vector3 previousLocalPosition;

	private Vector3 velocity;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		previousLocalPosition = ((Component)this).transform.localPosition;
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = (((Component)this).transform.localPosition - previousLocalPosition) / Time.deltaTime;
		velocity = ((Component)this).transform.parent.TransformDirection(val);
		previousLocalPosition = ((Component)this).transform.localPosition;
	}

	public Vector3 GetVelocity()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return velocity;
	}
}
