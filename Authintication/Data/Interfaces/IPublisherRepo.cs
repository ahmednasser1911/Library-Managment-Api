using Authintication.DTOs;
using Authintication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Data.Interfaces
{
    public interface IPublisherRepo
    {
        public List<PublisherBooksDto> GetPublishers();
        public PublisherBooksDto GetPublisherById(int Id);
        public Publisher GetPublisherByName(string name);
        public Publisher AddPublisher(Publisher publisher);
        public Publisher DeletePublisher(int id);

    }
}
