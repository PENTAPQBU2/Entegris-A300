# Entegris A300 Shell Assembly – HMI

HMI software (C# WPF, .NET Framework 4.8) for the Entegris A300 Shell Assembly machine.
It talks to a Keyence KV-7500 PLC; the PLC drives a UR e-Series robot over EtherNet/IP.

## Branches

Each version has its own branch, named after the version number.

| Branch      | Content |
|-------------|---------|
| `V1.3.13.0` | Original program as delivered. Kept unchanged as a backup. |
| `V1.4.0.0`  | Customer change request MT_001 + MT_003 (in development). |
| `master`    | Default branch, same as `V1.3.13.0`. New work is not merged here. |

## Build and run

- Visual Studio, solution `HMI_Master_KX7500.sln`, configuration **x86** (Debug or Release).
- Running on an office PC without PLC/robot: see [Docs/Offline_Simulation.md](Docs/Offline_Simulation.md).

## Adding a robot point set for a new shell type

1. Copy `default.installation` from the robot's programs folder to a USB stick.
2. HMI → Recipe → Robot point sets: select the file, the set to copy from, a new set number, a name and a
   pendant prefix (e.g. `Amb`), then **Create point set**. The HMI adds `Amb_P01 … Amb_P33` to the file and
   writes `recipe_points.script` next to it (original kept in `<HMI folder>\RobotBackup\`).
3. Copy both files back to the robot's programs folder, load the installation and the program.
4. Teach the 33 points on the pendant (P1–P21 screw 1–21, P22–P33 entry above screw 1, 2, 3, 7, 8, 10, 16–21).
5. Give the recipe the new point set number on the Recipe page.

The one-time robot program change and the PLC rung are in
[Docs/UR_Robot/UR_Program_Change_Spec.md](Docs/UR_Robot/UR_Program_Change_Spec.md).

## Changelog

The same changes are listed for operators in `HMI_Master_KX7500/VersionHistory.txt`
and in [CHANGELOG.md](CHANGELOG.md).

### V1.4.0.0 – in development (branch `V1.4.0.0`)

Customer change request MT_001 (recipe) and MT_003 (shell log per lot). MT_002 (robot page) is
cancelled by the customer; its code is kept in `Archive/MT_002_RobotPage/`. Items are added here as they are completed.

**Added**
- Offline / simulation mode (`OfflineMode` in `MachineConfig.xml`). The HMI runs without PLC,
  screwdriver COM port or Camline TCP server; PLC reads/writes go to an in-memory simulated PLC and
  the version label shows `[OFFLINE]`.
- `PLCIPAddress` and `PLCPort` settings in `MachineConfig.xml` (were hard-coded `192.168.0.10:8500`).
- `UseKVStudioSimulator` setting (value passed to KV DataBuilder `KVSimulator`, default `true` as before).
- `FoupCsvFolder` setting for the torque CSV log folder.
- `Docs/Offline_Simulation.md` – how to run the HMI on an office PC.
- **Recipe page (MT_001)** – menu "Recipe", not for Operator: list of recipes (ID, name, robot point set),
  save name and robot point set, create a copy, delete (moved to `Recipe\Deleted`, blocked for the current
  lot recipe). Recipe files in `<HMI folder>\Recipe\`; five starting recipes (EBM, PC Clear, PC Amber,
  PC Red, PC Green) are copied there on first start.
- **Lot Entry recipe drop-down (MT_001)** – recipe chosen from the recipe files instead of typing a number.
  New Lot writes the recipe ID (EM1559), name (EM1623) and the recipe's robot point set to the new
  address `HMI_Robot_PointSet` EM4250. The PLC moves it to `shell_int` (W0102) and the robot uses that
  point set (taught on the pendant). The Black/Transparent buttons (MR4500/MR4501) are no longer used.
- **Robot point sets (MT_001)** – Recipe page, section "Robot point sets": select the robot's
  `default.installation` (USB copy), the set to copy from, a new set number, name and pendant prefix, then
  **Create point set**. The HMI adds the 33 points `<Prefix>_P01 … <Prefix>_P33` to the installation file
  (copies of the chosen set, in one block), writes `recipe_points.script` next to it with the point lists of
  every set, and keeps a copy of the original file in `<HMI folder>\RobotBackup\`. Point sets are listed in
  `MachineSettings\RobotPointSets.xml` (sets 0 Black and 1 Transparent pre-defined). **Write script only**
  regenerates the script. Saving a recipe warns when its point set does not exist yet.
- `Docs/UR_Robot/UR_Program_Change_Spec.md` – robot and PLC changes for the recipes: one-time robot program
  change (`recipe_points.script` Script node, `rcp_height(shell_int)` / `rcp_entry(shell_int)` instead of
  `Switch shell_int`), PLC rung EM4250 → W0102, steps for each new shell type.

**Changed**
- Torque CSV log falls back to `<HMI folder>\FOUP CSV\` when the `D:` drive does not exist
  (default on the machine is unchanged: `D:\HMI\FOUP CSV\`).
- Chinese menu names: Recipe 方剂 → 配方, Robot 机械 → 机器人.
- PLC connections created before `MachineConfig.xml` is loaded now connect on first use.

**Fixed**
- `Alarm.mdb` path was built before the folder path was set, so Debug builds read the Release
  folder. It is now read from `MachineSettings\Alarm.mdb` of the running build.

### V1.3.13.0 – 21/05/2026 (branch `V1.3.13.0`, original program)
- Remove the screw count for bowl 1 and bowl 2.
- Remove the popup notification for refill.

Older versions: see `HMI_Master_KX7500/VersionHistory.txt`.
