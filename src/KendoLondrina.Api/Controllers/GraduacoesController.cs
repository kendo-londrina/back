using KenLo.Api.ApiModels.Response;
using KenLo.Application.UseCases.Graduacao;
using Microsoft.AspNetCore.Mvc;

namespace KendoLondrina.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GraduacoesController : ControllerBase
{
    private readonly ILogger<GraduacoesController> _logger;
    private readonly ICreateGraduacao _createGraduacaoUsecase;
    private readonly IReadGraduacao _readGraduacaoUsecase;
    private readonly IDeleteGraduacao _deleteGraduacaoUsecase;

    public GraduacoesController(
        ILogger<GraduacoesController> logger,
        ICreateGraduacao createGraduacaoUsecase,
        IReadGraduacao readGraduacaoUsecase,
        IDeleteGraduacao deleteGraduacaoUsecase
    )
    {
        _logger = logger;
        _createGraduacaoUsecase = createGraduacaoUsecase;
        _readGraduacaoUsecase = readGraduacaoUsecase;
        _deleteGraduacaoUsecase = deleteGraduacaoUsecase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(GraduacaoModelOutput), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateGraduacaoInput input,
        CancellationToken cancellationToken
    )
    {
        var result = await _createGraduacaoUsecase.Handle(input, cancellationToken);
        return CreatedAtAction(nameof(Create), new { result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GraduacaoModelOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var input = new ReadGraduacaoInput(id);
        var result = await _readGraduacaoUsecase.Handle(input, cancellationToken);
        // return Ok(new ApiResponse<GraduacaoModelOutput>(result));
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var input = new DeleteGraduacaoInput(id);
        await _deleteGraduacaoUsecase.Handle(input, cancellationToken);
        return NoContent();
    }    

}
