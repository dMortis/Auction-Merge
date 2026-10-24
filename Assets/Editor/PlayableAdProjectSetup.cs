using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

/// <summary>
/// Applies and reports the shared portrait-mobile and WebGL baseline for this project.
/// Run the apply command from the Tools menu after opening the project in Unity.
/// </summary>
public static class PlayableAdProjectSetup
{
    private static readonly NamedBuildTarget WebTarget = NamedBuildTarget.WebGL;

    [MenuItem("Tools/Playable Ad/Apply Portrait + WebGL Baseline")]
    private static void ApplyBaseline()
    {
        // Mobile builds launch in portrait and cannot rotate into a layout the playable does not support.
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.allowedAutorotateToPortrait = true;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = false;
        PlayerSettings.allowedAutorotateToLandscapeRight = false;

        // Gzip with Unity's fallback works on static hosts that do not serve precompressed files
        // with Content-Encoding headers. Switch to Brotli and disable fallback once hosting is configured.
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.stripEngineCode = true;
        PlayerSettings.SetManagedStrippingLevel(WebTarget, ManagedStrippingLevel.Medium);

        AssetDatabase.SaveAssets();
        ReportBaseline();
        Debug.Log("Playable ad portrait and WebGL baseline saved. Reopen Project Settings to confirm persisted values.");
    }

    [MenuItem("Tools/Playable Ad/Report Build Settings")]
    private static void ReportBaseline()
    {
        Debug.Log(
            "Playable ad build settings\n" +
            $"Active build target: {EditorUserBuildSettings.activeBuildTarget}\n" +
            $"Mobile orientation: {PlayerSettings.defaultInterfaceOrientation}\n" +
            $"WebGL compression: {PlayerSettings.WebGL.compressionFormat}\n" +
            $"WebGL decompression fallback: {PlayerSettings.WebGL.decompressionFallback}\n" +
            $"WebGL data caching: {PlayerSettings.WebGL.dataCaching}\n" +
            $"WebGL strip engine code: {PlayerSettings.stripEngineCode}\n" +
            $"WebGL managed stripping: {PlayerSettings.GetManagedStrippingLevel(WebTarget)}\n" +
            $"Frame rate in current Editor session: {Application.targetFrameRate}, vSync: {QualitySettings.vSyncCount}");
    }
}
