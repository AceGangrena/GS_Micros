using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
using web_app_domain;
using web_app_repository;

namespace web_app_performance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumoController : ControllerBase
    {
        private readonly IConsumoRepository _repository;
        private readonly IDatabase _redisDatabase;
        private const string CacheKey = "getconsumo";

        public ConsumoController(IConsumoRepository repository, ConnectionMultiplexer redis)
        {
            _repository = repository;
            _redisDatabase = redis.GetDatabase();
        }

        [HttpGet]
        public async Task<IActionResult> GetConsumo()
        {
            // Tenta obter os dados do cache Redis
            var cachedConsumos = await _redisDatabase.StringGetAsync(CacheKey);
            if (!string.IsNullOrEmpty(cachedConsumos))
            {
                return Ok(cachedConsumos);
            }

            // Caso não esteja em cache, consulta o repositório
            var consumos = await _repository.ListarConsumos();
            var consumosJson = JsonSerializer.Serialize(consumos);

            // Armazena o resultado no Redis com tempo de expiração
            await _redisDatabase.StringSetAsync(CacheKey, consumosJson, TimeSpan.FromSeconds(10));

            return Ok(consumos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Consumo consumo)
        {
            await _repository.SalvarConsumo(consumo);

            // Invalida o cache
            await _redisDatabase.KeyDeleteAsync(CacheKey);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Consumo consumo)
        {
            await _repository.AtualizarConsumo(consumo);

            // Invalida o cache
            await _redisDatabase.KeyDeleteAsync(CacheKey);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _repository.RemoverConsumo(id);

            // Invalida o cache
            await _redisDatabase.KeyDeleteAsync(CacheKey);

            return Ok();
        }
    }
}
