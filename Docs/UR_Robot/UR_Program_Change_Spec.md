# UR Robot Program Change Spec – A300 Shell Assembly (HMI V1.4.0.0)

For: the engineer who edits the UR program on the teach pendant, and the PLC engineer.
Program: `A300 Shell 05092022.urp` + `default.installation` (PolyScope 5.12.1, e-Series).
Purpose: MT_001 recipes EBM, PC Clear, PC Amber, PC Red, PC Green, each with its own robot points.

MT_002 (HMI Robot page) is cancelled. Points are taught on the pendant, the same way as today.
The earlier design (HMI-taught general variables) is kept in `Archive/MT_002_RobotPage/`.

## 1. How the recipe reaches the robot

1. Operator selects the recipe on Lot Entry and presses New Lot.
2. HMI writes the recipe ID (EM1559), recipe name (EM1623) and the recipe's **robot point set**
   number to the new address `HMI_Robot_PointSet` **EM4250** (2W, EM4250–EM4251).
3. PLC moves EM4250 to W0102 (`shell_int`, robot int input 1).
4. Robot `Switch shell_int` builds `screwHeightList` / `screwEntryList` from that point set.

The recipe ID and the point set are separate numbers, so recipe IDs can grow (copies get 6, 7 …)
and two recipes can share one point set. The point set of each recipe is set on the HMI Recipe page.

| Recipe (HMI default) | Robot point set = `shell_int` | Robot waypoints |
|---|---|---|
| EBM | 0 | `Point_*b` (today's black, unchanged) |
| PC Clear | 1 | `Point_t*` (today's transparent, unchanged) |
| PC Amber | 2 | `Point_a*` (new) |
| PC Red | 3 | `Point_r*` (new) |
| PC Green | 4 | `Point_g*` (new) |

## 2. PLC change

1. New label `HMI_Robot_PointSet`, 2W signed integer, **EM4250**.
2. New rung: `MOV.D EM4250 → W0102`, always on (e.g. contact CR2002). The HMI writes EM4250 only at
   New Lot, so W0102 changes only then.
3. Rungs 057/058 (Shell_Colour_Black / _Transparent → W0102) are no longer triggered: the HMI does not
   set MR4500/MR4501 any more. Delete or disable them so only one rung writes W0102.
4. After a PLC restart EM4250 keeps its value if EM is latched; otherwise the robot gets 0 (black)
   until the next New Lot. Check the EM latch setting.

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

Only the point lists change per case. The screw sequence stays one set for all recipes: it already
moves to `screwEntryList[ScrewNum]` and `screwHeightList[ScrewNum]`, so it is **not** changed or copied.

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
