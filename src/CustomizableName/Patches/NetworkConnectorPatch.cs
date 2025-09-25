using HarmonyLib;
using Photon.Pun;

namespace CustomizableName.Patches {
	[HarmonyPatch(typeof(NetworkConnector))]
	internal class NetworkConnectorPatch {
		[HarmonyPatch("Start"), HarmonyPostfix]
		public static void Start_Postfix(NetworkConnector __instance) {
			PhotonNetwork.NickName = NetworkConnector.GetUsername();
		}


		[HarmonyPatch("GetUsername"), HarmonyPrefix]
		public static bool GetUsername_Prefix(ref string __result) {
			__result = Plugin.Config.DisplayName;
			return false;
		}
	}
}
