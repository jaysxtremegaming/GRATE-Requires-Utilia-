using GorillaLocomotion;
using Grate.GUI;
using UnityEngine;

namespace Grate.Modules;

public class Swim : GrateModule
{
	public static readonly string DisplayName = "Swim";

	public GameObject? waterVolume;

	protected override void Start()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		waterVolume = Object.Instantiate<GameObject>(GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach/ForestToBeach_Prefab_V4/CaveWaterVolume"), ((Component)VRRig.LocalRig).transform);
		waterVolume.transform.localScale = new Vector3(5f, 1000f, 5f);
		waterVolume.transform.localPosition = new Vector3(0f, 50f, 0f);
		waterVolume.SetActive(false);
		if (Object.op_Implicit((Object)(object)waterVolume.GetComponent<Renderer>()))
		{
			waterVolume.GetComponent<Renderer>().enabled = false;
		}
		if (Object.op_Implicit((Object)(object)waterVolume.GetComponentInChildren<Renderer>()))
		{
			waterVolume.GetComponentInChildren<Renderer>().enabled = false;
		}
	}

	private void LateUpdate()
	{
		GTPlayer.Instance.audioManager.UnsetMixerSnapshot(0.1f);
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			waterVolume.SetActive(true);
		}
	}

	protected override void Cleanup()
	{
		if (MenuController.Instance.Built)
		{
			waterVolume.SetActive(false);
			GTPlayer.Instance.audioManager.UnsetMixerSnapshot(0.1f);
		}
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Effect: Surrounds you with invisible water.";
	}
}
