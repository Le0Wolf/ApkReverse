namespace ApkReverse.Core.SourcesTranslator.Smali.SmaliParser
{
    using System.Collections.Generic;

    public class SmaliClass
    {
        public string ClassName { get; set; }

        public Token ClassNameToken { get; set; }

        public string OriginalName { get; set; }

        public Dictionary<string, List<Token>> Links = new Dictionary<string, List<Token>>();
    }
}