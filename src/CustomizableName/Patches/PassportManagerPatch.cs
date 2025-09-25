using HarmonyLib;

namespace CustomizableName.Patches {
	[HarmonyPatch(typeof(PassportManager))]
	internal class PassportManagerPatch {
		[HarmonyPatch("ToggleOpen"), HarmonyPrefix]
		public static bool ToggleOpen_Prefix(PassportManager __instance) {
			// TODO: Find a way to update name above head for everyone so we can move input here
			string characterName = Character.localCharacter.characterName;
			string passportNumber = PassportManager.GeneratePassportNumber(characterName);
			__instance.nameText.text = characterName;
			__instance.passportNumberText.text = passportNumber;
			return true;
		}
	}
}
