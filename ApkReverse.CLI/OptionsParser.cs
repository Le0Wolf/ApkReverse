namespace ApkReverse.CLI
{
    using System;
    using System.Collections.Generic;

    internal class OptionsParser
    {
        private IList<string> positionalOptions;

        public OptionsParser(IList<string> positionalOptions)
        {
            this.positionalOptions = positionalOptions;
        }

        public void AddPositionalArgument(string name, string description)
        {
            
        }

        public void PrintHelp()
        {
            Console.WriteLine("Usage:");

            var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;



            Console.WriteLine($"    {appName}");
        }
    }
}
