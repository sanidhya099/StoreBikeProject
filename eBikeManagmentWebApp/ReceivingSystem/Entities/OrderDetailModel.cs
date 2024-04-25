
namespace ReceivingSystem.Entities;

public class OrderDetailModel
{
    public int PartID { get; set; }
    public string? Description { get; set; }
    public int OQty { get; set; }
    public int OStd { get; set; }
    public int RecQty { get; set; }
    public string? Comment { get; set; }
    public StandardJob Service { get; set; }
    public int ServiceId { get; set; }
    public decimal Hours { get; set; }
}
