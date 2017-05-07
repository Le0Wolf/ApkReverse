namespace ApkReverse.CLI
{
    using System;
    using System.IO;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using Core;
    using Core.Sevices;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            if(args.Length == 0 || !File.Exists(args[0])) return;

            var outputDir = args.Length > 1
                                ? args[1]
                                : Path.Combine(
                                    Path.GetDirectoryName(args[0]),
                                    Path.GetFileNameWithoutExtension(args[0]) + "_sources");

            using (var container = new WindsorContainer())
            {
                container.Register(
                Component.For<IDisplayService>()
                    .ImplementedBy<DisplayManager>()
                    .LifestyleSingleton());

                var app = new ReverseCoreInstaller();
                container.Install(app);
                app.Run(args[0], outputDir);
            }
        }

        private static void PrintUsage()
        {
            
        }
    }

    /// <summary>
    /// CLI implementation IDisplayService
    /// </summary>
    public class DisplayManager : IDisplayService
    {
        /// <summary>
        /// Выводит сообщение на экран
        /// </summary>
        /// <param name="msg"></param>
        public void WriteMsg(string msg)
        {
            Console.WriteLine(msg);
        }

        /// <summary>
        /// Отображает диалог с возможностью выбора одного из 2-х вариантов ответа
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="yesIsDefault"></param>
        /// <param name="yesMsg"></param>
        /// <param name="noMsg"></param>
        /// <returns></returns>
        public bool YesNoDialog(string msg, bool yesIsDefault = true, string yesMsg = "yes", string noMsg = "no")
        {
            if(yesMsg == noMsg)
                throw new ArgumentException("Yes msg equals no msg");

            string firstValue, secondValue;
            if (yesMsg[0] == noMsg[0])
            {
                firstValue = yesMsg;
                secondValue = noMsg;
            }
            else
            {
                firstValue = yesMsg[0].ToString();
                secondValue = noMsg[0].ToString();
            }
            var defaultValue = yesIsDefault ? firstValue : secondValue;

            Console.Write($"{msg} {firstValue}/{secondValue} [{defaultValue}] : ");
            var input = Console.ReadLine();
            Console.WriteLine();

            if (input == firstValue || input == yesMsg) return true;
            if (input == secondValue || input == noMsg) return false;
            return yesIsDefault;
        }
    }
}
