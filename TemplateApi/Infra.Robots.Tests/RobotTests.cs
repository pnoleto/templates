using Infra.DI;
using Infra.Robots.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
                .AddFeedRobots()
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

            var request = _g1Robot.ExecuteAsync(cts.Token);

            request.Wait();

            Assert.That(request.Result.Articles.Count, Is.Not.EqualTo(0));
        }

        [Test]
        public void FolhaLoadDocument()
        {
            CancellationTokenSource cts = new();

            var request = _folhaRobot.ExecuteAsync(cts.Token);

            request.Wait();

            Assert.That(request.Result.Articles.Count, Is.Not.EqualTo(0));
        }

        [Test]
        public void GdpLoadDocument()
        {
            CancellationTokenSource cts = new();

            var request = _gdpRobot.ExecuteAsync(cts.Token);

            request.Wait();

            Assert.That(request.Result.Articles.Count, Is.Not.EqualTo(0));
        }
    }
}