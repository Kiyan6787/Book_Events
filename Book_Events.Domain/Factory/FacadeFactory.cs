using Book_Events.Domain.Interfaces;
using Book_Events.Domain.Services;

namespace Book_Events.Domain.Factory
{
    public class FacadeFactory<T> : IFacadeFactory<T>
    {
        private readonly T _facade;

        public FacadeFactory(T facade)
        {
            _facade = facade;
        }

        public T GetFacade(T facade)
        {
            return _facade;
        }
    }
}
