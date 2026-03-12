using System;
using System.Collections.Generic;
using Grate;
using Grate.Gestures;
using Grate.Tools;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	public enum Blocker
	{
		MENU_FALLING,
		NOCLIP_BOUNDARY,
		PIGGYBACKING,
		BUTTON_PRESSED,
		MOD_INCOMPAT,
		MOD_CONTROLL
	}

	private readonly List<Blocker> blockers = new List<Blocker>();

	private readonly Dictionary<Blocker, string> blockerText = new Dictionary<Blocker, string>
	{
		{
			Blocker.MENU_FALLING,
			""
		},
		{
			Blocker.NOCLIP_BOUNDARY,
			"YOU ARE TOO CLOSE TO A WALL TO ACTIVATE THIS"
		},
		{
			Blocker.PIGGYBACKING,
			"NO COLLIDE CANNOT BE TOGGLED WHILE PIGGYBACK IS ACTIVE"
		},
		{
			Blocker.BUTTON_PRESSED,
			""
		},
		{
			Blocker.MOD_INCOMPAT,
			"YOU HAVE A MOD THAT DOSN't WORK WITH THIS MOD"
		},
		{
			Blocker.MOD_CONTROLL,
			"UNDER CONTROLL OF ANOTHER MOD"
		}
	};

	private readonly float cooldown = 0.1f;

	private bool _isPressed;

	private Transform buttonModel;

	public float buttonPushDistance = 0.03f;

	public Canvas canvas;

	private float lastPressed;

	private Material material;

	public Action<ButtonController, bool> OnPressed;

	public Text text;

	public bool IsPressed
	{
		get
		{
			return _isPressed;
		}
		set
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			_isPressed = value;
			material.color = (value ? Color.red : (Color.white * 0.75f));
		}
	}

	public bool Interactable
	{
		get
		{
			return blockers.Count == 0;
		}
		private set
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (value)
			{
				material.color = (IsPressed ? Color.red : (Color.white * 0.75f));
			}
			else
			{
				material.color = (Color)(IsPressed ? new Color(0.5f, 0.3f, 0.3f) : Color.gray);
			}
		}
	}

	protected void Awake()
	{
		string text = "";
		try
		{
			buttonModel = ((Component)this).transform.GetChild(0);
			material = ((Component)buttonModel).GetComponent<Renderer>().material;
			((Component)this).gameObject.layer = GrateInteractor.InteractionLayer;
			CollisionObserver collisionObserver = ((Component)this).gameObject.AddComponent<CollisionObserver>();
			collisionObserver.OnTriggerEntered = (Action<GameObject, Collider>)Delegate.Combine(collisionObserver.OnTriggerEntered, new Action<GameObject, Collider>(Press));
			collisionObserver.OnTriggerExited = (Action<GameObject, Collider>)Delegate.Combine(collisionObserver.OnTriggerExited, new Action<GameObject, Collider>(Unpress));
			this.text = ((Component)this).GetComponentInChildren<Text>();
			if (Object.op_Implicit((Object)(object)this.text))
			{
				this.text.fontSize = 26;
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
			Logging.Debug("Reached", text);
		}
	}

	public void Press(GameObject self, Collider collider)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!Interactable && blockerText[blockers[0]].Length > 0)
			{
				Plugin.MenuController.helpText.text = blockerText[blockers[0]];
			}
			else if (Interactable && (!((Object)(object)((Component)collider).gameObject != (Object)(object)((Component)GestureTracker.Instance.leftPointerInteractor).gameObject) || !((Object)(object)((Component)collider).gameObject != (Object)(object)((Component)GestureTracker.Instance.rightPointerInteractor).gameObject)) && !(Time.time - lastPressed < cooldown))
			{
				lastPressed = Time.time;
				IsPressed = !IsPressed;
				OnPressed?.Invoke(this, IsPressed);
				bool flag = ((Object)collider).name.ToLower().Contains("left");
				GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, flag, 0.05f);
				if (!flag)
				{
					_ = GestureTracker.Instance.rightController;
				}
				else
				{
					_ = GestureTracker.Instance.leftController;
				}
				GestureTracker.Instance.HapticPulse(flag);
				Plugin.MenuController.AddBlockerToAllButtons(Blocker.BUTTON_PRESSED);
				((MonoBehaviour)this).Invoke("RemoveCooldownBlocker", 0.1f);
				buttonModel.localPosition = Vector3.up * (0f - buttonPushDistance);
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	protected void Unpress(GameObject self, Collider collider)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (((Object)collider).name.Contains("Pointer"))
		{
			buttonModel.localPosition = Vector3.zero;
		}
	}

	private void RemoveCooldownBlocker()
	{
		Plugin.MenuController.RemoveBlockerFromAllButtons(Blocker.BUTTON_PRESSED);
	}

	public void SetText(string text)
	{
		this.text.text = text.ToUpper();
	}

	public void AddBlocker(Blocker blocker)
	{
		try
		{
			if (!NetworkSystem.Instance.GameModeString.Contains("MODDED_"))
			{
				NetworkSystem.Instance.ReturnToSinglePlayer();
			}
			if (!blockers.Contains(blocker))
			{
				Interactable = false;
				blockers.Add(blocker);
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public void RemoveBlocker(Blocker blocker)
	{
		try
		{
			if (blockers.Contains(blocker))
			{
				blockers.Remove(blocker);
				Interactable = blockers.Count == 0;
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
