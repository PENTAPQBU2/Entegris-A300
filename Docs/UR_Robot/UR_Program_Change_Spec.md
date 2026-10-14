# UR Robot Program Change Spec – A300 Shell Assembly (HMI V1.4.0.0)

For: the engineer who edits the UR program on the teach pendant, and the PLC engineer.
Program: `A300 Shell 05092022.urp` + `default.installation` (PolyScope 5.12.1, e-Series).
Purpose: MT_001 recipes EBM, PC Clear, PC Amber, PC Red, PC Green, each with its own robot points.

MT_002 (HMI Robot page) is cancelled. Points are taught on the pendant, the same way as today.
The earlier design (HMI-taught general variables) is kept in `Archive/MT_002_RobotPage/`.

## 1. How the recipe reaches the robot (unchanged method)

1. Operator selects the recipe on Lot Entry and presses New Lot.
2. HMI writes the recipe ID (EM1559), recipe name (EM1623) and sets the recipe's shell colour bit.
3. PLC, on the rising edge of the bit, resets the other colour bits and moves the point set number
   to W0102 (`shell_int`, robot int input 1).
4. Robot `Switch shell_int` builds `screwHeightList` / `screwEntryList` from that recipe's points.

| Recipe (HMI default) | Colour bit set by HMI | W0102 / `shell_int` | PLC rung |
|---|---|---|---|
| EBM | MR4500 Shell_Colour_Black | 0 | exists (057) |
| PC Clear | MR4501 Shell_Colour_Transparent | 1 | exists (058) |
| PC Amber | MR4502 Shell_Colour_Orange | 2 | **new** |
| PC Red | MR4504 Shell_Colour_Red | 3 | **new** |
| PC Green | MR4503 Shell_Colour_Clear | 4 | **new** |

The colour bit of each recipe can be changed on the HMI Recipe page.

## 2. PLC change

If rungs for Orange, Red and Clear do not exist yet, copy rung 058 three times:

- Contact: rising edge of `Shell_Colour_Orange` / `Shell_Colour_Red` / `Shell_Colour_Clear`.
- RES the other four colour bits.
- `MOV.D #2` / `#3` / `#4` → `W0102`.

## 3. Robot program change

### 3.1 Before you start

1. Back up `A300 Shell 05092022.urp`, `default.installation`, `default.variables` to USB.
2. Save the edited program under a new name, e.g. `A300_Shell_V1_4.urp`; make it the default
   program only after the test in section 4 passes.

### 3.2 New waypoints (Installation → Features → Point), taught on the pendant

For each new point set, create the features below. Start every point from the **black** point of the
same screw (Point_Nb / Point_NbEntry): move to the black point, then create the new feature there.
Fine-tune later with the real shell.

| Set | Screw points | Entry points (screws 1, 2, 3, 7, 8, 10, 16–21) |
|---|---|---|
| 2 (PC Amber) | `Point_a1 … Point_a21` | `Point_a1Entry …` |
| 3 (PC Red) | `Point_r1 … Point_r21` | `Point_r1Entry …` |
| 4 (PC Green) | `Point_g1 … Point_g21` | `Point_g1Entry …` |

Screws 4, 5, 6, 9, 11, 12, 13, 14, 15 have no entry point today (the screw point is used); keep that.

### 3.3 Main_PLC – `Switch shell_int`

Keep Case 0 (black) and Case 1 (transparent) as they are. Add, with the same two lines as Case 0:

```
Case 2
  screwHeightList := [idx0, Point_a1, Point_a2, … , Point_a21]
  screwEntryList  := [idx0, Point_a1Entry, Point_a2Entry, Point_a3Entry, Point_a4, Point_a5, Point_a6,
                      Point_a7Entry, Point_a8Entry, Point_a9, Point_a10Entry, Point_a11, … , Point_a15,
                      Point_a16Entry, … , Point_a21Entry]
Case 3   (same with Point_r…)
Case 4   (same with Point_g…)
```

Note: Case 1 today uses the **black** `Point_10bEntry` for screw 10. Check whether that is wanted
for PC Clear.

Recommended: add a `Default Case` that halts with a popup "Unknown shell_int", so a missing PLC
rung cannot run with the previous recipe's points.

## 4. Test (reduced speed, doors closed)

For each recipe: New Lot on the HMI → check W0102 in KV Studio → run one shell at low speed
and check every screw and entry position → then full speed.
