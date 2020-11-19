using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zoughaibandco.ViewModel
{
    public class EGiftCard_VM
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string ToEmail { get; set; }
        public string ToFirstName { get; set; }
        public string ToLastName { get; set; }
        public DateTime DeliverDate { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public bool? IsPaid { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeliver { get; set; }
        public bool IsPublished { get; set; }
        public string ToPhoneNo { get; set; }
        public int EGiftCardId { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderEmail { get; set; }
        public string DeliverDateTime { get; set; }
        public string ReferenceId { get; set; }
    }
}