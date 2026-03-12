using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.Gestures;
using Grate.Interaction;
using Grate.Modules;
using Grate.Modules.Misc;
using Grate.Modules.Movement;
using Grate.Modules.Multiplayer;
using Grate.Modules.Physics;
using Grate.Modules.Teleportation;
using Grate.Tools;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR;

namespace Grate.GUI;

public class MenuController : GrateGrabbable
{
	[CompilerGenerated]
	private sealed class _003CVerCheck_003Ed__40 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MenuController _003C_003E4__this;

		private UnityWebRequest _003Crequest_003E5__2;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CVerCheck_003Ed__40(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Crequest_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Invalid comparison between Unknown and I4
			bool result;
			try
			{
				int num = _003C_003E1__state;
				MenuController menuController = _003C_003E4__this;
				switch (num)
				{
				default:
					result = false;
					break;
				case 0:
					_003C_003E1__state = -1;
					_003Crequest_003E5__2 = UnityWebRequest.Get("https://raw.githubusercontent.com/The-Graze/Grate/master/ver.txt");
					_003C_003E1__state = -3;
					_003C_003E2__current = _003Crequest_003E5__2.SendWebRequest();
					_003C_003E1__state = 1;
					result = true;
					break;
				case 1:
				{
					_003C_003E1__state = -3;
					if ((int)_003Crequest_003E5__2.result != 1)
					{
						result = false;
						_003C_003Em__Finally1();
						break;
					}
					Version version = new Version(_003Crequest_003E5__2.downloadHandler.text);
					Version version2 = new Version("1.8.7");
					Text componentInChildren = ((Component)((Component)menuController).gameObject.transform.Find("Version Canvas")).GetComponentInChildren<Text>();
					if (version > version2)
					{
						componentInChildren.horizontalOverflow = (HorizontalWrapMode)1;
						componentInChildren.verticalOverflow = (VerticalWrapMode)1;
						componentInChildren.text = "!!Update Needed!! \n GoTo: \n https://graze.cc/grate";
					}
					else
					{
						componentInChildren.text = "Grate 1.8.7";
					}
					_003C_003Em__Finally1();
					_003Crequest_003E5__2 = null;
					result = false;
					break;
				}
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003Crequest_003E5__2 != null)
			{
				((IDisposable)_003Crequest_003E5__2).Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public static MenuController? Instance;

	private static InputTracker? _summonTracker;

	private static ConfigEntry<string>? _summonInput;

	private static ConfigEntry<string>? _summonInputHand;

	private static ConfigEntry<string>? _theme;

	public static Material[]? ShinyRocks;

	public static bool Debugger = true;

	public List<ButtonController>? buttons;

	private int debugButtons;

	private bool docked;

	public Material[]? grate;

	public Material[]? bark;

	public Material[]? hollopurp;

	public Material[]? monke;

	public Material[]? old;

	public Text? helpText;

	public Vector3 initialMenuOffset = new Vector3(0f, 0.035f, 0.65f);

	public Vector3 btnDimensions = new Vector3(0.3f, 0.05f, 0.05f);

	public GameObject? modPage;

	public GameObject? settingsPage;

	public List<Transform>? modPages;

	public List<GrateModule> modules = new List<GrateModule>();

	private int pageIndex;

	public Renderer? renderer;

	public Rigidbody? rigidbody;

	public bool Built { get; private set; }

	protected override void Awake()
	{
		if (!NetworkSystem.Instance.GameModeString.Contains("MODDED_"))
		{
			return;
		}
		Instance = this;
		try
		{
			base.Awake();
			throwOnDetach = true;
			((Component)this).gameObject.AddComponent<PositionValidator>();
			if (Plugin.ConfigFile != null)
			{
				Plugin.ConfigFile.SettingChanged += SettingsChanged;
			}
			List<GrateModule> collection = new List<GrateModule>
			{
				((Component)this).gameObject.AddComponent<Airplane>(),
				((Component)this).gameObject.AddComponent<Helicopter>(),
				((Component)this).gameObject.AddComponent<Bubble>(),
				((Component)this).gameObject.AddComponent<Fly>(),
				((Component)this).gameObject.AddComponent<HandFly>(),
				((Component)this).gameObject.AddComponent<GrapplingHooks>(),
				((Component)this).gameObject.AddComponent<Climb>(),
				((Component)this).gameObject.AddComponent<DoubleJump>(),
				((Component)this).gameObject.AddComponent<Platforms>(),
				((Component)this).gameObject.AddComponent<Frozone>(),
				((Component)this).gameObject.AddComponent<NailGun>(),
				((Component)this).gameObject.AddComponent<Rockets>(),
				((Component)this).gameObject.AddComponent<SpeedBoost>(),
				((Component)this).gameObject.AddComponent<Swim>(),
				((Component)this).gameObject.AddComponent<Wallrun>(),
				((Component)this).gameObject.AddComponent<Zipline>(),
				((Component)this).gameObject.AddComponent<LowGravity>(),
				((Component)this).gameObject.AddComponent<NoClip>(),
				((Component)this).gameObject.AddComponent<NoSlip>(),
				((Component)this).gameObject.AddComponent<Potions>(),
				((Component)this).gameObject.AddComponent<SlipperyHands>(),
				((Component)this).gameObject.AddComponent<DisableWind>(),
				((Component)this).gameObject.AddComponent<UpsideDown>(),
				((Component)this).gameObject.AddComponent<Checkpoint>(),
				((Component)this).gameObject.AddComponent<Portal>(),
				((Component)this).gameObject.AddComponent<Pearl>(),
				((Component)this).gameObject.AddComponent<Teleport>(),
				((Component)this).gameObject.AddComponent<Boxing>(),
				((Component)this).gameObject.AddComponent<Piggyback>(),
				((Component)this).gameObject.AddComponent<Telekinesis>(),
				((Component)this).gameObject.AddComponent<Grab>(),
				((Component)this).gameObject.AddComponent<Fireflies>(),
				((Component)this).gameObject.AddComponent<ESP>(),
				((Component)this).gameObject.AddComponent<RatSword>(),
				((Component)this).gameObject.AddComponent<Kamehameha>()
			};
			CatMeow item = ((Component)this).gameObject.AddComponent<CatMeow>();
			if (NetworkSystem.Instance.LocalPlayer.UserId == "FBE3EE50747CB892")
			{
				modules.Add(item);
			}
			StoneBroke item2 = ((Component)this).gameObject.AddComponent<StoneBroke>();
			if (NetworkSystem.Instance.LocalPlayer.UserId == "CA8FDFF42B7A1836")
			{
				modules.Add(item2);
			}
			Baggy item3 = ((Component)this).gameObject.AddComponent<Baggy>();
			if (NetworkSystem.Instance.LocalPlayer.UserId == "9ABD0C174289F58E")
			{
				modules.Add(item3);
			}
			Grazing item4 = ((Component)this).gameObject.AddComponent<Grazing>();
			if (NetworkSystem.Instance.LocalPlayer.UserId == "42D7D32651E93866")
			{
				modules.Add(item4);
			}
			Cheese item5 = ((Component)this).gameObject.AddComponent<Cheese>();
			if (NetworkSystem.Instance.LocalPlayer.UserId == "B1B20DEEEDB71C63")
			{
				modules.Add(item5);
			}
			ShadowFly item6 = ((Component)this).gameObject.AddComponent<ShadowFly>();
			if (NetworkSystem.Instance.LocalPlayer.UserId == "AE10C04744CCF6E7")
			{
				modules.Add(item6);
			}
			Supporter item7 = ((Component)this).gameObject.AddComponent<Supporter>();
			if (NetworkSystem.Instance.LocalPlayer.IsSupporter())
			{
				modules.Add(item7);
			}
			Developer item8 = ((Component)this).gameObject.AddComponent<Developer>();
			if (NetworkSystem.Instance.LocalPlayer.IsDev())
			{
				modules.Add(item8);
			}
			modules.AddRange(collection);
			ReloadConfiguration();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void Start()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		Summon();
		((Component)this).transform.SetParent((Transform)null);
		((Component)this).transform.position = Vector3.zero;
		if ((Object)(object)rigidbody != (Object)null)
		{
			rigidbody.isKinematic = false;
			rigidbody.useGravity = true;
		}
		((Component)this).transform.SetParent((Transform)null);
		AddBlockerToAllButtons(ButtonController.Blocker.MENU_FALLING);
		docked = false;
	}

	private void FixedUpdate()
	{
		if (PhotonNetwork.InRoom && !NetworkSystem.Instance.GameModeString.Contains("MODDED"))
		{
			((Component)this).gameObject.Obliterate();
		}
		if (Object.op_Implicit((Object)(object)GrateModule.LastEnabled) && (Object)(object)GrateModule.LastEnabled == (Object)(object)Potions.Instance)
		{
			helpText.text = Potions.Instance.Tutorial();
		}
	}

	private void Update()
	{
		if (!((ButtonControl)Keyboard.current.bKey).wasPressedThisFrame)
		{
			return;
		}
		if (!docked)
		{
			Summon();
			return;
		}
		if ((Object)(object)rigidbody != (Object)null)
		{
			rigidbody.isKinematic = false;
			rigidbody.useGravity = true;
		}
		((Component)this).transform.SetParent((Transform)null);
		AddBlockerToAllButtons(ButtonController.Blocker.MENU_FALLING);
		docked = false;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Plugin.ConfigFile.SettingChanged -= SettingsChanged;
	}

	private void ThemeChanged()
	{
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		if (!((Object)(object)Plugin.AssetBundle != (Object)null))
		{
			return;
		}
		if (!Object.op_Implicit((Object)(object)renderer))
		{
			renderer = (Renderer?)(object)((Component)this).GetComponent<MeshRenderer>();
		}
		if ((Object)(object)Plugin.AssetBundle != (Object)null)
		{
			if (grate == null)
			{
				Material val = Plugin.AssetBundle.LoadAsset<Material>("Zipline Rope Material");
				Material val2 = Plugin.AssetBundle.LoadAsset<Material>("Metal Material");
				if (Object.op_Implicit((Object)(object)val) && Object.op_Implicit((Object)(object)val2))
				{
					grate = (Material[]?)(object)new Material[2] { val, val2 };
				}
			}
			if (bark == null)
			{
				Material val3 = Plugin.AssetBundle.LoadAsset<Material>("m_Menu Outer");
				Material val4 = Plugin.AssetBundle.LoadAsset<Material>("m_Menu Inner");
				if (Object.op_Implicit((Object)(object)val3) && Object.op_Implicit((Object)(object)val4))
				{
					bark = (Material[]?)(object)new Material[2] { val3, val4 };
				}
			}
			if (hollopurp == null)
			{
				Material val5 = Plugin.AssetBundle.LoadAsset<Material>("m_TK Sparkles");
				if (Object.op_Implicit((Object)(object)val5))
				{
					hollopurp = (Material[]?)(object)new Material[2] { val5, val5 };
				}
			}
			if (monke == null || old == null)
			{
				Material val6 = Plugin.AssetBundle.LoadAsset<Material>("Gorilla Material");
				if (Object.op_Implicit((Object)(object)val6))
				{
					if (monke == null)
					{
						monke = (Material[]?)(object)new Material[2] { val6, val6 };
					}
					if (old == null)
					{
						old = (Material[]?)(object)new Material[2]
						{
							new Material(val6)
							{
								mainTexture = null,
								color = new Color(0.17f, 0.17f, 0.17f)
							},
							new Material(val6)
							{
								mainTexture = null,
								color = new Color(0.2f, 0.2f, 0.2f)
							}
						};
					}
				}
			}
		}
		switch (_theme.Value.ToLower())
		{
		case "grate":
			if (grate != null)
			{
				renderer.materials = grate;
			}
			break;
		case "bark":
			renderer.materials = bark;
			break;
		case "holowpurple":
			renderer.materials = hollopurp;
			break;
		case "oldgrate":
			renderer.materials = old;
			break;
		case "player":
			if ((Object)(object)VRRig.LocalRig.CurrentCosmeticSkin != (Object)null)
			{
				Material scoreboardMaterial = VRRig.LocalRig.CurrentCosmeticSkin.scoreboardMaterial;
				renderer.materials = (Material[])(object)new Material[2] { scoreboardMaterial, scoreboardMaterial };
			}
			else if (monke != null)
			{
				renderer.materials = monke;
				Color playerColor = VRRig.LocalRig.playerColor;
				monke[0].color = playerColor;
				monke[1].color = playerColor;
			}
			break;
		}
	}

	private void ReloadConfiguration()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		if (_summonTracker != null)
		{
			InputTracker? summonTracker = _summonTracker;
			summonTracker.OnPressed = (Action<InputTracker>)Delegate.Remove(summonTracker.OnPressed, new Action<InputTracker>(Summon));
		}
		GestureTracker instance = GestureTracker.Instance;
		instance.OnMeatBeat = (Action)Delegate.Remove(instance.OnMeatBeat, new Action(Summon));
		XRNode node = (XRNode)((_summonInputHand.Value == "left") ? 4 : 5);
		if (_summonInput.Value == "gesture")
		{
			GestureTracker instance2 = GestureTracker.Instance;
			instance2.OnMeatBeat = (Action)Delegate.Combine(instance2.OnMeatBeat, new Action(Summon));
			return;
		}
		_summonTracker = GestureTracker.Instance.GetInputTracker(_summonInput.Value, node);
		if (_summonTracker != null)
		{
			InputTracker? summonTracker2 = _summonTracker;
			summonTracker2.OnPressed = (Action<InputTracker>)Delegate.Combine(summonTracker2.OnPressed, new Action<InputTracker>(Summon));
		}
	}

	private void SettingsChanged(object sender, SettingChangedEventArgs e)
	{
		if (e.ChangedSetting == _summonInput || e.ChangedSetting == _summonInputHand)
		{
			ReloadConfiguration();
		}
		if (e.ChangedSetting == _theme)
		{
			ThemeChanged();
		}
	}

	private void Summon(InputTracker _)
	{
		Summon();
	}

	public void Summon()
	{
		if (!Built)
		{
			BuildMenu();
		}
		else
		{
			ResetPosition();
		}
	}

	private void ResetPosition()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		rigidbody.isKinematic = true;
		rigidbody.velocity = Vector3.zero;
		((Component)this).transform.SetParent(((Component)GTPlayer.Instance.bodyCollider).transform);
		((Component)this).transform.localPosition = initialMenuOffset;
		((Component)this).transform.localRotation = Quaternion.identity;
		((Component)this).transform.localScale = Vector3.one;
		foreach (ButtonController button in buttons)
		{
			button.RemoveBlocker(ButtonController.Blocker.MENU_FALLING);
		}
		docked = true;
	}

	[IteratorStateMachine(typeof(_003CVerCheck_003Ed__40))]
	private IEnumerator VerCheck()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CVerCheck_003Ed__40(0)
		{
			_003C_003E4__this = this
		};
	}

	private void BuildMenu()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		Logging.Debug("Building menu...");
		try
		{
			helpText = ((Component)((Component)this).gameObject.transform.Find("Help Canvas")).GetComponentInChildren<Text>();
			((Component)helpText).transform.parent.localPosition = new Vector3(0f, -0.0731f, -0.1f);
			((Component)helpText).transform.parent.localRotation = Quaternion.Euler(0f, 180f, 0f);
			helpText.text = "Enable a module to see its tutorial.";
			((MonoBehaviour)this).StartCoroutine(VerCheck());
			((Collider)((Component)this).gameObject.GetOrAddComponent<BoxCollider>()).isTrigger = true;
			rigidbody = ((Component)this).gameObject.GetComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			SetupInteraction();
			SetupModPages();
			SetupSettingsPage();
			((Component)this).transform.SetParent(((Component)GTPlayer.Instance.bodyCollider).transform);
			ResetPosition();
			Logging.Debug("Build successful.");
			ReloadConfiguration();
			ThemeChanged();
		}
		catch (Exception ex)
		{
			Logging.Warning(ex.Message);
			Logging.Warning(ex.StackTrace);
			return;
		}
		Built = true;
	}

