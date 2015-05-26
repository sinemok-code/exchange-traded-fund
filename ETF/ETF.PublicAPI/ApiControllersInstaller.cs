namespace ETF.PublicAPI
{
    using System.Web.Http;

    using Castle.MicroKernel.Registration;

    public class ApiControllersInstaller : IWindsorInstaller
    {
        public void Install(
            Castle.Windsor.IWindsorContainer container,
            Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestylePerWebRequest());
        }
    }
}