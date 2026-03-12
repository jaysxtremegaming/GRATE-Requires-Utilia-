using HarmonyLib;

namespace Grate;

public class HarmonyPatches
{
	public const string InstanceId = "com.kylethescientist.graze.gorillatag.Grate";

	private static Harmony instance;

	public static bool IsPatched { get; private set; }

	internal static void ApplyHarmonyPatches()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		if (!IsPatched)
		{
			if (instance == null)
			{
				instance = new Harmony("com.kylethescientist.graze.gorillatag.Grate");
			}
			IsPatched = true;
		}
	}

	internal static void RemoveHarmonyPatches()
	{
		if (instance != null && IsPatched)
		{
			instance.UnpatchSelf();
			IsPatched = false;
		}
	}
}
