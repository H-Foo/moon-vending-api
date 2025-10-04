using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Net;

public interface IReceiptService
{
    Task<string> AddReceipt(string receiptNo);
    Task<string> GetLatestRecord();
}

public class ReceiptService : IReceiptService
{
    private readonly VendingContext _ctx;
    private readonly ILogger<ReceiptService> _logger;

    public ReceiptService(VendingContext context, ILogger<ReceiptService> logger)
    {
        _ctx = context;
        _logger = logger;
    }

    public async Task<string> AddReceipt(string receiptNo)
    {
        Console.WriteLine("INFO: [AddReceipt] - receiptNo passed is " + receiptNo);
        // decode string
        var decoded = WebUtility.UrlDecode(receiptNo);

        var location = decoded.Substring(0, 3).ToUpper();

        if (IsReceiptNoValid(decoded.Substring(4)) == false){
            Console.WriteLine("Unrecognized receipt number: " + receiptNo + " | decoded: " + decoded);
            throw new Exception("Unrecognized receipt Number.");
        }

        var receipt = new Receipt
        {
            ReceiptNo = decoded.Substring(4),
            VendingBoxId = (int)Enum.Parse<BoxLocation>(location),
            DateAdded = DateTime.Now
        };
        _ctx.Receipt.Add(receipt);

        if (await _ctx.SaveChangesAsync() <= 0){
            Console.WriteLine("Failed to add new record for: " + decoded);
            throw new Exception("Failed to add new record receipt for " + decoded);
        }

        // return pin from db
        var pinNums = await _ctx.PinHistory.Where(p => p.VendingBoxId == receipt.VendingBoxId).ToListAsync();
        if (!pinNums.Any()) throw new Exception("No pin exist for vending box at " + location);

        var pin = pinNums.OrderByDescending(p => p.DateAdded).Select(p => p.Pin).FirstOrDefault();
        

        return pin;
    }

    public async Task<string> GetLatestRecord()
    {
        return await _ctx.Receipt.OrderByDescending(r => r.DateAdded).Select(r => r.ReceiptNo).FirstOrDefaultAsync() ?? "at least im here.";
    }

    private bool IsReceiptNoValid(string receiptNo)
    {
        var split = receiptNo.Split("-");
        var merchantNo = split[0];
        var ewalletNo = split[1];

        // regex for merchantRefNo here
        if (!Regex.IsMatch(merchantNo, @"^[A-Za-z0-9]{8}$"))
            return false;

        if (ewalletNo.Length != 7 || !ewalletNo.All(char.IsDigit))
            return false;

        return true;
    }
}