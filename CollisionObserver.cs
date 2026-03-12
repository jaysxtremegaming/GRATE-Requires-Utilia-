using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionObserver : MonoBehaviour
{
	public LayerMask layerMask = LayerMask.op_Implicit(-1);

	public Action<GameObject, Collision> OnCollisionEntered;

	public Action<GameObject, Collision> OnCollisionStayed;

	public Action<GameObject, Collision> OnCollisionExited;

	public Action<GameObject, Collider> OnTriggerEntered;

	public Action<GameObject, Collider> OnTriggerStayed;

	public Action<GameObject, Collider> OnTriggerExited;

	private void OnCollisionEnter(Collision collision)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (LayerMask.op_Implicit(layerMask) == (LayerMask.op_Implicit(layerMask) | (1 << collision.gameObject.layer)))
		{
			OnCollisionEntered?.Invoke(((Component)this).gameObject, collision);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (LayerMask.op_Implicit(layerMask) == (LayerMask.op_Implicit(layerMask) | (1 << collision.gameObject.layer)))
		{
			OnCollisionExited?.Invoke(((Component)this).gameObject, collision);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (LayerMask.op_Implicit(layerMask) == (LayerMask.op_Implicit(layerMask) | (1 << collision.gameObject.layer)))
		{
			OnCollisionStayed?.Invoke(((Component)this).gameObject, collision);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (LayerMask.op_Implicit(layerMask) == (LayerMask.op_Implicit(layerMask) | (1 << ((Component)collider).gameObject.layer)))
		{
			OnTriggerEntered?.Invoke(((Component)this).gameObject, collider);
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (LayerMask.op_Implicit(layerMask) == (LayerMask.op_Implicit(layerMask) | (1 << ((Component)collider).gameObject.layer)))
		{
			OnTriggerExited?.Invoke(((Component)this).gameObject, collider);
		}
	}

	private void OnTriggerStay(Collider collider)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (LayerMask.op_Implicit(layerMask) == (LayerMask.op_Implicit(layerMask) | (1 << ((Component)collider).gameObject.layer)))
		{
			OnTriggerStayed?.Invoke(((Component)this).gameObject, collider);
		}
	}
}
