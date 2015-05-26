namespace ETF.Web.Infrastructure.IoC
{
    using System.Web.Mvc;

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using ETF.Web.Service;
    using ETF.Web.Service.Interfaces;

    public class ComponentInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());

            container.Register(
                Component.For<IFileService>().ImplementedBy<FileService>().LifestylePerWebRequest());
        }
    }
}