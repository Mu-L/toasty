# Development Guide

## Architecture

Toasty uses C++/WinRT to call the `Windows.UI.Notifications` API directly. No Windows App SDK runtime required.

### Key Components

- **C++/WinRT**: Modern C++ projection for Windows Runtime APIs
- **AUMID Registration**: Creates a Start Menu shortcut with `System.AppUserModel.ID` property
- **ToastNotificationManager**: Windows built-in toast notification system

### Why Not Windows App SDK?

Windows App SDK requires the WindowsAppRuntime to be installed on the user's machine. For a simple CLI tool, this is unnecessary overhead. The raw `Windows.UI.Notifications` API works on any Windows 10/11 system.

### Why Not .NET?

The `csharp` branch has a .NET 10 Native AOT version that works, but produces a 3.4 MB binary. The C++ version is 229 KB - 15x smaller.

## Building

### Prerequisites

- Visual Studio 2022 with "Desktop development with C++" workload
- CMake 3.20+
- Windows 10 SDK

### Build Commands

```cmd
# Configure (from Developer Command Prompt)
cmake -S . -B build -G "Visual Studio 17 2022" -A x64

# Build Release
cmake --build build --config Release

# Build Debug
cmake --build build --config Debug
```

### Build for ARM64

```cmd
cmake -S . -B build -G "Visual Studio 17 2022" -A ARM64
cmake --build build --config Release
```

## How AUMID Registration Works

Windows toast notifications require an AppUserModelId (AUMID) to identify the sending application. For unpackaged desktop apps, this requires:

1. A Start Menu shortcut (.lnk file)
2. The shortcut must have the `System.AppUserModel.ID` property set
3. The AUMID used in code must match the shortcut's property

The `--register` flag creates this shortcut at:
```
%APPDATA%\Microsoft\Windows\Start Menu\Programs\Toasty.lnk
```

## Code Structure

```
main.cpp
├── print_usage()                     - CLI help text
├── escape_xml()                      - XML entity escaping for toast content
├── register_protocol()               - Register toasty:// protocol handler
├── save_console_window_handle()      - Save HWND to registry for later focus
├── get_saved_console_window_handle() - Retrieve saved HWND from registry
├── focus_console_window()            - Focus the console/terminal window
├── create_shortcut()                 - AUMID registration via Start Menu shortcut
├── is_registered()                   - Check if app is registered
├── ensure_registered()               - Auto-register if needed
└── wmain()                           - Entry point, argument parsing, toast display
```

## Toast XML Format

```xml
<toast activationType="protocol" launch="toasty://focus">
  <visual>
    <binding template="ToastGeneric">
      <text>Title</text>
      <text>Message</text>
    </binding>
  </visual>
</toast>
```

The `activationType="protocol"` and `launch="toasty://focus"` attributes enable click-to-focus functionality.

## Click-to-Focus Implementation

When a user clicks a toast notification, the following happens:

1. **Protocol Activation**: Windows launches the `toasty://focus` protocol
2. **Registry Lookup**: The protocol handler is registered at `HKCU\Software\Classes\toasty\shell\open\command`
3. **App Launch**: The handler invokes `toasty.exe --focus`
4. **Window Focus**: The app uses a three-tier approach to find and focus the console window:
   - **Primary**: `GetConsoleWindow()` - Direct console handle (when toasty has a console attached)
   - **Secondary**: Registry-stored HWND - Retrieved from `HKCU\Software\Toasty\LastConsoleWindow`
   - **Fallback**: Window enumeration - Find visible console or Windows Terminal windows

The registry-stored HWND approach is most reliable because:
- The protocol activation launches toasty in a new process without a console
- The saved handle points to the exact console window where toasty was originally run
- The handle is validated with `IsWindow()` before use

## Branches

| Branch | Description |
|--------|-------------|
| `cplusplus` | C++/WinRT implementation (229 KB) |
| `csharp` | .NET 10 Native AOT implementation (3.4 MB) |

## Troubleshooting

### Notifications not appearing

1. Run `toasty --register` first
2. Check Windows Settings > System > Notifications > Toasty is enabled
3. Check Focus Assist / Do Not Disturb is off

### Clicking notification doesn't focus terminal

1. Check protocol registration: `reg query "HKCU\Software\Classes\toasty\shell\open\command"`
2. Re-run `toasty --register` to update protocol handler
3. Ensure the command value points to the correct toasty.exe path

### Build errors about missing headers

Ensure Windows 10 SDK is installed via Visual Studio Installer.

### vswhere.exe not found during publish (C# branch)

Run from Developer Command Prompt for VS 2022.
