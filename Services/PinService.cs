using Microsoft.EntityFrameworkCore;
using AutoMapper;

public interface IPinService
{
    Task<bool> AddPin(string pin, string boxLocation);
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

    public async Task<bool> AddPin(string pin, string boxLocation)
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
            DateAdded = DateTime.Now,
            VendingBoxId = (int)Enum.Parse<BoxLocation>(boxLocation)
        };
        _ctx.PinHistory.Add(pinRecord);

        if (await _ctx.SaveChangesAsync() <= 0) throw new Exception("Failed to add pin.");
        Console.WriteLine($"INFO: [AddPin] Successfully added pin [{pin}] for {boxLocation}");
        return true;
    }

    public async Task<List<PinClean>> GetActivePins(string? boxLocation = "")
    {

        var boxId = string.IsNullOrEmpty(boxLocation) ? 0 : (int)Enum.Parse<BoxLocation>(boxLocation.ToUpper());

        var result = await _ctx.PinHistory.Where(p => boxId == 0 || p.VendingBoxId == boxId).OrderByDescending(p => p.DateAdded).ToListAsync();
        return _mapper.Map<List<PinClean>>(result);
    }

}