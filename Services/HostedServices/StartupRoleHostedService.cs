using CUG_ONLINE_COURSES.Services.RolesServices;

namespace CUG_ONLINE_COURSES.Services.HostedServices
{
    public class StartupRoleHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public StartupRoleHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
                await roleService.EnsureRolesExistAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}
