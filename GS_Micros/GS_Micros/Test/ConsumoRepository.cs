using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;
using web_app_domain;
using web_app_performance.Controllers;
using web_app_repository;

namespace Test
{
    public class ConsumoRepositoryTests
    {
        private readonly Mock<IConsumoRepository> _consumoRepositoryMock;
        private readonly ConsumoController _controller;

        public ConsumoRepositoryTests()
        {
            _consumoRepositoryMock = new Mock<IConsumoRepository>();
            _controller = new ConsumoController(_consumoRepositoryMock.Object);
        }

        [Fact]
        public async Task Delete_ShouldReturnOkWhenConsumoIsDeleted()
        {
            var consumo = new Consumo
            {
                Id = "12345",
                ConsumoEnergetico = 1,
                Status = "Removido",
                TipoConsumo = "Eletricidade",
                DataCriacao = DateTime.Now
            };

            _consumoRepositoryMock.Setup(repo => repo.RemoverConsumo(consumo.Id)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(consumo.Id);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithConsumos()
        {
            var consumos = new List<Consumo>
            {
                new Consumo
                {
                    ConsumoEnergetico = 1,
                    Status = "Ativo",
                    TipoConsumo = "Água",
                    DataCriacao = DateTime.Now
                },
                new Consumo
                {
                    ConsumoEnergetico = 2,
                    Status = "Ativo",
                    TipoConsumo = "Gás",
                    DataCriacao = DateTime.Now
                }
            };

            _consumoRepositoryMock.Setup(repo => repo.ListarConsumos()).ReturnsAsync(consumos);

            var result = await _controller.GetConsumo();

            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.Equal(JsonSerializer.Serialize(consumos), JsonSerializer.Serialize(okResult?.Value));
        }
    }
}
