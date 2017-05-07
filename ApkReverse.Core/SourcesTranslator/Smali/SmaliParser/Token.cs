namespace ApkReverse.Core.SourcesTranslator.Smali.SmaliParser
{
    public class Token
    {
        public int Offset { get; set; }
        public int End { get; set; }

        public TokenType TokenType { get; set; }

        public string Meta { get; set; }

        public bool Hidden { get; set; }

        public Token(TokenType type, string meta)
        {
            this.TokenType = type;
            this.Meta = meta;
            this.Hidden = false;
        }
    }
}