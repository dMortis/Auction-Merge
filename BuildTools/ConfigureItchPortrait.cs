// Run through Unity CLI script eval_file in the open Editor.
if (UnityEditor.EditorApplication.isPlaying || UnityEditor.EditorApplication.isCompiling)
    throw new System.InvalidOperationException("Wait until the Editor is idle and out of Play mode.");
if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty)
    throw new System.InvalidOperationException("Save the scene before building.");

var before = new {
    width = UnityEditor.PlayerSettings.defaultWebScreenWidth,
    height = UnityEditor.PlayerSettings.defaultWebScreenHeight,
    template = UnityEditor.PlayerSettings.WebGL.template,
    compression = UnityEditor.PlayerSettings.WebGL.compressionFormat.ToString(),
    fallback = UnityEditor.PlayerSettings.WebGL.decompressionFallback,
    development = UnityEditor.EditorUserBuildSettings.development
};
System.IO.Directory.CreateDirectory("BuildTools");
var backup = "BuildTools/itch-previous-web-settings.json";
if (!System.IO.File.Exists(backup))
    System.IO.File.WriteAllText(backup, Newtonsoft.Json.JsonConvert.SerializeObject(before, Newtonsoft.Json.Formatting.Indented));

UnityEditor.AssetDatabase.Refresh();
UnityEditor.PlayerSettings.defaultWebScreenWidth = 450;
UnityEditor.PlayerSettings.defaultWebScreenHeight = 800;
UnityEditor.PlayerSettings.WebGL.template = "PROJECT:ItchPortrait";
UnityEditor.PlayerSettings.WebGL.compressionFormat = UnityEditor.WebGLCompressionFormat.Gzip;
UnityEditor.PlayerSettings.WebGL.decompressionFallback = true;
UnityEditor.EditorUserBuildSettings.development = false;
UnityEditor.EditorUserBuildSettings.connectProfiler = false;
UnityEditor.EditorUserBuildSettings.allowDebugging = false;
UnityEditor.AssetDatabase.SaveAssets();

return new {
    target = UnityEditor.EditorUserBuildSettings.activeBuildTarget.ToString(),
    width = UnityEditor.PlayerSettings.defaultWebScreenWidth,
    height = UnityEditor.PlayerSettings.defaultWebScreenHeight,
    template = UnityEditor.PlayerSettings.WebGL.template,
    compression = UnityEditor.PlayerSettings.WebGL.compressionFormat.ToString(),
    fallback = UnityEditor.PlayerSettings.WebGL.decompressionFallback,
    development = UnityEditor.EditorUserBuildSettings.development
};
