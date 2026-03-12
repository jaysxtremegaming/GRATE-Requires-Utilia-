using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.Gestures;
using Grate.Patches;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Multiplayer;

public class Fireflies : GrateModule
{
	[CompilerGenerated]
	private sealed class _003CReleaseFireflies_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Fireflies _003C_003E4__this;

		private List<Firefly>.Enumerator _003C_003E7__wrap1;

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
		public _003CReleaseFireflies_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<Firefly>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			try
			{
				int num = _003C_003E1__state;
				Fireflies fireflies = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					fireflies.charging = false;
					foreach (Firefly firefly in Fireflies.fireflies)
					{
						firefly.hand = null;
					}
					_003C_003E7__wrap1 = Fireflies.fireflies.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003E7__wrap1.Current.Launch();
					Sounds.Play(Sounds.Sound.BeeSqueeze, 0.1f, (Object)(object)fireflies.hand == (Object)(object)((Component)GestureTracker.Instance.leftPalmInteractor).transform);
					_003C_003E2__current = (object)new WaitForSeconds(0.05f);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Firefly>.Enumerator);
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpawnFireflies_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool isLeft;

		public Transform hand;

		private List<VRRig> _003Crigs_003E5__2;

		private int _003Ccount_003E5__3;

		private int _003Ci_003E5__4;

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
		public _003CSpawnFireflies_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Crigs_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Crigs_003E5__2 = ((GorillaParent)GorillaParent.instance).vrrigs;
				_003Ccount_003E5__3 = _003Crigs_003E5__2.Count;
				Sounds.Play(Sounds.Sound.BeeSqueeze, 0.1f, isLeft);
				_003Ci_003E5__4 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__4++;
				break;
			}
			if (_003Ci_003E5__4 < _003Ccount_003E5__3)
			{
				VRRig val = _003Crigs_003E5__2[_003Ci_003E5__4];
				try
				{
					if ((Object)(object)val != (Object)null && !val.isOfflineVRRig)
					{
						Firefly orAddComponent = ((Component)val).gameObject.GetOrAddComponent<Firefly>();
						orAddComponent.Reset(val, hand);
						if (!fireflies.Contains(orAddComponent))
						{
							fireflies.Add(orAddComponent);
						}
					}
				}
				catch (Exception e)
				{
					Logging.Exception(e);
				}
				_003C_003E2__current = (object)new WaitForFixedUpdate();
				_003C_003E1__state = 1;
				return true;
			}
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

	public static readonly string DisplayName = "Fireflies";

	public static List<Firefly> fireflies = new List<Firefly>();

	public static Fireflies instance;

	private bool charging;

	private Transform hand;

	private void FixedUpdate()
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		if (!charging || !Object.op_Implicit((Object)(object)hand))
		{
			fireflies.RemoveAll((Firefly fly) => fly == null);
			return;
		}
		Vector3 val = default(Vector3);
		for (int i = 0; i < fireflies.Count; i++)
		{
			float num = (float)i * MathF.PI * 2f / (float)fireflies.Count + Time.time;
			float num2 = Mathf.Cos(num);
			float num3 = Mathf.Sin(num);
			((Vector3)(ref val))._002Ector(num2, num3, 0f);
			GameObject fly2 = fireflies[i].fly;
			fly2.transform.position = ((Component)hand).transform.TransformPoint(val * 2f);
			fly2.transform.localScale = Vector3.one * GTPlayer.Instance.scale;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		try
		{
			ReloadConfiguration();
			InputTracker<float>? leftTrigger = GestureTracker.Instance.leftTrigger;
			leftTrigger.OnPressed = (Action<InputTracker>)Delegate.Combine(leftTrigger.OnPressed, new Action<InputTracker>(OnTriggerPressed));
			InputTracker<float>? rightTrigger = GestureTracker.Instance.rightTrigger;
			rightTrigger.OnPressed = (Action<InputTracker>)Delegate.Combine(rightTrigger.OnPressed, new Action<InputTracker>(OnTriggerPressed));
			InputTracker<float>? leftTrigger2 = GestureTracker.Instance.leftTrigger;
			leftTrigger2.OnReleased = (Action<InputTracker>)Delegate.Combine(leftTrigger2.OnReleased, new Action<InputTracker>(OnTriggerReleased));
			InputTracker<float>? rightTrigger2 = GestureTracker.Instance.rightTrigger;
			rightTrigger2.OnReleased = (Action<InputTracker>)Delegate.Combine(rightTrigger2.OnReleased, new Action<InputTracker>(OnTriggerReleased));
			VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
			instance = this;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnTriggerPressed(InputTracker tracker)
	{
		((MonoBehaviour)this).StopAllCoroutines();
		bool flag = tracker == GestureTracker.Instance.leftTrigger;
		GrateInteractor grateInteractor = (flag ? GestureTracker.Instance.leftPalmInteractor : GestureTracker.Instance.rightPalmInteractor);
		hand = ((Component)grateInteractor).transform;
		((MonoBehaviour)this).StartCoroutine(SpawnFireflies(hand, flag));
		charging = true;
	}

	private void OnTriggerReleased(InputTracker tracker)
	{
		if ((tracker == GestureTracker.Instance.leftTrigger && (Object)(object)hand == (Object)(object)((Component)GestureTracker.Instance.leftPalmInteractor).transform) || (tracker == GestureTracker.Instance.rightTrigger && (Object)(object)hand == (Object)(object)((Component)GestureTracker.Instance.rightPalmInteractor).transform))
		{
			((MonoBehaviour)this).StartCoroutine(ReleaseFireflies());
		}
	}

	[IteratorStateMachine(typeof(_003CReleaseFireflies_003Ed__9))]
	private IEnumerator ReleaseFireflies()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CReleaseFireflies_003Ed__9(0)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CSpawnFireflies_003Ed__10))]
	private IEnumerator SpawnFireflies(Transform hand, bool isLeft)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpawnFireflies_003Ed__10(0)
		{
			hand = hand,
			isLeft = isLeft
		};
	}

	protected override void Cleanup()
	{
		InputTracker<float>? leftTrigger = GestureTracker.Instance.leftTrigger;
		leftTrigger.OnPressed = (Action<InputTracker>)Delegate.Remove(leftTrigger.OnPressed, new Action<InputTracker>(OnTriggerPressed));
		InputTracker<float>? rightTrigger = GestureTracker.Instance.rightTrigger;
		rightTrigger.OnPressed = (Action<InputTracker>)Delegate.Remove(rightTrigger.OnPressed, new Action<InputTracker>(OnTriggerPressed));
		InputTracker<float>? leftTrigger2 = GestureTracker.Instance.leftTrigger;
		leftTrigger2.OnReleased = (Action<InputTracker>)Delegate.Remove(leftTrigger2.OnReleased, new Action<InputTracker>(OnTriggerReleased));
		InputTracker<float>? rightTrigger2 = GestureTracker.Instance.rightTrigger;
		rightTrigger2.OnReleased = (Action<InputTracker>)Delegate.Remove(rightTrigger2.OnReleased, new Action<InputTracker>(OnTriggerReleased));
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Remove(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
		fireflies.Clear();
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		Firefly component = ((Component)rig).GetComponent<Firefly>();
		if ((Object)(object)component != (Object)null)
		{
			fireflies.Remove(component);
			((Component)(object)component).Obliterate();
		}
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Effect: Hold [Trigger] to summon fireflies that will follow each player upon release";
	}
}
