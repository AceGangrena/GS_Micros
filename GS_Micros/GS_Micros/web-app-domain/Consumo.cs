using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace web_app_domain
{
    public class Consumo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; } = string.Empty;

        public string Status { get; init; } = string.Empty;

        public double ConsumoEnergetico { get; init; }

        public string TipoConsumo { get; init; } = string.Empty;

        public DateTime DataCriacao { get; init; } = DateTime.UtcNow;
    }
}
