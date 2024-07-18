using KenLo.Application.UseCases.Graduacao;
using Microsoft.AspNetCore.Mvc;

namespace KendoLondrina.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GraduacoesController : ControllerBase
{
    private readonly ILogger<GraduacoesController> _logger;

    private readonly ICreateGraduacao _createGraduacaoUsecase;

    public GraduacoesController(
        ILogger<GraduacoesController> logger,
        ICreateGraduacao createGraduacaoUsecase
    )
    {
        _logger = logger;
        _createGraduacaoUsecase = createGraduacaoUsecase;
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
        // return Created("", result);
    }
}
