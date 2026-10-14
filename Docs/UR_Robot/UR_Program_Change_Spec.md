# UR Robot Program Change Spec – A300 Shell Assembly (HMI V1.4.0.0)

For: the engineer who edits the UR program on the teach pendant.
Program: `A300 Shell 05092022.urp` + `default.installation` (PolyScope 5.12.1, e-Series).
Purpose: support MT_001 (recipe with robot points) and MT_002 (HMI Robot page).

**Result after the change**

- One general set of point variables (`Scr01 … Scr21`, `Scr01_E … Scr21_E`) replaces the black and
  transparent point sets. Their values come from the HMI recipe that is loaded.
- A new "HMI teach" branch lets the HMI jog, move, teach and load points, only when the PLC allows it.
- The robot reports which recipe it has loaded, so the PLC can block Run on a mismatch.
- A watchdog stops the program if the HMI connection is lost while in HMI teach.
- The normal PLC-driven sequence (To Home, To Screw Position, Exit, Calibration …) works as today.

Items marked **VERIFY** are behaviour to confirm on the robot during test (section 6) before production.

---

## 1. Before you start

1. Save a backup of the current program and installation to USB:
   `A300 Shell 05092022.urp`, `default.installation`, `default.variables`.
   (A copy received on 14/10/2026 is the reference for this spec.)
2. Save the edited program under a new name, e.g. `A300_Shell_V1_4.urp`, and set it as the default
   program (Installation → Startup) only after the test in section 6 passes.
3. Keep **Remote Control** enabled and the robot in **Remote** mode (as today).
4. Settings → Security → Services: make sure **Dashboard Server**, **RTDE** and the
   **Interpreter** socket (port 30020, if listed) are enabled.

## 2. Installation changes

### 2.1 Installation variables (Installation → General → Variables → Create New)

Installation variables keep their value on the robot after power off, and are visible and
editable on the pendant.

| Variable | Type | Initial value | Purpose |
|---|---|---|---|
| `Scr01` … `Scr21` | pose | from Appendix A (black set) | Screw positions 1–21 |
| `Scr01_E` … `Scr21_E` | pose | from Appendix A (black set) | Entry positions above screws 1–21 |
| `P_Home` | pose | value of feature `homept` (Appendix B) | Home position |
| `P_Safe` | pose | value of feature `safe_point` | Safe position |
| `P_CalReady` | pose | value of feature `cal_ready` | Calibration ready |
| `P_CalStart` | pose | value of feature `calibStart` | Calibration start |
| `P_Cal85Nm` | pose | value of feature `Calib_8_5Nm_pos` | Calibration 8.5 Nm |
| `P_Cal14Nm` | pose | value of feature `Calib_14Nm_pos` | Calibration 14 Nm |
| `Rcp_ID` | int | `1` | ID of the recipe whose points are loaded (1 = EBM) |
| `hmi_hb_last` | int | `0` | Watchdog: last heartbeat value seen |
| `hmi_hb_age` | float | `0` | Watchdog: seconds since heartbeat changed |

Notes:

- Screws 4, 5, 6, 9, 11, 12, 13, 14, 15 have no separate entry point today (the program uses the
  screw point itself). Their `_E` variable starts with the same value as the screw point, so the
  robot behaves exactly as today. A separate entry point can now be taught for every screw.
- Today the transparent set uses the **black** `Point_10bEntry` for screw 10. With one general set
  this no longer applies.
- `P_Home`, `P_Safe` and the calibration points are shared by all recipes. They are installation
  variables so the HMI can teach them and the value is kept on the robot.
- The old features (`Point_*b`, `Point_t*`, `homept` …) can stay in the installation until the new
  program is accepted, then be deleted.

### 2.2 Register names (Installation → Fieldbus / General Purpose registers)

| Register | New name | Written by | Purpose |
|---|---|---|---|
| Bool input 16 | `HMI_TeachMode` | PLC (EtherNet/IP, W0FD bit 0) | PLC allows HMI teach mode |
| Bool output 9 | `InTeachMode` | Robot (EtherNet/IP, W08C bit 9) | Robot is in the HMI teach branch |
| Int output 1 | `Rcp_ID_Out` | Robot (EtherNet/IP, W092–W093) | Recipe ID whose points are loaded |

