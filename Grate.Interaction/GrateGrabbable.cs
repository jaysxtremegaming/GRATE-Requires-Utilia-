using GorillaLocomotion;
using Grate.Gestures;
using UnityEngine;

namespace Grate.Interaction;

public class GrateGrabbable : GrateInteractable
{
	private readonly Vector3 mirrorScale = new Vector3(-1f, 1f, 1f);

	private Vector3 _localPos;

	private bool kinematicCache;

	public Vector3 LocalRotation = Vector3.zero;

	public float throwForceMultiplier = 1f;

	public bool throwOnDetach;

	private GorillaVelocityEstimator velEstimator;

	public Vector3 LocalPosition
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _localPos;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			_localPos = value;
			MirroredLocalPosition = Vector3.Scale(value, mirrorScale);
		}
	}

	public Vector3 MirroredLocalPosition { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		GestureTracker instance = GestureTracker.Instance;
		validSelectors = new GrateInteractor[2] { instance.leftPalmInteractor, instance.rightPalmInteractor };
		velEstimator = ((Component)this).gameObject.AddComponent<GorillaVelocityEstimator>();
	}

	public override void OnSelect(GrateInteractor interactor)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Rigidbody component = ((Component)this).GetComponent<Rigidbody>();
		if (component != null)
		{
			kinematicCache = component.isKinematic;
			component.isKinematic = true;
		}
		((Component)this).transform.SetParent(((Component)interactor).transform);
		if (interactor.IsLeft)
		{
			((Component)this).transform.localPosition = LocalPosition;
		}
		else
		{
			((Component)this).transform.localPosition = MirroredLocalPosition;
		}
		((Component)this).transform.localRotation = Quaternion.Euler(LocalRotation);
		base.OnSelect(interactor);
	}

	public override void OnDeselect(GrateInteractor interactor)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.SetParent((Transform)null);
		Rigidbody component = ((Component)this).GetComponent<Rigidbody>();
		if (component != null)
		{
			if (throwOnDetach)
			{
				component.isKinematic = false;
				component.useGravity = true;
				component.velocity = ((Component)GTPlayer.Instance).GetComponent<Rigidbody>().velocity + velEstimator.linearVelocity * throwForceMultiplier;
				component.velocity *= 1f / GTPlayer.Instance.scale;
				component.angularVelocity = velEstimator.angularVelocity;
			}
			else
			{
				component.isKinematic = kinematicCache;
			}
		}
		base.OnDeselect(interactor);
	}
}
