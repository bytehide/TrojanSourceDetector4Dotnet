/*
 * We wanted to create this tool to raise awareness of the problem, 
 * it will not be stable for programs with Arabic alphabet and could cause false positives with some emoticons.
 * Please note that this tool is an indicative template but you should not rely on it, it is really simple and basic.
 * 
 * Check https://dotnetsafer.com to know about us.
 * 
 */

using System.Globalization;

var defaultColor = Console.ForegroundColor;
Console.ForegroundColor = ConsoleColor.DarkCyan;

Console.WriteLine(@"
______      _              _              __          
|  _  \    | |            | |            / _|         
| | | |___ | |_ _ __   ___| |_ ___  __ _| |_ ___ _ __ 
| | | / _ \| __| '_ \ / _ \ __/ __|/ _` |  _/ _ \ '__|
| |/ / (_) | |_| | | |  __/ |_\__ \ (_| | ||  __/ |   
|___/ \___/ \__|_| |_|\___|\__|___/\__,_|_| \___|_|   
                                                      
Hidden value checker to prevent source trojan vulnerability.

###############################################################
");

Console.ForegroundColor = defaultColor;

Console.WriteLine(@"
Based on the examples of: 
https://github.com/nickboucher/trojan-source/tree/main/C%23

and based on the article by Nicholas Boucher and Ross Anderson; 
https://www.trojansource.codes

This small tool will help you to verify if any file in your source code
contains any hidden characters that could be linked to this vulnerability.

######

To understand the why of the tool, check:

https://medium.com/@juanal98/trojan-source-in-dotnet-25f6ce190c1a
");


static void alert(string text)
{
    var defaultColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.Write("[Warning]: ");
    Console.ForegroundColor = defaultColor;
    Console.Write("Hidden characters have been detected in: ");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.Write(text);
    Console.ForegroundColor = defaultColor;
    Console.WriteLine();
}

Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("Please enter a directory with one or more .NET projects to start (Full dir path): ");
Console.ForegroundColor = defaultColor;

var path = Console.ReadLine();

Console.WriteLine();


if(string.IsNullOrEmpty(path) || !Directory.Exists(path))
{
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine("Invalid path, check your .NET project directory");
    Console.ForegroundColor = defaultColor;
    return;
}

string[] dotnetFiles = Directory.GetFiles(path: path, "*.cs", SearchOption.AllDirectories);

var issuesCount = 0;

var scannedFiles = 0;

foreach (var dotnetFile in dotnetFiles)
{
    scannedFiles++;

    var nonRenderingCategories = new UnicodeCategory[] {
    UnicodeCategory.Control,
    UnicodeCategory.OtherNotAssigned,
    UnicodeCategory.Format,
    UnicodeCategory.Surrogate };

    using StreamReader sr = new StreamReader(dotnetFile);

    while (sr.Peek() >= 0)
    {
        var c = (char)sr.Read();
        var category = Char.GetUnicodeCategory(c);

        var isPrintable = Char.IsWhiteSpace(c) ||
              !nonRenderingCategories.Contains(category);

        if (!isPrintable)
        {
            alert(dotnetFile);
            issuesCount++;
            break;
        }
    }

    sr.Close();

    sr.Dispose();
}


if(issuesCount == 0)
{
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine();
    Console.WriteLine($"Perfect! No problems have been detected in the analysis of {scannedFiles} files.");
    Console.ForegroundColor = defaultColor;
    Console.ReadKey();
    return;
}

Console.ForegroundColor = ConsoleColor.DarkYellow;
Console.WriteLine();
Console.WriteLine($"{issuesCount} compromised files have been detected based on a total of {scannedFiles} files scanned, please review.");
Console.ForegroundColor = defaultColor;

Console.ReadKey();





