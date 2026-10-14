# Running the HMI on an office PC (Offline mode)

Use this to open and click through the HMI without a PLC, robot, screwdriver COM port or Camline server.
Never enable Offline mode on the machine.

## 1. Build

- Open `HMI_Master_KX7500.sln` in Visual Studio.
- Select **Debug | x86** (or Release | x86). Do **not** use Any CPU: the user/alarm databases use the
  32-bit Microsoft Jet OLEDB 4.0 driver, which does not load in a 64-bit process.

## 2. Folders (no D: drive needed)

The HMI creates its folders on the root of the drive the `.exe` runs from:

| Build   | Folder           |
|---------|------------------|
| Debug   | `C:\HMI_A300\`   |
| Release | `C:\HMI\`        |

(`C:` if the exe is on C:.) Sub-folders such as `MachineConfiguration\`, `MachineSettings\` and `Logs\`
are created on first start.

## 3. Files to copy from the machine PC

Copy into `<HMI folder>\MachineSettings\`:

- `UserAccountSetting.mdb` – users and passwords (sign-in)
- `Alarm.mdb` – alarm texts (optional: without it alarms show no text)

## 4. Turn on Offline mode

1. Start the HMI once and close it. This creates `<HMI folder>\MachineSettings\MachineConfig.xml`
   (an existing file gets the new settings added).
2. Edit `MachineConfig.xml`:

```xml
<OfflineMode>true</OfflineMode>
```

3. Start the HMI again. The version text shows **[OFFLINE]**.

In Offline mode:

- PLC reads/writes go to an in-memory simulated PLC (values written by the HMI can be read back;
  everything else reads 0 / empty). KV DataBuilder does not need to be installed.
- The screwdriver COM port is not opened.
- The Camline TCP server is not contacted.

## Other MachineConfig.xml settings

| Setting                | Default         | Meaning |
|------------------------|-----------------|---------|
| `PLCIPAddress`         | `192.168.0.10`  | KV-7500 IP address |
| `PLCPort`              | `8500`          | KV-7500 port |
| `UseKVStudioSimulator` | `true`          | Passed to KV DataBuilder `KVSimulator` (always true before V1.4.0.0) |
| `FoupCsvFolder`        | empty           | Torque CSV folder. Empty = `D:\HMI\FOUP CSV\` (Debug: `D:\HMI_A300\FOUP CSV\`), or `<HMI folder>\FOUP CSV\` if there is no D: drive |
| `OfflineMode`          | `false`         | See above |
