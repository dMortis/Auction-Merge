# Collection progress sprite sheets

Each PNG contains eight transparent states arranged in a 4-column by 2-row grid. In Unity, import as **Sprite (2D and UI)** with **Sprite Mode: Multiple**, then slice by grid: 4 columns × 2 rows. Use the full panel rectangle for each sprite.

Panels are read left to right, top row first, then bottom row:

| Position | Found items |
| --- | --- |
| Row 1, Column 1 | None |
| Row 1, Column 2 | Item 1 |
| Row 1, Column 3 | Item 2 |
| Row 1, Column 4 | Item 3 |
| Row 2, Column 1 | Items 1 + 2 |
| Row 2, Column 2 | Items 1 + 3 |
| Row 2, Column 3 | Items 2 + 3 |
| Row 2, Column 4 | All three |

Item order per collection:

- Skeleton: top hat, star wand, stage cape.
- Detective: magnifying glass, jeweled bone, burglar mask.
- Concert: microphone, feather boa, theatrical fan.
