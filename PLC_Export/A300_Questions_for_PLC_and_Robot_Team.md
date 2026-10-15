# A300 Shell Assembly – Questions for PLC Team & Robot Engineer

Prepared by: Amirul (HMI) · Date: 14 Oct 2026

---

## 0. Background (what HMI has done)

| Customer request | Issue ID | HMI status |
|---|---|---|
| Recipe for 6 shell colours, operator chooses which to run | **MT_001** | **Simple version done** – Lot Entry page now has 6 colour buttons (Black, Transparent, Amber, Green, Red, Blue → `MR4500`, `MR4501`, `MR4502`, `MR4503`, `MR4504`, `MR4511`). Full Recipe page (create / edit / delete + robot points) **waiting for Robot Engineer** (Section B). |
| CSV: one file per Lot ID + unit, 1 set of data per file | **MT_003** | **Code written** – one folder + one CSV per shell, named `<LotID>_<FoupID>_<Pass/Fail>`. **Waiting for PLC Team** to provide the FOUP ID (Section A). |

### How the new CSV works (for reference)

```
1. PLC step 1  → HMI pop-up "Shell Choice" (MR1513) appears
2. Operator presses  [New]  or  [Rework]   → PLC bit MR4507 / MR4508 = ON
3. At that moment HMI reads:  Lot ID  (EM1501)  and  FOUP ID (EM3050)
   → creates  D:\HMI\FOUP CSV\<LotID>_<FoupID>_InProgress_<time>\
4. Every screw result from torque driver → 1 row in the CSV
5. PLC step 340: last screw set (18–21) done → Overall_Pass (MR49600) = ON
6. PLC step 360 (shell done) or Hard Reset → MR4507 / MR4508 = OFF
   → HMI renames folder + file to  <LotID>_<FoupID>_Pass   (or _Fail if no Pass seen)
```

### Current problem

| Value | Address | Problem |
|---|---|---|
| Lot ID | `EM1501` (String[50]) | Was always empty because old HMI code read it with length 0. **Fixed in HMI.** Filled when operator presses *New Lot* on Lot Entry. |
| FOUP ID | `EM3050` (String[20]) | **Always empty.** Nothing in the current PLC program writes to this address (it is only read by HMI to send to CAMLINE). → **Need PLC Team.** |

---

## A. Questions for PLC Team (MT_003 – CSV per shell)

### A1. Where does the FOUP ID come from?
Today no PLC module writes the FOUP ID (`Foup_Barcode`, `EM3050`).
Please tell us the source:
- [ ] Barcode scanner on machine (which model / interface – RS232, Ethernet, IO-Link?)
- [ ] Operator scans or types it on HMI
- [ ] Received from CAMLINE
- [ ] Other: ______________

### A2. When is the FOUP ID ready? (timing)
HMI reads the FOUP ID **only once per shell**, at the moment the operator presses **New / Rework** on the "Shell Choice" pop-up.

```
   FOUP ID written  ──►  operator presses New/Rework  ──►  HMI reads EM3050      ✔ correct name
   operator presses New/Rework  ──►  HMI reads EM3050 (empty / old)  ──►  FOUP ID written   ✘ wrong / NoFoupID
```

**Question:** Can the PLC put the FOUP ID into `EM3050` (or spare `Lot_Info_DB_Shell_Barcode`, `EM1563`) **before** the operator presses New / Rework?
If **not**, please tell us **which step / bit** shows the FOUP ID is ready, so HMI can read it at that point instead.

### A3. Is the FOUP ID cleared after each shell?
If the old FOUP ID stays in `EM3050`, the next shell may be saved under the previous FOUP's name.
**Question:** Does / can the PLC clear `EM3050` at step 360 (shell done)?

### A4. Rework of the same FOUP
**Question:** When a shell comes back for Rework, will the same FOUP ID be available again (re-scan / re-enter)?
(HMI will keep both files: `<LotID>_<FoupID>_Pass` and `<LotID>_<FoupID>_Pass_<date_time>`.)

### A5. Is CAMLINE enabled on the machine?
- Setting: HMI **Settings** page → Bypass → *Bypass_CAMLINE* (`MR1209`). Also forced ON when *Inhouse_Testing* (`MR1011`) is ON.
- Evidence suggests it is **bypassed** (no CAMLINE log in production backup, FOUP ID never filled).
**Question:** Please confirm ON / OFF, and whether CAMLINE should be used in future.

