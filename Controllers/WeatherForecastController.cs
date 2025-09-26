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
    [HttpHead]
    [Route("Create")]
    public async Task<IActionResult> CreateReceiptRecord(string userInput)
    {

        if (HttpContext.Request.Method == HttpMethods.Head)
        {
            HttpContext.Response.Headers["X-Service-Status"] = "Am Awake";
            return Ok();
        }

        return Ok(await _receipt.AddReceipt(userInput));
    }

    [HttpGet("DontSleep")]
    [HttpHead("DontSleep")]
    public async Task<IActionResult> WakeDbService()
    {
        return Ok(await _receipt.GetLatestRecord());
    }

}