Register input action for bool input 16: **NONE**.

The HMI also uses these RTDE registers directly (not mapped to the PLC):

| RTDE register | Direction | Purpose |
|---|---|---|
| input int register 24 | HMI → robot | Heartbeat counter, changed by the HMI every 100 ms while in teach |

## 3. Program changes

### 3.1 BeforeStart

Keep `idx0 := p[0,0,0,0,0,0]` and `prevScrew := 0`. Add after them:

```
screwHeightList := [idx0, Scr01, Scr02, Scr03, Scr04, Scr05, Scr06, Scr07, Scr08, Scr09, Scr10, Scr11, Scr12, Scr13, Scr14, Scr15, Scr16, Scr17, Scr18, Scr19, Scr20, Scr21]
screwEntryList  := [idx0, Scr01_E, Scr02_E, Scr03_E, Scr04_E, Scr05_E, Scr06_E, Scr07_E, Scr08_E, Scr09_E, Scr10_E, Scr11_E, Scr12_E, Scr13_E, Scr14_E, Scr15_E, Scr16_E, Scr17_E, Scr18_E, Scr19_E, Scr20_E, Scr21_E]
```

The suppressed folders ("uncomment this to use offset", "… direct teach point", "[black] …",
"[transparent] …") can be deleted.

### 3.2 Main_PLC – replace the shell colour switch

**Delete** `Switch shell_int` with its `Case 0` and `Case 1` folders. **Put in its place**, as the
first lines of `Main_PLC`:

```
screwHeightList := [idx0, Scr01, … , Scr21]        (same line as 3.1)
screwEntryList  := [idx0, Scr01_E, … , Scr21_E]    (same line as 3.1)
Set Rcp_ID_Out = Rcp_ID                            (Set node: int output 1, expression Rcp_ID)
```

The lists are rebuilt on every loop, so points loaded or taught by the HMI are used at the next
move. `shell_int` (int input 1) is no longer used by the robot.

### 3.3 Main_PLC – replace waypoints that use the old features

| Branch | Old waypoint | New waypoint (Variable waypoint) |
|---|---|---|
| ToHome | `homept` | `P_Home` |
| ToSafePos | `safe_point` | `P_Safe` |
| ToCalReadyPos | `cal_ready` | `P_CalReady` |
| ToCalStartPos, Cal_Down_Move | `calibStart` | `P_CalStart` |
| Cal_Down_Move (Cal_Prog on) | `Calib_8_5Nm_pos` | `P_Cal85Nm` |
| Cal_Down_Move (Cal_Prog off) | `Calib_14Nm_pos` | `P_Cal14Nm` |

### 3.4 Main_PLC – new HMI teach branch (first branch of the If chain)

Today the chain starts with `If ToHome and not AtHome`. Insert a new **first** branch and change
`If ToHome …` to `ElseIf ToHome …`:

```
If HMI_TeachMode
    Folder "Reset Signals"
        Set AtHome = 0
        Set AtSafePos = 0
        Set InScrewPos = 0
        Set inEntryPos = 0
        Set AtScrewExitPt = 0
        Set AtCalReadyPos = 0
        Set AtCalStartPos = 0
        Set AtCalDonePos = 0
    hmi_hb_last := read_input_integer_register(24)
    hmi_hb_age := 0
    Set InTeachMode = 1
    Script (line): interpreter_mode()
    Set InTeachMode = 0
    Rebuild screwHeightList and screwEntryList (same two lines as 3.1)
ElseIf ToHome and not AtHome
    … (unchanged)
```

While `interpreter_mode()` runs, the HMI sends URScript statements to port 30020 (section 4). It
returns when the HMI sends `end_interpreter()`. After leaving HMI teach, all "At…" signals are 0,
so the PLC must send the robot home (`ToHome`) before production.

### 3.5 New thread – HMI watchdog

