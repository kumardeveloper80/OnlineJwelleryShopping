using System;
using System.Collections.Generic;
using System.Linq;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class OrdersRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public OrdersRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public List<Order_VM> GetAll(string paymentType, string startDate, string endDate)
        {
            var orderList = (from _order in _DBContext.Orders

                                 //join _orderDetails in _DBContext.OrderDetails on _order.Id equals _orderDetails.OrderId

                             join _deliveryDetails in _DBContext.OrderDeliveryDetails on _order.Id equals _deliveryDetails.OrderId
                             orderby _order.CreatedDate descending
                             select new Order_VM
                             {
                                 OrderRef = _order.ReferenceId,
                                 ClientName = _DBContext.Clients.Where(x => x.Id == _order.UserId && x.DeletedDate == null).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault().ToString(),
                                 CheoutDate = _order.CreatedDate.Value,
                                 GrandTotal = (decimal)_DBContext.OrderDetails.Where(x => x.OrderId == _order.Id).Sum(x => (x.OrderQy) * (x.ProductPrice)),
                                 PaymentMethod = _order.PaymentMethod,
                                 PaymentStatus = _order.PaymentStatus == PaymentStatus.NOTPAID.ToString() ? "NOT PAID" : _order.PaymentStatus,
                                 BillingAddress = _deliveryDetails.PrimaryAddress,
                                 Phone = _deliveryDetails.PrimaryPhoneNo,
                                 ShippingAddress = _deliveryDetails.DeliveryAddress,
                                 AdditionalInformation = _deliveryDetails.AdditionalInformation != null ? _deliveryDetails.AdditionalInformation : "N/A",
                                 DeliveryMessage = _deliveryDetails.DeliveryMessage != null ? _deliveryDetails.DeliveryMessage : "N/A",
                                 OrderItems = (from _orderDetails in _DBContext.OrderDetails
                                               join _product in _DBContext.Products on _orderDetails.ProductId equals _product.Id
                                               where _orderDetails.OrderId == _order.Id
                                               select new OrderItems_VM
                                               {
                                                   ProductName = _orderDetails.ProductName,
                                                   ProductImg = _product.ImageName,
                                                   Price = (decimal)_orderDetails.ProductPrice,
                                                   Qty = (int)_orderDetails.OrderQy,
                                                   Total = (decimal)_orderDetails.ProductPrice * (int)_orderDetails.OrderQy
                                               }).ToList(),
                                 Email = _order.EmailId,
                                 IsGuest = _order.IsGuest,
                             }).ToList();



            if (orderList.Any())
            {
                if(paymentType == PaymentType.COD.ToString())
                {
                    orderList = orderList.Where(x => x.PaymentMethod.Contains(PaymentType.COD.ToString())).ToList();
                }

                else if (paymentType == PaymentType.ONLINE.ToString())
                {
                    orderList = orderList.Where(x => x.PaymentMethod.Contains(PaymentType.ONLINE.ToString())).ToList();
                }

                if(startDate != null && endDate != null)
                {
                    var dStartDate = DateTime.Parse(startDate);
                    var dEndDate = DateTime.Parse(endDate);
                    orderList = orderList.Where(x => x.CheoutDate.Date >= dStartDate && x.CheoutDate.Date <= dEndDate).ToList();
                }
            }

            return orderList;
        }
    }
}