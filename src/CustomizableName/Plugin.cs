using BepInEx;
using BepInEx.Logging;
using CustomizableName.Components;
using CustomizableName.Configuration;
using HarmonyLib;
using PEAKLib.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CustomizableName;

[BepInAutoPlugin]
[BepInDependency(UIPlugin.Id)]
public partial class Plugin: BaseUnityPlugin {
	new internal static ConfigManager Config = new ConfigManager();
	new internal static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(Name);
	internal static readonly BepInPlugin Metadata = MetadataHelper.GetMetadata(typeof(Plugin));
	
	private Harmony harmony = new Harmony(Id);
	private FileSystemWatcher watcher = new FileSystemWatcher {
		Path = Paths.ConfigPath,
		Filter = "Adeithe-CustomizableName.cfg",
		NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
	};
	
	private void Awake() {
		Type[] types = Assembly.GetExecutingAssembly().GetTypes();
		List<Type> patches = types.Where(t => t.Namespace == $"{GetType().Namespace}.Patches" && t.Name.EndsWith("Patch")).ToList();
		patches.ForEach(harmony.PatchAll);
		watcher.Changed += (_, _) => Config.Reload();
		Logger.LogInfo($"Loaded {this.Info.Metadata.Name} v{this.Info.Metadata.Version}");
	}

	private void Start() {
		MenuAPI.AddToSettingsMenu(PreferredNameSetting.Initialize);
	}

	private void OnDestroy() {
		watcher.Dispose();
		harmony.UnpatchSelf();
	}
}
