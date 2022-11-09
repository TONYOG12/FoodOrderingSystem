using Microsoft.AspNetCore.Hosting.Server;

namespace Domedo.API.Database.Seeds
{
    public class DatabaseSeeder
    {
        private readonly IServiceScope _scope;

        public DatabaseSeeder(IServiceScope scope)
        {
            _scope = scope;
        }

        public void SeedData()
        {
            var items = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x =>
                    typeof(ISeeder).IsAssignableFrom(x)
                    &&
                    !x.IsInterface
                    &&
                    !x.IsAbstract
                ).ToList();
            foreach (var item in items)
            {
                if (Activator.CreateInstance(item) is ISeeder instance)
                    instance.Handle(_scope);
            }

        }
    }
}
