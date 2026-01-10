# Testing Guide

## Manual Testing Checklist

Since this is a Windows-specific application, testing must be performed on a Windows 10 or Windows 11 system.

### Prerequisites
1. Build the application using the instructions in README.md
2. Ensure you have registered the app with `toasty --register`
3. Ensure Windows notifications are enabled (Settings > System > Notifications)

### Basic Features

#### Title and Message
- [ ] `toasty "Test message"`
- [ ] `toasty "Test message" -t "Custom Title"`
- [ ] Verify default title is "Notification"
- [ ] Verify title and message display correctly

### Windows 11/10 Advanced Features

#### Hero Image
- [ ] `toasty "Test" --hero-image C:\Windows\Web\Wallpaper\Windows\img0.jpg`
- [ ] Verify large banner image appears at top
- [ ] Test with absolute path
- [ ] Test with relative path
- [ ] Test with invalid path (should show warning)
- [ ] Test with URI (e.g., `https://...`)

#### Scenarios
- [ ] `toasty "Test" --scenario reminder`
- [ ] `toasty "Test" --scenario alarm`
- [ ] `toasty "Test" --scenario incomingCall`
- [ ] `toasty "Test" --scenario urgent`
- [ ] Verify urgent notifications stay on screen longer
- [ ] Test invalid scenario value (should show error)

#### Attribution
- [ ] `toasty "Test" --attribution "via Testing"`
- [ ] Verify small gray text appears at bottom
- [ ] Test with long attribution text
- [ ] Test with special characters: `toasty "Test" --attribution "via A & B"`

#### Progress Bar
- [ ] `toasty "Test" --progress "Task:0.0:0%"`
- [ ] `toasty "Test" --progress "Task:0.5:50%"`
- [ ] `toasty "Test" --progress "Task:1.0:Complete"`
- [ ] Verify progress bar displays correctly
- [ ] Test invalid format: `toasty "Test" --progress "invalid"` (should show warning)
- [ ] Test empty components: `toasty "Test" --progress "::"`  (should show warning)

#### Audio
- [ ] `toasty "Test" --audio default`
- [ ] `toasty "Test" --audio im`
- [ ] `toasty "Test" --audio mail`
- [ ] `toasty "Test" --audio reminder`
- [ ] `toasty "Test" --audio sms`
- [ ] `toasty "Test" --audio loopingAlarm`
- [ ] `toasty "Test" --audio loopingCall`
- [ ] `toasty "Test" --audio silent`
- [ ] Verify each sound is distinct
- [ ] Verify looping sounds continue until dismissed
- [ ] Test invalid audio value (should show error)

### Combined Features

- [ ] `toasty "Build complete" -t "CI/CD" --progress "Build:1.0:Done" --attribution "via Jenkins" --audio default`
- [ ] `toasty "Alert!" --scenario urgent --hero-image <path> --audio loopingAlarm`
- [ ] `toasty "Meeting soon" --scenario reminder --audio reminder --attribution "via Calendar"`

### Error Handling

#### Missing Required Values
- [ ] `toasty "Test" -t` (should show error about missing title)
- [ ] `toasty "Test" --hero-image` (should show error)
- [ ] `toasty "Test" --scenario` (should show error)
- [ ] `toasty "Test" --attribution` (should show error)
- [ ] `toasty "Test" --progress` (should show error)
- [ ] `toasty "Test" --audio` (should show error)

#### Invalid Values
- [ ] `toasty "Test" --scenario invalid` (should show error with valid options)
- [ ] `toasty "Test" --audio invalid` (should show error with valid options)

#### Special Characters in Content
- [ ] `toasty "5 > 3 & 2 < 4" -t "Math & Logic"`
- [ ] `toasty "Quote: \"Hello\""`
- [ ] `toasty "Apostrophe's test"`
- [ ] Verify XML special characters are properly escaped

### Edge Cases

- [ ] Very long message text (>500 characters)
- [ ] Very long title (>100 characters)
- [ ] Very long attribution text
- [ ] Progress with decimal values: `--progress "Test:0.333:33.3%"`
- [ ] Progress with text status: `--progress "Step:0.5:Step 2 of 4"`
- [ ] Empty message (should show error)
- [ ] Unicode characters in message: `toasty "こんにちは 🎉"`

## Compatibility Testing

### Windows 10
Test on Windows 10 (build 15063 or later):
- [ ] All features work correctly
- [ ] No errors or crashes
- [ ] Notifications appear in Action Center

### Windows 11
Test on Windows 11:
- [ ] All features work correctly
- [ ] Modern notification styling
- [ ] Notifications appear in Notification Center

## Build Testing

### Release Build
```cmd
cmake -S . -B build -G "Visual Studio 17 2022" -A x64
cmake --build build --config Release
```
- [ ] Build completes without errors or warnings
- [ ] Executable size is reasonable (~230 KB)
- [ ] All features work in release build

### Debug Build
```cmd
cmake --build build --config Debug
```
- [ ] Build completes without errors or warnings
- [ ] All features work in debug build

## Performance Testing

- [ ] Notification appears within 1 second
- [ ] No memory leaks (test with multiple notifications)
- [ ] Process exits cleanly after showing notification

## Documentation Review

- [ ] README.md is accurate and complete
- [ ] EXAMPLES.md examples work correctly
- [ ] Help text matches actual functionality: `toasty --help`
- [ ] All command-line options are documented

## Integration Testing

### Claude Code Integration
Test the example from README.md in Claude Code settings.

## Known Issues / Limitations

Document any issues found during testing:
- Image formats supported: JPG, PNG, GIF
- Image size limits (if any)
- Path length limitations
- Character encoding issues (if any)

## Test Results Template

```
Date: ___________
Tester: ___________
OS Version: Windows 10/11 Build _____
Build Config: Release/Debug

Basic Features: PASS/FAIL
Hero Image: PASS/FAIL
Scenarios: PASS/FAIL
Attribution: PASS/FAIL
Progress Bar: PASS/FAIL
Audio: PASS/FAIL
Error Handling: PASS/FAIL
Edge Cases: PASS/FAIL

Notes:
___________
```
