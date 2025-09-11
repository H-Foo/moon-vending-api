using System.ComponentModel.DataAnnotations.Schema;

public class PinHistory
{
    public int RecordId { get; set; }
    public string Pin { get; set; }
    public int VendingBoxId { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime ExpiryDate { get; set; }
    [NotMapped]
    public bool IsExpired => ExpiryDate <= DateTime.Now;
}

public class PinClean
{
    public string Pin { get; set; }
    public string VendingBoxId { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsExpired => ExpiryDate <= DateTime.Now;
}

public class PinDto
{
    public string pin { get; set; }
    public string location { get; set; }
    public int validUntilDays { get; set; }
}

public class Receipt
{
    public int SaleId { get; set; }
    public string ReceiptNo { get; set; }
    public int VendingBoxId { get; set; }
    public DateTime DateAdded { get; set; }
}

public enum BoxLocation
{
    LPR = 1,
    FOM = 2,
    FOE = 3
}