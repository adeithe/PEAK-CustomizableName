using HarmonyLib;
using Peak.Network;
using Photon.Pun;

namespace CustomizableName.Patches {
	[HarmonyPatch(typeof(NetworkConnector))]
	internal class NetworkConnectorPatch {
		[HarmonyPatch("Start"), HarmonyPostfix]
		public static void Start_Postfix(NetworkConnector __instance) {
			PhotonNetwork.NickName = NetworkingUtilities.GetUsername();
		}

		[HarmonyPatch(typeof(NetworkingUtilities), "GetUsername"), HarmonyPrefix]
		public static bool GetUsername_Prefix(ref string __result) {
			__result = Plugin.Config.DisplayName;
			return false;
		}
	}
}