Add a Thread (Structure → Advanced → Thread), ending with `sync()`:

```
Thread_HMI_Watchdog
    If InTeachMode
        hb := read_input_integer_register(24)
        If hb != hmi_hb_last
            hmi_hb_last := hb
            hmi_hb_age := 0
        Else
            hmi_hb_age := hmi_hb_age + 0.1
        If hmi_hb_age > 1.0
            Popup (error, non-blocking): "HMI connection lost - robot program stopped"
            Halt
    sleep(0.1)
```

If the HMI stops sending its heartbeat for 1 s while in HMI teach (PC hang, cable unplugged), the
program stops and any motion stops. **VERIFY**: `Halt` from a thread stops the program and the
current interpreter motion.

## 4. What the HMI sends (for reference)

All statements go to port 30020 while `InTeachMode` = 1. Units are metres and radians (UR native).

| HMI action | Statements sent |
|---|---|
| Load recipe | `Scr01 := p[x, y, z, rx, ry, rz]` … for all 42 points, then `Rcp_ID := <id>` |
| Jog one step | `movel(p[target], a=0.3, v=0.02)` – target computed by the HMI from the current pose and the step (Base or Tool frame, 0.01–10.00 mm or the same in degrees) |
| Move to screw point (hold to move) | lift 25 mm: `movel(pose_add(get_actual_tcp_pose(), p[0, 0, 0.025, 0, 0, 0]), a=0.3, v=0.02)`, then `movel(Scr05_E, a=0.3, v=0.05)`, then `movel(Scr05, a=0.3, v=0.02)` |
| Move to entry point (hold to move) | lift 25 mm (as above), then `movel(Scr05_E, a=0.3, v=0.05)` |
| Move to Home / Safe / Calibration (hold to move) | `movej(get_inverse_kin(P_Home), a=0.5, v=0.3)` |
| Button released / Stop | `abort` (own line), then `skipbuffer` |
| Teach (save current position) | `Scr05 := p[…]` with the pose the HMI read from RTDE; the HMI saves the same value in the recipe file |
| Leave HMI teach | `end_interpreter()` |
| Speed | RTDE speed slider (not an interpreter statement) |
| Unlock protective stop | Dashboard server port 29999: `close safety popup`, `unlock protective stop` |

Speeds above are the HMI defaults; they can be changed in `<HMI folder>\MachineSettings\RobotSettings.xml`
and are limited further by the speed slider. Moving to or teaching a recipe point is only allowed when the
robot reports the same recipe ID as the recipe shown on the HMI.

### 4.1 Checklist – what the HMI expects in the robot program

The HMI code (V1.4.0.0) depends on these names and numbers. If any is spelled differently on the robot,
the matching HMI function will fail with "Robot refused" or will not see the robot state.

| HMI function | Robot program must have |
|---|---|
| Teach mode | Branch `If HMI_TeachMode` (bool input 16) that sets bool output 9 (`InTeachMode`) and runs `interpreter_mode()` |
| Teach mode ends | `interpreter_mode()` returns on `end_interpreter()`; bool output 9 set back to 0 |
| Recipe points | Installation variables `Scr01`…`Scr21` and `Scr01_E`…`Scr21_E` (pose), spelled exactly so |
| Shared points | Installation variables `P_Home`, `P_Safe`, `P_CalReady`, `P_CalStart`, `P_Cal85Nm`, `P_Cal14Nm` (pose) |
| Recipe on robot | Installation variable `Rcp_ID` (int), copied every loop to int output 1 (`Rcp_ID_Out`) |
| Production uses taught points | `screwHeightList` / `screwEntryList` rebuilt from `Scr..` every loop (section 3.2) |
| Watchdog | Thread reading RTDE input int register 24; stops the program after 1 s without change in teach |
| Status on the Robot page | Bool outputs 0–8 as today (`AtHome` … `AtCalDonePos`), int output 0 `current_Pos` |
| Network | Remote mode; Dashboard (29999), RTDE (30004) and interpreter (30020) enabled; robot IP entered on the Robot page |

## 5. PLC changes that go with this (for the PLC engineer)

