using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class EGiftCardRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public EGiftCardRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public int AddGiftCard(EGiftCard_VM eGiftCard_VM)
        {
            var eGiftCardObj = Mapper.Map<EGiftCard>(eGiftCard_VM);
            eGiftCardObj.CreatedDate = DateTime.Now;
            _DBContext.EGiftCards.Add(eGiftCardObj);
            return _DBContext.SaveChanges();
        }
        public List<EGiftCard_VM> GetEGiftCards()
        {
            var eGiftCards = (from e in _DBContext.EGiftCards
                              join c in _DBContext.Clients on e.UserId equals c.Id
                              where c.DeletedDate == null && e.DeletedDate == null
                              select new EGiftCard_VM
                              {
                                  EGiftCardId = e.Id,
                                  SenderFirstName = c.FirstName,
                                  SenderLastName = c.LastName,
                                  ReferenceId = e.Referenceid,
                                  IsPaid = e.IsPaid,
                              }).ToList();
            return eGiftCards;
        }

        public EGiftCard_VM GetEGiftCardById(int EGiftCardId)
        {
            try
            {
                var eGiftCards = (from e in _DBContext.EGiftCards
                                  join c in _DBContext.Clients on e.UserId equals c.Id
                                  where e.Id == EGiftCardId && c.DeletedDate == null && e.DeletedDate == null
                                  select new EGiftCard_VM
                                  {
                                      Id = e.Id,
                                      SenderFirstName = c.FirstName,
                                      SenderLastName = c.LastName,
                                      SenderEmail = c.Email,
                                      Amount = e.Amount.Value,
                                      ToFirstName = e.ToFirstName,
                                      ToLastName = e.ToLastName,
                                      ToEmail = e.ToEmail,
                                      DeliverDate = e.DeliverDate != null ? e.DeliverDate.Value : new DateTime(),
                                      Address = e.Address,
                                      Description = e.Description,
                                      IsDeliver = e.IsDeliver != null ? e.IsDeliver.Value : false,
                                      IsPublished = e.IsPublished != null ? e.IsPublished.Value : false,
                                      ToPhoneNo = e.ToPhoneNo,
                                      UserId = e.UserId.Value,
                                  }).FirstOrDefault();
                return eGiftCards;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public int DeleteEGiftCard(int EGiftCardId)
        {
            var eGiftCard = _DBContext.EGiftCards.Find(EGiftCardId);
            if (eGiftCard != null)
            {
                eGiftCard.DeletedDate = DateTime.Now;
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int UpdateEGiftCard(EGiftCard_VM eGiftCard_VM)
        {

            var Old_eGiftCard = _DBContext.EGiftCards.Find(eGiftCard_VM.Id);
            if (Old_eGiftCard != null)
            {
                var eGiftCard = Mapper.Map<EGiftCard>(eGiftCard_VM);
                eGiftCard.CreatedDate = Old_eGiftCard.CreatedDate;
                try
                {
                    eGiftCard.DeliverDate = Convert.ToDateTime(eGiftCard_VM.DeliverDateTime);
                }
                catch (Exception ex)
                {
                    eGiftCard.DeliverDate = null;
                }

                eGiftCard.CreatedDate = Old_eGiftCard.CreatedDate;
                eGiftCard.UpdatedDate = DateTime.Now;
                _DBContext.Entry(Old_eGiftCard).CurrentValues.SetValues(eGiftCard);
                return _DBContext.SaveChanges();
            }
            return 0;
        }
    }
}