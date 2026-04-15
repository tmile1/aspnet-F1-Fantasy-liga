using F1_Fantasy_liga.Models;

namespace F1_Fantasy_liga.Repositories
{
    public class FantasyLeagueMockRepository
    {
        private readonly MockDataStore _mockDataStore;

        public FantasyLeagueMockRepository(MockDataStore mockDataStore)
        {
            _mockDataStore = mockDataStore;
        }

        public List<FantasyLeague> GetAll()
        {
            return _mockDataStore.FantasyLeagues;
        }

        public FantasyLeague? GetById(int id)
        {
            return _mockDataStore.FantasyLeagues.FirstOrDefault(l => l.Id == id);
        }
    }
}