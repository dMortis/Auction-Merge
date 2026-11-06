# Locks and keys artwork

## Current lock redesign — 2026-10-28

The three current padlocks match the keys currently used in the project: a four-lobed/quatrefoil silhouette, smooth glossy cobalt-blue, emerald-green or ruby-red enamel, thick polished gold bevels, broad upper-left white highlights, and a large readable dark keyhole. The golden U-shaped shackle is closed, with a transparent opening.

Final files remain `Assets/_Game/Art/Items/LocksAndKeys/Lock_Blue.png`, `Lock_Green.png`, and `Lock_Red.png`. Each is a native 1254 x 1254 alpha PNG, generated with the built-in image generation tool and copied without cropping, resampling, recoloring or re-encoding. The Blue key was the style reference for the Blue lock; Green and Red were generated as edits of that Blue lock, using their respective key colors as references.

Existing Unity GUIDs and scene references are preserved. The key files were not modified. Imports are Single Sprite, Full Rect, 100 pixels per unit, Bilinear, Clamp, alpha transparency, no mipmaps, max size 2048 and uncompressed RGBA32 at full native resolution.

The old three lock PNGs and their original `.meta` files are archived outside Assets in `_Archive/Locks_Redesign_20261028`. Exact generation prompts and validation results are stored there as `Generation_Prompts.md` and `Verification.txt`.

## Open lock states — 2026-10-28

Matching open versions are available as `Lock_Blue_Open.png`, `Lock_Green_Open.png`, and `Lock_Red_Open.png` in the same LocksAndKeys folder. They use the same quatrefoil gold-and-enamel design with an attached left shackle leg and a raised, disconnected right tip. The open gap and arch interior have transparent alpha.

All three are native 1254 x 1254 PNGs, imported as centered Full Rect Single Sprites, 100 pixels per unit, RGBA32 without compression or mipmaps, Bilinear filtering and Clamp wrapping. They were generated with the built-in image generation tool, one call per asset, and copied without post-generation image processing. Closed locks, keys and their metadata were retained unchanged.

Exact prompts and verification are stored outside Assets in `_Archive/Locks_Open_20261028/Generation_Prompts.md` and `Verification.txt`. These are additional sprite assets; no scene state-switching logic was added.

## Historical generation record

The descriptions and prompts below document earlier designs and do not describe the current quatrefoil keys and locks.

Created using the built-in image generation tool, one generation per PNG. All six files are copied directly from the native 1254×1254 RGBA outputs without cropping, resampling or re-encoding.

Project folder: Assets/_Game/Art/Items/LocksAndKeys

Source folder: F:\Playable Ads\Auction Merge\Locks_Keys_Icons

| Color | Lock | Key |
| --- | --- | --- |
| Ruby red | Lock_Red.png | Key_Red.png |
| Emerald green | Lock_Green.png | Key_Green.png |
| Light blue / cyan | Lock_Blue.png | Key_Blue.png |

The family uses gold trim, matching enamel colors, upper-left highlights and the painterly 3D material style of Skeleton_Icons, Detective_Icons and Concert_Icons. Locks share an upright closed-shackle design; keys share a near-horizontal open-ring design. Both exterior backgrounds and the openings inside shackles/key bows are transparent.

Unity imports: single Sprite, Full Rect, 100 pixels per unit, centered pivot, bilinear filtering, clamp wrapping, alpha transparency, no mipmaps, uncompressed, max texture size 2048. The scene-building utility preserves uncompressed imports for this folder. No scene placement or gameplay logic is added by these assets.

## Reference images

- Skeleton_Icons/02_StarWand_L3_FinishedStarWand.png
- Detective_Icons/04_Magnifier_L3_FinishedMagnifier.png
- Concert_Icons/07_Microphone_L3_VintageMicrophone.png

## Final prompt set

### Lock_Red.png

