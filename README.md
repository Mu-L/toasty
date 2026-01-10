# Toasty

A tiny Windows toast notification CLI. 229 KB, no dependencies.

## Quick Start

```cmd
toasty "Hello World" -t "Toasty"
```

That's it. Toasty auto-registers on first run.

## Usage

```
toasty <message> [options]

Options:
  -t, --title <text>        Set notification title (default: "Notification")
  --hero-image <path>       Add a large banner image at the top
  --scenario <type>         Set scenario: reminder, alarm, incomingCall, urgent
  --attribution <text>      Add small gray text at bottom showing source
  --progress <title:value:status>  Add progress bar (e.g., "Building:0.75:75%")
  --audio <sound>           Set audio: default, im, mail, reminder, sms, loopingAlarm, loopingCall, silent
  -h, --help                Show this help
  --register                Register app for notifications (run once)
```

### Examples

Basic notification:
```cmd
toasty "Build completed"
toasty "Task done" -t "Claude Code"
```

With hero image:
```cmd
toasty "Deploy successful" --hero-image C:\images\success.jpg
```

Urgent notification:
```cmd
toasty "Critical error!" --scenario urgent --audio loopingAlarm
```

Progress bar:
```cmd
toasty "Processing..." --progress "Build:0.75:75%" --attribution "via CI/CD"
```

Reminder:
```cmd
toasty "Meeting in 5 minutes" --scenario reminder --audio reminder
```

## Windows 11 Toast Features

Toasty supports modern Windows toast notification features that work on both Windows 10 (with Creators Update or later) and Windows 11:

### Hero Image
Large banner image across the top of the toast. Supports file paths or URIs.

### Scenarios
Control notification behavior and priority:
- `reminder` - For reminders and calendar events
- `alarm` - For alarms (persistent, looping sound)
- `incomingCall` - For VoIP calls (full-screen on mobile)
- `urgent` - High priority, stays on screen longer

### Attribution Text
Small gray text at the bottom showing the notification source (e.g., "via Claude Code").

### Progress Bar
Visual progress indicator with customizable title, value (0.0-1.0), and status text.

### Audio
Choose from various system sounds or silence notifications. Looping audio options available for alarms and calls.

**Note**: All these features are backwards compatible with Windows 10 Creators Update (build 15063) or later.

## Claude Code Integration

Add to `~/.claude/settings.json`:

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

## Building

Requires Visual Studio 2022 with C++ workload.

```cmd
cmake -S . -B build -G "Visual Studio 17 2022" -A x64
cmake --build build --config Release
```

Output: `build\Release\toasty.exe`

## License

MIT
