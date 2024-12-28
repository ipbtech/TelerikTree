using System.Text.Json;
using TelerikTreeExample.ViewModels;

namespace TelerikTreeExample.Services
{
    public class HttpSessionService
    {
        private const string SESSION_KEY = "TreeList.Content";
        private readonly HttpContext _httpContext;

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

        public HttpSessionService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public IEnumerable<ItemViewModel>? GetItems()
        {
            if (!string.IsNullOrEmpty(_httpContext.Session.GetString(SESSION_KEY)))
            {
                var sessionValue = _httpContext.Session.GetString(SESSION_KEY);
                var sessionItems = JsonSerializer.Deserialize<IEnumerable<ItemViewModel>>(sessionValue);
                return sessionItems;
            }

            var items = _items.Value.Select(item =>
            {
                if (!item.ParentId.HasValue)
                {
                    var valueSum = _items.Value.Where(i => i.ParentId == item.Id).Sum(i => i.Value);
                    item.AggregateValue = valueSum + item.Value;
                }
                return item;
            }).ToList();

            var json = JsonSerializer.Serialize<IEnumerable<ItemViewModel>>(items);
            _httpContext.Session.SetString(SESSION_KEY, json);
            return items;
        }
    }
}
