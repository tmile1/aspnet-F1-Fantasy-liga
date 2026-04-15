using F1_Fantasy_liga.Models;

namespace F1_Fantasy_liga.Repositories
{
    public class DriverMockRepository
    {
        private readonly MockDataStore _mockDataStore;

        public DriverMockRepository(MockDataStore mockDataStore)
        {
            _mockDataStore = mockDataStore;
        }

        public List<Driver> GetAll()
        {
            return _mockDataStore.Drivers;
        }

        public Driver? GetById(int id)
        {
            return _mockDataStore.Drivers.FirstOrDefault(d => d.Id == id);
        }
    }
}