	private void SetupSettingsPage()
	{
		ButtonController buttonController = ((Component)((Component)this).gameObject.transform.Find("Settings Button")).gameObject.AddComponent<ButtonController>();
		buttons.Add(buttonController);
		buttonController.OnPressed = (Action<ButtonController, bool>)Delegate.Combine(buttonController.OnPressed, (Action<ButtonController, bool>)delegate(ButtonController obj, bool pressed)
		{
			settingsPage.SetActive(pressed);
			if (pressed)
			{
				settingsPage.GetComponent<SettingsPage>().UpdateText();
			}
			modPage.SetActive(!pressed);
		});
		settingsPage = ((Component)((Component)this).transform.Find("Settings Page")).gameObject;
		settingsPage.AddComponent<SettingsPage>();
		settingsPage.SetActive(false);
	}

	public void SetupModPages()
	{
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		Transform val = ((Component)this).gameObject.transform.Find("Mod Page");
		int num = val.childCount - 2;
		int num2 = (modules.Count - 1) / num + 1;
		if (Plugin.DebugMode)
		{
			num2++;
		}
		modPages = new List<Transform> { val };
		for (int i = 0; i < num2 - 1; i++)
		{
			modPages.Add(Object.Instantiate<Transform>(val, ((Component)this).gameObject.transform));
		}
		buttons = new List<ButtonController>();
		for (int j = 0; j < modules.Count; j++)
		{
			GrateModule module = modules[j];
			ButtonController buttonController = ((Component)modPages[j / num].Find($"Button {j % num}")).gameObject.AddComponent<ButtonController>();
			buttons.Add(buttonController);
			buttonController.OnPressed = (Action<ButtonController, bool>)Delegate.Combine(buttonController.OnPressed, (Action<ButtonController, bool>)delegate(ButtonController obj, bool pressed)
			{
				((Behaviour)module).enabled = pressed;
				if (pressed)
				{
					helpText.text = module.GetDisplayName().ToUpper() + "\n\n" + module.Tutorial().ToUpper();
				}
			});
			module.button = buttonController;
			buttonController.SetText(module.GetDisplayName().ToUpper());
		}
		AddDebugButtons();
		foreach (Transform modPage in modPages)
		{
			foreach (Transform item in modPage)
			{
				Transform val2 = item;
				if (((Object)val2).name == "Button Left" && (Object)(object)modPage != (Object)(object)modPages[0])
				{
					ButtonController buttonController2 = ((Component)val2).gameObject.AddComponent<ButtonController>();
					buttonController2.OnPressed = (Action<ButtonController, bool>)Delegate.Combine(buttonController2.OnPressed, new Action<ButtonController, bool>(PreviousPage));
					buttonController2.SetText("Prev Page");
					buttons.Add(buttonController2);
				}
				else if (((Object)val2).name == "Button Right" && (Object)(object)modPage != (Object)(object)modPages[modPages.Count - 1])
				{
					ButtonController buttonController3 = ((Component)val2).gameObject.AddComponent<ButtonController>();
					buttonController3.OnPressed = (Action<ButtonController, bool>)Delegate.Combine(buttonController3.OnPressed, new Action<ButtonController, bool>(NextPage));
					buttonController3.SetText("Next Page");
					buttons.Add(buttonController3);
				}
				else if (!Object.op_Implicit((Object)(object)((Component)val2).GetComponent<ButtonController>()))
				{
					((Component)val2).gameObject.SetActive(false);
				}
			}
			((Component)modPage).gameObject.SetActive(false);
		}
		((Component)val).gameObject.SetActive(true);
		this.modPage = ((Component)val).gameObject;
	}

