using System;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace Grate.Gestures;

public class ControllerInputPollerExt
{
	public static ControllerInputPollerExt Instance;

	public Vector2 rightControllerStickAxis;

	public Vector2 leftControllerStickAxis;

	public bool rightControllerStickButton;

	public bool leftControllerStickButton;

	private bool steam;

	public ControllerInputPollerExt()
	{
		Instance = this;
	}

	public void Update()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Plugin.IsSteam)
			{
				leftControllerStickButton = SteamVR_Actions.gorillaTag_LeftJoystickClick.state;
				rightControllerStickButton = SteamVR_Actions.gorillaTag_RightJoystickClick.state;
				leftControllerStickAxis = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis;
				rightControllerStickAxis = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis;
			}
			else
			{
				InputDevice leftController = GestureTracker.Instance.leftController;
				InputDevice rightController = GestureTracker.Instance.rightController;
				((InputDevice)(ref leftController)).TryGetFeatureValue(CommonUsages.primary2DAxisClick, ref leftControllerStickButton);
				((InputDevice)(ref rightController)).TryGetFeatureValue(CommonUsages.primary2DAxisClick, ref rightControllerStickButton);
				((InputDevice)(ref leftController)).TryGetFeatureValue(CommonUsages.primary2DAxis, ref leftControllerStickAxis);
				((InputDevice)(ref rightController)).TryGetFeatureValue(CommonUsages.primary2DAxis, ref rightControllerStickAxis);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError((object)("Error in Inputs, Should resolve itself: " + ex.Message + ex.StackTrace));
		}
	}
}
