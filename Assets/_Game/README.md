# Auction Merge — playable ad scene

A portrait Unity scene based on `Art/Backgrounds/Auction_Merge_Background.png`. The expanded room includes an empty 4×5 board and generous scenery beyond it. Balance and Collections are separate UI elements and are absent from this background. Tapping any board tile gives it a short gold highlight.

## Open the scene

Open the project in Unity 6.6.3f1 and open `Assets/_Game/Scenes/AuctionMerge_PlayableAd.unity`. The scene is included in Build Settings. To regenerate the scene and apply the portrait import settings, use **Auction Merge → Build playable ad scene** from the Unity menu.

## Project layout

- `Art/Backgrounds` — full-screen room and empty-board background used by the playable ad.
- `Art/UI/Collections` — reusable empty collection-card artwork with transparency outside the frame.
- `Art/Concept` — approved full-screen reference concept.
- `Art/Characters` — the three collection reward illustrations.
- `Art/Items` — individual item artwork grouped by collection.
- `Art/Items/LocksAndKeys` — six matching red, green and light-blue lock/key icons on transparent square canvases.
- `Art/ProgressStates` — transparent sprite sheets and high-quality collection states.
- `Scenes` — game scenes.
- `Prefabs/UI/Collections` — separate three-card Collections background, ready to place under a Canvas.
- `Prefabs/UI/BoardIcon.prefab` — a stretch-to-cell UI Image with Preserve Aspect and an alpha-aware drop shadow; change Source Image to choose an item.
- `Scripts` — runtime safe-area handling and tile feedback.
- `Editor` — scene setup utility.
- `Documentation` — source notes for progress-state art.

## Portrait and portfolio builds

The scene now includes Canvas/Background Content/BoardItems/Cell_01 … Cell_20 and Canvas/Safe Area/Collections with three empty cards. Use UI Image for board items, because Screen Space Overlay UI renders over ordinary SpriteRenderers. See `Documentation/AddingIconsAndShadows_RU.md` for step-by-step placement and shadow settings. The original scene before these UI changes is saved under Scenes/Backups.

The project defaults to portrait orientation (1080×1920) and uses a 1080×1920 Canvas Scaler reference. One continuous expanded background uses ResponsiveBoardBackground to keep the board approximately 96% of the viewport width while filling the screen. With Collections present, short viewports also constrain board height to leave room below it. The image preserves its aspect ratio, and its position is clamped so no uncovered edges appear. The board is front-facing, with a regular 4×5 grid; every interaction cell has the same dimensions. Excess scenery is cropped at the edges. The board stays inside the visible region for common portrait phone ratios from 9:16 through approximately 9:22; arbitrary extreme viewports can crop the board. The Collections row fits the space between the board feet and the bottom safe area. The same scene is suitable for a portrait WebGL embed in a portfolio page. Select Android, iOS, or WebGL in Unity Build Profiles when preparing a platform build.

The background PNG is 971×1620 and the card PNG is 997×1578, at the image generator's native output resolution. Both use uncompressed Unity texture import settings, no mipmaps and a 4096 import limit to avoid additional compression or downscaling. These are illustrated redraws rather than pixel-identical retouches. See `Documentation/Background_Collections_Art.md` for generation prompts and asset details.

## Source artwork

The source files were copied from `F:\Playable Ads\Auction Merge` into this project's `Assets/_Game/Art` tree. The high-quality progress illustrations are full-screen character renders on transparency; the progress-state sprite sheets are preserved separately with their source slicing notes.
