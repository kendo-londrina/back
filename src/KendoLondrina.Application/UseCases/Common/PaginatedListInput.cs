using KenLo.Domain.SeedWork.SearchableRepository;

namespace KenLo.Application.Common;

public abstract class PaginatedListInput
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string Search { get; set; }
    public string Sort { get; set; }
    public SearchOrder Dir { get; set; }
    protected PaginatedListInput(
        int page, 
        int perPage, 
        string search, 
        string sort, 
        SearchOrder dir)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        Sort = sort;
        Dir = dir;
    }

    public SearchInput ToSearchInput()
        => new(Page, PerPage, Search, Sort, Dir);
}
