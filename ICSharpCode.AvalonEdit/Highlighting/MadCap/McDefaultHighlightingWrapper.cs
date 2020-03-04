using System.Collections.Generic;

namespace ICSharpCode.AvalonEdit.Highlighting
{
    public class McDefaultHighlightingWrapper : IHighlightingDefinition
    {
        private IHighlightingDefinition mDef;

        public string Name { get; }

        public HighlightingRuleSet MainRuleSet { get { return mDef.MainRuleSet; } }

        public IEnumerable<HighlightingColor> NamedHighlightingColors { get { return mDef.NamedHighlightingColors; } }

        public IDictionary<string, string> Properties { get { return mDef.Properties; } }

        HighlightingColor IHighlightingDefinition.GetNamedColor(string name)
        {
            return mDef.GetNamedColor(name);
        }

        HighlightingRuleSet IHighlightingDefinition.GetNamedRuleSet(string name)
        {
            return mDef.GetNamedRuleSet(name);
        }

        public McDefaultHighlightingWrapper(string definitionName) : this(McHighlightingManager.GetDefinition(definitionName)) { }

        public McDefaultHighlightingWrapper(IHighlightingDefinition definition)
        {
            mDef = definition;
            Name = /*UI*/"(inherit)";
        }
    }
}
