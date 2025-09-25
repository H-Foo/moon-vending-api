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

    [HttpGet("WakeUp")]
    [HttpHead("WakeUp")]
    public async Task<IActionResult> WakeService()
    {
        return Ok("im awake");
    }

    [HttpGet("DontSleep")]
    [HttpHead("DontSleep")]
    public async Task<IActionResult> WakeDbService()
    {
        return Ok(await _receipt.GetLatestRecord());
    }

}
