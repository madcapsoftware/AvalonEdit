using System;
using System.Linq;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace ICSharpCode.AvalonEdit.Highlighting
{
    public static class McHighlightingManager
    {
        static readonly string CssLanguageNames = "ActionScript AppleScript D Erlang Go Haskell Lua Makefile MATLAB ObjectiveC OCaml R SQL Ruby Scala Bash Batch PlainText";

        #region Convert between CSS / AvalonEdit

        private static readonly Dictionary<string, string> CssToAvalon
            = new Dictionary<string, string>
            {
                { "CSharp", "C#" },
                { "CPP", "C/C++" },
                { "ObjectiveC", "Objective-C" }
            };

        private static readonly Dictionary<string, string> AvalonToCss
            = CssToAvalon.ToDictionary((i) => i.Value, (i) => i.Key);

        public static string GetCssLanguageName(string avalonLangName)
        {
            return AvalonToCss.ContainsKey(avalonLangName) ? AvalonToCss[avalonLangName] : avalonLangName;
        }

        public static string GetCssLanguageName(IHighlightingDefinition def)
        {
            return GetCssLanguageName(def.Name);
        }

        private static string GetAvalonLanguageName(string cssLangName)
        {
            return CssToAvalon.ContainsKey(cssLangName) ? CssToAvalon[cssLangName] : cssLangName;
        }

        public static IHighlightingDefinition GetDefinition(string cssName)
        {
            return HighlightingManager.Instance.GetDefinition(GetAvalonLanguageName(cssName));
        }

        #endregion

        #region register languages
        internal static void RegisterHighlightingAndSort(HighlightingManager.DefaultHighlightingManager hm)
        {
            var langArr = CssLanguageNames.Split(' ');
            foreach (string lang in langArr)
            {
				hm.RegisterHighlighting(GetAvalonLanguageName(lang), new string[0], $"{lang}.xshd");
            }
            hm.SortDefinitions();
        }
        #endregion
    }
}
