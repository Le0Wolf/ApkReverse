namespace ApkReverse.Core
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using Sevices;

    /// <summary>
    /// Точка входа в библиотеку, реалиизует инициализацию контейнера
    /// </summary>
    public class ReverseCoreInstaller: IWindsorInstaller
    {
        private IWindsorContainer _container;

        /// <summary>
        /// Инициализация и запуск рабора apk
        /// </summary>
        /// <param name="apkPath"></param>
        /// <param name="outputDir"></param>
        public void Run(string apkPath, string outputDir)
        {
            var logger = this._container.Resolve<IDisplayService>();

            logger?.WriteMsg($"Parsing apk: {apkPath}");
            logger?.WriteMsg($"Output: {outputDir}");
            if (logger == null || logger.YesNoDialog("Continue?"))
            {
                var project = this._container.Resolve<IProject>(new { apkpath = apkPath, projectPath = outputDir });

                project.ExtractAll();
                if (logger == null || logger.YesNoDialog("Create project?"))
                    project.GenerateProject();

                this._container.Release(project);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apkPath"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        public IProject NewProject(string apkPath, string outputDir)
        {
            var logger = this._container.Resolve<IDisplayService>();

            logger?.WriteMsg($"Parsing apk: {apkPath}");
            logger?.WriteMsg($"Output: {outputDir}");

            return this._container.Resolve<IProject>(new { apkpath = apkPath, projectPath = outputDir });
        }

        /// <summary>
        /// IWindsorInstaller implementation
        /// </summary>
        /// <param name="container"></param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            this._container = container;
            throw new System.NotImplementedException();
        }
    }
}
