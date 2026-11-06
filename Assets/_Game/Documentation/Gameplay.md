# Merge gameplay

Scene: `Assets/_Game/Scenes/StorageAuction3_PlayableAd.unity`.
Press Play to start a fresh session. No progress is persisted between sessions.

## Scene and configuration

`MergeGameplay` holds `MergeGameController`. Its Inspector contains the 20 existing
board cells, all nine three-level sprite chains, the three collections, complete
dot sprites, open locks, the tutorial hand and animation timings.
All references are serialized. Runtime code does not search by filenames.
The authored initial visuals are preserved: no sprites, active states, transforms
or layout settings are changed at startup. Pointer handlers and the transparent
effects layer are attached at runtime.

Initial cards: Cell_01, Cell_07, Cell_09, Cell_12. Initial locks: blue Cell_20,
green Cell_17, red Cell_04. Inactive placeholder sprites in other cells are empty.
The initial skeleton cape and wand are already complete.

## Rules

- Drag to an empty cell to move an item.
- Only equal chain AND equal level can merge. Two level-1 items make one level-2;
  two level-2 items make one level-3. Level-3 items cannot merge further.
- Drop a level-3 item inside the corresponding collection artwork to deliver it.
  Premature, wrong-collection, mismatched and outside drops return the item.
- Skeleton: deliver TopHat; deactivate its overlay, complete all blue dots,
  open the compartment and expose the blue key.
- Detective: deliver Magnifier, JeweledBone, BurglarMask, in any order. Deactivate
  each corresponding overlay and complete its green dot. All three unlock the key.
- Concert: deliver Microphone, TheatricalFan, FeatherBoa, in any order. Activate
  each corresponding layer and complete its red dot. All three unlock the key.
- FeatherBoa uses BlueFeather → CabaretMask → FinishedFeatherBoa. The source brief
  repeated CabaretMask at L3 and mentioned L1 for delivery; the distinct L3 asset
  is used consistently with the other chains.
- Keys open only their matching lock, once. Open locks remain on their cells.
- Blue key: 4 PawprintEvidenceCard, 4 SafeKey, 4 Lockpick. Flight origin Cell_16.
- Green key: 4 SheetMusic, 4 TheatreTicket, 4 BlueFeather. Flight origin green lock.
- The 12 destinations are shuffled free cells, reserved before animation begins.
  No occupied item or lock is overwritten. Capacity is checked before consuming a key.
- Red key: victory overlay and `onGameCompleted` event. Further gameplay is disabled.

The hand demonstrates a currently available merge, then the finished hat delivery,
then the blue-key drop. It hides during dragging/animations, resumes after idle time,
and never appears in Detective or Concert.

`MergeGameState` contains rules independently of Unity UI. `MergeDragSource`
receives uGUI mouse/touch pointer events; `MergeGameController` handles presentation.
Input is locked during short resolution animations to avoid duplicate deliveries.

## Verification and recovery

`_Archive/Gameplay_20261028/SceneBefore.unity` is the original scene backup.
`TestRules.cs` exercises 100 seeded complete games and rejects invalid moves.
`TestUI.cs` runs a fresh Play session via uGUI pointer handlers, checks actual
raycast hits and verifies all transitions through the victory screen.
Reports are saved beside these scripts outside Assets.
`TestInputSystem.cs` also checks mouse down, movement over separate frames and
release through the actual Input System UI module, producing a crystal-ball merge.

No existing scene object was replaced. The source scene comparison confirmed
every original serialized object remained unchanged when gameplay was attached.
