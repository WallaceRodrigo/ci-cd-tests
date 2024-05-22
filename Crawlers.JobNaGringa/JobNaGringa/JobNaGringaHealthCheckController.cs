using Crawlers.Core.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Crawlers.JobNaGringa.JobNaGringa;

[Route("jobNaGringa")]
[ApiController]
public class JobNaGringaHealthCheckController
{
    private readonly IJobNaGringaHealthCheckUseCase _jobNaGringaHealthCheckUseCase;
    private readonly ILogger<JobNaGringaHealthCheckController> _logger;

    public JobNaGringaHealthCheckController(IJobNaGringaHealthCheckUseCase jobNaGringaHealthCheckUseCase, ILogger<JobNaGringaHealthCheckController> logger)
    {
        _jobNaGringaHealthCheckUseCase = jobNaGringaHealthCheckUseCase;
        _logger = logger;
    }
        
    [HttpGet("healthCheck")]
    [ProducesResponseType(typeof(IList<Job>), StatusCodes.Status200OK)]
    public async Task<IActionResult> HealthCheck()
    {
        try
        {
            var response = await _jobNaGringaHealthCheckUseCase.HealthCheck();

            if (response.Count == 0)
            {
                return new ObjectResult("Health Check Failed")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error getting the HealthCheck for Job na Gringa");
            
            return new ObjectResult(ex)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        return new OkObjectResult("Health Check Successfully");
    }
}