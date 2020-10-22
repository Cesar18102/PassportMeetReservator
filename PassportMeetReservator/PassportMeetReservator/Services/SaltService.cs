using System;

using Autofac;

namespace PassportMeetReservator.Services
{
    public class SaltService
    {
        private const int MIN_SALT = 100000;
        private const int MAX_SALT = 1000000;

        private static Random Random = new Random();
        private static HashingService HashingService = DependencyHolder.ServiceDependencies.Resolve<HashingService>();

        public string GetRandomSalt()
        {
            return Random.Next(MIN_SALT, MAX_SALT).ToString();
        }

        public string GetSaltedHash(string data, string salt)
        {
            return HashingService.GetHash(data + salt);
        }
    }
}