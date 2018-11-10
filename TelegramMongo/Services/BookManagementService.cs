using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Telegram.Bot.Types.InputFiles;
using TelegramMongo.Models;
using TelegramMongo.Repository;

namespace TelegramMongo.Services
{
    public class BookManagementService
    {
        private readonly GenericRepository<Book> _bookStorage;
        private readonly Random _rnd;

        public BookManagementService(Random random)
        {
            _bookStorage = new GenericRepository<Book>();
            _rnd = random;
        }

        public async Task<InputOnlineFile> GetBook()
        {
            var books = (await _bookStorage.ReadAllAsync()).ToList();
            var randomBook = books.ElementAt(_rnd.Next(0, books.Count()));

            var file = await ProcessAsync(randomBook.Name);

            return new InputOnlineFile(new MemoryStream(file), randomBook.Name);
        }

        private async Task<byte[]> ProcessAsync(string blobName)
        {
            if (!CloudStorageAccount.TryParse(PrivateTokens.BlobStorageConnectionString, out var storageAccount))
                return null;

            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("books");

            var blob = cloudBlobContainer.GetBlockBlobReference(blobName);

            var fileStream = new MemoryStream();
            await blob.DownloadToStreamAsync(fileStream);

            return fileStream.ToArray();
        }
    }
}