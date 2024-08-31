namespace Opcion1LosBorbotones.Presentation.Handlers;

public class DefaultSearchCriteria<I> : ISearchCriteriaRequester<I>
{
    private I _defaultCriteria;

    public DefaultSearchCriteria(I defaultCriteria)
    {
        _defaultCriteria = defaultCriteria;
    }

    public I RequestCriteria()
    {
        return _defaultCriteria;
    }
}