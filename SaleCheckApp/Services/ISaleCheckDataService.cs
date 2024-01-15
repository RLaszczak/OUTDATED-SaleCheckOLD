using SaleCheckApp.Models;

namespace SaleCheckApp.Services;

public interface ISaleCheckDataService
{
    public Task<List<ScrapedDataModel>> GetAllAsync(CancellationToken cancellationToken);
    public Task<ScrapedDataModel> GetOneAsync(string id, CancellationToken cancellationToken);
    public Task CreateAsync(ScrapedDataModel scrapedDataModel);
    public Task RemoveAsync(string id, CancellationToken cancellationToken);
}
