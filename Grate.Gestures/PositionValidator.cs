using System;
using GorillaLocomotion;
using Grate.Modules.Multiplayer;
using Grate.Modules.Physics;
using Grate.Tools;
using UnityEngine;

namespace Grate.Gestures;

public class PositionValidator : MonoBehaviour
{
	public static PositionValidator Instance;

	private readonly float stabilityPeriod = 1f;

	public bool isValid;

	public bool isValidAndStable;

	public bool hasValidPosition;

	public Vector3 lastValidPosition;

	private float stabilityPeriodStart;

	private void Awake()
	{
		Instance = this;
	}

	private void FixedUpdate()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Collider[] array = Physics.OverlapSphere(GTPlayer.Instance.lastHeadPosition, 0.15f * GTPlayer.Instance.scale, LayerMask.op_Implicit(GTPlayer.Instance.locomotionEnabledLayers));
			bool num = isValid;
			isValid = array.Length == 0;
			if (!num && isValid)
			{
				stabilityPeriodStart = Time.time;
			}
			else if (isValid && Time.time - stabilityPeriodStart > stabilityPeriod)
			{
				lastValidPosition = ((Component)GTPlayer.Instance.bodyCollider).transform.position;
				hasValidPosition = true;
				isValidAndStable = true;
				if (Object.op_Implicit((Object)(object)NoClip.Instance?.button))
				{
					NoClip.Instance.button.RemoveBlocker(ButtonController.Blocker.NOCLIP_BOUNDARY);
				}
				if (Object.op_Implicit((Object)(object)Piggyback.Instance?.button))
				{
					Piggyback.Instance.button.RemoveBlocker(ButtonController.Blocker.NOCLIP_BOUNDARY);
				}
			}
			else if (!isValid)
			{
				isValidAndStable = false;
				if (Object.op_Implicit((Object)(object)NoClip.Instance?.button))
				{
					NoClip.Instance.button.AddBlocker(ButtonController.Blocker.NOCLIP_BOUNDARY);
				}
				if (Object.op_Implicit((Object)(object)Piggyback.Instance?.button))
				{
					Piggyback.Instance.button.AddBlocker(ButtonController.Blocker.NOCLIP_BOUNDARY);
				}
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
