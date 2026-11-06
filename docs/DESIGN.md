# Storage Auction 3 — design breakdown

## Intent and contribution

Present a short, readable merge-to-reward loop in a portrait interface. Dima Rychka's contribution: game design, gameplay logic, sketches and storyboards that guided the gameplay.

## Decisions visible in the prototype

| Decision | Player-facing effect | Implementation evidence |
| --- | --- | --- |
| Start with two of the first collection's three deliveries complete | Brings the opening close to a visible collection reward | `MergeGameState` constructor |
| Use matching item chains with three levels | Gives repeated actions a clear upgrade direction | `CanMerge`, `MoveOrMerge`, `Deliver` |
| Show a key only after collection completion | Connects a completed goal to the next interaction | `KeyAvailable`, collection UI update |
| Demonstrate the next valid opening action with a hand hint | Offers help after inactivity | Hint selection in `MergeGameController.Update` |
| Use item flights, pulses and bursts | Marks accepted actions and reward transitions | Merge, delivery and unlock sequences |
| Check board capacity before unlocking | Prevents a reward sequence from consuming a key without space for its items | `Unlock` in `MergeGameState` |

These effects are design interpretations of the source; they are not measured user-study results.

## Suggested review sequence

1. Observe the initial board and attempt the first merge without instructions.
2. Wait for the hint, then complete and deliver the first upgraded item.
3. Follow the key interaction into the next collection.
4. Complete all three stages and inspect the final screen.

## Next validation

- Observe whether new players understand the first drag and the collection drop target.
- Compare the current partially completed opening with an opening that starts empty; measure time to the first reward and completion rate.
- Verify touch targets and portrait cropping on representative phones.
- For an actual playable-ad delivery, define the target network, add its required call to action and lifecycle integration, and measure the final export.

No engagement, conversion or performance figures are claimed for this prototype.
