using System;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Movement;

public class Climb : GrateModule
{
	public static readonly string DisplayName = "Climb";

	public GameObject? climbableLeft;

	public GameObject? climbableRight;

	private InputTracker<float>? leftGrip;

	private Transform? leftHand;

	private Transform? rightHand;

	private InputTracker<float>? rightGrip;

	protected override void OnEnable()
	{
		if (!MenuController.Instance.Built)
		{
			return;
		}
		base.OnEnable();
		try
		{
			leftGrip = GestureTracker.Instance.leftGrip;
			rightGrip = GestureTracker.Instance.rightGrip;
			leftHand = GestureTracker.Instance.leftHand.transform;
			rightHand = GestureTracker.Instance.rightHand.transform;
			if (!Object.op_Implicit((Object)(object)climbableLeft))
			{
				climbableLeft = CreateClimbable(leftGrip);
			}
			climbableLeft.SetActive(true);
			if (!Object.op_Implicit((Object)(object)climbableRight))
			{
				climbableRight = CreateClimbable(rightGrip);
			}
			GameObject? obj = climbableRight;
			if (obj != null)
			{
				obj.SetActive(true);
			}
			ReloadConfiguration();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private GameObject CreateClimbable(InputTracker<float>? grip)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		GameObject obj = GameObject.CreatePrimitive((PrimitiveType)0);
		((Object)obj).name = "Grate Climb Obj";
		obj.AddComponent<GorillaClimbable>();
		obj.layer = LayerMask.NameToLayer("GorillaInteractable");
		obj.GetComponent<Renderer>().enabled = false;
		obj.transform.localScale = Vector3.one * 0.15f;
		obj.SetActive(false);
		if (grip != null)
		{
			grip.OnPressed = (Action<InputTracker>)Delegate.Combine(grip.OnPressed, new Action<InputTracker>(OnGrip));
		}
		if (grip != null)
		{
			grip.OnReleased = (Action<InputTracker>)Delegate.Combine(grip.OnReleased, new Action<InputTracker>(OnRelease));
		}
		return obj;
	}

	public void OnGrip(InputTracker tracker)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		if (!((Behaviour)this).enabled)
		{
			return;
		}
		GameObject val;
		Transform val2;
		if (tracker == leftGrip)
		{
			val = climbableLeft;
			val2 = leftHand;
		}
		else
		{
			val = climbableRight;
			val2 = rightHand;
		}
		if (Physics.OverlapSphere(val2.position, 0.15f, LayerMask.op_Implicit(GTPlayer.Instance.locomotionEnabledLayers)).Length == 0)
		{
			return;
		}
		if (val != null)
		{
			val.transform.position = val2.position;
		}
		if (tracker == leftGrip)
		{
			GameObject? obj = climbableRight;
			if (obj != null)
			{
				obj.SetActive(false);
			}
		}
		else
		{
			GameObject? obj2 = climbableLeft;
			if (obj2 != null)
			{
				obj2.SetActive(false);
			}
		}
		if (val != null)
		{
			val.SetActive(true);
		}
	}

	public void OnRelease(InputTracker tracker)
	{
		if (tracker == leftGrip)
		{
			GameObject? obj = climbableLeft;
			if (obj != null)
			{
				obj.SetActive(false);
			}
		}
		else
		{
			GameObject? obj2 = climbableRight;
			if (obj2 != null)
			{
				obj2.SetActive(false);
			}
		}
	}

	protected override void Cleanup()
	{
		if (leftGrip != null)
		{
			InputTracker<float>? inputTracker = leftGrip;
			inputTracker.OnPressed = (Action<InputTracker>)Delegate.Remove(inputTracker.OnPressed, new Action<InputTracker>(OnGrip));
			InputTracker<float>? inputTracker2 = leftGrip;
			inputTracker2.OnReleased = (Action<InputTracker>)Delegate.Remove(inputTracker2.OnReleased, new Action<InputTracker>(OnRelease));
		}
		if (rightGrip != null)
		{
			InputTracker<float>? inputTracker3 = rightGrip;
			inputTracker3.OnPressed = (Action<InputTracker>)Delegate.Remove(inputTracker3.OnPressed, new Action<InputTracker>(OnGrip));
			InputTracker<float>? inputTracker4 = rightGrip;
			inputTracker4.OnReleased = (Action<InputTracker>)Delegate.Remove(inputTracker4.OnReleased, new Action<InputTracker>(OnRelease));
		}
		climbableLeft?.Obliterate();
		climbableRight?.Obliterate();
	}

	public override string GetDisplayName()
	{
		return "Climb";
	}

	public override string Tutorial()
	{
		return "Press [Grip] with either hand to stick to a surface.";
	}
}
