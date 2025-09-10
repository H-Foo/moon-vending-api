using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

public interface IReceiptService
{
    Task<string> AddReceipt(string receiptNo);
}

// =====

public class ReceiptService : IReceiptService
{
    private readonly VendingContext _ctx;

    public ReceiptService(VendingContext context)
    {
        _ctx = context;
    }

    public async Task<string> AddReceipt(string receiptNo)
    {
        var location = receiptNo.Substring(0, 3).ToUpper();

        if (IsReceiptNoValid(receiptNo.Substring(4)) == false)
            throw new Exception("Unrecognized receipt Number.");

        var receipt = new Receipt
        {
            ReceiptNo = receiptNo.Substring(4),
            VendingBoxId = (int)Enum.Parse<BoxLocation>(location),
            DateAdded = DateTime.Now
        };
        _ctx.Receipt.Add(receipt);

        if (await _ctx.SaveChangesAsync() <= 0) throw new Exception("Failed to add new record receipt for " + receiptNo);

        // return pin from db
        var pinNums = await _ctx.PinHistory.Where(p => p.VendingBoxId == receipt.VendingBoxId && p.ExpiryDate >= DateTime.Now).ToListAsync();
        var pin = pinNums.Where(p => !p.IsExpired).Select(p => p.Pin).FirstOrDefault();
        Console.WriteLine("Pin:" + pin);

        return pin;

    }

    private bool IsReceiptNoValid(string receiptNo)
    {
        var split = receiptNo.Split("-");
        var merchantNo = split[0];
        var ewalletNo = split[1];

        // regex for merchantRefNo here
        if (!Regex.IsMatch(merchantNo, @"^[A-Za-z0-9]{8}$"))
            return false;

        if (ewalletNo.Length != 6 || !ewalletNo.All(char.IsDigit))
            return false;
        
        return true;
    }
}