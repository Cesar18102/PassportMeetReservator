using Autofac;

using PassportMeetBlocker.Services;

namespace PassportMeetBlocker
{
    public sealed class BlockerDependencyHolder
    {
        private static IContainer serviceDependencies = null;
        public static IContainer ServiceDependencies
        {
            get
            {
                if (serviceDependencies == null)
                    serviceDependencies = BuildDependencies();
                return serviceDependencies;
            }
        }

        private static IContainer BuildDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<BlockerLogger>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