| Item | Address | Logic |
|---|---|---|
| Robot page open | MR6000 (HMI→PLC) | Information only |
| Teach request | MR6001 (HMI→PLC) | Operator wants HMI teach |
| Teach allowed | MR6002 (PLC→HMI) | ON when no lot is running, machine not in auto, doors/safety OK |
| Teach mode to robot | W0FD bit 0 (PLC→robot `HMI_TeachMode`) | = MR6001 AND MR6002 |
| Robot in teach | MR6003 (PLC→HMI) | = robot bool output 9 `InTeachMode` = **W08C bit 9** |
| Robot start / stop / pause / reset | MR6004 / MR6005 / MR6006 / MR6007 (HMI→PLC) | Map to existing robot bits: Start_Prog (W0FC.11), Stop_Program (W0FC.2), Pause_Program (W0FC.3); reset = existing init/home sequence |
| Robot recipe ID | EM4200 (2 words, PLC) | = robot int output 1 `Rcp_ID_Out` = **W092–W093** (int output 0 `current_Pos` is W090–W091) |
| Robot recipe OK | MR6008 (PLC→HMI) | ON when EM4200 = lot recipe ID EM1559; **Run interlock**: no Start while OFF |
| Teach lost | – | If W0FD.0 goes OFF while robot `InTeachMode` is ON for > 0.5 s → pulse Stop_Program |
| Hard reset prompt (MT_006) | MR6010 (PLC→HMI), EM4210 string[100] (EM4210–EM4259) | Reset held 5 s → MR6010 ON + message |
| Hard reset reply | MR6011 Proceed, MR6012 Cancel (HMI→PLC) | PLC acts on Proceed only, clears MR6010 |

MR6000–MR6015 and EM4200–EM4259 confirmed free in KV Studio (14/10/2026).

EtherNet/IP words used (robot unit [0-1] Universal Robot): W0FC/W0FD bit input registers 0–15/16–31,
W08C bit output registers 0–15, W090–W091 int output 0, W092–W093 int output 1 (new),
W0100–W0103 int input 0–1. After this change the robot no longer reads `shell_int`
(int input 1, W0102–W0103); the PLC may keep writing it.

## 6. Test procedure (robot at reduced speed, doors closed)

1. Run the new program from the pendant with `HMI_TeachMode` OFF. Check the normal sequence:
   To Home, To Safe, To Screw Position for screws 1, 10 and 21, Exit, Calibration ready/start/down.
   Positions must be the same as with the old program (black set).
2. Check `Rcp_ID_Out` reads 1 on the PLC (EM4200).
3. Turn `HMI_TeachMode` ON (force W0FD.0). Check `InTeachMode` goes ON and all "At…" signals go OFF.
4. From a PC on the robot network, open port 30020 and send `Rcp_ID := 2`. Check the reply and
   that `Rcp_ID_Out` changes to 2. **VERIFY**: after a power cycle `Rcp_ID` is still 2
   (installation variable kept).
5. Send `movel(pose_add(get_actual_tcp_pose(), p[0,0,0.005,0,0,0]), a=0.3, v=0.02)`. The robot moves
   up 5 mm.
6. Send a 50 mm move, then `abort` while it moves. **VERIFY**: the robot stops in a controlled way.
7. Stop sending heartbeats (register 24 not changing). **VERIFY**: the watchdog stops the program
   within about 1 s.
8. Send `end_interpreter()`. Check `InTeachMode` goes OFF and the program continues with the normal
   branches.
9. Set `Rcp_ID := 1` again before production.

---

## Appendix A – Initial screw and entry points (from the black set)

All five starting recipes (EBM, PC Clear, PC Amber, PC Red, PC Green) use these values; the HMI
recipe files are in `Docs/UR_Robot/InitialRecipes/`. Fine-tune each shell type on the machine.
Positions shown in mm, rotation vector in radians; the recipe files hold full precision in metres.

