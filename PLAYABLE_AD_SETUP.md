# Vertical playable ad project setup

The project uses Unity 6000.6.3f1 and URP 2D. The current enabled build scene is `Assets/Scenes/SampleScene.unity`; there is no playable-ad gameplay yet.

## Apply the project baseline

Open the project in Unity and choose **Tools > Playable Ad > Apply Portrait + WebGL Baseline**. This editor command saves portrait orientation for mobile builds and a conservative WebGL baseline: Gzip compression with decompression fallback, browser data caching, engine-code stripping, and Medium managed stripping. It then logs the stored settings. Choose **Tools > Playable Ad > Report Build Settings** to inspect them again.

Gzip plus fallback keeps the first browser build compatible with static hosts that do not set compression headers. Once the portfolio host is confirmed to serve precompressed Brotli files over HTTPS with `Content-Encoding: br`, change WebGL compression to Brotli and turn fallback off to reduce payload and startup cost.

## Vertical layout and browser delivery

- Design the play area around a 9:16 safe frame. Keep essential controls away from the top and bottom edges; mobile builds must respect `Screen.safeArea`.
- Make the game view adapt to tall phones and short browser embeds while keeping the play area readable. On wider pages, letterbox the 9:16 game area rather than stretching it.
- Use touch input that also accepts mouse input so the same interaction works in mobile browsers and desktop portfolio previews.
- Keep the initial WebGL download small; profile a real browser build before moving assets or changing stripping further.
- For the portfolio embed, serve the WebGL output over HTTPS and configure the site to serve compressed files with matching `Content-Encoding` and WebAssembly MIME headers. Test on iOS Safari as well as a Chromium browser.
- The WebGL target is not selected automatically by the editor command. Install the WebGL Build Support module for Unity 6000.6.3f1 and select WebGL in Build Profiles when ready to build.

The `Assets/Editor/HubForceResolve.cs` bootstrap currently resolves packages once and then deletes its own tracked source file. Check version control after the first project open and restore that file if it was removed unintentionally.
