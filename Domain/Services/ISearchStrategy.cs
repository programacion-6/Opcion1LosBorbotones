namespace Opcion1LosBorbotones.Domain;

public interface ISearchStrategy<T, I>
{
    public Task<List<T>> SearchByPage(I criteria, int pageNumber, int pageSize);
}