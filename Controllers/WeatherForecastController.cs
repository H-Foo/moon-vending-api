using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ReceiptController : ControllerBase
{

    private readonly IReceiptService _receipt;

    public ReceiptController(IReceiptService receipt)
    {
        _receipt = receipt;
    }

    [HttpGet]
    [Route("Create")]
    public async Task<IActionResult> CreateReceiptRecord(string userInput)
    {
        return Ok(await _receipt.AddReceipt(userInput));
    }

    [HttpGet]
    [Route("WakeUp")]
    public Task<IActionResult> WakeService()
    {
        return Task.FromResult<IActionResult>(Ok("I'm awake"));
    }
}
