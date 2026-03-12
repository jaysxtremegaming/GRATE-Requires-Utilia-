using System;
using System.Collections.Generic;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Multiplayer;

public class Telekinesis : GrateModule
{
	public class TKMarker : MonoBehaviour
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
			grippingRight = ((VRMap)rig.rightIndex).calcT < 0.5f && ((VRMap)rig.rightMiddle).calcT > 0.5f;
			grippingLeft = ((VRMap)rig.leftIndex).calcT < 0.5f && ((VRMap)rig.leftMiddle).calcT > 0.5f;
			if (!grippingRight)
			{
				return grippingLeft;
			}
			return true;
		}

		public bool PointingAtMe()
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
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
				Ray val2 = default(Ray);
				((Ray)(ref val2))._002Ector(val.position, val.up);
				Logging.Debug("DOING THE THING WITH THE COLLIDER");
				SphereCollider tkCollider = Instance.tkCollider;
				RaycastHit val3 = default(RaycastHit);
				Physics.SphereCast(val2, 0.2f * GTPlayer.Instance.scale, ref val3, (float)((Component)tkCollider).gameObject.layer);
				return (Object)(object)((RaycastHit)(ref val3)).collider == (Object)(object)tkCollider;
			}
			catch (Exception e)
			{
				Logging.Exception(e);
			}
			return false;
		}
	}

	public static readonly string DisplayName = "Telekinesis";

	public static Telekinesis Instance;

	private readonly List<TKMarker> markers = new List<TKMarker>();

	private Joint joint;

	private ParticleSystem playerParticles;

	private ParticleSystem sithlordHandParticles;

	private AudioSource sfx;

	private TKMarker sithLord;

	public SphereCollider tkCollider;

	private void Awake()
	{
		Instance = this;
	}

	private void FixedUpdate()
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		if (Time.frameCount % 300 == 0)
		{
			DistributeMidichlorians();
		}
		if (!Object.op_Implicit((Object)(object)sithLord))
		{
			TryGetSithLord();
		}
		if (Object.op_Implicit((Object)(object)sithLord))
		{
			Rigidbody attachedRigidbody = ((Collider)GTPlayer.Instance.bodyCollider).attachedRigidbody;
			if (!sithLord.IsGripping())
			{
				sithLord = null;
				sfx.Stop();
				sithlordHandParticles.Stop();
				sithlordHandParticles.Clear();
				playerParticles.Stop();
				playerParticles.Clear();
				attachedRigidbody.velocity = GTPlayer.Instance.bodyVelocityTracker.GetAverageVelocity(true, 0.15f, false) * 2f;
			}
			else
			{
				Vector3 val = sithLord.controllingHand.position + sithLord.controllingHand.up * 3f * sithLord.rig.scaleFactor - ((Component)GTPlayer.Instance.bodyCollider).transform.position;
				attachedRigidbody.AddForce(val * 10f, (ForceMode)1);
				_ = ((Vector3)(ref val)).magnitude;
				attachedRigidbody.velocity = Vector3.Lerp(attachedRigidbody.velocity, Vector3.zero, 0.1f);
			}
		}
	}

	protected override void OnEnable()
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		if (!MenuController.Instance.Built)
		{
			return;
		}
		base.OnEnable();
		try
		{
			ReloadConfiguration();
			GameObject obj = Plugin.AssetBundle.LoadAsset<GameObject>("TK Hitbox");
			GameObject val = Object.Instantiate<GameObject>(obj);
			((Object)val).name = "Grate TK Hitbox";
			val.transform.SetParent(((Component)GTPlayer.Instance.bodyCollider).transform, false);
			val.layer = GrateInteractor.InteractionLayer;
			tkCollider = val.GetComponent<SphereCollider>();
			((Collider)tkCollider).isTrigger = true;
			playerParticles = val.GetComponent<ParticleSystem>();
			playerParticles.Stop();
			playerParticles.Clear();
			sfx = val.GetComponent<AudioSource>();
			GameObject val2 = Object.Instantiate<GameObject>(obj);
			((Object)val2).name = "Grate Sithlord Particles";
			val2.transform.SetParent(((Component)GTPlayer.Instance.bodyCollider).transform, false);
			val2.layer = GrateInteractor.InteractionLayer;
			sithlordHandParticles = val2.GetComponent<ParticleSystem>();
			ShapeModule shape = sithlordHandParticles.shape;
			((ShapeModule)(ref shape)).radius = 0.2f;
			((ShapeModule)(ref shape)).position = Vector3.zero;
			Object.Destroy((Object)(object)val2.GetComponent<SphereCollider>());
			DistributeMidichlorians();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void TryGetSithLord()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		foreach (TKMarker marker in markers)
		{
			try
			{
				if (Object.op_Implicit((Object)(object)marker) && marker.IsGripping() && marker.PointingAtMe())
				{
					sithLord = marker;
					playerParticles.Play();
					((Component)sithlordHandParticles).transform.SetParent(marker.controllingHand);
					((Component)sithlordHandParticles).transform.localPosition = Vector3.zero;
					sithlordHandParticles.Play();
					sfx.Play();
					break;
				}
			}
			catch (Exception e)
			{
				Logging.Exception(e);
			}
		}
	}

	private void DistributeMidichlorians()
	{
		foreach (VRRig vrrig in ((GorillaParent)GorillaParent.instance).vrrigs)
		{
			try
			{
				if (!vrrig.OwningNetPlayer.IsLocal && !Object.op_Implicit((Object)(object)((Component)vrrig).gameObject.GetComponent<TKMarker>()))
				{
					markers.Add(((Component)vrrig).gameObject.AddComponent<TKMarker>());
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
		foreach (TKMarker marker in markers)
		{
			((Component)(object)marker)?.Obliterate();
		}
		SphereCollider obj = tkCollider;
		if (obj != null)
		{
			((Component)obj).gameObject?.Obliterate();
		}
		ParticleSystem obj2 = sithlordHandParticles;
		if (obj2 != null)
		{
			((Component)obj2).gameObject?.Obliterate();
		}
		((Component)(object)joint)?.Obliterate();
		sithLord = null;
		markers.Clear();
		tkCollider = null;
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Effect: If another player points their index finger at you, they can pick you up with telekinesis.";
	}
}
