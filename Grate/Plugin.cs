using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Configuration;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaTag;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Modules;
using Grate.Networking;
using Grate.Tools;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace Grate;

[BepInDependency(/*Could not decode attribute arguments.*/)]
[BepInPlugin("com.kylethescientist.graze.gorillatag.Grate", "Grate", "1.8.7")]
public class Plugin : BaseUnityPlugin
{
	[CompilerGenerated]
	private sealed class _003CJoinLobbyInternal_003Ed__33 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string lobbyName;

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
		public _003CJoinLobbyInternal_003Ed__33(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.RoomName == lobbyName)
				{
					NetworkSystem.Instance.ReturnToSinglePlayer();
				}
				_003C_003E2__current = (object)new WaitForSeconds(3f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				((PhotonNetworkController)PhotonNetworkController.Instance).AttemptToJoinSpecificRoom(lobbyName, (JoinType)0);
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CJоοin_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Plugin _003C_003E4__this;

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
		public _003CJоοin_003Ed__31(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			int num = _003C_003E1__state;
			Plugin plugin = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				plugin.Cleanup();
				_003C_003E2__current = (object)new WaitForSeconds(1f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (NetworkSystem.Instance.InRoom)
				{
					if (NetworkSystem.Instance.GameModeString.Contains("MODDED_"))
					{
						WaWaGrazeDotCc = true;
						plugin.Setup();
					}
					else
					{
						WaWaGrazeDotCc = false;
						plugin.Cleanup();
					}
				}
				else
				{
					WaWaGrazeDotCc = false;
					plugin.Cleanup();
				}
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public static Plugin? Instance;

	public static bool Initialized;

	public static bool WaWaGrazeDotCc;

	public static AssetBundle? AssetBundle;

	public static MenuController? MenuController;

	private static GameObject? monkeMenuPrefab;

	public static ConfigFile? ConfigFile;

	public static bool LocalPlayerSupporter;

	public static bool LocalPlayerDev;

	public static bool LocalPlayerAdmin;

	public static Text? DebugText;

	private GestureTracker? gt;

	private NetworkPropertyHandler? nph;

	public static bool IsSteam { get; private set; }

	public static bool DebugMode { get; protected set; }

	private void Awake()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		Logging.Init();
		Instance = this;
		HarmonyPatches.ApplyHarmonyPatches();
		ConfigFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "Grate.cfg"), true);
		foreach (MethodInfo item in (from moduleType in GrateModule.GetGrateModuleTypes()
			select moduleType.GetMethod("BindConfigEntries") into info
			select (info)).OfType<MethodInfo>())
		{
			item.Invoke(null, null);
		}
		GorillaTagger.OnPlayerSpawned((Action)OnGameInitialized);
		AssetBundle = AssetUtils.LoadAssetBundle("Grate/Resources/gratebundle");
		AssetBundle? assetBundle = AssetBundle;
		monkeMenuPrefab = ((assetBundle != null) ? assetBundle.LoadAsset<GameObject>("Bark Menu") : null);
		((Object)monkeMenuPrefab).name = "Grate Menu";
		Grate.GUI.MenuController.BindConfigEntries();
	}

	public void Setup()
	{
		gt = ((Component)this).gameObject.GetOrAddComponent<GestureTracker>();
		nph = ((Component)this).gameObject.GetOrAddComponent<NetworkPropertyHandler>();
		GameObject obj = Object.Instantiate<GameObject>(monkeMenuPrefab);
		MenuController = ((obj != null) ? obj.AddComponent<MenuController>() : null);
		LocalPlayerDev = NetworkSystem.Instance.LocalPlayer.IsDev();
		LocalPlayerAdmin = NetworkSystem.Instance.LocalPlayer.IsAdmin();
		LocalPlayerSupporter = NetworkSystem.Instance.LocalPlayer.IsSupporter();
	}

