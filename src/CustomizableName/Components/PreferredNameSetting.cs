using PEAKLib.UI;
using Photon.Pun;
using Steamworks;
using TMPro;
using UnityEngine;
using Zorro.Core;
using Zorro.Settings;
using Zorro.Settings.UI;

namespace CustomizableName.Components {
	internal class PreferredNameSetting: StringSetting, IExposedSetting, IConditionalSetting {
		private static bool initialized = false;

		private PreferredNameSetting() : base() {}

		public override void Load(ISettingsSaveLoad loader) {
			Value = Plugin.Config.PreferredName.Value ?? "";
		}

		public override void Save(ISettingsSaveLoad saver) {
			Plugin.Config.DisplayName = Value.Trim();
		}

		public override void ApplyValue() { }

		protected override string GetDefaultValue() {
			return "";
		}

		public string GetDisplayName() {
			return "PREFERREDNAMESETTING";
		}

		public string GetCategory() {
			return "Accessibility";
		}

		public bool ShouldShow() {
			return !PhotonNetwork.InRoom;
		}

		public override GameObject GetSettingUICell() {
			GameObject existing = GameObject.Find("PreferredNameInputCell");
			if(existing != null) return existing;

			GameObject uiCell = Object.Instantiate(SingletonAsset<InputCellMapper>.Instance.FloatSettingCell);
			uiCell.name = "PreferredNameInputCell";

			FloatSettingUI old = uiCell.GetComponent<FloatSettingUI>();
			StringSettingUI ui = uiCell.AddComponent<StringSettingUI>();
			ui.inputField = old.inputField;

			Object.DestroyImmediate(old.slider.gameObject);
			Object.DestroyImmediate(old);

			ui.disable = PhotonNetwork.InRoom;
			ui.inputField.readOnly = ui.disable;
			ui.inputField.interactable = !ui.disable;
			ui.inputField.characterValidation = TMP_InputField.CharacterValidation.None;
			ui.inputField.characterLimit = 24;
			ui.inputField.contentType = TMP_InputField.ContentType.Standard;
			RectTransform rect = ui.inputField.GetComponent<RectTransform>();
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.anchorMin = new Vector2(0f, 0f);
			rect.anchorMax = new Vector2(1f, 1f);
			rect.offsetMin = new Vector2(20f, 15f);
			rect.offsetMax = new Vector2(-20f, -15f);
			rect.sizeDelta = new Vector2(-40f, -30f);

			TextMeshProUGUI[] labels = uiCell.GetComponentsInChildren<TextMeshProUGUI>(true);
			foreach(TextMeshProUGUI label in labels) {
				if(label.name == "Placeholder") label.text = SteamFriends.GetPersonaName();
				label.fontSize = label.fontSizeMin = label.fontSizeMax = 24f;
				label.alignment = TextAlignmentOptions.MidlineLeft;
			}
			return uiCell;
		}

		internal static void Initialize(Transform parent) {
			if(initialized) return;
			TranslationKey i18n = MenuAPI.CreateLocalization("PREFERREDNAMESETTING")
				.AddLocalization("Preferred Name", LocalizedText.Language.English)
				.AddLocalization("Nom préféré", LocalizedText.Language.French)
				.AddLocalization("Nome preferito", LocalizedText.Language.Italian)
				.AddLocalization("Bevorzugter Name", LocalizedText.Language.German)
				.AddLocalization("Nombre preferido", LocalizedText.Language.SpanishSpain)
				.AddLocalization("Nombre preferido", LocalizedText.Language.SpanishLatam)
				.AddLocalization("Nome preferido", LocalizedText.Language.BRPortuguese)
				.AddLocalization("Предпочтительное имя", LocalizedText.Language.Russian)
				.AddLocalization("Бажане ім'я", LocalizedText.Language.Ukrainian)
				.AddLocalization("首选名称", LocalizedText.Language.SimplifiedChinese)
				.AddLocalization("首選名稱", LocalizedText.Language.TraditionalChinese)
				.AddLocalization("希望する名前", LocalizedText.Language.Japanese)
				.AddLocalization("선호하는 이름", LocalizedText.Language.Korean)
				.AddLocalization("Preferowana nazwa", LocalizedText.Language.Polish)
				.AddLocalization("Tercih edilen isim", LocalizedText.Language.Turkish);
			SettingsHandler.Instance.AddSetting(new PreferredNameSetting());
			initialized = true;
		}
	}
}
