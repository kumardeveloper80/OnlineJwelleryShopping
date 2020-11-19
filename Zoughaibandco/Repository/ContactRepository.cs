using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class ContactRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public ContactRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public int Add(Contact_VM contact_VM)
        {
            try
            {
                var contactObj = Mapper.Map<Contact>(contact_VM);
                contactObj.CreatedDate = DateTime.Now;
                _DBContext.Contacts.Add(contactObj);
                return _DBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<Contact_VM> GetAllHelpMessage()
        {
            var helpMessages = _DBContext.Contacts.Where(x=>x.DeletedDate == null).ToList();
            return Mapper.Map<List<Contact>, List<Contact_VM>>(helpMessages);
        }

        public int DeleteHelpMessage(int HelpId)
        {
            var helpMessage = _DBContext.Contacts.Find(HelpId);
            if (helpMessage != null)
            {
                helpMessage.DeletedDate = DateTime.Now;
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public Contact_VM GetHelpMessage(int HelpId)
        {
            var helpMessage = _DBContext.Contacts.Find(HelpId);
            if (helpMessage != null)
            {
                return Mapper.Map<Contact_VM>(helpMessage);
            }
            return null;
        }

        public int UpdateHelpMessage(Contact_VM contact_VM)
        {
            var oldHelpMessage = _DBContext.Contacts.Find(contact_VM.Id);
            Contact contact = new Contact();
            if (oldHelpMessage != null)
            {
                contact = Mapper.Map<Contact>(contact_VM);
                contact.IsRead = true;
                contact.CreatedDate = oldHelpMessage.CreatedDate;
                contact.UpdatedDate = DateTime.Now;
                _DBContext.Contacts.AddOrUpdate(contact);
                return _DBContext.SaveChanges();
            }
            return 0;
        }
    }
}
