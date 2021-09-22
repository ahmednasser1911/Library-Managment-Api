using Authintication.DTOs;
using Authintication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Data.Interfaces
{
    public class PublisherRepo : IPublisherRepo
    {
        private readonly AppDbContext publisherContext;

        public PublisherRepo(AppDbContext publisherContext)
        {
            this.publisherContext = publisherContext;
        }

        public Publisher AddPublisher(Publisher publisher)
        {
            publisherContext.Publishers.Add(publisher);
            publisherContext.SaveChanges();
            return publisher;
        }

        public Publisher DeletePublisher(int id)
        {
            var publisher = publisherContext.Publishers.Find(id);

            if (publisher != null)
            {
                publisherContext.Publishers.Remove(publisher);
                publisherContext.SaveChanges();
                return publisher;
            }
            return null;
        }

        public PublisherBooksDto GetPublisherById(int Id)
        {
            var publisherData = publisherContext.Publishers.Where(p => p.Id == Id)
                .Select(pub => new PublisherBooksDto()
                {
                Name = pub.Name,
                BooksAuthors = pub.Books.Select(book => new BooksAuthorsDto()
                {
                    BookName = book.Title,
                    BookAuthors = book.Book_Authors.Select(bookauthor => bookauthor.Author.FullName).ToList()

                }).ToList()
            }).FirstOrDefault();
            return publisherData;
        }

        public Publisher GetPublisherByName(string name)
        {
            return publisherContext.Publishers.SingleOrDefault(a => a.Name == name);
        }

        public List<PublisherBooksDto> GetPublishers()
        {
            var publishers = new List<PublisherBooksDto>();

            foreach (var item in publisherContext.Publishers.ToList())
            {
                publishers.Add(GetPublisherById(item.Id));
            }
            return publishers;
        }
    }
}
