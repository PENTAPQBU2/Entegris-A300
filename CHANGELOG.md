# Changelog – Entegris A300 Shell Assembly HMI

Each version has its own branch named after the version number (e.g. `V1.4.0.0`).
`V1.3.13.0` is the original program as delivered, kept unchanged as a backup.
The same changes are listed for operators in `HMI_Master_KX7500/VersionHistory.txt`.

## V1.4.0.0 – in development (branch `V1.4.0.0`)

Customer change request MT_001 (recipe) and MT_003 (shell log per lot). MT_002 (robot page) is
cancelled by the customer; its code is kept in `Archive/MT_002_RobotPage/`. Items are added here as they are completed.

### Added
- **Offline / simulation mode** (`OfflineMode` in `MachineConfig.xml`). The HMI runs without
  PLC, screwdriver COM port or Camline TCP server. PLC reads/writes go to an in-memory simulated
  PLC, and the version label shows `[OFFLINE]`. KV DataBuilder does not need to be installed.
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

### Changed
- Torque CSV log falls back to `<HMI folder>\FOUP CSV\` when the `D:` drive does not exist
  (default on the machine is unchanged: `D:\HMI\FOUP CSV\`).
- Chinese menu names: Recipe 方剂 → 配方, Robot 机械 → 机器人.
- PLC connections created before `MachineConfig.xml` is loaded now connect on first use.

### Fixed
- `Alarm.mdb` path was built before the folder path was set, so Debug builds read the Release
  folder. It is now read from `MachineSettings\Alarm.mdb` of the running build.

## V1.3.13.0 – 21/05/2026 (branch `V1.3.13.0`, original program)
- Remove the screw count for bowl 1 and bowl 2.
- Remove the popup notification for refill.

Older versions: see `HMI_Master_KX7500/VersionHistory.txt`.
