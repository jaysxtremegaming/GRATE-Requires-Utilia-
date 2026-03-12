using System;
using System.Collections.Generic;
using Grate.GUI;
using Grate.Gestures;
using Grate.Networking;
using UnityEngine;
using UnityEngine.XR;

namespace Grate.Modules.Movement;

internal class Frozone : GrateModule
{
	public static GameObject IcePrefab;

	public static Vector3 LhandOffset = Vector3.down * 0.05f;

	public static Vector3 RhandOffset = Vector3.down * 0.107f;

	private readonly List<GameObject> prevLIce = new List<GameObject>();

	private readonly List<GameObject> prevRIce = new List<GameObject>();

	private InputTracker? inputL;

	private InputTracker? inputR;

	private bool leftPress;

	private bool rightPress;

	private Transform leftHandTransform => VRRig.LocalRig.leftHandTransform;

	private Transform rightHandTransform => VRRig.LocalRig.rightHandTransform;

	protected override void Start()
	{
		base.Start();
		IcePrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Ice");
		((Collider)IcePrefab.GetComponent<BoxCollider>()).enabled = true;
		IcePrefab.AddComponent<GorillaSurfaceOverride>().overrideIndex = 59;
	}

	private void FixedUpdate()
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		if (leftPress)
		{
			if (prevLIce.Count > 19)
			{
				GameObject val = prevLIce[0];
				prevLIce.RemoveAt(0);
				val.SetActive(true);
				val.transform.position = leftHandTransform.position + LhandOffset;
				val.transform.rotation = leftHandTransform.rotation;
				prevLIce.Add(val);
			}
			else
			{
				GameObject val2 = Object.Instantiate<GameObject>(IcePrefab);
				val2.AddComponent<RoomSpecific>();
				val2.transform.position = leftHandTransform.position + LhandOffset;
				val2.transform.rotation = leftHandTransform.rotation;
				prevLIce.Add(val2);
			}
		}
		else
		{
			foreach (GameObject item in prevLIce)
			{
				item.SetActive(false);
			}
		}
		if (rightPress)
		{
			if (prevRIce.Count >= 20)
			{
				GameObject val3 = prevRIce[0];
				prevRIce.RemoveAt(0);
				val3.SetActive(true);
				val3.transform.position = rightHandTransform.position + RhandOffset;
				val3.transform.rotation = rightHandTransform.rotation;
				prevRIce.Add(val3);
			}
			else
			{
				GameObject val4 = Object.Instantiate<GameObject>(IcePrefab);
				val4.AddComponent<RoomSpecific>();
				val4.transform.position = rightHandTransform.position + RhandOffset;
				val4.transform.rotation = rightHandTransform.rotation;
				prevRIce.Add(val4);
			}
			return;
		}
		foreach (GameObject item2 in prevRIce)
		{
			item2.SetActive(false);
		}
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			inputL = GestureTracker.Instance.GetInputTracker("grip", (XRNode)4);
			InputTracker? inputTracker = inputL;
			inputTracker.OnPressed = (Action<InputTracker>)Delegate.Combine(inputTracker.OnPressed, new Action<InputTracker>(OnActivate));
			InputTracker? inputTracker2 = inputL;
			inputTracker2.OnReleased = (Action<InputTracker>)Delegate.Combine(inputTracker2.OnReleased, new Action<InputTracker>(OnDeactivate));
			inputR = GestureTracker.Instance.GetInputTracker("grip", (XRNode)5);
			InputTracker? inputTracker3 = inputR;
			inputTracker3.OnPressed = (Action<InputTracker>)Delegate.Combine(inputTracker3.OnPressed, new Action<InputTracker>(OnActivate));
			InputTracker? inputTracker4 = inputR;
			inputTracker4.OnReleased = (Action<InputTracker>)Delegate.Combine(inputTracker4.OnReleased, new Action<InputTracker>(OnDeactivate));
			((Component)Plugin.MenuController).GetComponent<Platforms>().button.AddBlocker(ButtonController.Blocker.MOD_INCOMPAT);
		}
	}

	public override string GetDisplayName()
	{
		return "Frozone";
	}

	public override string Tutorial()
	{
		return "Like Platforms but you slide!";
	}

	private void OnActivate(InputTracker tracker)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if ((int)tracker.node == 4)
		{
			leftPress = true;
		}
		if ((int)tracker.node == 5)
		{
			rightPress = true;
		}
	}

	private void Unsub()
	{
		if (inputL != null)
		{
			InputTracker? inputTracker = inputL;
			inputTracker.OnPressed = (Action<InputTracker>)Delegate.Remove(inputTracker.OnPressed, new Action<InputTracker>(OnActivate));
			InputTracker? inputTracker2 = inputL;
			inputTracker2.OnReleased = (Action<InputTracker>)Delegate.Remove(inputTracker2.OnReleased, new Action<InputTracker>(OnDeactivate));
		}
		if (inputR != null)
		{
			InputTracker? inputTracker3 = inputR;
			inputTracker3.OnPressed = (Action<InputTracker>)Delegate.Remove(inputTracker3.OnPressed, new Action<InputTracker>(OnActivate));
			InputTracker? inputTracker4 = inputR;
			inputTracker4.OnReleased = (Action<InputTracker>)Delegate.Remove(inputTracker4.OnReleased, new Action<InputTracker>(OnDeactivate));
		}
	}

	private void OnDeactivate(InputTracker tracker)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if ((int)tracker.node == 4)
		{
			leftPress = false;
		}
		if ((int)tracker.node == 5)
		{
			rightPress = false;
		}
	}

	protected override void Cleanup()
	{
		Unsub();
		((Component)Plugin.MenuController).GetComponent<Platforms>().button.RemoveBlocker(ButtonController.Blocker.MOD_INCOMPAT);
	}
}
