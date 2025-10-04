using System.ComponentModel.DataAnnotations.Schema;

public class PinHistory
{
    public int RecordId { get; set; }
    public string Pin { get; set; }
    public int VendingBoxId { get; set; }
    public DateTime DateAdded { get; set; }
}

public class PinClean
{
    public string Pin { get; set; }
    public string VendingBoxId { get; set; }
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