	private void AddDebugButtons()
	{
		AddDebugButton("Debug Log", delegate(ButtonController btn, bool isPressed)
		{
			Debugger = isPressed;
			Logging.Debug("Debugger", Debugger ? "active" : "inactive");
			Plugin.DebugText.text = "";
		});
		AddDebugButton("Close game", delegate(ButtonController btn, bool isPressed)
		{
			Debugger = isPressed;
			if (btn.text.text == "You sure?")
			{
				Application.Quit();
			}
			else
			{
				btn.text.text = "You sure?";
			}
		});
		AddDebugButton("Show Colliders", delegate(ButtonController btn, bool isPressed)
		{
			if (isPressed)
			{
				Collider[] array = Object.FindObjectsOfType<Collider>();
				for (int i = 0; i < array.Length; i++)
				{
					((Component)array[i]).gameObject.AddComponent<ColliderRenderer>();
				}
			}
			else
			{
				ColliderRenderer[] array2 = Object.FindObjectsOfType<ColliderRenderer>();
				for (int i = 0; i < array2.Length; i++)
				{
					((Component)(object)array2[i]).Obliterate();
				}
			}
		});
	}

	private void AddDebugButton(string title, Action<ButtonController, bool> onPress)
	{
		if (Plugin.DebugMode)
		{
			ButtonController buttonController = ((Component)modPages.Last().Find($"Button {debugButtons}")).gameObject.gameObject.AddComponent<ButtonController>();
			buttonController.OnPressed = (Action<ButtonController, bool>)Delegate.Combine(buttonController.OnPressed, onPress);
			buttonController.SetText(title);
			buttons.Add(buttonController);
			debugButtons++;
		}
	}

