using System;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.Gestures;
using Grate.Interaction;
using UnityEngine;

namespace Grate.Modules.Movement;

public class Rocket : GrateGrabbable
{
	public AudioSource exhaustSound;

	private GestureTracker gt;

	private bool isLeft;

	public float power = 5f;

	public float volume = 0.2f;

	private Rigidbody rb;

	public Vector3 force { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		rb = ((Component)this).GetComponent<Rigidbody>();
		exhaustSound = ((Component)this).GetComponent<AudioSource>();
		exhaustSound.Stop();
	}

	private void FixedUpdate()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		GTPlayer instance = GTPlayer.Instance;
		force = ((Component)this).transform.forward * power * Time.fixedDeltaTime * GTPlayer.Instance.scale;
		if (base.Selected)
		{
			instance.AddForce(force);
		}
		else
		{
			Rigidbody obj = rb;
			obj.velocity += force * 10f;
			force = Vector3.zero;
			((Component)this).transform.Rotate(Random.insideUnitSphere);
		}
		exhaustSound.volume = Mathf.Lerp(0.5f, 0f, Vector3.Distance(((Component)instance.headCollider).transform.position, ((Component)this).transform.position) / 20f) * volume;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Object.op_Implicit((Object)(object)gt))
		{
			InputTracker<float>? leftGrip = gt.leftGrip;
			leftGrip.OnPressed = (Action<InputTracker>)Delegate.Remove(leftGrip.OnPressed, new Action<InputTracker>(Attach));
			InputTracker<float>? rightGrip = gt.rightGrip;
			rightGrip.OnPressed = (Action<InputTracker>)Delegate.Remove(rightGrip.OnPressed, new Action<InputTracker>(Attach));
		}
	}

	public Rocket Init(bool isLeft)
	{
		this.isLeft = isLeft;
		gt = GestureTracker.Instance;
		if (isLeft)
		{
			InputTracker<float>? leftGrip = gt.leftGrip;
			leftGrip.OnPressed = (Action<InputTracker>)Delegate.Combine(leftGrip.OnPressed, new Action<InputTracker>(Attach));
		}
		else
		{
			InputTracker<float>? rightGrip = gt.rightGrip;
			rightGrip.OnPressed = (Action<InputTracker>)Delegate.Combine(rightGrip.OnPressed, new Action<InputTracker>(Attach));
		}
		return this;
	}

	private void Attach(InputTracker _)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		GrateInteractor grateInteractor = (isLeft ? gt.leftPalmInteractor : gt.rightPalmInteractor);
		if (CanBeSelected(grateInteractor))
		{
			((Component)this).transform.parent = null;
			((Component)this).transform.localScale = Vector3.one * GTPlayer.Instance.scale;
			grateInteractor.Select(this);
			exhaustSound.Stop();
			exhaustSound.time = Random.Range(0f, exhaustSound.clip.length);
			exhaustSound.Play();
		}
	}

	public override void OnDeselect(GrateInteractor interactor)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		base.OnDeselect(interactor);
		rb.velocity = ((Component)GTPlayer.Instance).GetComponent<Rigidbody>().velocity;
	}

	public void SetupInteraction()
	{
		throwOnDetach = true;
		((Component)this).gameObject.layer = GrateInteractor.InteractionLayer;
	}
}
