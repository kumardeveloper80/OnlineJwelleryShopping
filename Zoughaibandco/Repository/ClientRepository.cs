using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class ClientRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public ClientRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public List<Client_VM> GetAllClient(string search)
        {
            var clients = (from c in _DBContext.Clients where c.DeletedDate == null
                           select new Client_VM
                           {
                               Id = c.Id,
                               FirstName = c.FirstName,
                               LastName = c.LastName,
                           }).ToList();


            if (search != null && clients.Count > 0)
            {
                clients = clients.Where(x => x.FirstName.ToLower().Contains(search.ToLower()) || x.LastName.ToLower().Contains(search)).ToList();
            }

            return clients;
        }

        public Client_VM GetClientById(int ClientId)
        {
            var client = (from c in _DBContext.Clients
                           where c.Id == ClientId && c.DeletedDate == null
                           select new Client_VM
                           {
                               Id = c.Id,
                               FirstName = c.FirstName,
                               LastName = c.LastName,
                               UserName = c.Email,
                               Email = c.Email,
                               AllowToUsePlateform = c.AllowToUsePlateform == null ? false : (bool)c.AllowToUsePlateform
                           }).FirstOrDefault();
            return client;
        }

        public void UpdateClient(Client_VM client_VM)
        {
            var client = _DBContext.Clients.Find(client_VM.Id);
            if(client != null)
            {
                client.AllowToUsePlateform = client_VM.AllowToUsePlateform;
                client.UpdatedDate = DateTime.Now;
                _DBContext.SaveChanges();
            }
        }

        public int DeleteClient(int ClientId)
        {
            var client = _DBContext.Clients.Find(ClientId);
            if (client != null)
            {
                client.DeletedDate = DateTime.Now;
                return _DBContext.SaveChanges();
            }
            return 0;
        }
    }
}