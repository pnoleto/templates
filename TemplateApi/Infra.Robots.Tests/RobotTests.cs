using Microsoft.Extensions.DependencyInjection;
using Infra.Robots.Interfaces;
using Infra.DI;

namespace Infra.Robots.Tests
{
    public class Tests
    {
        private IServiceProvider _serviceProvider;
        private IGloboRobot _g1Robot;
        private IFolhaRobot _folhaRobot;
        private IGazetaRobot _gdpRobot;

        [SetUp]
        public void Setup()
        {
            _serviceProvider = new ServiceCollection()
                .AddRepositories()
                .AddInMemoryDbContext()
                .BuildServiceProvider();

            _g1Robot = _serviceProvider.GetRequiredService<IGloboRobot>();
            _folhaRobot = _serviceProvider.GetRequiredService<IFolhaRobot>();
            _gdpRobot = _serviceProvider.GetRequiredService<IGazetaRobot>();
        }

        [TearDown]
        public void Dispose() { ((IDisposable)_serviceProvider).Dispose(); }

        [Test]
        public void G1LoadDocument()
        {
            CancellationTokenSource cts = new();

            Assert.ThrowsAsync<NotImplementedException>(()=> _g1Robot.ExecuteAsync(cts.Token));

            //var request = _g1Robot.ExecuteAsync(cts.Token);

            //request.Wait();

            //Assert.That(request.Result.Articles.Count, Is.Not.EqualTo(0));
        }

        [Test]
        public void FolhaLoadDocument()
        {
            CancellationTokenSource cts = new();

            Assert.ThrowsAsync<NotImplementedException>(() => _folhaRobot.ExecuteAsync(cts.Token));
            
            //var request = _folhaRobot.ExecuteAsync(cts.Token);

            //request.Wait();

            //Assert.That(request.Result.Articles.Count, Is.Not.EqualTo(0));
        }

        [Test]
        public void GdpLoadDocument()
        {
            CancellationTokenSource cts = new();

            Assert.ThrowsAsync<NotImplementedException>(() => _gdpRobot.ExecuteAsync(cts.Token));

            //var request = _gdpRobot.ExecuteAsync(cts.Token);

            //request.Wait();

            //Assert.That(request.Result.Articles.Count, Is.Not.EqualTo(0));
        }
    }
}