using System;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Misc;

public class HaloMarker : MonoBehaviour
{
	private readonly Quaternion rotation = Quaternion.Euler(180f, 0f, 0f);

	private GameObject halo;

	private GameObject lightBeam;

	private void Start()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			halo = Object.Instantiate<GameObject>(Halo.haloPrefab);
			lightBeam = Object.Instantiate<GameObject>(Halo.lightBeamPrefab);
			VRRig component = ((Component)this).GetComponent<VRRig>();
			halo.transform.SetParent(component.headMesh.transform, false);
			halo.transform.localPosition = new Vector3(0f, 0.15f, 0f);
			halo.transform.localRotation = Quaternion.Euler(69f, 0f, 0f);
			lightBeam.transform.SetParent(((Component)component).transform, false);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void FixedUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		lightBeam.transform.rotation = rotation;
	}

	private void OnDestroy()
	{
		Object.Destroy((Object)(object)halo);
		Object.Destroy((Object)(object)lightBeam);
	}
}
