using MongoDB.Driver;
using SaleCheckApp.Models;

namespace SaleCheckApp.Services;

public class SaleCheckDataService : ISaleCheckDataService
{
    private readonly IMongoCollection<ScrapedDataModel> _SaleCheckDataCollection;
    public SaleCheckDataService(IMongoCollection<ScrapedDataModel> weatherDataCollection)
    {
        _SaleCheckDataCollection = weatherDataCollection;
    }

    public async Task CreateAsync(ScrapedDataModel scrapedDataModel)
    {
        await _SaleCheckDataCollection.InsertOneAsync(scrapedDataModel);
    }

    public async Task<List<ScrapedDataModel>> GetAllAsync(CancellationToken cancellationToken)
    {
       return await _SaleCheckDataCollection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<ScrapedDataModel> GetOneAsync(string id, CancellationToken cancellationToken)
    {
        return await _SaleCheckDataCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        await _SaleCheckDataCollection.DeleteOneAsync(x => x.Id == id, cancellationToken);
    }
}
