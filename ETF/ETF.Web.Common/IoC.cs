namespace ETF.Web.Common
{
    using Castle.Windsor;
    using Castle.Windsor.Installer;

    public static class IoC
    {
        private static readonly object ContainerLock = new object();

        private static IWindsorContainer container;

        public static IWindsorContainer Container
        {
            get
            {
                if (container != null)
                {
                    return container;
                }

                lock (ContainerLock)
                {
                    container = new WindsorContainer().Install(Configuration.FromAppConfig());
                }

                return container;
            }

            set
            {
                container = value;
            }
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static T Resolve<T>(string key)
        {
            return Container.Resolve<T>(key);
        }
    }
}