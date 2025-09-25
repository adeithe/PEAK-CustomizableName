using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using Photon.Pun;

namespace CustomizableName.Configuration {
	public sealed class ConfigManager {
		private static readonly string ConfigPath = Path.Combine(Paths.ConfigPath, "Adeithe-CustomizableName.cfg");
		private ConfigFile Config = new ConfigFile(ConfigPath, true, Plugin.Metadata);
		
		public ConfigEntry<string?> PreferredName;

		public string DisplayName {
			get => PreferredName.Value ?? Steamworks.SteamFriends.GetPersonaName();
			set {
				string steamName = Steamworks.SteamFriends.GetPersonaName();
				string? preferredName = String.IsNullOrWhiteSpace(value) ? steamName : value;
				if(steamName.Equals(value, StringComparison.OrdinalIgnoreCase)) preferredName = null;
				if(PhotonNetwork.IsConnected) PhotonNetwork.NickName = preferredName ?? steamName;
				PreferredName.Value = preferredName;
				if(preferredName != null) Plugin.Logger.LogInfo($"Set preferred name to '{preferredName}'");
			}
		}

		internal ConfigManager() {
			Config.SaveOnConfigSet = true;
			PreferredName = Config.Bind<string?>("General", "PreferredName", null, "The preferred name to use for your scout. If empty, your Steam name will be used instead.");
		}

		public void Reload() {
			Config.Reload();
		}
	}
}