| Variable | Point | From | X mm | Y mm | Z mm | Rx rad | Ry rad | Rz rad |
|---|---|---|---|---|---|---|---|---|
| `Scr01` | Screw 1 | `Point_1b` | 310.20 | -415.80 | 491.26 | 2.37814 | 2.04098 | -0.01234 |
| `Scr02` | Screw 2 | `Point_2b` | 312.26 | -71.37 | 490.57 | 2.37926 | 2.04097 | -0.01486 |
| `Scr03` | Screw 3 | `Point_3b` | 294.91 | -197.76 | 509.29 | 2.37905 | 2.04109 | -0.01305 |
| `Scr04` | Screw 4 | `Point_4b` | 202.60 | -199.38 | 509.37 | 2.37886 | 2.04064 | -0.01283 |
| `Scr05` | Screw 5 | `Point_5b` | 202.25 | -287.08 | 509.37 | 2.37878 | 2.04067 | -0.01262 |
| `Scr06` | Screw 6 | `Point_6b` | 294.25 | -289.62 | 509.30 | 2.37905 | 2.04075 | -0.01278 |
| `Scr07` | Screw 7 | `Point_7b` | 247.14 | -414.13 | 481.54 | 2.37759 | 2.04429 | -0.00565 |
| `Scr08` | Screw 8 | `Point_8b` | 248.13 | -74.79 | 481.54 | 2.37728 | 2.04433 | -0.00383 |
| `Scr09` | Screw 9 | `Point_9b` | 362.74 | -398.21 | 503.83 | 2.37853 | 2.04569 | 0.00349 |
| `Scr10` | Screw 10 | `Point_10b` | 261.20 | -176.05 | 503.04 | 0.16436 | 3.13513 | 0.00818 |
| `Scr11` | Screw 11 | `Point_11b` | 186.29 | -245.72 | 503.08 | 2.10660 | 2.32460 | 0.00440 |
| `Scr12` | Screw 12 | `Point_12b` | 169.38 | -321.23 | 503.04 | 2.98209 | 0.97916 | -0.00908 |
| `Scr13` | Screw 13 | `Point_13b` | 177.69 | -253.21 | 503.00 | 3.05619 | 0.71943 | -0.00729 |
| `Scr14` | Screw 14 | `Point_14b` | 287.52 | -313.83 | 503.03 | 2.40376 | 2.01088 | -0.00558 |
| `Scr15` | Screw 15 | `Point_15b` | 285.61 | -175.12 | 503.06 | 2.37721 | 2.04213 | -0.00649 |
| `Scr16` | Screw 16 | `Point_16b` | 180.77 | -407.96 | 487.48 | 2.37713 | 2.04477 | -0.00436 |
| `Scr17` | Screw 17 | `Point_17b` | 180.94 | -81.96 | 481.55 | 2.37837 | 2.04598 | 0.00336 |
| `Scr18` | Screw 18 | `Point_18b` | 325.60 | -395.99 | 501.50 | 2.38071 | 2.04871 | 0.01051 |
| `Scr19` | Screw 19 | `Point_19b` | 224.05 | -190.80 | 501.46 | -0.01164 | 3.14003 | 0.00846 |
| `Scr20` | Screw 20 | `Point_20b` | 170.52 | -376.85 | 491.25 | 2.36863 | 2.05102 | -0.00330 |
| `Scr21` | Screw 21 | `Point_21b` | 171.14 | -111.43 | 491.25 | 2.37606 | 2.04347 | -0.00299 |
| `Scr01_E` | Screw 1 entry | `Point_1bEntry` | 310.17 | -416.14 | 610.99 | 2.37803 | 2.04114 | -0.01228 |
| `Scr02_E` | Screw 2 entry | `Point_2bEntry` | 314.85 | -69.63 | 581.76 | 2.37823 | 2.04083 | -0.01264 |
| `Scr03_E` | Screw 3 entry | `Point_3bEntry` | 294.55 | -197.50 | 530.02 | 2.37887 | 2.04051 | -0.01276 |
| `Scr04_E` | Screw 4 entry | `Point_4b` | 202.60 | -199.38 | 509.37 | 2.37886 | 2.04064 | -0.01283 |
| `Scr05_E` | Screw 5 entry | `Point_5b` | 202.25 | -287.08 | 509.37 | 2.37878 | 2.04067 | -0.01262 |
| `Scr06_E` | Screw 6 entry | `Point_6b` | 294.25 | -289.62 | 509.30 | 2.37905 | 2.04075 | -0.01278 |
| `Scr07_E` | Screw 7 entry | `Point_7bEntry` | 246.43 | -414.14 | 508.98 | 2.37778 | 2.04434 | -0.00578 |
| `Scr08_E` | Screw 8 entry | `Point_8bEntry` | 248.77 | -74.81 | 509.00 | 2.37733 | 2.04417 | -0.00392 |
| `Scr09_E` | Screw 9 entry | `Point_9b` | 362.74 | -398.21 | 503.83 | 2.37853 | 2.04569 | 0.00349 |
| `Scr10_E` | Screw 10 entry | `Point_10bEntry` | 262.77 | -180.90 | 534.00 | 0.07229 | 3.13873 | 0.00833 |
| `Scr11_E` | Screw 11 entry | `Point_11b` | 186.29 | -245.72 | 503.08 | 2.10660 | 2.32460 | 0.00440 |
| `Scr12_E` | Screw 12 entry | `Point_12b` | 169.38 | -321.23 | 503.04 | 2.98209 | 0.97916 | -0.00908 |
| `Scr13_E` | Screw 13 entry | `Point_13b` | 177.69 | -253.21 | 503.00 | 3.05619 | 0.71943 | -0.00729 |
| `Scr14_E` | Screw 14 entry | `Point_14b` | 287.52 | -313.83 | 503.03 | 2.40376 | 2.01088 | -0.00558 |
| `Scr15_E` | Screw 15 entry | `Point_15b` | 285.61 | -175.12 | 503.06 | 2.37721 | 2.04213 | -0.00649 |
| `Scr16_E` | Screw 16 entry | `Point_16bEntry` | 180.54 | -408.27 | 508.46 | 2.37744 | 2.04436 | -0.00459 |
| `Scr17_E` | Screw 17 entry | `Point_17bEntry` | 180.84 | -82.26 | 508.48 | 2.37830 | 2.04613 | 0.00323 |
| `Scr18_E` | Screw 18 entry | `Point_18bEntry` | 325.62 | -395.96 | 523.40 | 2.38061 | 2.04887 | 0.01059 |
| `Scr19_E` | Screw 19 entry | `Point_19bEntry` | 224.03 | -190.81 | 523.40 | -0.01160 | 3.14009 | 0.00853 |
| `Scr20_E` | Screw 20 entry | `Point_20bEntry` | 170.52 | -376.83 | 523.40 | 2.36872 | 2.05109 | -0.00340 |
| `Scr21_E` | Screw 21 entry | `Point_21bEntry` | 171.15 | -111.42 | 512.00 | 2.37606 | 2.04353 | -0.00293 |

## Appendix B – Shared points (from the current features)

Same values as today; only the name changes.

| Old feature | New variable | X mm | Y mm | Z mm | Rx rad | Ry rad | Rz rad |
|---|---|---|---|---|---|---|---|
| `homept` | `P_Home` | -105.87 | -442.20 | 603.38 | -2.92329 | -1.13234 | -0.03139 |
| `safe_point` | `P_Safe` | 227.29 | -423.40 | 582.51 | 2.37846 | 2.04100 | -0.01287 |
| `cal_ready` | `P_CalReady` | 193.42 | -206.92 | 632.37 | 2.37851 | 2.04097 | -0.01287 |
| `calibStart` | `P_CalStart` | 193.40 | -206.92 | 632.38 | 2.37849 | 2.04096 | -0.01290 |
| `Calib_8_5Nm_pos` | `P_Cal85Nm` | 193.42 | -206.91 | 632.37 | 2.37851 | 2.04099 | -0.01285 |
| `Calib_14Nm_pos` | `P_Cal14Nm` | 193.42 | -206.91 | 632.36 | 2.37853 | 2.04105 | -0.01290 |
