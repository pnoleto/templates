namespace Infra.Robots.Interfaces
{
    public interface IRobot<T>
    {
        Task<T> ExecuteAsync(CancellationToken cancellationToken);
    }
}
