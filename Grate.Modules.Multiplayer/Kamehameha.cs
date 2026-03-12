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
using UnityEngine.XR;

namespace Grate.Modules.Multiplayer;

public class Kamehameha : GrateModule
{
	[CompilerGenerated]
	private sealed class _003CGrowBananas_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Kamehameha _003C_003E4__this;

		private Transform _003CleftHand_003E5__2;

		private Transform _003CrightHand_003E5__3;

		private float _003Cdiameter_003E5__4;

		private float _003ClastHaptic_003E5__5;

		private float _003ChapticDuration_003E5__6;

		private float _003CchargeTime_003E5__7;

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
		public _003CGrowBananas_003Ed__19(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CleftHand_003E5__2 = null;
			_003CrightHand_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Expected O, but got Unknown
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Expected O, but got Unknown
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_042f: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0469: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Expected O, but got Unknown
			int num = _003C_003E1__state;
			Kamehameha kamehameha = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				kamehameha.isCharging = true;
				((Component)orb).gameObject.SetActive(true);
				kamehameha.orbBody.isKinematic = true;
				kamehameha.orbBody.velocity = Vector3.zero;
				GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Random.Range(40, 56), false, 0.1f);
				_003CleftHand_003E5__2 = ((Component)GestureTracker.Instance.leftPalmInteractor).transform;
				_003CrightHand_003E5__3 = ((Component)GestureTracker.Instance.rightPalmInteractor).transform;
				_003Cdiameter_003E5__4 = 0f;
				_003ClastHaptic_003E5__5 = Time.time;
				_003ChapticDuration_003E5__6 = 0.1f;
				goto IL_021e;
			case 1:
				_003C_003E1__state = -1;
				goto IL_021e;
			case 2:
				_003C_003E1__state = -1;
				goto IL_027b;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_021e:
				if (GestureTracker.Instance.PalmsFacingEachOther())
				{
					float num2 = GTPlayer.Instance.scale / 2f;
					if (Time.time - _003ClastHaptic_003E5__5 > _003ChapticDuration_003E5__6)
					{
						float num3 = Mathf.SmoothStep(0f, 1f, _003Cdiameter_003E5__4 / maxOrbSize * num2);
						GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Random.Range(40, 48), false, num3 / 10f);
						((InputDevice)(ref GestureTracker.Instance.leftController)).SendHapticImpulse(0u, num3, _003ChapticDuration_003E5__6);
						((InputDevice)(ref GestureTracker.Instance.rightController)).SendHapticImpulse(0u, num3, _003ChapticDuration_003E5__6);
						_003ClastHaptic_003E5__5 = Time.time;
					}
					_003Cdiameter_003E5__4 = Vector3.Distance(_003CleftHand_003E5__2.position, _003CrightHand_003E5__3.position);
					_003Cdiameter_003E5__4 = Mathf.Clamp(_003Cdiameter_003E5__4, 0f, maxOrbSize * num2);
					((Component)orb).transform.position = (_003CleftHand_003E5__2.position + _003CrightHand_003E5__3.position) / 2f;
					((Component)orb).transform.localScale = Vector3.one * _003Cdiameter_003E5__4 * num2;
					_003C_003E2__current = (object)new WaitForEndOfFrame();
					_003C_003E1__state = 1;
					return true;
				}
				kamehameha.isCharging = false;
				Logging.Debug("Charging is done");
				_003CchargeTime_003E5__7 = Time.time;
				goto IL_027b;
				IL_027b:
				if (Time.time - _003CchargeTime_003E5__7 < 1f && !GestureTracker.Instance.PalmsFacingSameWay())
				{
					_003C_003E2__current = (object)new WaitForEndOfFrame();
					_003C_003E1__state = 2;
					return true;
				}
				((Component)bananaLine).gameObject.SetActive(true);
				kamehameha.isFiring = true;
				break;
			}
			if (GestureTracker.Instance.PalmsFacingSameWay() && kamehameha.HandProximity() < 0.6f)
			{
				if (Time.time - _003ClastHaptic_003E5__5 > _003ChapticDuration_003E5__6)
				{
					float num4 = 1f;
					GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Random.Range(40, 56), false, num4 / 10f);
					((InputDevice)(ref GestureTracker.Instance.leftController)).SendHapticImpulse(0u, num4, _003ChapticDuration_003E5__6);
					((InputDevice)(ref GestureTracker.Instance.rightController)).SendHapticImpulse(0u, num4, _003ChapticDuration_003E5__6);
					_003ClastHaptic_003E5__5 = Time.time;
				}
				float num5 = GTPlayer.Instance.scale / 2f;
				_003Cdiameter_003E5__4 = Vector3.Distance(_003CleftHand_003E5__2.position, _003CrightHand_003E5__3.position);
				_003Cdiameter_003E5__4 = Mathf.Clamp(_003Cdiameter_003E5__4, 0f, maxOrbSize * num5 * 2f);
				bananaLine.startWidth = _003Cdiameter_003E5__4 * num5;
				bananaLine.endWidth = _003Cdiameter_003E5__4 * num5;
				Vector3 val = (GestureTracker.Instance.leftHandVectors.palmNormal + GestureTracker.Instance.rightHandVectors.palmNormal) / 2f;
				Vector3 val2 = (_003CleftHand_003E5__2.position + _003CrightHand_003E5__3.position) / 2f + val * 0.1f;
				orb.position = val2;
				((Component)orb).transform.localScale = Vector3.one * _003Cdiameter_003E5__4 * num5;
				bananaLine.SetPosition(0, val2);
				bananaLine.SetPosition(1, val2 + val * 100f);
				GTPlayer.Instance.AddForce(val * -40f * _003Cdiameter_003E5__4 * Time.fixedDeltaTime);
				_003C_003E2__current = (object)new WaitForEndOfFrame();
				_003C_003E1__state = 3;
				return true;
			}
			Logging.Debug("Firing is done");
			((Component)orb).gameObject.SetActive(false);
			((Component)bananaLine).gameObject.SetActive(false);
			kamehameha.isFiring = false;
			return false;
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

	public static readonly string DisplayName = "Kamehameha";

	public static Transform orb;

	public static LineRenderer bananaLine;

	public static readonly float maxOrbSize = 0.4f;

	public static readonly string KamehamehaKey = "KameState";

	public static readonly string KamehamehaColorKey = "KameColor";

	public static ConfigEntry<string> c_khameColor;

	public static ConfigEntry<bool> c_Networked;

	public static ConfigEntry<bool> networked;

	private ParticleSystem Effects;

	public bool isCharging;

	public bool isFiring;

	private Color khameColor;

	private Rigidbody orbBody;

	private string state;

	protected override void Start()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		bananaLine = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("Banana Line")).GetComponent<LineRenderer>();
		((Renderer)bananaLine).material = Plugin.AssetBundle.LoadAsset<Material>("Laser Sight Material");
		((Component)bananaLine).gameObject.SetActive(false);
		orb = GameObject.CreatePrimitive((PrimitiveType)0).transform;
		orb.localScale = new Vector3(maxOrbSize, maxOrbSize, maxOrbSize);
		((Component)orb).gameObject.GetComponent<Collider>().isTrigger = true;
		orbBody = ((Component)orb).gameObject.AddComponent<Rigidbody>();
		orbBody.isKinematic = true;
		orbBody.useGravity = false;
		((Component)orb).gameObject.layer = GrateInteractor.InteractionLayer;
		((Component)orb).gameObject.GetComponent<Renderer>().material = ((Renderer)bananaLine).material;
		Effects = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("Kahme PSystem")).GetComponent<ParticleSystem>();
		((Component)Effects).transform.SetParent(((Component)orbBody).transform, false);
		((Component)Effects).transform.localPosition = Vector3.zero;
		NetworkPropertyHandler.Instance?.ChangeProperty(KamehamehaKey, "None");
		((Component)orb).gameObject.SetActive(false);
		ReloadConfiguration();
	}

	private void FixedUpdate()
	{
		if (isCharging && !isFiring)
		{
			state = "Charging";
		}
		if (isFiring && !isCharging)
		{
			state = "FIRE!";
		}
		if (!isCharging && !isFiring)
		{
			state = "None";
		}
		if (NetworkSystem.Instance.LocalPlayer.GetProperty<string>(KamehamehaKey) != state)
		{
			NetworkPropertyHandler.Instance.ChangeProperty(KamehamehaKey, state);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (MenuController.Instance.Built)
		{
			GestureTracker instance = GestureTracker.Instance;
			instance.OnKamehameha = (Action)Delegate.Combine(instance.OnKamehameha, new Action(OnKamehameha));
			ReloadConfiguration();
		}
	}

	private void OnKamehameha()
	{
		if (((Behaviour)this).enabled && !isCharging && !isFiring)
		{
			((MonoBehaviour)this).StartCoroutine(GrowBananas());
		}
	}

	[IteratorStateMachine(typeof(_003CGrowBananas_003Ed__19))]
	private IEnumerator GrowBananas()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGrowBananas_003Ed__19(0)
		{
			_003C_003E4__this = this
		};
	}

	private float HandProximity()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return Vector3.Distance(((Component)GestureTracker.Instance.leftPalmInteractor).transform.position, ((Component)GestureTracker.Instance.rightPalmInteractor).transform.position);
	}

	public static void BindConfigEntries()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		AcceptableValueList<string> val = new AcceptableValueList<string>(new string[11]
		{
			Color.red.ColorName(),
			Color.green.ColorName(),
			Color.blue.ColorName(),
			Color.yellow.ColorName(),
			Color.magenta.ColorName(),
			Color.cyan.ColorName(),
			Color.white.ColorName(),
			Color.black.ColorName(),
			Color.gray.ColorName(),
			Color.clear.ColorName(),
			"#5c3a93"
		});
		ConfigDescription val2 = new ConfigDescription("Color for your Ultimate Power!", (AcceptableValueBase)(object)val, Array.Empty<object>());
		c_khameColor = Plugin.ConfigFile.Bind<string>(DisplayName, "Color", Color.yellow.ColorName(), val2);
		c_Networked = Plugin.ConfigFile.Bind<bool>(DisplayName, "Network?", true, "Decide weather you want to see Other peoples power!");
	}

	protected override void ReloadConfiguration()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		khameColor = c_khameColor.Value.StringToColor();
		((Component)orb).GetComponent<Renderer>().material.color = khameColor;
		bananaLine.SetColors(khameColor, khameColor);
		((Component)Effects).GetComponent<Renderer>().material.color = khameColor;
		if (!c_Networked.Value)
		{
			NetworkedKaemeManager[] array = Resources.FindObjectsOfTypeAll<NetworkedKaemeManager>();
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy((Object)(object)array[i]);
			}
		}
		NetworkPropertyHandler.Instance.ChangeProperty(KamehamehaColorKey, khameColor.ColorName());
	}

	protected override void Cleanup()
	{
		GestureTracker instance = GestureTracker.Instance;
		instance.OnKamehameha = (Action)Delegate.Remove(instance.OnKamehameha, new Action(OnKamehameha));
		if ((Object)(object)orb != (Object)null)
		{
			Transform obj = orb;
			if (obj != null)
			{
				((Component)obj).gameObject.SetActive(false);
			}
			LineRenderer obj2 = bananaLine;
			if (obj2 != null)
			{
				((Component)obj2).gameObject.SetActive(false);
			}
		}
		state = "None";
		isCharging = false;
		isFiring = false;
		NetworkPropertyHandler.Instance.ChangeProperty(KamehamehaKey, state);
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Copy the Show!";
	}
}