Use case: stylized-concept. Asset: ONE standalone ruby-red closed padlock icon for the antique magician / storage-auction game. The three input images are STYLE REFERENCES ONLY: match their polished warm golden metal, jewel-rich colors, crisp beveled outlines and premium painterly 3D mobile-game rendering; do not reproduce their objects. Design a compact elegant closed padlock with a chunky rounded rectangular ruby-red enamel body, softly bowed lower edge, thick polished antique-gold U-shaped shackle, raised gold perimeter trim, two tiny gold rivets, restrained engraved corner flourishes. The red face must dominate and instantly read RED at tiny gameplay size, with bright vermilion highlights and deep ruby edge shading. Large clear dark keyhole centered on the body: round aperture and short narrow stem, set in a simple thin gold escutcheon. Slight three-quarter depth showing the right side while the face stays nearly frontal; shackle upright, no tilt. Clean appealing toy-like proportions, substantial thickness, beautifully designed but not cluttered. Light from upper left, smooth readable material shading with delicate hand-painted texture like the reference icons; detailed but no photorealistic grunge. Exactly one complete closed lock, centered on a square canvas, approximately 78 percent canvas height, comfortable transparent margin around every edge. Genuine transparent background including the open space inside the shackle; no floor, cast shadow outside the object, card, tile, frame around image, text, key, sparkles or extra objects. Highest available native resolution.

### Key_Red.png

Use case: stylized-concept. Create ONE standalone antique ruby-red KEY icon matching the padlock in reference image 1. Reference 1 controls the gold bevels, red enamel color, small elegant scroll ornament, warm upper-left lighting and painterly premium 3D mobile-game finish. Reference 2 supplies the polished icon rendering style only. Design a classic old-fashioned warded key, immediately recognizable as a key at small size: a large OPEN OVAL RING bow on the LEFT, a sturdy short straight shaft extending to the RIGHT, and a blocky stepped bit with TWO clear rectangular teeth at the right tip. The key is nearly horizontal, rising only about 12 degrees toward the right, full silhouette readable, slight 3D thickness. Wide oval ring has rich RUBY-RED enamel on its face with a thick fine beveled antique-gold border and a genuinely transparent open center. The metal shaft has an uninterrupted saturated red-enamel inset down its face, edged in polished gold; gold/red stepped teeth, two little gold collars at the neck. Small restrained scroll details tie it to the padlock. Red must be clearly prominent across the bow and shaft, not only one tiny jewel. Keep a compact chunky beautiful design, no excess filigree obscuring the key silhouette, no bow shaped like a padlock, no closed medallion in the ring opening. Premium crisp hand-painted 3D collectible icon, fine material detail and warm specular highlights matching existing game assets. Exactly ONE key centered on a SQUARE transparent canvas, roughly 82 percent of canvas width, generous transparent breathing room above and below, nothing clipped. No lock, ribbon, floor, cast shadow outside the object, card, tile, text, sparkles, extra keys or other objects. Highest available native detail.

### Lock_Green.png

Use case: precise-object-edit. Produce the GREEN color variant of the padlock in reference image 1, preserving its exact design, silhouette, size, view angle, gold shackle, gold border and ornaments, dark keyhole, texture detail, lighting, square canvas and transparent framing. Change ONLY the ruby-red enamel body and colored side surfaces to luminous saturated EMERALD / fresh GREEN enamel: bright leaf-green midtones around #38B91A, lighter jewel-green highlights, darker emerald edge shading. The green color must be instantly clear on small UI icons and cover the same large body areas as the original red. Keep all antique gold regions GOLD; do not turn gold green. Maintain the same premium hand-painted 3D mobile-game quality and crisp clean contours. Exactly one closed green padlock, same framing as the reference, genuinely transparent background including inside the shackle, no extra objects, text, cast shadow outside the object, sparkles, floor, card or tile.

### Lock_Blue.png

