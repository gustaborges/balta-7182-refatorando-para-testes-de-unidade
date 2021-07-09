using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Store.Domain.Commands;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Handlers;
using Store.Domain.Repositories;
using Store.Tests.Repositories;

namespace Store.Tests.Handlers
{
    public class OrderHandlerTests
    {
        ICustomerRepository customerRepository = new FakeCustomerRepository();
        IDeliveryFeeRepository deliveryFeeRepository = new FakeDeliveryFeeRepository();
        IDiscountVoucherRepository discountVoucherRepository = new FakeDiscountVoucherRepository();
        IProductRepository productRepository = new FakeProductRepository();
        IOrderRepository orderRepository = new FakeOrderRepository();
        OrderHandler handler;

        [SetUp]
        public void SetUp()
        {
            handler = new OrderHandler
            (
                customerRepository,
                deliveryFeeRepository,
                discountVoucherRepository,
                productRepository,
                orderRepository
            );
        }
        
        [Test]
        [Category("Handlers")]
        public void Dado_um_cliente_inexistente_o_pedido_nao_deve_ser_gerado()
        {
            CreateOrderCommand command = new CreateOrderCommand
            (
                customer: FakeCustomerRepository.InexistentDocument,
                zipCode: "12345678",
                discountVoucher: null,
                items: new List<CreateOrderItemCommand>() {
                    new CreateOrderItemCommand(FakeProductRepository.GetGuids()[0], 1)
                }
            );

            ICommandResult commandResult = handler.Handle(command);
            Assert.False(commandResult.Success);
        }

        [Test]
        [Category("Handlers")]
        public void Dado_um_cep_invalido_o_pedido_nao_deve_ser_gerado()
        {

            CreateOrderCommand command = new CreateOrderCommand
            (
                customer: FakeCustomerRepository.ValidDocument,
                zipCode: "1234",
                discountVoucher: null,
                items: new List<CreateOrderItemCommand>() {
                    new CreateOrderItemCommand(FakeProductRepository.GetGuids()[0], 1)
                }
            );

            ICommandResult commandResult = handler.Handle(command);
            Assert.False(commandResult.Success);
        }

        [Test]
        [Category("Handlers")]
        public void Dado_um_promocode_inexistente_o_pedido_deve_ser_gerado_normalmente()
        {
            CreateOrderCommand command = new CreateOrderCommand
            (
                customer: FakeCustomerRepository.ValidDocument,
                zipCode: "12345678",
                discountVoucher: "inexistente",
                items: new List<CreateOrderItemCommand>() {
                    new CreateOrderItemCommand(FakeProductRepository.GetGuids()[0], 1)
                }
            );

            ICommandResult commandResult = handler.Handle(command);
            Assert.True(commandResult.Success);
        }

        [Test]
        [Category("Handlers")]
        public void Dado_um_pedido_sem_itens_o_mesmo_nao_deve_ser_gerado()
        {
            CreateOrderCommand command = new CreateOrderCommand
            (
                customer: FakeCustomerRepository.ValidDocument,
                zipCode: "12345678",
                discountVoucher: null,
                items: new List<CreateOrderItemCommand>()
            );

            ICommandResult commandResult = handler.Handle(command);
            Assert.False(commandResult.Success);
        }

        [Test]
        [Category("Handlers")]
        public void Dado_um_comando_invalido_o_pedido_nao_deve_ser_gerado()
        {
            CreateOrderCommand command = new CreateOrderCommand
            (
                customer: "",
                zipCode: "12345678",
                discountVoucher: null,
                items: new List<CreateOrderItemCommand>() {
                    new CreateOrderItemCommand(FakeProductRepository.GetGuids()[0], 1),
                    new CreateOrderItemCommand(FakeProductRepository.GetGuids()[1], 2),
                    new CreateOrderItemCommand(FakeProductRepository.GetGuids()[2], 1)
                }
            );

            ICommandResult commandResult = handler.Handle(command);
            Assert.False(commandResult.Success);
        }

        [Test]
        [Category("Handlers")]

        public void Dado_um_comando_valido_o_pedido_deve_ser_gerado()
        {
            CreateOrderCommand command = new CreateOrderCommand
            (
                customer: FakeCustomerRepository.ValidDocument,
                zipCode: "12345678",
                discountVoucher: null,
                items: new List<CreateOrderItemCommand>() {
                    new CreateOrderItemCommand(FakeProductRepository.GetGuids()[0], 1),
                    new CreateOrderItemCommand(FakeProductRepository.GetGuids()[1], 2),
                    new CreateOrderItemCommand(FakeProductRepository.GetGuids()[2], 1)
                }
            );

            ICommandResult commandResult = handler.Handle(command);
            Assert.True(commandResult.Success);
        }
    }
}