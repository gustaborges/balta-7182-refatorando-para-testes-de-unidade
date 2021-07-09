using System.Collections.Generic;
using System.Linq;
using Flunt.Notifications;
using Store.Domain.Commands;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Entities;
using Store.Domain.Handlers.Interfaces;
using Store.Domain.Repositories;
using Store.Domain.Utils;

namespace Store.Domain.Handlers
{
    //Pode-se tratar varios commands relacionado ao pedido em um só handler,
    // ou segregar um command por classe
    public class OrderHandler : Notifiable<Notification>, IHandler<CreateOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryFeeRepository _deliveryFeeRepository;
        private readonly IDiscountVoucherRepository _discountVoucherRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderHandler(
            ICustomerRepository customerRepository,
            IDeliveryFeeRepository deliveryFeeRepository,
            IDiscountVoucherRepository discountVoucherRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _deliveryFeeRepository = deliveryFeeRepository;
            _discountVoucherRepository = discountVoucherRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public ICommandResult Handle(CreateOrderCommand command)
        {
            command.Validate();
            if(!command.IsValid)    
                return new GenericCommandResult(false, "Pedido inválido", command.Notifications);

            // 1. Recupera o cliente
            var customer = _customerRepository.Get(command.Customer);
            
            // 2. Calcula a taxa de entrega
            var deliveryFee = _deliveryFeeRepository.Get(command.ZipCode);

            // 3. Obtém o cupom de desconto
            var discount = _discountVoucherRepository.Get(command.DiscountVoucher);

            // 4. Gera o pedido
            var products = _productRepository.Get(ExtractGuids.Extract(command.Items)).ToList();
            
            var order = new Order(customer, deliveryFee, discount);
            foreach (var item in command.Items)
            {
                var product = products.Where(x => x.Id == item.Product).FirstOrDefault();
                order.AddItem(product, item.Quantity);
            }
            
            // 5. Agrupa as notificações para facilitar pra API
            order.Validate();
            AddNotifications(order);

            // 6. Verifica se deu tudo certo
            if(!IsValid)
                return new GenericCommandResult(false, "Falha ao gerar pedido", Notifications);

            // 7. Salva no banco
            // Se der problema no banco, pode ser lançada uma exception mais especifica para a API, Console,
            // quem estiver usando conseguir identificar
            // Problema de dar erro pode ser tratado com padrão Unit of Work
            _orderRepository.Save(order);

            return new GenericCommandResult(true, "Pedido gerado com sucesso", order);
        }
    }
}