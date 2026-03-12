using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Networking;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Multiplayer;

public class Boxing : GrateModule
{
	[CompilerGenerated]
	private sealed class _003CDelGloves_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Boxing _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CDelGloves_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			int num = _003C_003E1__state;
			Boxing boxing = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				foreach (BoxingGlove glove in boxing.gloves)
				{
					((Component)(object)glove).Obliterate();
				}
				boxing.gloves.Clear();
				_003C_003E2__current = (object)new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public static readonly string DisplayName = "Boxing";

	public static ConfigEntry<int> PunchForce;

	public static ConfigEntry<bool> BuffMonke;

	private readonly List<VRRig> glovedRigs = new List<VRRig>();

	private readonly List<BoxingGlove> gloves = new List<BoxingGlove>();

	public float forceMultiplier = 5000f;

	private float lastPunch;

	private Collider punchCollider;

	private void FixedUpdate()
	{
	}

	protected override void OnEnable()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		if (!MenuController.Instance.Built)
		{
			return;
		}
		base.OnEnable();
		try
		{
			ReloadConfiguration();
			GameObject val = GameObject.CreatePrimitive((PrimitiveType)1);
			((Object)val).name = "GratePunchDetector";
			val.transform.SetParent(((Component)GTPlayer.Instance.bodyCollider).transform, false);
			val.layer = GrateInteractor.InteractionLayer;
			((Renderer)val.GetComponent<MeshRenderer>()).enabled = false;
			punchCollider = val.GetComponent<Collider>();
			punchCollider.isTrigger = true;
			((Component)punchCollider).transform.localScale = new Vector3(0.5f, 0.35f, 0.5f);
			Transform transform = ((Component)punchCollider).transform;
			transform.localPosition += new Vector3(0f, 0.3f, 0f);
			CollisionObserver collisionObserver = val.AddComponent<CollisionObserver>();
			collisionObserver.OnTriggerEntered = (Action<GameObject, Collider>)Delegate.Combine(collisionObserver.OnTriggerEntered, (Action<GameObject, Collider>)delegate(GameObject obj, Collider collider)
			{
				BoxingGlove componentInParent = ((Component)collider).GetComponentInParent<BoxingGlove>();
				if (componentInParent != null)
				{
					DoPunch(componentInParent);
				}
			});
			NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
			instance.OnPlayerJoined = (Action<NetPlayer>)Delegate.Combine(instance.OnPlayerJoined, new Action<NetPlayer>(OnPlayerJoined));
			CreateGloves();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	protected override void OnDisable()
	{
		BoxingGlove[] array = Resources.FindObjectsOfTypeAll<BoxingGlove>();
		foreach (BoxingGlove boxingGlove in array)
		{
			if (gloves.Contains(boxingGlove))
			{
				gloves.Remove(boxingGlove);
			}
			((Component)boxingGlove).gameObject.Obliterate();
		}
		base.OnDisable();
	}

	protected override void Cleanup()
	{
		Collider obj = punchCollider;
		if (obj != null)
		{
			((Component)obj).gameObject?.Obliterate();
		}
		glovedRigs.Clear();
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		if (instance != null)
		{
			instance.OnPlayerJoined = (Action<NetPlayer>)Delegate.Remove(instance.OnPlayerJoined, new Action<NetPlayer>(OnPlayerJoined));
		}
		((MonoBehaviour)this).StartCoroutine(DelGloves());
	}

	[IteratorStateMachine(typeof(_003CDelGloves_003Ed__12))]
	public IEnumerator DelGloves()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CDelGloves_003Ed__12(0)
		{
			_003C_003E4__this = this
		};
	}

	private void OnPlayerJoined(NetPlayer player)
	{
		GiveGlovesTo(player.Rig());
	}

	private void CreateGloves()
	{
		foreach (VRRig vrrig in ((GorillaParent)GorillaParent.instance).vrrigs)
		{
			try
			{
				if ((Object)(object)vrrig != (Object)(object)GorillaTagger.Instance.offlineVRRig && (Object)(object)vrrig != (Object)(object)GorillaTagger.Instance.myVRRig && !glovedRigs.Contains(vrrig) && vrrig.OwningNetPlayer != GorillaTagger.Instance.myVRRig.Owner)
				{
					GiveGlovesTo(vrrig);
				}
			}
			catch (Exception e)
			{
				Logging.Exception(e);
			}
		}
	}

	private void GiveGlovesTo(VRRig rig)
	{
		glovedRigs.Add(rig);
		BoxingGlove boxingGlove = CreateGlove(rig.leftHandTransform);
		boxingGlove.rig = rig;
		gloves.Add(boxingGlove);
		BoxingGlove boxingGlove2 = CreateGlove(rig.rightHandTransform, isLeft: false);
		boxingGlove2.rig = rig;
		gloves.Add(boxingGlove2);
		Logging.Debug("Gave gloves to", rig.OwningNetPlayer.NickName);
	}

	private BoxingGlove CreateGlove(Transform parent, bool isLeft = true)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("Boxing Glove"));
		string text = (isLeft ? "Left" : "Right");
		((Object)val).name = "Boxing Glove (" + text + ")";
		val.transform.SetParent(parent, false);
		float num = (isLeft ? 1 : (-1));
		val.transform.localScale = new Vector3(num, 1f, 1f);
		val.layer = GrateInteractor.InteractionLayer;
		foreach (Transform item in val.transform)
		{
			((Component)item).gameObject.layer = GrateInteractor.InteractionLayer;
		}
		return val.AddComponent<BoxingGlove>();
	}

	private void DoPunch(BoxingGlove glove)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		if (!(Time.time - lastPunch < 1f))
		{
			Vector3 val = glove.velocity.linearVelocity;
			if (!(((Vector3)(ref val)).magnitude < 0.5f * GTPlayer.Instance.scale))
			{
				((Vector3)(ref val)).Normalize();
				val *= forceMultiplier * (float)Buffness();
				Rigidbody attachedRigidbody = ((Collider)GTPlayer.Instance.bodyCollider).attachedRigidbody;
				attachedRigidbody.velocity += val;
				lastPunch = Time.time;
				GestureTracker.Instance.HapticPulse(isLeft: false);
				GestureTracker.Instance.HapticPulse(isLeft: true);
				glove.punchSound.pitch = Random.Range(0.8f, 1.2f);
				glove.punchSound.Play();
			}
		}
	}

	public int Buffness()
	{
		if (BuffMonke.Value)
		{
			return 100;
		}
		return 1;
	}

	protected override void ReloadConfiguration()
	{
		forceMultiplier = PunchForce.Value * 5;
	}

	public static void BindConfigEntries()
	{
		Logging.Debug("Binding", DisplayName, "to config");
		PunchForce = Plugin.ConfigFile.Bind<int>(DisplayName, "punch force", 5, "How much force will be applied to you when you get punched");
		BuffMonke = Plugin.ConfigFile.Bind<bool>(DisplayName, "Buff monke", false, "WEEEEEEEEEEEE");
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Effect: Other players can punch you around.";
	}
}
