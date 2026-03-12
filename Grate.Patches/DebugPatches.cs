namespace Grate.Patches;

public static class DebugPatches
{
	public static string[] ignoreables = new string[5] { "JoinRandomRoom failed.", "PhotonView does not exist!", "Photon.Pun.PhotonHandler.DelGloves ()", "Photon.Voice.Unity.VoiceConnection.DelGloves ()", "GorillaNetworking.PhotonNetworkController.DisconnectCleanup ()" };

	public static bool Ignore(string s)
	{
		string[] array = ignoreables;
		foreach (string value in array)
		{
			if (s.Contains(value))
			{
				return true;
			}
		}
		return false;
	}
}
