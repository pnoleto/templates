using Shared.Mediator.Base;
using Shared.Mediator.Interface;

namespace Shared.Mediator
{
    public class Result
    {

    }
    public class Test: IEvent
    {

    }

    public class Test1 : IEvent<Result>
    {

    }
    public class TestEventHandler: EventHandlerBase<Test>
    {

    }
    public class TestEventHandlerWithResult : EventHandlerBase<Test1, Result>
    {

    }
}
