# A300 Shell Assembly – Questions for Customer (Entegris)

Prepared by: Amirul (HMI) · Date: 14 Oct 2026
Reference: *A300 Shell Assembly Software Open Item and Enhancement.xlsx* (MT_001 – MT_010)

---

## 1. Recipe / Shell Colour (MT_001)

Current machine has **6 shell colours** in PLC: Black, Transparent, Amber, Green, Red, Blue.
Your list has **5 recipes**: EBM, PC Clear, PC Amber, PC Green, PC Red.

| # | Question |
|---|---|
| 1.1 | Please confirm the recipe names and mapping: **EBM (or ECM?) = Black**, **PC Clear = Transparent**, PC Amber = Amber, PC Green = Green, PC Red = Red. Is that correct? |
| 1.2 | **Blue** exists in the machine but not in your list. Keep it or remove it? |
| 1.3 | Besides colour, what is **different between recipes**? (robot screw points only? also torque / screw program, speed, vision, TSO column, A300 vs T300 shell?) |
| 1.4 | Will there be **more recipes in future** (new part numbers / types)? Roughly how many? |
| 1.5 | **Who** may create / edit / delete recipes? (Operator / Technician / Engineer / Pentamaster level) |
| 1.6 | Recipe selected at **New Lot** and **locked until End Lot**? Or can operator change recipe in the middle of a lot? |
| 1.7 | Should the HMI **show the current recipe** on the Main page? |
| 1.8 | Should a recipe **change be logged** (who / when / what changed)? |

---

## 2. Shell CSV Log (MT_003)

Proposed layout (one folder + one CSV per shell):
```
D:\HMI\FOUP CSV\<LotID>_<FoupID>_Pass\<LotID>_<FoupID>_Pass.csv
D:\HMI\FOUP CSV\<LotID>_<FoupID>_Fail\<LotID>_<FoupID>_Fail.csv
```

| # | Question |
|---|---|
| 2.1 | **FOUP ID / Barcode:** where does it come from? Is there a **barcode / label on the shell**? Will operator **scan** it (handheld scanner) or **type** it, or should the machine read it automatically? What is the format / max length? |
| 2.2 | Please **confirm the folder & file naming** above. Is "FOUP ID" the same as "unit number" / "shell barcode" in your request? |
| 2.3 | **Pass / Fail definition:** Pass = all screws (1–21) completed OK. Fail = shell removed / process ended before all screws completed. Is that correct? Should a shell with a NOK screw that was **retried and passed** be Pass? |
| 2.4 | **Rework:** when the same FOUP comes back for rework, should we **create a new file** (keep both) or **add to the old file**? |
| 2.5 | **"Result remedy":** what exactly do you want recorded? (e.g. rework type: Conveyor Plate / Side Column, number of retries, operator action, error code?) |
| 2.6 | **Columns needed** in the CSV. Current columns: DateTime, FoupId, LotId, ScrewNumber, ScrewDateTime, Result, ScrewType, Error, TorqueValue, TighteningAngle, TighteningTime, ShellType. Anything to **add / remove**? (e.g. **Shell colour / Recipe**, Operator ID, Equipment ID, error message) |
| 2.7 | **Save location:** `D:\HMI\FOUP CSV\` OK? Need a copy to a **network / server folder**? |
| 2.8 | **How long to keep** the files? (auto-delete after X months, or keep forever) |
| 2.9 | Is **CAMLINE** going to be used on this machine? (currently appears bypassed) |

---

## 3. Lot Entry (MT_004)

| # | Question |
|---|---|
| 3.1 | Mandatory before machine can run: **Lot ID, Operator ID, Quantity, Recipe** – confirm all 4? Any other (e.g. Customer ID)? |
| 3.2 | **Lot Quantity** means **target quantity** of the lot? Should machine **stop / warn** when target is reached? |
| 3.3 | Lot ID format / max length (machine supports up to 50 characters)? Allowed characters? |
| 3.4 | After **End Lot**, should the HMI create a **lot summary** (total pass / fail / time)? |

---

## 4. Robot Page (MT_002)

| # | Question |
|---|---|
| 4.1 | Which **user level** may open the Robot page and teach points? |
| 4.2 | Jog step 0.01–10 mm: per axis in **mm (X/Y/Z)** only, or also **rotation (deg)** / joint jog? |
| 4.3 | Should teaching be allowed **only in Engineering mode / door closed / low speed**? |
| 4.4 | Should every taught point change be **logged** (who / when / old → new value)? |

---

## 5. Other Items

| # | Item | Question |
|---|---|---|
| 5.1 | MT_005 Initialize button | Allowed for which user level? Need confirmation pop-up before initialize? |
| 5.2 | MT_006 Hard Reset | Pop-up text to show? Which user level may Proceed? |
| 5.3 | MT_008 Remove Top Clamp | Remove from HMI screens completely (Main layout, Dry Run, IO), or just hide? |
| 5.4 | MT_009 Error messages | Please give the list of errors that show wrong / "Spare" messages (we found Error 5, 12, 35, 56, 79, 80, 130 and Warning 21). Any others? |
| 5.5 | MT_010 Start button | Which step / situation needs Start to be pressed twice? (photo / video if possible) |
| 5.6 | Priority | Which items are needed **first** for the next visit? |
