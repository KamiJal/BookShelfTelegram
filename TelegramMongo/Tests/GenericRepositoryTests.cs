using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelegramMongo.Models;
using TelegramMongo.Repository;

namespace TelegramMongo.Tests
{
    public class GenericRepositoryTests
    {
        [TestMethod]
        public async Task UserRepository_ShouldHandleCrudOperations()
        {
            var userRepository = new GenericRepository<User>();
            var user = new User
            {
                FirstName = "testName",
                LastName = "testLastName",
                BooksGot = 0,
                ChatId = 1111111111,
                BooksGotDate = DateTime.Now
            };

            await userRepository.AddAsync(user);

            var userFromDb = (await userRepository.ReadAsync(q => q.ChatId == user.ChatId)).Single();
            Assert.AreEqual(userFromDb.ChatId, user.ChatId);

            var count = await userRepository.RemoveAsync(q => q.ChatId == user.ChatId);
            Assert.IsTrue(count == 1);
        }

        [TestMethod]
        public async Task UserRepository_ShouldReadAll()
        {
            var userRepository = new GenericRepository<User>();
            var all = (await userRepository.ReadAllAsync()).ToList();

            Assert.IsTrue(all.Any());
        }
    }
}