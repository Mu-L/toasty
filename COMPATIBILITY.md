# Windows Toast Features Compatibility Report

This document summarizes the research findings on Windows toast notification features and their compatibility.

## Executive Summary

All features implemented in this PR are **compatible with both Windows 10 and Windows 11**. Most features were introduced in Windows 10 Creators Update (build 15063) and work identically on Windows 11.

## Feature Compatibility Matrix

| Feature | Windows 10 | Windows 11 | Minimum Build | Notes |
|---------|-----------|-----------|---------------|-------|
| Hero Image | ✅ Yes | ✅ Yes | 10240 (Anniversary Update) | Large banner image at top |
| Scenario (reminder) | ✅ Yes | ✅ Yes | 10240 | For calendar events |
| Scenario (alarm) | ✅ Yes | ✅ Yes | 10240 | Persistent, looping |
| Scenario (incomingCall) | ✅ Yes | ✅ Yes | 10240 | For VoIP calls |
| Scenario (urgent) | ✅ Yes | ✅ Yes | 10240 | High priority |
| Attribution Text | ✅ Yes | ✅ Yes | 10240 | Small gray text at bottom |
| Progress Bar | ✅ Yes | ✅ Yes | 15063 (Creators Update) | Desktop only |
| Audio (Default) | ✅ Yes | ✅ Yes | 10240 | Standard notification sound |
| Audio (IM) | ✅ Yes | ✅ Yes | 10240 | Instant message sound |
| Audio (Mail) | ✅ Yes | ✅ Yes | 10240 | Email notification sound |
| Audio (Reminder) | ✅ Yes | ✅ Yes | 10240 | Reminder sound |
| Audio (SMS) | ✅ Yes | ✅ Yes | 10240 | Text message sound |
| Audio (Looping Alarm) | ✅ Yes | ✅ Yes | 10240 | Continues until dismissed |
| Audio (Looping Call) | ✅ Yes | ✅ Yes | 10240 | Continues until dismissed |
| Audio (Silent) | ✅ Yes | ✅ Yes | 10240 | No sound |

## Detailed Feature Analysis

### Hero Image

**XML Schema:**
```xml
<image placement="hero" src="file:///C:/path/to/image.jpg"/>
```

**Compatibility:**
- Windows 10 Anniversary Update (10240) and later
- Windows 11 - all builds
- Supported image formats: JPG, PNG, GIF, BMP
- Maximum file size: 1024 KB
- Recommended dimensions: 364x180 pixels

**Fallback Behavior:**
- Image not found: Toast displays without image
- Invalid format: Toast displays without image
- No error shown to user

### Scenarios

**XML Schema:**
```xml
<toast scenario="reminder|alarm|incomingCall|urgent">
```

**Compatibility:**
- All scenarios available since Windows 10 build 10240
- Identical behavior on Windows 11

**Scenario Behaviors:**

1. **reminder**
   - Appears in Action Center
   - Standard dismiss behavior
   - No looping audio
   - Use for: Calendar events, task reminders

2. **alarm**
   - Persistent notification
   - Dismisses only on user action
   - Looping audio (if specified)
   - Use for: Alarms, timers

3. **incomingCall**
   - Full-screen on mobile devices
   - Standard notification on desktop
   - Supports looping audio
   - Use for: VoIP calls, video calls

4. **urgent**
   - High priority in Action Center
   - Stays on screen longer
   - More prominent display
   - Use for: Critical alerts, emergencies

### Attribution Text

**XML Schema:**
```xml
<text placement="attribution">via Application Name</text>
```

**Compatibility:**
- Windows 10 build 10240 and later
- Windows 11 - all builds
- Maximum length: ~100 characters (longer text may be truncated)

**Styling:**
- Small gray text
- Appears at bottom of toast
- System font, reduced size

### Progress Bar

**XML Schema:**
```xml
<progress title="Task Name" value="0.75" status="75%"/>
```

