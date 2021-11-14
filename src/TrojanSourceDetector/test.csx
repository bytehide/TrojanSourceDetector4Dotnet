#r "nuget: System.Text.Json, 6.0.0"
#load ".\EmojiRecord.cs"

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


Console.OutputEncoding = System.Text.Encoding.UTF8;

const string FILENAME = @"E:\GitHub\TrojanSourceDetector4Dotnet\src\TrojanSourceDetector\emoji-test.txt";
const string OUT_FILENAME = @"E:\GitHub\TrojanSourceDetector4Dotnet\src\TrojanSourceDetector\emojis.json";

Console.WriteLine(File.Exists(FILENAME));

if(!File.Exists(FILENAME)) return 1;

List<EmojiRecord> Tests = new();
var lines = File.ReadAllLines(FILENAME).ToArray();

foreach(var line in lines)
{
    string output = string.Empty;

    if(line.Trim().StartsWith("#")) continue;

    var parts = line.Split(";".ToCharArray());

    var codes = parts[0].Split("#".ToCharArray())[0];

    var codeList = codes.Split(" ".ToCharArray());

    string emoji = codeList[0].ToLowerInvariant();

    if(emoji is null or "") continue;

    var emojiInteger = int.Parse(emoji, NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier);
    var emojiString = Char.ConvertFromUtf32(emojiInteger);

    foreach(var code in codeList.Skip(1))
    {
        var toDecode = code.Trim();
        if(toDecode is null or "") continue;

        var codeInteger = int.Parse(toDecode, NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier);
        emojiString += Char.ConvertFromUtf32(codeInteger);
    }

    var emojiChars = emojiString.ToCharArray();
    var regexPattern = string.Join(@"\s*", emojiChars.Select(c => $@"\u{(short)c,4:X}".Replace(" ", "0").Trim())).Trim();
    var regex = new Regex(regexPattern, RegexOptions.Singleline | RegexOptions.CultureInvariant);
    var match = regex.Match(emojiString);
    Console.WriteLine($"|{parts[0].Trim()}| {emojiString} | {regexPattern} | {match.Success} | {match.Value} |{parts[1].Trim()}");
    if(!match.Success) throw new ApplicationException($"Could not match {emojiString} to {regexPattern}");

    Tests.Add(new (Emoji: emojiString, RegexPattern: regexPattern, UnicodeParts: parts[0].Trim(), Description: parts[1].Trim() ));
}

if(Tests.Count > 0)
{
    var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(Tests, Tests.GetType(), null);

    if(File.Exists(OUT_FILENAME)) {
        File.Delete(OUT_FILENAME);
        if (File.Exists(OUT_FILENAME)) throw new IOException($"Could not delete existing json file at {OUT_FILENAME}");
    }

    File.WriteAllBytes(OUT_FILENAME, jsonBytes);

    if(!File.Exists(OUT_FILENAME)) throw new IOException($"Could not write json to {OUT_FILENAME}");
}