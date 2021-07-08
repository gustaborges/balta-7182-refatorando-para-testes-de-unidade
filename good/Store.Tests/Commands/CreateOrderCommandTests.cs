using System;
using NUnit.Framework;
using Store.Domain.Commands;

namespace Store.Tests.Commands
{
    public class CreateOrderCommandTests
    {
        [Test]
        public void Dado_um_pedido_com_cliente_invalido_o_pedido_nao_deve_ser_gerado()
        {
            var command = new CreateOrderCommand()
            {
                Customer = "",
                ZipCode = "12345678",
                DiscountVoucher = "12345678"
            };
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Validate();

            Assert.IsFalse(command.IsValid);
        }

        [Test]
        public void Dado_um_pedido_com_cep_maior_que_8_digitos_o_pedido_deve_ser_invalido()
        {
            var command = new CreateOrderCommand()
            {
                Customer = "12345678900",
                ZipCode = "123456789",
                DiscountVoucher = "12345678"
            };
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Validate();

            Assert.IsFalse(command.IsValid);
        }

        [Test]
        public void Dado_um_pedido_com_cep_menor_que_8_digitos_o_pedido_deve_ser_invalido()
        {
            var command = new CreateOrderCommand()
            {
                Customer = "12345678900",
                ZipCode = "1234567",
                DiscountVoucher = "12345678"
            };
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Validate();

            Assert.IsFalse(command.IsValid);
        }

        
        [Test]
        public void Dado_item_do_pedido_invalido_entao_pedido_deve_ser_invalido()
        {
            var command = new CreateOrderCommand()
            {
                Customer = "12345678900",
                ZipCode = "12345678",
                DiscountVoucher = "12345678"
            };
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 0));
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Validate();

            Assert.IsFalse(command.IsValid);
        }

        [Test]
        public void Dado_um_pedido_valido_o_pedido_deve_ser_gerado()
        {
            var command = new CreateOrderCommand()
            {
                Customer = "12345678900",
                ZipCode = "12345678",
                DiscountVoucher = "12345678"
            };
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Validate();

            Assert.IsTrue(command.IsValid);
        }
    }
}