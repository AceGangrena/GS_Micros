using MongoDB.Driver;
using web_app_domain;

namespace web_app_repository
{
    public class ConsumoRepository : IConsumoRepository
    {
        private readonly IMongoCollection<Consumo> _consumosCollection;

        public ConsumoRepository()
        {
            // Configuração do cliente MongoDB
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("consumoDB");
            _consumosCollection = database.GetCollection<Consumo>("consumos");
        }

        public async Task<IEnumerable<Consumo>> ListarConsumos()
        {
            // Busca todos os consumos
            return await _consumosCollection.Find(_ => true).ToListAsync();
        }

        public async Task SalvarConsumo(Consumo consumo)
        {
            // Insere um novo consumo
            await _consumosCollection.InsertOneAsync(consumo);
        }

        public async Task AtualizarConsumo(Consumo consumo)
        {
            // Atualiza um consumo pelo ID
            await _consumosCollection.ReplaceOneAsync(x => x.Id == consumo.Id, consumo);
        }

        public async Task RemoverConsumo(string id)
        {
            // Remove um consumo pelo ID
            await _consumosCollection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
