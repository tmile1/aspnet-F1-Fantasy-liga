using F1_Fantasy_liga.Models;

namespace F1_Fantasy_liga.Repositories
{
    public class CircuitMockRepository
    {
        private readonly MockDataStore _mockDataStore;

        public CircuitMockRepository(MockDataStore mockDataStore)
        {
            _mockDataStore = mockDataStore;
        }

        public List<Circuit> GetAll()
        {
            return _mockDataStore.Circuits;
        }

        public Circuit? GetById(int id)
        {
            return _mockDataStore.Circuits.FirstOrDefault(c => c.Id == id);
        }
    }
}