using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrojanSourceDetector;

public record EmojiRecord(string Emoji, string RegexPattern, string UnicodeParts, string Description);
