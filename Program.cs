using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

if (args.Length == 0)
{
    PrintUsage();
    return 0;
}

string? message = null;
string title = "Notification";
string? iconPath = null;

for (int i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "-h":
        case "--help":
            PrintUsage();
            return 0;

        case "-t":
        case "--title":
            if (i + 1 < args.Length)
                title = args[++i];
            break;

        case "-i":
        case "--icon":
            if (i + 1 < args.Length)
                iconPath = args[++i];
            break;

        default:
            if (!args[i].StartsWith('-') && message == null)
                message = args[i];
            break;
    }
}

if (string.IsNullOrEmpty(message))
{
    Console.Error.WriteLine("Error: Message is required.");
    PrintUsage();
    return 1;
}

try
{
    // Build toast XML
    string imageXml = "";
    if (!string.IsNullOrEmpty(iconPath) && File.Exists(iconPath))
    {
        var fullPath = Path.GetFullPath(iconPath).Replace('\\', '/');
        imageXml = $"""<image placement="appLogoOverride" src="file:///{fullPath}"/>""";
    }

    string toastXml = $"""
        <toast>
            <visual>
                <binding template="ToastGeneric">
                    <text>{EscapeXml(title)}</text>
                    <text>{EscapeXml(message)}</text>
                    {imageXml}
                </binding>
            </visual>
        </toast>
        """;

    var doc = new XmlDocument();
    doc.LoadXml(toastXml);

    var toast = new ToastNotification(doc);

    // For unpackaged apps, use a well-known AppUserModelId
    // Windows PowerShell is a good choice as it's always present
    var notifier = ToastNotificationManager.CreateToastNotifier("Microsoft.Windows.PowerShell");
    notifier.Show(toast);

    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    if (ex.InnerException != null)
        Console.Error.WriteLine($"Inner: {ex.InnerException.Message}");
    return 1;
}

static string EscapeXml(string text)
{
    return text
        .Replace("&", "&amp;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;")
        .Replace("\"", "&quot;")
        .Replace("'", "&apos;");
}

static void PrintUsage()
{
    Console.WriteLine("""
        toasty - Windows toast notification CLI

        Usage: toasty <message> [options]

        Options:
          -t, --title <text>   Set notification title (default: "Notification")
          -i, --icon <path>    Set notification icon (local file path)
          -h, --help           Show this help

        Examples:
          toasty "Build completed"
          toasty "Task done" -t "Claude Code"
          toasty "Hello" --title "My App" --icon "C:\icons\app.png"
        """);
}
