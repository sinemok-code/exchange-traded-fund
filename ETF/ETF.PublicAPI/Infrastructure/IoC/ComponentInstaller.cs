namespace ETF.PublicAPI.Infrastructure.IoC
{
    using System.Web.Mvc;

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using ETF.API.Service;
    using ETF.API.Service.Interface;

    public class ComponentInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());

            container.Register(
                Component.For<IEtfContext>().ImplementedBy<EtfContext>().LifestyleSingleton());

            container.Register(
                Component.For<IEtfService>().ImplementedBy<EtfService>().LifestylePerWebRequest());

            container.Register(
                Component.For<IStockService>().ImplementedBy<StockService>().LifestylePerWebRequest());

            container.Register(
                Component.For<IFileService>().ImplementedBy<FileService>().LifestylePerWebRequest());
        }
    }
}