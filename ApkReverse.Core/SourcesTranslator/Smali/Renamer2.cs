namespace ApkReverse.Core.SourcesTranslator.Smali
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using ApkReverse.Core.SourcesTranslator.Smali.SmaliParser;

    public class Renamer2
    {
        private List<string> _projectFilesIndex;

        private static SmaliClass GetClass(string file)
        {
            Token prevToken = null;
            var smaliClass = new SmaliClass();
            using (var lexer = new SmaliLexer(new StreamReader(file)))
            {
                foreach (var token in lexer.Where(t => !t.Hidden))
                {
                    if (token.TokenType == TokenType.AccessSpec) continue;

                    if (prevToken?.TokenType == TokenType.SourceDirective && token.TokenType == TokenType.StringLiteral) smaliClass.OriginalName = token.Meta.Trim('"').Split('.')[0];

                    if (token.TokenType == TokenType.ClassDescriptor && !token.Meta.StartsWith("Ljava")
                        && !token.Meta.StartsWith("Ldalvik"))
                    {
                        var link = token.Meta.Substring(1, token.Meta.Length - 2).Replace('/', '.');
                        if (prevToken?.TokenType == TokenType.ClassDirective)
                        {
                            smaliClass.ClassName = link;
                            smaliClass.ClassNameToken = token;
                        }
                        else
                        {
                            if (!smaliClass.Links.ContainsKey(link)) smaliClass.Links.Add(link, new List<Token>());

                            smaliClass.Links[link].Add(token);
                        }
                    }
                    prevToken = token;
                }
            }
            return smaliClass;
        }

        public Dictionary<string, SmaliClass> GetAllClasses(string rootDir)
        {
            return Directory.EnumerateFiles(rootDir, "*.smali", SearchOption.AllDirectories).ToDictionary(file => file, GetClass);
        }
    }
}