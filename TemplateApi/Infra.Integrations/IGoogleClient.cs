using Refit;

namespace Infra.Integrations
{
    [Headers("Content-Type: text/html", "Content-Type: application/json")]
    public interface IGoogleClient
    {
        [Get("/")]
        public Task Get([Query] int? id = null);

        [Post("/")]
        public Task Post();

        [Put("/{id}")]
        public Task Put( int id);

        [Delete("/{id}")]
        public Task Delete(int id);
    }
}
