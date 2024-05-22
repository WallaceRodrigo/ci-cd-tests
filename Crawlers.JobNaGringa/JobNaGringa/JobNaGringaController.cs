using Crawlers.Core.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Crawlers.JobNaGringa.JobNaGringa;

[Route("jobNaGringa")]
[ApiController]
public class JobNaGringaController
{
    private readonly IJobNaGringaUseCase _jobNaGringaUseCase;

    public JobNaGringaController(IJobNaGringaUseCase jobNaGringaUseCase)
    {
        _jobNaGringaUseCase = jobNaGringaUseCase;
    }
    
    [HttpGet("run")]
    [ProducesResponseType(typeof(IList<Job>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Run()
    {
        var response = await _jobNaGringaUseCase.RunCrawler();

        return new OkObjectResult(response);
    }    
}