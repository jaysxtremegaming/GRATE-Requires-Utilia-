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
using Grate.Patches;
using Grate.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Grate.Modules;

public class Teleport : GrateModule
{
	[CompilerGenerated]
	private sealed class _003CGrowBananas_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Teleport _003C_003E4__this;

		private float _003CstartTime_003E5__2;

		private Transform _003CleftHand_003E5__3;

		private Transform _003CrightHand_003E5__4;

		private bool _003CplayedSound_003E5__5;

		private GTPlayer _003Cplayer_003E5__6;

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
		public _003CGrowBananas_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CleftHand_003E5__3 = null;
			_003CrightHand_003E5__4 = null;
			_003Cplayer_003E5__6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Expected O, but got Unknown
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Expected O, but got Unknown
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Expected O, but got Unknown
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			Teleport teleport = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				teleport.isTeleporting = true;
				((Component)teleport.teleportMarker).gameObject.SetActive(true);
				_003CstartTime_003E5__2 = Time.time;
				_003CleftHand_003E5__3 = ((Component)GestureTracker.Instance.leftPalmInteractor).transform;
				_003CrightHand_003E5__4 = ((Component)GestureTracker.Instance.rightPalmInteractor).transform;
				_003CplayedSound_003E5__5 = false;
				_003Cplayer_003E5__6 = GTPlayer.Instance;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			case 3:
				_003C_003E1__state = -1;
				break;
			}
			if (GestureTracker.Instance.isIlluminatiing)
			{
				((Component)teleport.window).transform.position = (_003CleftHand_003E5__3.position + _003CrightHand_003E5__4.position) / 2f;
				if (!teleport.TriangleInRange())
				{
					teleport.teleportMarker.position = Vector3.up * 100000f;
					_003CstartTime_003E5__2 = Time.time;
					_003C_003E2__current = (object)new WaitForEndOfFrame();
					_003C_003E1__state = 1;
					return true;
				}
				Vector3 pointerDirection = GestureTracker.Instance.headVectors.pointerDirection;
				RaycastHit val = default(RaycastHit);
				Physics.Raycast(new Ray(((Component)_003Cplayer_003E5__6.headCollider).transform.position, pointerDirection), ref val, float.PositiveInfinity, layerMask);
				if (!Object.op_Implicit((Object)(object)((RaycastHit)(ref val)).transform))
				{
					_003CstartTime_003E5__2 = Time.time;
					teleport.teleportMarker.position = Vector3.up * 100000f;
					if (_003CplayedSound_003E5__5)
					{
						GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(98, false, 0.1f);
						_003CplayedSound_003E5__5 = false;
					}
					_003C_003E2__current = (object)new WaitForEndOfFrame();
					_003C_003E1__state = 2;
					return true;
				}
				if (!_003CplayedSound_003E5__5)
				{
					GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Random.Range(40, 56), false, 0.1f);
					_003CplayedSound_003E5__5 = true;
				}
				float num2 = MathExtensions.Map(ChargeTime.Value, 0f, 10f, 0.25f, 1.5f);
				float num3 = Mathf.Lerp(0f, 1f, (Time.time - _003CstartTime_003E5__2) / num2);
				teleport.teleportMarker.position = ((RaycastHit)(ref val)).point - pointerDirection * _003Cplayer_003E5__6.scale;
				teleport.teleportMarker.localScale = Vector3.one * GTPlayer.Instance.scale * num3;
				if (!(num3 >= 1f))
				{
					_003C_003E2__current = (object)new WaitForFixedUpdate();
					_003C_003E1__state = 3;
					return true;
				}
				TeleportPatch.TeleportPlayer(teleport.teleportMarker.position, teleport.teleportMarker.rotation.y);
			}
			((Component)teleport.teleportMarker).gameObject.SetActive(false);
			teleport.isTeleporting = false;
			teleport.poly.renderer.enabled = false;
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

	public static readonly string DisplayName = "Teleport";

	public static readonly int layerMask = LayerMask.GetMask(new string[2] { "Default", "Gorilla Object" });

	public static ConfigEntry<int> ChargeTime;

	private bool isTeleporting;

	private DebugPoly poly;

	private Transform teleportMarker;

	private Transform window;

	private void FixedUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		teleportMarker.Rotate(Vector3.up, 90f * Time.fixedDeltaTime, (Space)0);
	}

	protected override void OnEnable()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		if (!MenuController.Instance.Built)
		{
			return;
		}
		base.OnEnable();
		try
		{
			teleportMarker = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("Checkpoint Banana")).transform;
			((Component)teleportMarker).gameObject.SetActive(false);
			window = new GameObject("Teleport Window").transform;
			poly = ((Component)window).gameObject.AddComponent<DebugPoly>();
			GestureTracker instance = GestureTracker.Instance;
			instance.OnIlluminati = (Action)Delegate.Combine(instance.OnIlluminati, new Action(OnIlluminati));
			Application.onBeforeRender += new UnityAction(RenderTriangle);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnIlluminati()
	{
		if (((Behaviour)this).enabled && !isTeleporting)
		{
			((MonoBehaviour)this).StartCoroutine(GrowBananas());
		}
	}

	[IteratorStateMachine(typeof(_003CGrowBananas_003Ed__10))]
	private IEnumerator GrowBananas()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGrowBananas_003Ed__10(0)
		{
			_003C_003E4__this = this
		};
	}

	private bool TriangleInRange()
	{
		return true;
	}

	private void RenderTriangle()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		if (GestureTracker.Instance.isIlluminatiing)
		{
			poly.renderer.enabled = true;
			GestureTracker instance = GestureTracker.Instance;
			Vector3 val = instance.leftThumbTransform.position - instance.leftThumbTransform.up * 0.03f + instance.leftThumbTransform.right * -0.02f;
			Vector3 val2 = instance.rightThumbTransform.position - instance.rightThumbTransform.up * 0.03f + instance.rightThumbTransform.right * 0.02f;
			Vector3 val3 = (instance.rightPointerTransform.position + instance.leftPointerTransform.position) / 2f;
			val = ((Component)poly).transform.InverseTransformPoint(val);
			val2 = ((Component)poly).transform.InverseTransformPoint(val2);
			val3 = ((Component)poly).transform.InverseTransformPoint(val3);
			poly.vertices = (Vector3[])(object)new Vector3[3] { val, val2, val3 };
		}
	}

	protected override void Cleanup()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (MenuController.Instance.Built)
		{
			Application.onBeforeRender -= new UnityAction(RenderTriangle);
			Transform obj = teleportMarker;
			if (obj != null)
			{
				((Component)obj).gameObject?.Obliterate();
			}
			Transform obj2 = window;
			if (obj2 != null)
			{
				((Component)obj2).gameObject?.Obliterate();
			}
			isTeleporting = false;
			if (GestureTracker.Instance != null)
			{
				GestureTracker instance = GestureTracker.Instance;
				instance.OnIlluminati = (Action)Delegate.Remove(instance.OnIlluminati, new Action(OnIlluminati));
			}
		}
	}

	public static void BindConfigEntries()
	{
		ChargeTime = Plugin.ConfigFile.Bind<int>(DisplayName, "charge time", 5, "How long it takes to charge the teleport");
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "To teleport, make a triangle with your thumbs and index fingers andlook at where you want to teleport.";
	}
}
