using System;
using System.Collections.Generic;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Multiplayer;

public class Grab : GrateModule
{
	public class GBMarker : MonoBehaviour
	{
		public static int count;

		public Rigidbody controllingBody;

		private DebugRay dr;

		private bool grippingRight;

		private bool grippingLeft;

		public Transform leftHand;

		public Transform rightHand;

		public Transform controllingHand;

		public VRRig rig;

		private int uuid;

		private void Awake()
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			rig = ((Component)this).GetComponent<VRRig>();
			uuid = count++;
			leftHand = SetupHand("L");
			rightHand = SetupHand("R");
			dr = new GameObject($"{uuid} (Debug Ray)").AddComponent<DebugRay>();
		}

		private void OnDestroy()
		{
			DebugRay debugRay = dr;
			if (debugRay != null)
			{
				((Component)debugRay).gameObject?.Obliterate();
			}
			Transform obj = leftHand;
			if (obj != null)
			{
				((Component)(object)((Component)obj).GetComponent<Rigidbody>())?.Obliterate();
			}
			Transform obj2 = rightHand;
			if (obj2 != null)
			{
				((Component)(object)((Component)obj2).GetComponent<Rigidbody>())?.Obliterate();
			}
		}

		public Transform SetupHand(string hand)
		{
			Transform obj = ((Component)this).transform.Find(string.Format("/rig/hand.{0}/palm.01.{0}", hand).Substring(1));
			((Component)obj).gameObject.AddComponent<Rigidbody>().isKinematic = true;
			return obj;
		}

		public bool IsGripping()
		{
			grippingRight = ((VRMap)rig.rightMiddle).calcT > 0.5f;
			grippingLeft = ((VRMap)rig.leftMiddle).calcT > 0.5f;
			if (!grippingRight)
			{
				return grippingLeft;
			}
			return true;
		}

		public bool GrabbingMe()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!grippingRight && !grippingLeft)
				{
					return false;
				}
				Transform val = (controllingHand = (grippingRight ? rightHand : leftHand));
				if (!Object.op_Implicit((Object)(object)val))
				{
					return false;
				}
				controllingBody = ((val != null) ? ((Component)val).GetComponent<Rigidbody>() : null);
				if (!Object.op_Implicit((Object)(object)controllingBody))
				{
					return false;
				}
				SphereCollider gbCollider = Instance.gbCollider;
				float num = 0.05f * GTPlayer.Instance.scale;
				Collider[] array = Physics.OverlapSphere(val.position, num);
				for (int i = 0; i < array.Length; i++)
				{
					if ((Object)(object)array[i] == (Object)(object)gbCollider)
					{
						return true;
					}
				}
			}
			catch (Exception e)
			{
				Logging.Exception(e);
			}
			return false;
		}
	}

	public static readonly string DisplayName = "Grab";

	public static Grab Instance;

	private readonly List<GBMarker> markers = new List<GBMarker>();

	public SphereCollider gbCollider;

	private GBMarker grabber;

	private Joint joint;

	private void Awake()
	{
		Instance = this;
	}

	private void FixedUpdate()
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (Time.frameCount % 300 == 0)
		{
			DistributeGrabbyThings();
		}
		if (!Object.op_Implicit((Object)(object)grabber))
		{
			TryGetGrabber();
		}
		if (Object.op_Implicit((Object)(object)grabber))
		{
			Rigidbody attachedRigidbody = ((Collider)GTPlayer.Instance.bodyCollider).attachedRigidbody;
			if (!grabber.IsGripping())
			{
				grabber = null;
				attachedRigidbody.velocity = GTPlayer.Instance.bodyVelocityTracker.GetAverageVelocity(true, 0.15f, false) * 4.6f;
			}
			else
			{
				Vector3 val = grabber.controllingHand.position - ((Component)GTPlayer.Instance.bodyCollider).transform.position;
				attachedRigidbody.AddForce(val * 30f, (ForceMode)1);
				attachedRigidbody.velocity = Vector3.Lerp(attachedRigidbody.velocity, Vector3.zero, 0.1f);
			}
		}
	}

	protected override void OnEnable()
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		if (!MenuController.Instance.Built)
		{
			return;
		}
		base.OnEnable();
		try
		{
			ReloadConfiguration();
			GameObject val = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("TK Hitbox"));
			((Object)val).name = "Grate GB Hitbox";
			val.transform.SetParent(((Component)GTPlayer.Instance.bodyCollider).transform, false);
			val.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			val.layer = GrateInteractor.InteractionLayer;
			gbCollider = val.GetComponent<SphereCollider>();
			((Collider)gbCollider).isTrigger = true;
			DistributeGrabbyThings();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void TryGetGrabber()
	{
		foreach (GBMarker marker in markers)
		{
			try
			{
				if (Object.op_Implicit((Object)(object)marker) && marker.IsGripping() && marker.GrabbingMe())
				{
					grabber = marker;
					break;
				}
			}
			catch (Exception e)
			{
				Logging.Exception(e);
			}
		}
	}

	private void DistributeGrabbyThings()
	{
		foreach (VRRig vrrig in ((GorillaParent)GorillaParent.instance).vrrigs)
		{
			try
			{
				if (!vrrig.OwningNetPlayer.IsLocal && !Object.op_Implicit((Object)(object)((Component)vrrig).gameObject.GetComponent<GBMarker>()))
				{
					markers.Add(((Component)vrrig).gameObject.AddComponent<GBMarker>());
				}
			}
			catch (Exception e)
			{
				Logging.Exception(e);
			}
		}
	}

	protected override void Cleanup()
	{
		foreach (GBMarker marker in markers)
		{
			((Component)(object)marker)?.Obliterate();
		}
		SphereCollider obj = gbCollider;
		if (obj != null)
		{
			((Component)obj).gameObject?.Obliterate();
		}
		((Component)(object)joint)?.Obliterate();
		grabber = null;
		markers.Clear();
		gbCollider = null;
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Allows players to grab you!";
	}
}
