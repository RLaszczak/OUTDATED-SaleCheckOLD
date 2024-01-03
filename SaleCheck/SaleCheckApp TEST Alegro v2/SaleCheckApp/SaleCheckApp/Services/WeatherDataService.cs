using MongoDB.Driver;
using PogodaApp.Models;

namespace PogodaApp.Services;

public class WeatherDataService : IWeatherDataService
{
    private readonly IMongoCollection<ScrapedDataModel> _WeatherDataCollection;
    public WeatherDataService(IMongoCollection<ScrapedDataModel> weatherDataCollection)
    {
        _WeatherDataCollection = weatherDataCollection;
    }

    public async Task CreateAsync(ScrapedDataModel scrapedDataModel)
    {
        await _WeatherDataCollection.InsertOneAsync(scrapedDataModel);
    }

    public async Task<List<ScrapedDataModel>> GetAllAsync(CancellationToken cancellationToken)
    {
       return await _WeatherDataCollection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<ScrapedDataModel> GetOneAsync(string id, CancellationToken cancellationToken)
    {
        return await _WeatherDataCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        await _WeatherDataCollection.DeleteOneAsync(x => x.Id == id, cancellationToken);
    }
}