	public void PreviousPage(ButtonController button, bool isPressed)
	{
		button.IsPressed = false;
		pageIndex--;
		for (int i = 0; i < modPages.Count; i++)
		{
			((Component)modPages[i]).gameObject.SetActive(i == pageIndex);
		}
		modPage = ((Component)modPages[pageIndex]).gameObject;
	}

	public void NextPage(ButtonController button, bool isPressed)
	{
		button.IsPressed = false;
		pageIndex++;
		for (int i = 0; i < modPages.Count; i++)
		{
			((Component)modPages[i]).gameObject.SetActive(i == pageIndex);
		}
		modPage = ((Component)modPages[pageIndex]).gameObject;
	}

	public void SetupInteraction()
	{
		throwOnDetach = true;
		priority = 100;
		OnSelectExit = (Action<GrateInteractable, GrateInteractor>)Delegate.Combine(OnSelectExit, (Action<GrateInteractable, GrateInteractor>)delegate
		{
			AddBlockerToAllButtons(ButtonController.Blocker.MENU_FALLING);
			docked = false;
		});
		OnSelectEnter = (Action<GrateInteractable, GrateInteractor>)Delegate.Combine(OnSelectEnter, (Action<GrateInteractable, GrateInteractor>)delegate
		{
			RemoveBlockerFromAllButtons(ButtonController.Blocker.MENU_FALLING);
		});
	}

