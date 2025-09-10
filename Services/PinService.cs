using Microsoft.EntityFrameworkCore;
using AutoMapper;

public interface IPinService
{
    Task<bool> AddPin(string pin, string boxLocation, int validUntil);
    Task<List<PinClean>> GetAllRecords();
    Task<List<PinClean>> GetActivePins(string? boxLocation);

}

public class PinService : IPinService
{
    private readonly VendingContext _ctx;
    private readonly IMapper _mapper;
    public PinService(VendingContext context, IMapper mapper)
    {
        _ctx = context;
        _mapper = mapper;
    }

    public async Task<bool> AddPin(string pin, string boxLocation, int validFor)
    {
        // do checking
        if (pin.Length != 3 || !pin.All(char.IsDigit))
            throw new Exception("Invalid pin format.");

        var location = boxLocation.ToUpper();
        if (location != "LPR" && location != "FOM" && location != "FOE")
            throw new Exception("Unknown box location: " + boxLocation);


        var pinRecord = new PinHistory
        {
            Pin = pin,
            VendingBoxId = (int)Enum.Parse<BoxLocation>(boxLocation),
            DateAdded = DateTime.Now,
            ExpiryDate = DateTime.Now.AddDays(validFor)
        };
        _ctx.PinHistory.Add(pinRecord);

        if (await _ctx.SaveChangesAsync() <= 0) throw new Exception("Failed to add pin.");

        return true;
    }

    public async Task<List<PinClean>> GetActivePins(string? boxLocation = "")
    {

        var boxId = string.IsNullOrEmpty(boxLocation) ? 0 : (int)Enum.Parse<BoxLocation>(boxLocation.ToUpper());

        var result = await _ctx.PinHistory.Where(p => p.ExpiryDate >= DateTime.Now && (boxId == 0 || p.VendingBoxId == boxId)).ToListAsync();
        return _mapper.Map<List<PinClean>>(result);
    }

    public async Task<List<PinClean>> GetAllRecords()
    {
        var result = await _ctx.PinHistory.ToListAsync();
        return _mapper.Map<List<PinClean>>(result.OrderByDescending(p => p.ExpiryDate).ToList());
    }
}