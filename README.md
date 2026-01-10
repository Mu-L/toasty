# Toasty

A tiny Windows toast notification CLI. 229 KB, no dependencies.

## Quick Start

```cmd
toasty "Hello World" -t "Toasty"
```

That's it. Toasty auto-registers on first run.

**Click the notification to bring your terminal back to focus!**

## Usage

```
toasty <message> [options]

Options:
  -t, --title <text>   Set notification title (default: "Notification")
  -h, --help           Show this help
```

## Features

- **Tiny**: 229 KB, no dependencies
- **Auto-register**: No setup needed, works on first run
- **Click to focus**: Clicking the notification brings your terminal window back to focus
- **Works with**: Command Prompt, PowerShell, Windows Terminal

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

Now when Claude finishes, you'll get a notification you can click to jump back to your terminal!

## Building

Requires Visual Studio 2022 with C++ workload.

```cmd
cmake -S . -B build -G "Visual Studio 17 2022" -A x64
cmake --build build --config Release
```

Output: `build\Release\toasty.exe`

## License

MIT