### A6. Fail signal
| | CAMLINE **OFF** | CAMLINE **ON** |
|---|---|---|
| Pass | `Overall_Pass` `MR49600` (step 340, screw set 4 done) | same |
| Fail | **No signal** – HMI assumes "shell ended without Pass = Fail" | `Overall_Fail` `MR49601` set at step 360 |

Hard Reset / shell removed halfway skips step 360, so **no Fail bit even with CAMLINE ON**.
**Question:** Is HMI's rule "no Pass = Fail" acceptable? Or can PLC set `Overall_Fail` (`MR49601`) for every shell that ends without Pass (including Hard Reset), regardless of CAMLINE?

### A7. "Result remedy" (from MT_003)
Customer asked the log to include *Result remedy*. HMI currently records **ShellType** = New / Rework / Rework-ConveyorPlate / Rework-SideColumn.
**Question:** Is that what "result remedy" means? If not, what data and which PLC address?

> ~~Lot ID valid until End Lot~~ – **No need to ask.** Checked in PLC program: nothing changes `EM1501` during a lot. It is only cleared by *End Lot* (`MR49502` → PLC copies ID to `EM1527` and empties `EM1501`).

---

## B. Questions for Robot Engineer (MT_001 Recipe page, MT_002 Robot page)

Customer wants a Recipe page with one recipe per shell type (**EBM, PC Clear, PC Amber, PC Green, PC Red**), and **each recipe includes its own robot points**.

Today the PLC sends to the robot:
| Register | Content |
|---|---|
| `W0100` | Screw number (1–21) |
| `W0102` | Shell colour number 0–5 (Black, Transparent, Amber, Green, Red, Blue) |
| `W0104` | Shell type (A300 / T300) – PLC currently writes 0 for both (possible PLC bug) |

We are not sure the robot program uses `W0102` today.

### B1. Point storage
How does the UR program store screw points today (installation variables, waypoints inside the program, file)? Can it hold **a separate point set per recipe**?

### B2. Recipe selection
How should the robot know which recipe is active?
- [ ] Read recipe / colour number from `W0102` at program start
- [ ] Load a different robot program per recipe (by name / number)
- [ ] Other: ______________

### B3. What changes per recipe?
Only screw positions, or also approach / safe points, speed, offset, tool settings?

### B4. Where should the points live?
To let users **create / edit / delete** recipes from HMI:
- [ ] Points stay in robot – HMI/PLC only sends recipe number
- [ ] Points stored on HMI PC – sent to robot via PLC registers or robot network interface (please give register map)

### B5. Teach & save from HMI (MT_002)
Can a point taught from the HMI Robot page be **saved into the robot** permanently (survive robot restart)? Which interface?

### B6. Point details
Number of points per recipe, units (mm / deg), frame (base / tool).

### B7. Safety
Any limits for moving / jogging robot from HMI (max speed, Engineering mode only, door closed, jog step 0.01–10 mm as customer requested)?

---

## C. Address glossary (simple)

| Address | Name | Meaning |
|---|---|---|
| `EM1501` | Lot_Info_DB_New_Lot_ID_Str | Current Lot ID (from Lot Entry → New Lot) |
| `EM1527` | Lot_Info_DB_End_Lot_ID_Str | Last finished Lot ID (copied at End Lot) |
| `MR49502` | Lot_Info_DB_End_Lot_Bit | End Lot button signal from HMI |
| `EM3050` | Foup_Barcode | FOUP ID – **empty today** |
| `EM1563` | Lot_Info_DB_Shell_Barcode | Spare text field (unused) |
| `MR1513` | HMI_Prompt_Shell_Choice | PLC asks operator: New or Rework? |
| `MR4507` / `MR4508` | New_Shell / Rework_Shell | Operator pressed New / Rework |
| `MR4509` / `MR4510` | Rework_ConveyorPlate / Rework_SideColumn | Rework component chosen |
| `MR49600` | Overall_Pass | Last screw set done (step 340) |
| `MR49601` | Overall_Fail | Fail – only when CAMLINE ON |
| `MR1209` | Bypass_DB_Bypass_CAMLINE | CAMLINE bypass setting |
| `MR4500`–`MR4504`, `MR4511` | Shell_Colour_* | Black, Transparent, Amber, Green, Red, Blue |
| `W0100` / `W0102` / `W0104` | – | To robot: screw no. / colour no. / shell type |
