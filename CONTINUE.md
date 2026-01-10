# Toasty - Continuation Guide

## Current Status

The project is **fully coded and builds successfully**. The debug build runs and shows notifications. Only the Native AOT publish requires a Developer Command Prompt due to vswhere.exe PATH issues.

## Project Structure

```
D:\github\toasty\
├── toasty.csproj      # .NET 10 project with Native AOT config
├── Program.cs         # Main CLI code using Windows.UI.Notifications
├── app.manifest       # Windows app manifest
└── CONTINUE.md        # This file
```

## What's Done

1. **Project file** (`toasty.csproj`): .NET 10, Native AOT enabled, CsWinRT for AOT-compatible WinRT projection
2. **Main code** (`Program.cs`): Uses `Windows.UI.Notifications` API directly (not Windows App SDK)
3. **App manifest**: Proper Windows 10/11 compatibility

## What Works

- `dotnet build` - Succeeds
- `dotnet run -- --help` - Shows help
- `dotnet run -- "Test message" -t "Title"` - Shows notification (using PowerShell's AppUserModelId)

## What's Left

Run this from a **Developer Command Prompt for VS 2022** (or wherever vswhere.exe is in PATH):

```cmd
cd D:\github\toasty
dotnet publish -c Release -r win-arm64
```

For x64 systems:
```cmd
dotnet publish -c Release -r win-x64
```

The output will be in:
- `bin\Release\net10.0-windows10.0.22621.0\win-arm64\publish\toasty.exe` (ARM64)
- `bin\Release\net10.0-windows10.0.22621.0\win-x64\publish\toasty.exe` (x64)

## Expected Binary Size

With Native AOT and size optimization, expect ~2-4 MB.

## Usage

```
toasty <message> [options]

Options:
  -t, --title <text>   Set notification title (default: "Notification")
  -i, --icon <path>    Set notification icon (local file path)
  -h, --help           Show this help

Examples:
  toasty "Build completed"
  toasty "Task done" -t "Claude Code"
  toasty "Hello" --title "My App" --icon "C:\icons\app.png"
```

## Claude Code Hook Integration

After publishing, add to `~/.claude/settings.json`:

```json
{
  "hooks": {
    "Stop": [
      {
        "hooks": [
          {
            "type": "command",
            "command": "C:\\path\\to\\toasty.exe \"Claude finished\" -t \"Claude Code\"",
            "timeout": 5
          }
        ]
      }
    ]
  }
}
```

## Technical Notes

### Why Windows.UI.Notifications instead of Windows App SDK?

Windows App SDK requires heavy Visual Studio build tools and MrtCore for resource packaging. For a simple CLI tool, the raw `Windows.UI.Notifications` API via CsWinRT is lighter and AOT-compatible.

### AppUserModelId

Unpackaged apps need an AppUserModelId to show notifications. We use `"Microsoft.Windows.PowerShell"` as a well-known system app. Notifications appear attributed to PowerShell in Action Center.

For custom app identity, you'd need to:
1. Create a shortcut with custom AUMID
2. Register the AUMID
3. Use that AUMID in CreateToastNotifier()

### CsWinRT Source Generator

The `CsWinRTAotOptimizerEnabled=true` setting enables compile-time generation of COM vtables, making WinRT calls AOT-compatible without runtime reflection.

## Prompt to Continue

```
Continue working on the toasty project in D:\github\toasty.
Read CONTINUE.md for context. The code is complete - just need to test the AOT publish
and verify the binary size. I'm now in a Developer Command Prompt with proper PATH.
```
