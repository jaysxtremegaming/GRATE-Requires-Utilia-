using Grate.Gestures;
using Grate.Interaction;
using UnityEngine;

namespace Grate.Modules.Physics;

public class Cork : GrateGrabbable
{
	private AudioSource popSource;

	public Rigidbody rb;

	public bool shouldPlayPopSound = true;

	protected override void Awake()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		base.Awake();
		base.LocalPosition = new Vector3(0.5f, 0.5f, 0.425f);
		LocalRotation = new Vector3(8f, 0f, 0f);
		throwOnDetach = true;
		rb = ((Component)this).GetComponent<Rigidbody>();
		rb.isKinematic = true;
		popSource = ((Component)this).GetComponent<AudioSource>();
	}

	public override void OnSelect(GrateInteractor interactor)
	{
		base.OnSelect(interactor);
		if (shouldPlayPopSound)
		{
			popSource.Play();
		}
		shouldPlayPopSound = false;
	}

	public void Pop()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.SetParent((Transform)null);
		rb.isKinematic = false;
		rb.velocity = ((Component)this).transform.up * 2.5f;
		popSource.Play();
	}
}
