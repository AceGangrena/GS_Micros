using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;
using web_app_domain;
using web_app_performance.Controllers;
using web_app_repository;

namespace Test
{
    public class ConsumoControllerTests
    {
        private readonly Mock<IConsumoRepository> _consumoRepositoryMock;
        private readonly ConsumoController _controller;

        public ConsumoControllerTests()
        {
            _consumoRepositoryMock = new Mock<IConsumoRepository>();
            _controller = new ConsumoController(_consumoRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithConsumos()
        {
            var consumos = new List<Consumo> { new Consumo() };

            _consumoRepositoryMock
                .Setup(repo => repo.ListarConsumos())
                .ReturnsAsync(consumos);

            var result = await _controller.GetConsumo();

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(JsonSerializer.Serialize(consumos), JsonSerializer.Serialize(okResult?.Value));
        }

        [Fact]
        public async Task Get_ShouldReturnNotFoundWhenNoConsumos()
        {
            _consumoRepositoryMock
                .Setup(repo => repo.ListarConsumos())
                .ReturnsAsync(new List<Consumo>());

            var result = await _controller.GetConsumo();

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithSpecificConsumos()
        {
            var consumos = new List<Consumo>
            {
                new Consumo
                {
                    ConsumoEnergetico = 1,
                    Status = "Ativo",
                    TipoConsumo = "Eletricidade",
                    DataCriacao = DateTime.Now
                }
            };

            _consumoRepositoryMock
                .Setup(repo => repo.ListarConsumos())
                .ReturnsAsync(consumos);

            var result = await _controller.GetConsumo();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Post_ShouldSaveConsumoAndReturnOk()
        {
            var consumo = new Consumo
            {
                ConsumoEnergetico = 1,
                Status = "Novo",
                TipoConsumo = "Gás",
                DataCriacao = DateTime.Now
            };

            _consumoRepositoryMock
                .Setup(repo => repo.SalvarConsumo(It.IsAny<Consumo>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Post(consumo);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Put_ShouldUpdateConsumoAndReturnOk()
        {
            var consumo = new Consumo
            {
                ConsumoEnergetico = 10,
                Status = "Atualizado",
                TipoConsumo = "Água",
                DataCriacao = DateTime.Now
            };

            _consumoRepositoryMock
                .Setup(repo => repo.AtualizarConsumo(consumo))
                .Returns(Task.CompletedTask);

            var result = await _controller.Put(consumo);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldDeleteConsumoAndReturnOk()
        {
            var id = "teste-id";

            _consumoRepositoryMock
                .Setup(repo => repo.RemoverConsumo(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(id);

            Assert.IsType<OkResult>(result);
        }
    }
}
