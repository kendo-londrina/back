using KenLo.Application.Common;

namespace KenLo.Application.UseCases.Graduacao;

public class ListGraduacaoOutput
    : PaginatedListOutput<GraduacaoModelOutput>
{
    public ListGraduacaoOutput(
        int page, 
        int perPage, 
        int total, 
        IReadOnlyList<GraduacaoModelOutput> items) 
        : base(page, perPage, total, items)
    {
    }
}
