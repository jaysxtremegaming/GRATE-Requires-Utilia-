using System;
using System.Collections.Generic;
using Grate.Interaction;
using Grate.Tools;
using UnityEngine;
using UnityEngine.XR;

namespace Grate.Gestures;

public class GrateInteractor : MonoBehaviour
{
	public static string InteractionLayerName = "TransparentFX";

	public static int InteractionLayer = LayerMask.NameToLayer(InteractionLayerName);

	public static int InteractionLayerMask = LayerMask.GetMask(new string[1] { InteractionLayerName });

	public InputDevice device;

	public List<GrateInteractable> hovered = new List<GrateInteractable>();

	public List<GrateInteractable> selected = new List<GrateInteractable>();

	public XRNode node;

	public bool IsLeft { get; protected set; }

	public bool Selecting { get; protected set; }

	public bool Activating { get; protected set; }

	public bool PrimaryPressed { get; protected set; }

	public bool IsSelecting => selected.Count > 0;

	protected void Awake()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			IsLeft = ((Object)this).name.Contains("Left");
			((Collider)((Component)this).gameObject.AddComponent<SphereCollider>()).isTrigger = true;
			((Component)this).gameObject.layer = InteractionLayer;
			GestureTracker instance = GestureTracker.Instance;
			device = (IsLeft ? instance.leftController : instance.rightController);
			node = (XRNode)(IsLeft ? 4 : 5);
			InputTracker? inputTracker = instance.GetInputTracker("grip", node);
			inputTracker.OnPressed = (Action<InputTracker>)Delegate.Combine(inputTracker.OnPressed, new Action<InputTracker>(OnGrip));
			InputTracker? inputTracker2 = instance.GetInputTracker("grip", node);
			inputTracker2.OnReleased = (Action<InputTracker>)Delegate.Combine(inputTracker2.OnReleased, new Action<InputTracker>(OnGripRelease));
			InputTracker? inputTracker3 = instance.GetInputTracker("trigger", node);
			inputTracker3.OnPressed = (Action<InputTracker>)Delegate.Combine(inputTracker3.OnPressed, new Action<InputTracker>(OnTrigger));
			InputTracker? inputTracker4 = instance.GetInputTracker("trigger", node);
			inputTracker4.OnReleased = (Action<InputTracker>)Delegate.Combine(inputTracker4.OnReleased, new Action<InputTracker>(OnTriggerRelease));
			InputTracker? inputTracker5 = instance.GetInputTracker("primary", node);
			inputTracker5.OnPressed = (Action<InputTracker>)Delegate.Combine(inputTracker5.OnPressed, new Action<InputTracker>(OnPrimary));
			InputTracker? inputTracker6 = instance.GetInputTracker("primary", node);
			inputTracker6.OnReleased = (Action<InputTracker>)Delegate.Combine(inputTracker6.OnReleased, new Action<InputTracker>(OnPrimaryRelease));
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public void Select(GrateInteractable interactable)
	{
		try
		{
			if (!interactable.CanBeSelected(this))
			{
				return;
			}
			if (selected.Count > 0)
			{
				DeselectAll(interactable.priority);
				if (selected.Count > 0)
				{
					return;
				}
			}
			interactable.OnSelect(this);
			selected.Add(interactable);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public void Deselect(GrateInteractable interactable)
	{
		interactable.OnDeselect(this);
	}

	public void Hover(GrateInteractable interactable)
	{
		hovered.Add(interactable);
	}

	private void OnGrip(InputTracker _)
	{
		if (Selecting)
		{
			return;
		}
		Selecting = true;
		GrateInteractable grateInteractable = null;
		foreach (GrateInteractable item in hovered)
		{
			if (item.CanBeSelected(this))
			{
				grateInteractable = item;
				break;
			}
		}
		if (Object.op_Implicit((Object)(object)grateInteractable))
		{
			Select(grateInteractable);
		}
	}

	private void OnGripRelease(InputTracker _)
	{
		if (Selecting)
		{
			Selecting = false;
			DeselectAll();
		}
	}

	private void DeselectAll(int competingPriority = -1)
	{
		foreach (GrateInteractable item in selected)
		{
			if (competingPriority < 0 || item.priority < competingPriority)
			{
				Deselect(item);
			}
		}
		selected.RemoveAll((GrateInteractable g) => !g.selectors.Contains(this));
	}

	private void OnTrigger(InputTracker _)
	{
		Activating = true;
		foreach (GrateInteractable item in selected)
		{
			item.OnActivate(this);
		}
	}

	private void OnTriggerRelease(InputTracker _)
	{
		Activating = false;
		foreach (GrateInteractable item in selected)
		{
			item.OnDeactivate(this);
		}
	}

	private void OnPrimary(InputTracker _)
	{
		Activating = true;
		foreach (GrateInteractable item in selected)
		{
			item.OnPrimary(this);
		}
	}

	private void OnPrimaryRelease(InputTracker _)
	{
		Activating = false;
		foreach (GrateInteractable item in selected)
		{
			item.OnPrimaryReleased(this);
		}
	}
}
