# Entegris A300 Shell Assembly – HMI

HMI software (C# WPF, .NET Framework 4.8) for the Entegris A300 Shell Assembly machine.
It talks to a Keyence KV-7500 PLC; the PLC drives a UR e-Series robot over EtherNet/IP.

## Branches

Each version has its own branch, named after the version number.

| Branch      | Content |
|-------------|---------|
| `V1.3.13.0` | Original program as delivered. Kept unchanged as a backup. |
| `V1.4.0.0`  | Customer change request MT_001 – MT_010 (in development). |
| `master`    | Default branch, same as `V1.3.13.0`. New work is not merged here. |

## Build and run

- Visual Studio, solution `HMI_Master_KX7500.sln`, configuration **x86** (Debug or Release).
- Running on an office PC without PLC/robot: see [Docs/Offline_Simulation.md](Docs/Offline_Simulation.md).

## Changelog

The same changes are listed for operators in `HMI_Master_KX7500/VersionHistory.txt`
and in [CHANGELOG.md](CHANGELOG.md).

### V1.4.0.0 – in development (branch `V1.4.0.0`)

Customer change request MT_001 – MT_010 (recipe, robot page, shell CSV log, lot/initialize,
hard reset, start button). Items are added here as they are completed.

**Added**
- Offline / simulation mode (`OfflineMode` in `MachineConfig.xml`). The HMI runs without PLC,
  screwdriver COM port or Camline TCP server; PLC reads/writes go to an in-memory simulated PLC and
  the version label shows `[OFFLINE]`.
- `PLCIPAddress` and `PLCPort` settings in `MachineConfig.xml` (were hard-coded `192.168.0.10:8500`).
- `UseKVStudioSimulator` setting (value passed to KV DataBuilder `KVSimulator`, default `true` as before).
- `FoupCsvFolder` setting for the torque CSV log folder.
- `Docs/Offline_Simulation.md` – how to run the HMI on an office PC.
- `Docs/UR_Robot/UR_Program_Change_Spec.md` – UR robot program changes for MT_001/MT_002: one general
  set of point variables (`Scr01…Scr21`, `Scr01_E…Scr21_E`), HMI teach branch, recipe ID, watchdog,
  and the new PLC addresses (MR6000–MR6012, EM4200–EM4259).
- `Docs/UR_Robot/InitialRecipes/` – five starting robot recipes (EBM, PC Clear, PC Amber, PC Red,
  PC Green), all copied from the current black point set.
- UR robot communication layer (`CommunicationProtocols/UR/`): RTDE client (status, positions,
  heartbeat, speed slider), Dashboard client, interpreter-mode client, jog pose math (Base/Tool frame).
  Used by the Robot page (in progress).
- Robot recipe model and file service (`<HMI folder>\Recipe\<ID>_<Name>.xml`); the five starting
  recipes are copied there on first start.
- **Robot page (MT_002, first version)** – menu "Robot", Technician and Pentamaster only. Robot IP setting,
  connect, status (mode, safety, program, current point, speed, recipe on robot), tool position and joints,
  Start (power on), Run, Pause, Stop, Reset (unlocks a protective stop), speed. Teach mode request to the PLC;
  in teach mode: jog X/Y/Z/Rx/Ry/Rz in Base or Tool frame, step 0.01–10.00 mm/deg; hold-to-move to a
  recipe or shared point; teach point; load recipe points to the robot; save recipe. Offline mode uses a
  simulated robot. New PLC tags MR6000–MR6008, EM4200.
- **Recipe page (MT_001)** – menu "Recipe", not for Operator: list of recipes, create a copy (new ID + name,
  robot points copied), rename, delete (moved to `Recipe\Deleted`, blocked for the current lot recipe), view
  the 42 robot points.
- **Lot Entry recipe drop-down (MT_001)** – recipe chosen from the recipe files instead of typing a number.
  New Lot first loads the recipe's robot points to the robot (short HMI teach session), then writes the lot
  to the PLC with the recipe ID (EM1559) and name (EM1623). New Lot is refused when the robot load fails.
  `LoadRecipeAtNewLot` in `RobotSettings.xml` can switch the robot load off during commissioning.
- The robot connection is shared by all pages and connects at start-up when a robot IP is set.

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
