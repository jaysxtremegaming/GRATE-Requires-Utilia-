using GorillaLocomotion;
using Grate.Extensions;
using Grate.Gestures;
using Grate.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Grate.Modules.Movement;

public class BananaGun : GrateGrabbable
{
	public enum RopeType
	{
		ELASTIC,
		STATIC
	}

	private float baseLaserWidth;

	private float baseRopeWidth;

	private Vector3 hitPosition;

	public Transform holster;

	private bool isGrappling;

	private SpringJoint joint;

	private GameObject openModel;

	private GameObject closedModel;

	public float pullForce = 10f;

	public float steerForce = 5f;

	public float maxLength = 30f;

	private LineRenderer rope;

	private LineRenderer laser;

	public RopeType ropeType;

	protected override void Awake()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		base.Awake();
		base.LocalPosition = new Vector3(0.55f, 0f, 0.85f);
		openModel = ((Component)((Component)this).transform.Find("Banana Gun Open")).gameObject;
		closedModel = ((Component)((Component)this).transform.Find("Banana Gun Closed")).gameObject;
		rope = openModel.GetComponentInChildren<LineRenderer>();
		rope.useWorldSpace = false;
		baseRopeWidth = rope.startWidth;
		laser = closedModel.GetComponentInChildren<LineRenderer>();
		laser.useWorldSpace = false;
		baseLaserWidth = laser.startWidth;
	}

	private void FixedUpdate()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (base.Selected && !isGrappling && Activated)
		{
			StartSwing();
		}
		else if (isGrappling)
		{
			Rigidbody attachedRigidbody = ((Collider)GTPlayer.Instance.bodyCollider).attachedRigidbody;
			attachedRigidbody.velocity += ((Component)this).transform.forward * steerForce * Time.fixedDeltaTime * GTPlayer.Instance.scale;
		}
	}

	private void OnEnable()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		Application.onBeforeRender += new UnityAction(UpdateLineRenderer);
	}

	private void OnDisable()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		Application.onBeforeRender -= new UnityAction(UpdateLineRenderer);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		((Component)(object)joint)?.Obliterate();
	}

	public void Holster(Transform holster)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		Close();
		this.holster = holster;
		((Component)this).transform.SetParent(holster);
		((Component)this).transform.localPosition = Vector3.zero;
		((Component)this).transform.localRotation = Quaternion.identity;
		((Component)this).transform.localScale = Vector3.one;
		if (Object.op_Implicit((Object)(object)laser))
		{
			((Renderer)laser).enabled = false;
		}
	}

	public override void OnActivate(GrateInteractor interactor)
	{
		base.OnActivate(interactor);
		Activated = true;
	}

	public override void OnDeactivate(GrateInteractor interactor)
	{
		base.OnDeactivate(interactor);
		Activated = false;
		Close();
	}

	private void StartSwing()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit val = default(RaycastHit);
		Physics.SphereCast(new Ray(((Component)rope).transform.position, ((Component)this).transform.forward), 0.5f * GTPlayer.Instance.scale, ref val, maxLength, Teleport.layerMask);
		if (Object.op_Implicit((Object)(object)((RaycastHit)(ref val)).transform))
		{
			isGrappling = true;
			Open();
			rope.SetPosition(0, ((Component)rope).transform.position);
			rope.SetPosition(1, ((RaycastHit)(ref val)).point);
			hitPosition = ((RaycastHit)(ref val)).point;
			joint = ((Component)GTPlayer.Instance).gameObject.AddComponent<SpringJoint>();
			((Joint)joint).autoConfigureConnectedAnchor = false;
			((Joint)joint).connectedAnchor = hitPosition;
			float num = Vector3.Distance(((Component)rope).transform.position, hitPosition);
			switch (ropeType)
			{
			case RopeType.ELASTIC:
				joint.maxDistance = 0.8f;
				joint.minDistance = 0.25f;
				joint.spring = pullForce;
				joint.damper = 7f;
				((Joint)joint).massScale = 4.5f;
				break;
			case RopeType.STATIC:
				joint.maxDistance = num;
				joint.minDistance = num;
				joint.spring = pullForce * 2f;
				joint.damper = 100f;
				((Joint)joint).massScale = 4.5f;
				break;
			}
		}
	}

	private void UpdateLineRenderer()
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		if (!isGrappling && base.Selected)
		{
			RaycastHit val = default(RaycastHit);
			Physics.SphereCast(new Ray(((Component)rope).transform.position, ((Component)this).transform.forward), 0.5f * GTPlayer.Instance.scale, ref val, maxLength, Teleport.layerMask);
			if (!Object.op_Implicit((Object)(object)((RaycastHit)(ref val)).transform))
			{
				((Renderer)laser).enabled = false;
				return;
			}
			Vector3 zero = Vector3.zero;
			Vector3 val2 = ((Component)laser).transform.InverseTransformPoint(((RaycastHit)(ref val)).point);
			((Renderer)laser).enabled = true;
			laser.SetPosition(0, zero);
			laser.SetPosition(1, val2);
			laser.startWidth = baseLaserWidth * GTPlayer.Instance.scale;
			laser.endWidth = baseLaserWidth * GTPlayer.Instance.scale;
		}
		else if (isGrappling)
		{
			Vector3 zero2 = Vector3.zero;
			Vector3 val3 = ((Component)rope).transform.InverseTransformPoint(hitPosition);
			rope.SetPosition(0, zero2);
			rope.SetPosition(1, val3);
			rope.startWidth = baseRopeWidth * GTPlayer.Instance.scale;
			rope.endWidth = baseRopeWidth * GTPlayer.Instance.scale;
		}
	}

	public override void OnDeselect(GrateInteractor interactor)
	{
		base.OnDeselect(interactor);
		((Renderer)laser).enabled = false;
		Holster(holster);
	}

	public void SetupInteraction()
	{
		throwOnDetach = false;
		((Component)this).gameObject.layer = GrateInteractor.InteractionLayer;
		if (Object.op_Implicit((Object)(object)openModel))
		{
			openModel.layer = GrateInteractor.InteractionLayer;
		}
		if (Object.op_Implicit((Object)(object)closedModel))
		{
			closedModel.layer = GrateInteractor.InteractionLayer;
		}
	}

	private void Open()
	{
		GameObject obj = openModel;
		if (obj != null)
		{
			obj.SetActive(true);
		}
		GameObject obj2 = closedModel;
		if (obj2 != null)
		{
			obj2.SetActive(false);
		}
		GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(96, false, 0.05f);
	}

	private void Close()
	{
		GameObject obj = openModel;
		if (obj != null)
		{
			obj.SetActive(false);
		}
		GameObject obj2 = closedModel;
		if (obj2 != null)
		{
			obj2.SetActive(true);
		}
		Activated = false;
		isGrappling = false;
		((Component)(object)joint)?.Obliterate();
	}
}
