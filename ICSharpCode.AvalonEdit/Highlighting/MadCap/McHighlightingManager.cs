using System;
using System.Linq;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace ICSharpCode.AvalonEdit.Highlighting
{
    public static class McHighlightingManager
    {
        static readonly string CssLanguageNames = /*NUI*/"ActionScript AppleScript D Erlang Go Haskell Lua Makefile MATLAB ObjectiveC OCaml R SQL Ruby Scala Bash Batch PlainText";

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
                RegisterLanguage(hm, lang);
            }
            hm.SortDefinitions();
        }

        static void RegisterLanguage(HighlightingManager.DefaultHighlightingManager hm, string cssLanguageName)
        {
            // Load our custom highlighting definition
            IHighlightingDefinition customHighlighting;
            using (Stream s = typeof(McHighlightingManager).Assembly.GetManifestResourceStream(Resources.Prefix + cssLanguageName + ".xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    customHighlighting = Xshd.HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            // and register it in the HighlightingManager
            string avalonLangName = GetAvalonLanguageName(cssLanguageName);
            hm.RegisterHighlighting(avalonLangName, new string[] { }, customHighlighting);
        }
        #endregion
    }
}
