using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
// add authenthication
public class PinController : ControllerBase
{
    private readonly string _phrase;
    private readonly IPinService _pinService;

    public PinController(IPinService service)
    {
        _pinService = service;
        _phrase = Environment.GetEnvironmentVariable("passPhrase");
    }

    [HttpPost]
    [Route("AddPin")]
    public async Task<IActionResult> CreateReceiptRecord(PinDto pinDto)
    {
        if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKey) || apiKey != _phrase)
        {
            return Unauthorized("who r u.");
        }

        return Ok(await _pinService.AddPin(pinDto.pin, pinDto.location, pinDto.validUntilDays));
    }

    [HttpGet]
    [Route("GetActivePins")]
    public async Task<IActionResult> GetActivePins(string? location = "")
    {
        if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKey) || apiKey != _phrase)
        {
            return Unauthorized("who r u.");
        }

        return Ok(await _pinService.GetActivePins(location));
    }

    [HttpGet]
    [Route("GetAllRecords")]
    public async Task<IActionResult> GetAllRecords()
    {
        if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKey) || apiKey != _phrase)
        {
            return Unauthorized("who r u.");
        }

        return Ok(await _pinService.GetAllRecords());
    }
}