	public void Cleanup()
	{
		try
		{
			MenuController? menuController = MenuController;
			if (menuController != null)
			{
				((Component)menuController).gameObject.Obliterate();
			}
			((Component)(object)gt)?.Obliterate();
			((Component)(object)nph)?.Obliterate();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void CreateDebugGUI()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Object.op_Implicit((Object)(object)GTPlayer.Instance))
			{
				Canvas componentInChildren = ((Component)((Component)GTPlayer.Instance.headCollider).transform).GetComponentInChildren<Canvas>();
				if (!Object.op_Implicit((Object)(object)componentInChildren))
				{
					componentInChildren = new GameObject("~~~Grate Debug Canvas").AddComponent<Canvas>();
					componentInChildren.renderMode = (RenderMode)2;
					((Component)componentInChildren).transform.SetParent(((Component)GTPlayer.Instance.headCollider).transform);
					((Component)componentInChildren).transform.localPosition = Vector3.forward * 0.35f;
					((Component)componentInChildren).transform.localRotation = Quaternion.identity;
					((Component)componentInChildren).transform.localScale = Vector3.one;
					((Component)componentInChildren).gameObject.AddComponent<CanvasScaler>();
					((Component)componentInChildren).gameObject.AddComponent<GraphicRaycaster>();
					((Transform)((Component)componentInChildren).GetComponent<RectTransform>()).localScale = Vector3.one * 0.035f;
					Text obj = new GameObject("~~~Text").AddComponent<Text>();
					((Component)obj).transform.SetParent(((Component)componentInChildren).transform);
					((Component)obj).transform.localPosition = Vector3.zero;
					((Component)obj).transform.localRotation = Quaternion.identity;
					((Component)obj).transform.localScale = Vector3.one;
					((Graphic)obj).color = Color.green;
					obj.fontSize = 24;
					obj.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
					obj.alignment = (TextAnchor)4;
					obj.horizontalOverflow = (HorizontalWrapMode)1;
					obj.verticalOverflow = (VerticalWrapMode)1;
					((Graphic)obj).color = Color.white;
					((Transform)((Component)obj).GetComponent<RectTransform>()).localScale = Vector3.one * 0.02f;
					DebugText = obj;
				}
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnGameInitialized()
	{
		((MonoBehaviour)this).Invoke("DelayedSetup", 2f);
	}

	private void DelayedSetup()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			Logging.Debug("OnGameInitialized");
			Initialized = true;
			PlatformTagJoin val = (PlatformTagJoin)Traverse.Create((object)PlayFabAuthenticator.instance).Field("platform").GetValue();
			Logging.Info("Platform: ", val);
			IsSteam = val.PlatformTag.Contains("Steam");
			NetworkSystem instance = NetworkSystem.Instance;
			instance.OnJoinedRoomEvent = (DelegateListProcessorPlusMinus<DelegateListProcessor, Action>)(object)instance.OnJoinedRoomEvent + (Action)Аaа;
			NetworkSystem instance2 = NetworkSystem.Instance;
			instance2.OnReturnedToSinglePlayer = (DelegateListProcessorPlusMinus<DelegateListProcessor, Action>)(object)instance2.OnReturnedToSinglePlayer + (Action)Аaа;
			Application.wantsToQuit += Quit;
			if (DebugMode)
			{
				CreateDebugGUI();
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private bool Quit()
	{
		if (NetworkSystem.Instance.InRoom)
		{
			NetworkSystem instance = NetworkSystem.Instance;
			instance.OnReturnedToSinglePlayer = (DelegateListProcessorPlusMinus<DelegateListProcessor, Action>)(object)instance.OnReturnedToSinglePlayer + (Action)AQuit;
			NetworkSystem.Instance.ReturnToSinglePlayer();
			return false;
		}
		return true;
	}

	private void AQuit()
	{
		WaWaGrazeDotCc = false;
		Cleanup();
		((MonoBehaviour)this).Invoke("DelayQuit", 1f);
	}

	private void DelayQuit()
	{
		Application.Quit();
	}

	private void Аaа()
	{
		((MonoBehaviour)this).StartCoroutine(Jоοin());
	}

	[IteratorStateMachine(typeof(_003CJоοin_003Ed__31))]
	private IEnumerator Jоοin()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CJоοin_003Ed__31(0)
		{
			_003C_003E4__this = this
		};
	}

	public void JoinLobby(string lobbyName)
	{
		((MonoBehaviour)this).StartCoroutine(JoinLobbyInternal(lobbyName));
	}

	[IteratorStateMachine(typeof(_003CJoinLobbyInternal_003Ed__33))]
	private IEnumerator JoinLobbyInternal(string lobbyName)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CJoinLobbyInternal_003Ed__33(0)
		{
			lobbyName = lobbyName
		};
	}
}
