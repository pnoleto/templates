using Application.DTO;
using MediatR;
using Application.Results;

namespace Application.Events
{
    public class LoginEvent : LoginQuery, IRequest<LoginResult>
    {
    }
}
