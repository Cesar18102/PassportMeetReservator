using System.Text;
using System.Security.Cryptography;

using Autofac;

using Common.Services;

namespace Common
{
    public sealed class CommonDependencyHolder
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

            TypedParameter hasher = new TypedParameter(typeof(HashAlgorithm), SHA256.Create());
            TypedParameter encoding = new TypedParameter(typeof(Encoding), Encoding.UTF8);

            builder.RegisterType<HashingService>()
                   .UsingConstructor(typeof(HashAlgorithm), typeof(Encoding))
                   .WithParameters(new TypedParameter[] { hasher, encoding })
                   .AsSelf().SingleInstance();

            builder.RegisterType<SaltService>().AsSelf().SingleInstance();
            builder.RegisterType<AuthService>().AsSelf().SingleInstance();
            builder.RegisterType<FileService>().AsSelf().SingleInstance();
            builder.RegisterType<ProxyProvider>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
