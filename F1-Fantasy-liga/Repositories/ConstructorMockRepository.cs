using F1_Fantasy_liga.Models;

namespace F1_Fantasy_liga.Repositories
{
    public class ConstructorMockRepository
    {
        private readonly MockDataStore _mockDataStore;

        public ConstructorMockRepository(MockDataStore mockDataStore)
        {
            _mockDataStore = mockDataStore;
        }

        public List<Constructor> GetAll()
        {
            return _mockDataStore.Constructors;
        }

        public Constructor? GetById(int id)
        {
            return _mockDataStore.Constructors.FirstOrDefault(c => c.Id == id);
        }
    }
}