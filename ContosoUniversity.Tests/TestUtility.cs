using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoUniversity.Tests
{
    public static class TestUtility
    {
        public static SchoolContext GetPopulatedContext()
        {
            var context = new SchoolContext(TestDbContextOptions());
            DbInitializer.Initialize(context);
            return context;
        }

        // https://docs.microsoft.com/en-us/aspnet/core/test/razor-pages-tests?view=aspnetcore-3.1
        public static DbContextOptions<SchoolContext> TestDbContextOptions()
        {
            // Create a new service provider to create a new in-memory database.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance using an in-memory database and 
            // IServiceProvider that the context should resolve all of its 
            // services from.
            var builder = new DbContextOptionsBuilder<SchoolContext>()
                .UseInMemoryDatabase("SchoolContext6")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
