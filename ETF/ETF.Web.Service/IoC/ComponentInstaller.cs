namespace ETF.Web.Service.IoC
{
    using System.Web.Configuration;

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using ETF.Web.Repository;
    using ETF.Web.Repository.DataAccess;
    using ETF.Web.Repository.DataAccess.Factory;
    using ETF.Web.Repository.Interfaces;

    public class ComponentInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IApiHelperFactory>()
                    .ImplementedBy<ApiHelperFactory>()
                    .LifeStyle.Singleton);

            container.Register(
                Component.For<IFileRepository>().ImplementedBy<FileRepository>().LifestylePerWebRequest());

            container.Register(
                Component.For<IApiHelper>()
                    .ImplementedBy<HttpHelper>()
                    .Named(Dictionary.UploadFileApiData)
                    .DependsOn(
                        new
                        {
                            path =
                                WebConfigurationManager.AppSettings[Dictionary.UploadFileApiData],
                                httpMethod = HttpMethod.Post
                        })
                    .LifestylePerWebRequest());
        }
    }
}