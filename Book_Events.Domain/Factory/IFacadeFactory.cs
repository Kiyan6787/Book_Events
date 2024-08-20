using Book_Events.Domain.Interfaces;
using Book_Events.Domain.Services;

namespace Book_Events.Domain.Factory
{
    public interface IFacadeFactory<T>
    {
        public T GetFacade(T facade);
    }
}