**Compatibility:**
- Windows 10 Creators Update (build 15063) and later
- Windows 11 - all builds
- Desktop only (not supported on mobile)

**Parameters:**
- `title`: Display name for the task (required)
- `value`: Progress from 0.0 to 1.0 (required, use "indeterminate" for unknown)
- `status`: Status text displayed (required)
- `valueStringOverride`: Optional, overrides the visual representation

**Limitations:**
- Desktop context required
- Value must be numeric 0.0-1.0 or "indeterminate"
- Cannot be updated after toast is shown (requires data binding for dynamic updates)

### Audio

**XML Schema:**
```xml
<audio src="ms-winsoundevent:Notification.Default"/>
<audio silent="true"/>
```

**Compatibility:**
- All audio events available since Windows 10 build 10240
- Identical sounds on Windows 11

**Audio Events:**

| Event | Use Case |
|-------|----------|
| Notification.Default | General notifications |
| Notification.IM | Instant messages, chat |
| Notification.Mail | Email notifications |
| Notification.Reminder | Reminders, calendar |
| Notification.SMS | Text messages |
| Notification.Looping.Alarm | Alarms (loops until dismissed) |
| Notification.Looping.Call | Incoming calls (loops until dismissed) |

**Silent Mode:**
- `<audio silent="true"/>` suppresses all sound
- Respects system "Do Not Disturb" settings

## OS Version Detection

The application does **not** need to detect OS version because:
1. All implemented features work on both Windows 10 and 11
2. The Windows.UI.Notifications API gracefully handles unsupported features
3. Minimum requirement is Windows 10 build 15063 (Creators Update, 2017)

If version detection is needed in the future:
```cpp
// Get Windows version
OSVERSIONINFOEXW osvi = { sizeof(osvi) };
GetVersionExW((OSVERSIONINFOW*)&osvi);

// Windows 11 is build 22000+
bool isWindows11 = osvi.dwBuildNumber >= 22000;
```

## Fallback Behavior

When features are not supported or fail:

1. **Hero Image Fails:**
   - Toast displays without image
   - No error shown

2. **Invalid Scenario:**
   - Treated as default notification
   - No error shown

3. **Progress Bar on Unsupported Build:**
   - Element ignored
   - Toast displays normally

4. **Invalid Audio:**
   - Falls back to default sound
   - No error shown

## Testing Recommendations

### Minimum Test Environments

1. **Windows 10 (build 15063)** - Creators Update
   - Oldest supported build
   - All features should work

2. **Windows 10 (build 19045)** - Latest 10
   - Current Windows 10 LTSC/Enterprise
   - Verify modern improvements work

3. **Windows 11 (build 22000+)**
   - Latest consumer OS
   - Test modern styling

### Focus Assist / Do Not Disturb

Be aware that Windows Focus Assist modes affect toast behavior:
- **Off**: All toasts appear normally
- **Priority only**: Only urgent scenarios appear
- **Alarms only**: Only alarm scenarios appear

## References

- [Microsoft Docs: Toast Content Schema](https://learn.microsoft.com/en-us/windows/apps/develop/notifications/app-notifications/toast-schema)
- [Microsoft Docs: Adaptive Interactive Toasts](https://learn.microsoft.com/en-us/windows/apps/develop/notifications/app-notifications/adaptive-interactive-toasts)
- [Microsoft Docs: Toast Progress Bar](https://learn.microsoft.com/en-us/windows/apps/develop/notifications/app-notifications/toast-progress-bar)
- [Microsoft Docs: Toast Schema Root](https://learn.microsoft.com/en-us/uwp/schemas/tiles/toastschema/schema-root)

## Conclusion

All features implemented in this PR are production-ready and compatible with:
- ✅ Windows 10 Creators Update (build 15063) and later
- ✅ Windows 11 all builds

No special version detection or fallback logic is required. The Windows notification system handles compatibility gracefully.
