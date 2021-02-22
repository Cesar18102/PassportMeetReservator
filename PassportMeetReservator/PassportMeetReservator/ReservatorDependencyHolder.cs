using Autofac;

using PassportMeetReservator.Services;

namespace PassportMeetReservator
{
    public sealed class ReservatorDependencyHolder
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

            builder.RegisterType<ReservatorLogger>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
