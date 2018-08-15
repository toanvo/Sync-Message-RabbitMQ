using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DineConnect.DomainObject
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public bool CalculatePrice { get; set; }
        public string CreatingUserName { get; set; }
        public bool DecreaseInventory { get; set; }
        public string DepartmentName { get; set; }
        public bool IncreaseInventory { get; set; }
        public bool IsPromotionOrder { get; set; }
        public bool Locked { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public int MenuItemPortionId { get; set; }
        public object Note { get; set; }
        public DateTime OrderCreatedTime { get; set; }
        
        public string OrderNumber { get; set; }
        public string OrderStates { get; set; }
        public string OrderTags { get; set; }
        public int PortionCount { get; set; }
        public string PortionName { get; set; }
        public double Price { get; set; }
        public string PriceTag { get; set; }
        public double PromotionAmount { get; set; }
        public int PromotionSyncId { get; set; }
        public double Quantity { get; set; }

        public string Taxes { get; set; }

        public int TicketId { get; set; }
    }

    public class Payment
    {   
        [Key]
        public int Id { get; set; }
        public int PaymentTypeId { get; set; }

        public double Amount { get; set; }
        public DateTime PaymentCreatedTime { get; set; }
        public string PaymentTags { get; set; }
        
        public string PaymentUserName { get; set; }
        public double TenderedAmount { get; set; }
        public string TerminalName { get; set; }
    }

    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public int TransactionTypeId { get; set; }
        public int AccountId { get; set; }
        public double Amount { get; set; }        
    }

    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public int WorkPeriodId { get; set; }
        public string DepartmentName { get; set; }
        
        public bool IsClosed { get; set; }
        public bool IsLocked { get; set; }
        public string LastModifiedUserName { get; set; }
        public DateTime LastOrderTime { get; set; }
        public DateTime LastPaymentTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int LocationId { get; set; }
        public object Note { get; set; }
        public List<Order> Orders { get; set; }
        public List<Payment> Payments { get; set; }
        public bool PreOrder { get; set; }
        public double RemainingAmount { get; set; }
        public bool TaxIncluded { get; set; }
        public int TenantId { get; set; }
        public string TerminalName { get; set; }
        public DateTime TicketCreatedTime { get; set; }
        public string TicketEntities { get; set; }
        public int TicketId { get; set; }
        public object TicketLogs { get; set; }
        public string TicketNumber { get; set; }
        public string TicketStates { get; set; }
        public string TicketTags { get; set; }
        public object TicketTypeName { get; set; }
        public double TotalAmount { get; set; }
        public List<Transaction> Transactions { get; set; }
    }

    public class RootObject
    {
        public Ticket Ticket { get; set; }
    }
}
