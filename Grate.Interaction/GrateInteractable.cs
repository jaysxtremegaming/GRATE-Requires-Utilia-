using System;
using System.Collections.Generic;
using System.Linq;
using Grate.Gestures;
using UnityEngine;

namespace Grate.Interaction;

public class GrateInteractable : MonoBehaviour
{
	public bool Activated;

	private GestureTracker gt;

	public List<GrateInteractor> hoverers = new List<GrateInteractor>();

	public Action<GrateInteractable, GrateInteractor> OnActivateEnter;

	public Action<GrateInteractable, GrateInteractor> OnActivateExit;

	public Action<GrateInteractable, GrateInteractor> OnHoverEnter;

	public Action<GrateInteractable, GrateInteractor> OnHoverExit;

	public Action<GrateInteractable, GrateInteractor> OnPrimaryEnter;

	public Action<GrateInteractable, GrateInteractor> OnPrimaryExit;

	public Action<GrateInteractable, GrateInteractor> OnSelectEnter;

	public Action<GrateInteractable, GrateInteractor> OnSelectExit;

	public bool Primary;

	public int priority;

	public List<GrateInteractor> selectors = new List<GrateInteractor>();

	public GrateInteractor[] validSelectors;

	public bool Selected => selectors.Count > 0;

	protected virtual void Awake()
	{
		gt = GestureTracker.Instance;
		((Component)this).gameObject.layer = GrateInteractor.InteractionLayer;
		validSelectors = new GrateInteractor[2] { gt.leftPalmInteractor, gt.rightPalmInteractor };
	}

	protected virtual void OnDestroy()
	{
		foreach (GrateInteractor hoverer in hoverers)
		{
			hoverer?.hovered.Remove(this);
		}
		foreach (GrateInteractor selector in selectors)
		{
			selector?.selected.Remove(this);
		}
	}

	protected virtual void OnTriggerEnter(Collider collider)
	{
		GrateInteractor component = ((Component)collider).GetComponent<GrateInteractor>();
		if (component != null && CanBeSelected(component) && !component.hovered.Contains(this))
		{
			if (component.Selecting)
			{
				component.Select(this);
				return;
			}
			component.Hover(this);
			hoverers.Add(component);
			OnHoverEnter?.Invoke(this, component);
		}
	}

	protected virtual void OnTriggerExit(Collider collider)
	{
		if (((Behaviour)this).enabled)
		{
			GrateInteractor component = ((Component)collider).GetComponent<GrateInteractor>();
			if (component != null && component.hovered.Contains(this))
			{
				component.hovered.Remove(this);
				hoverers.Remove(component);
				OnHoverExit?.Invoke(this, component);
			}
		}
	}

	public virtual bool CanBeSelected(GrateInteractor interactor)
	{
		if (((Behaviour)this).enabled && !Selected)
		{
			return validSelectors.Contains(interactor);
		}
		return false;
	}

	public virtual void OnSelect(GrateInteractor interactor)
	{
		selectors.Add(interactor);
		OnSelectEnter?.Invoke(this, interactor);
	}

	public virtual void OnDeselect(GrateInteractor interactor)
	{
		selectors.Remove(interactor);
		OnSelectExit?.Invoke(this, interactor);
	}

	public virtual void OnActivate(GrateInteractor interactor)
	{
		Activated = true;
		OnActivateEnter?.Invoke(this, interactor);
	}

	public virtual void OnDeactivate(GrateInteractor interactor)
	{
		Activated = false;
		OnActivateExit?.Invoke(this, interactor);
	}

	public virtual void OnPrimary(GrateInteractor interactor)
	{
		Primary = true;
		OnPrimaryEnter?.Invoke(this, interactor);
	}

	public virtual void OnPrimaryReleased(GrateInteractor interactor)
	{
		Primary = false;
		OnPrimaryExit?.Invoke(this, interactor);
	}
}
