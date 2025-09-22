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
    public IActionResult WakeService()
    {
        return Ok("I'm awake");
    }
}
