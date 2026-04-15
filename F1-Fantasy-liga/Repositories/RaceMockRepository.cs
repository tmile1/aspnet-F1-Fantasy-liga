using F1_Fantasy_liga.Models;

namespace F1_Fantasy_liga.Repositories
{
    public class RaceMockRepository
    {
        private readonly MockDataStore _mockDataStore;

        public RaceMockRepository(MockDataStore mockDataStore)
        {
            _mockDataStore = mockDataStore;
        }

        public List<Race> GetAll()
        {
            return _mockDataStore.Races;
        }

        public Race? GetById(int id)
        {
            return _mockDataStore.Races.FirstOrDefault(r => r.Id == id);
        }
    }
}