Use case: precise-object-edit. Produce the BLUE / CYAN color variant of the padlock in reference image 1, preserving its exact design, silhouette, size, view angle, gold shackle, gold border and ornaments, dark keyhole, texture detail, lighting, square canvas and transparent framing. Change ONLY the ruby-red enamel body and colored side surfaces to luminous saturated SKY-BLUE / CYAN enamel: azure midtones around #00AAF0, bright icy-cyan highlights and rich turquoise-blue edge shading. It must clearly read as LIGHT BLUE, not navy, purple or green, and cover the same large body areas as the original red. Keep all antique gold regions GOLD. Maintain the same premium hand-painted 3D mobile-game quality and crisp clean contours. Exactly one closed blue padlock, same framing as the reference, genuinely transparent background including inside the shackle, no extra objects, text, cast shadow outside the object, sparkles, floor, card or tile.

### Key_Green.png

Use case: precise-object-edit. Make the GREEN variant of the key in reference image 1. Preserve the exact key design and proportions, same open oval ring bow on the left, same short shaft, same two-tooth stepped bit, same nearly horizontal 12-degree orientation, gold scroll ornaments and gold collars, square canvas, object framing, upper-left highlights and high-detail premium painterly 3D rendering. Change ONLY all the ruby-red enamel in the bow, neck, shaft and bit to rich vivid GREEN jewel enamel matching the green padlock in reference image 2: leaf-green / emerald midtones around #38B91A, bright jewel-green highlights, deep emerald shadows. Keep all the gold regions GOLD. Both bow and shaft must clearly read GREEN at small UI size. Preserve the transparent opening inside the ring and genuinely transparent background around the complete isolated key. Exactly one key, no padlock, text, sparkles, shadows outside the object, floor, tile, card or extra objects. Do not alter silhouette or lose details.

### Key_Blue.png

Use case: precise-object-edit. Make the LIGHT BLUE / CYAN variant of the key in reference image 1. Preserve the exact key design and proportions, same open oval ring bow on the left, same short shaft, same two-tooth stepped bit, same nearly horizontal 12-degree orientation, gold scroll ornaments and gold collars, square canvas, object framing, upper-left highlights and high-detail premium painterly 3D rendering. Change ONLY all the ruby-red enamel in the bow, neck, shaft and bit to saturated luminous SKY-BLUE / CYAN jewel enamel matching the blue padlock in reference image 2: azure midtones around #00AAF0, icy cyan highlights, rich turquoise-blue shadows. It must clearly read LIGHT BLUE, not purple, navy or green. Keep all gold regions GOLD. Both bow and shaft must clearly read BLUE at small UI size. Preserve the transparent opening inside the ring and genuinely transparent background around the complete isolated key. Exactly one key, no padlock, text, sparkles, shadows outside the object, floor, tile, card or extra objects. Do not alter silhouette or lose details.

## Key redesign — 2026-10-26

The key sprites were revised after review. The selected first redesign keeps the polished, sculpted fantasy-antique bow that the user preferred, while lengthening the shaft and making the key read more naturally in the square item cells. The bow remains an open loop; the center is transparent. Red, blue and green share the same silhouette, angle, gold finish and tooth layout, with enamel and gemstone colors matched to their locks. The final PNGs replace the earlier keys at the same paths; Unity `.meta` files and GUIDs remain unchanged.

Final files: `Key_Red.png`, `Key_Blue.png`, `Key_Green.png`.

Redesign brief: Create a slightly longer antique warded key for a polished mobile game icon. Arrange one complete key diagonally from lower-left open oval bow to upper-right bit. Retain the user's preferred sculpted, decorative fantasy-antique bow from the first redesign, with a genuinely transparent open center, polished gold bevels, a small colored jewel at the neck and matching saturated enamel on the shaft and bit. Use a slender extended shaft, clear mechanically readable teeth, warm upper-left lighting and the same premium gold-and-enamel finish across all three variants. Keep red, cyan-blue and emerald-green colorways consistent with the matching locks. Use a transparent square canvas; no tile, frame, text, extra objects or external cast shadow.
