using Refit;

namespace Infra.Integrations
{
    [Headers()]
    public interface IHttpClient
    {
        [Get("/")]
        public Task Get([Query] int? id = null);

        [Post("/")]
        public Task Post();

        [Patch("/{id}")]
        public Task Patch(int id);

        [Put("/{id}")]
        public Task Put( int id);

        [Delete("/{id}")]
        public Task Delete(int id);
    }
}
