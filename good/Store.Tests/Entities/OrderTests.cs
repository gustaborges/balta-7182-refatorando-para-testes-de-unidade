using System;
using System.Reflection;
using NUnit.Framework;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Tests.Entities
{
    public class OrderTests
    {
        private readonly Customer _customer = new Customer(name: "John Bayne", email: "john.bayne@email.com");
        private readonly Product _product = new Product("Produto 1", 10.00m, true);
        private readonly DiscountVoucher _cupomDescontoValido = new DiscountVoucher(5, DateTime.Now.AddDays(1));

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Dado_um_novo_pedido_valido_ele_deve_gerar_um_numero_com_8_caracteres()
        {
            Order order = new Order(_customer, 0, null);
            Assert.AreEqual(8, order.Number.Length);
        }

        [Test]
        public void Dado_um_novo_pedido_seu_status_deve_ser_aguardando_pagamento()
        {
            Order order = new Order(_customer, 0, null);
            Assert.AreEqual(EOrderStatus.WaitingPayment, order.Status);
        }

        [Test]
        public void Dado_um_pagamento_do_pedido_seu_status_deve_ser_aguardando_entrega()
        {
            Order order = new Order(_customer, 0, null);
            order.AddItem(_product, 1);
            order.Pay(10);
            Assert.AreEqual(EOrderStatus.WaitingDelivery, order.Status);
        }

        [Test]
        public void Dado_um_pedido_cancelado_seu_status_deve_ser_cancelado()
        {
            var order = new Order(_customer, 0, null);
            order.Cancel();
            Assert.AreEqual(order.Status, EOrderStatus.Canceled);
        }

        [Test]
        public void Dado_um_novo_item_sem_produto_entao_este_nao_deve_ser_adicionado()
        {
            var order = new Order(_customer, 0, null);
            order.AddItem(null, 1);
            Assert.AreEqual(0, order.Items.Count);
        }
                
        [Test]
        public void Dado_um_novo_item_com_quantidade_positiva_entao_deve_ser_adicionado()
        {
            var order = new Order(_customer, 0, null);
            order.AddItem(_product, 2);
            Assert.AreEqual(1, order.Items.Count);
        }

        [Test]
        public void Dado_um_novo_item_com_quantidade_zero_entao_este_nao_deve_ser_adicionado()
        {
            var order = new Order(_customer, 0, null);
            order.AddItem(_product, 0);
            Assert.AreEqual(0, order.Items.Count);
        }


        [Test]
        public void Dado_um_novo_item_com_quantidade_negativa_entao_este_nao_deve_ser_adicionado()
        {
            var order = new Order(_customer, 0, null);
            order.AddItem(_product, -1);
            Assert.AreEqual(0, order.Items.Count);
        }

        [Test]
        public void Dado_um_novo_pedido_valido_seu_valor_total_deve_ser_40()
        {
            var order = new Order(_customer, 0, null);
            order.AddItem(_product, 4);
            Assert.AreEqual(40, order.Total());
        }
        
        [Test]
        public void Dado_um_cupom_de_desconto_expirado_entao_ele_nao_deve_ser_aplicado()
        {
            var order = new Order(_customer, 0, new DiscountVoucher(5, DateTime.Now.AddDays(-1)));
            order.AddItem(_product, 1);
            Assert.AreEqual(10, order.Total());
        }

        [Test]
        public void Dado_um_cupom_de_desconto_valido_entao_ele_deve_ser_aplicado()
        {
            var order = new Order(_customer, 0, _cupomDescontoValido);
            order.AddItem(_product, 1);
            Assert.AreEqual(5, order.Total());
        }

        
        [Test]
        public void Dado_ausencia_do_cupom_de_desconto_entao_total_deve_ser_calculado_normalmente()
        {
            var order = new Order(_customer, 0, null);
            order.AddItem(_product, 1);
            Assert.AreEqual(10, order.Total());
        }
        
        [Test]
        public void Dado_frete_negativo_entao_o_pedido_deve_ser_invalido()
        {
            var order = new Order(_customer, -1, _cupomDescontoValido);
            order.AddItem(_product, 1);
            order.Validate();
            Assert.IsFalse(order.IsValid);
        }

        [Test]
        public void Dado_pedido_valido_e_frete_zero_entao_o_pedido_deve_ser_valido()
        {
            var order = new Order(_customer, 0, _cupomDescontoValido);
            order.AddItem(_product, 1);
            order.Validate();
            Assert.IsTrue(order.IsValid);
        }

        [Test]
        public void Dado_valor_de_frete_valido_entao_total_deve_incluir_frete()
        {
            var order = new Order(_customer, 15, null);
            order.AddItem(_product, 1);
            Assert.AreEqual(25, order.Total());
        }

        [Test]
        public void Dado_pedido_sem_cliente_entao_ele_deve_ser_invalido()
        {
            var order = new Order(null, 15, _cupomDescontoValido);
            order.AddItem(_product, 1);
            order.Validate();
            Assert.IsFalse(order.IsValid);
        }

        [Test]
        public void Dado_pedido_sem_itens_entao_ele_deve_ser_invalido()
        {
            var order = new Order(_customer, 15, _cupomDescontoValido);
            order.Validate();
            Assert.IsFalse(order.IsValid);
        }
    }
}