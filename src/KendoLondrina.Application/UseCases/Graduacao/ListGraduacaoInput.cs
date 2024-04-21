using KenLo.Application.Common;
using KenLo.Domain.SeedWork.SearchableRepository;

namespace KenLo.Application.UseCases.Graduacao;

public class ListGraduacaoInput : PaginatedListInput
{
    public ListGraduacaoInput(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc
    ) : base(page, perPage, search, sort, dir)
    { }
    
    public ListGraduacaoInput() 
        : base(1, 15, "", "", SearchOrder.Asc)
    { }
}