using F1_Fantasy_liga.Models;

namespace F1_Fantasy_liga.Repositories
{
    public class RaceResultMockRepository
    {
        private readonly MockDataStore _mockDataStore;

        public RaceResultMockRepository(MockDataStore mockDataStore)
        {
            _mockDataStore = mockDataStore;
        }

        public List<RaceResult> GetAll()
        {
            return _mockDataStore.RaceResults;
        }

        public RaceResult? GetById(int id)
        {
            return _mockDataStore.RaceResults.FirstOrDefault(rr => rr.Id == id);
        }
    }
}