	public Material GetMaterial(string name)
	{
		Renderer[] array = Object.FindObjectsOfType<Renderer>();
		foreach (Renderer val in array)
		{
			if (((Object)val.material).name.ToLower().Contains(name))
			{
				return val.material;
			}
		}
		return null;
	}

	public void AddBlockerToAllButtons(ButtonController.Blocker blocker)
	{
		foreach (ButtonController button in buttons)
		{
			button.AddBlocker(blocker);
		}
	}

	public void RemoveBlockerFromAllButtons(ButtonController.Blocker blocker)
	{
		foreach (ButtonController button in buttons)
		{
			button.RemoveBlocker(blocker);
		}
	}

	public static void BindConfigEntries()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		try
		{
			ConfigDescription val = new ConfigDescription("Which button you press to open the menu", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[4] { "gesture", "stick", "a/x", "b/y" }), Array.Empty<object>());
			_summonInput = Plugin.ConfigFile.Bind<string>("General", "open menu", "gesture", val);
			ConfigDescription val2 = new ConfigDescription("Which hand can open the menu", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[2] { "left", "right" }), Array.Empty<object>());
			_summonInputHand = Plugin.ConfigFile.Bind<string>("General", "open hand", "right", val2);
			ConfigDescription val3 = new ConfigDescription("Which Theme Should Grate Use?", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[5] { "grate", "OldGrate", "bark", "holowpurple", "Player" }), Array.Empty<object>());
			_theme = Plugin.ConfigFile.Bind<string>("General", "theme", "Grate", val3);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
