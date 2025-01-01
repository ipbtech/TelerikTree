using System.Text.Json;
using TelerikTreeExample.ViewModels;

namespace TelerikTreeExample.Services
{
    public class HttpSessionStorage
    {
        private const string SESSION_KEY = "TreeList.Content";
        private readonly ISession? _httpSession;
        private readonly ILogger<HttpSessionStorage> _logger;

        public HttpSessionStorage(IHttpContextAccessor httpContextAccessor, ILogger<HttpSessionStorage> logger)
        {
            _httpSession = httpContextAccessor.HttpContext?.Session;
            _logger = logger;
        }


        public IEnumerable<ItemViewModel> GetAll()
        {
            try
            {
                if (string.IsNullOrEmpty(_httpSession?.GetString(SESSION_KEY)))
                    IntializeSession();

                var sessionValue = _httpSession?.GetString(SESSION_KEY);
                var sessionItems = JsonSerializer.Deserialize<IEnumerable<ItemViewModel>>(sessionValue);
                return sessionItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong");
                throw;
            }
        }

        public void Create(params ItemViewModel[] createdItems)
        {
            try
            {
                var items = GetAll().OrderByDescending(i => i.Id).ToList();
                int itemsIdIncrement = items.FirstOrDefault()?.Id ?? 0;
                var newItemIds = new List<int>(createdItems.Length);

                foreach (var created in createdItems)
                {
                    itemsIdIncrement++;
                    created.Id = itemsIdIncrement;
                    items.Add(created);
                    newItemIds.Add(created.Id);
                }

                items.ForEach(i => i.AggregateValue = CalculateAggregateValue(i, items));
                var json = JsonSerializer.Serialize<IEnumerable<ItemViewModel>>(items);

                _httpSession?.SetString(SESSION_KEY, json);
                _logger.LogInformation("New items with Ids:{@Ids} were created successful and were added to the session storage",
                    string.Join("; ", newItemIds));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong");
                throw;
            }
        }

        public void Update(params ItemViewModel[] updatedItems)
        {
            try
            {
                var items = GetAll().ToList();
                foreach (var updated in updatedItems)
                {
                    var existed = items.FirstOrDefault(i => i.Id == updated.Id);
                    if (existed is null)
                        return;

                    existed.Name = updated.Name;
                    existed.Value = updated.Value;
                }

                items.ForEach(i => i.AggregateValue = CalculateAggregateValue(i, items));
                var json = JsonSerializer.Serialize<IEnumerable<ItemViewModel>>(items);

                _httpSession?.SetString(SESSION_KEY, json);
                _logger.LogInformation("Items with Ids:{@Ids} were updated successful",
                    string.Join("; ", updatedItems.Select(i => i.Id)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong");
                throw;
            }
        }


        public void Delete(params ItemViewModel[] deletedItems)
        {
            try
            {
                var items = GetAll().ToList();
                foreach(var deleted in deletedItems)
                {
                    var existed = items.FirstOrDefault(i => i.Id == deleted.Id);
                    if (existed is null)
                        return;

                    items.Remove(existed);
                }

                items.ForEach(i => i.AggregateValue = CalculateAggregateValue(i, items));
                var json = JsonSerializer.Serialize<IEnumerable<ItemViewModel>>(items);

                _httpSession?.SetString(SESSION_KEY, json);
                _logger.LogInformation("Items with Ids:{@Ids} were deleted successful",
                    string.Join("; ", deletedItems.Select(i => i.Id)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something wrong");
                throw;
            }
        }


        private void IntializeSession()
        {
            var items = _items.Value.ToList();
            foreach(var item in items)
            {
                item.AggregateValue = CalculateAggregateValue(item, items);
            }
            var json = JsonSerializer.Serialize<IEnumerable<ItemViewModel>>(items);
            _httpSession?.SetString(SESSION_KEY, json);
            _logger.LogInformation("Session was intialized with initial data");
        }

        private double CalculateAggregateValue(ItemViewModel currentItem, IEnumerable<ItemViewModel> allItems)
        {
            var childValues = allItems.Where(i => i.ParentId == currentItem.Id)
                .Select(i => CalculateAggregateValue(i, allItems)).Sum();
            return currentItem.Value.HasValue ? currentItem.Value.Value + childValues : childValues;
        }


        private Lazy<IEnumerable<ItemViewModel>> _items = new Lazy<IEnumerable<ItemViewModel>>(() => new List<ItemViewModel>
        {
            new ItemViewModel { Id = 1, Name = "Initial Root Item#1", Value = 1.11 },
            new ItemViewModel { Id = 2, Name = "Initial Root Item#2", Value = 1.34 },
            new ItemViewModel { Id = 3, Name = "Initial Branch Item#1.1", Value = 3.14, ParentId = 1 },
            new ItemViewModel { Id = 4, Name = "Initial Branch Item#1.2", Value = 5.18, ParentId = 1 },
            new ItemViewModel { Id = 5, Name = "Initial Branch Item#1.3", Value = 7.79, ParentId = 1 },
            new ItemViewModel { Id = 6, Name = "Initial Branch Item#2.1", Value = 1.11, ParentId = 2 },
            new ItemViewModel { Id = 7, Name = "Initial Branch Item#2.2", ParentId = 2 },
        });
    }
}
