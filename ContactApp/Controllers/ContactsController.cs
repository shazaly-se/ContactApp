using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ContactApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ContactApp.Controllers
{
    public class ContactsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _containerName;
        public ContactsController(AppDbContext context, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _configuration = configuration;
            _connectionString = configuration["AzureBlob:ConnectionString"];
            _containerName = configuration["AzureBlob:ContainerName"];
        }   

        // READ
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contacts.ToListAsync());
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            return View(contact);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact, IFormFile ProfileImage)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(contact);
            }

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
              
                BlobContainerClient container = new BlobContainerClient(_connectionString, _containerName);
                await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

                string fileName = Guid.NewGuid() + Path.GetExtension(ProfileImage.FileName);

                BlobClient blob = container.GetBlobClient(fileName);

                using (var stream = ProfileImage.OpenReadStream())
                {
                    await blob.UploadAsync(stream, true);
                }

               
                contact.ProfileImage = fileName;
            }

            _context.Add(contact);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            return View(contact);
        }

        // EDIT POST
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Contact contact, IFormFile ProfileImage)
        {
            if (id != contact.Id)
                return NotFound();

            // ✅ Validate model FIRST
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(contact);
            }

            // ✅ Get existing record
            var contactFromDb = await _context.Contacts.FindAsync(id);
            if (contactFromDb == null)
                return NotFound();

            // ✅ Update basic fields
            contactFromDb.FirstName = contact.FirstName;
            contactFromDb.LastName = contact.LastName;
            contactFromDb.Email = contact.Email;
            contactFromDb.Phone = contact.Phone;

            // ✅ Handle image upload
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                BlobContainerClient container = new BlobContainerClient(_connectionString, _containerName);
                await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

                // 🔥 Delete old image (important fix)
                if (!string.IsNullOrEmpty(contactFromDb.ProfileImage))
                {
                    var oldBlobClient = container.GetBlobClient(contactFromDb.ProfileImage);
                    await oldBlobClient.DeleteIfExistsAsync();
                }

                // 🔥 Upload new image
                string fileName = Guid.NewGuid() + Path.GetExtension(ProfileImage.FileName);
                var newBlobClient = container.GetBlobClient(fileName);

                using (var stream = ProfileImage.OpenReadStream())
                {
                    await newBlobClient.UploadAsync(stream, overwrite: true);
                }

                // Save filename only
                contactFromDb.ProfileImage = fileName;
            }
            else
            {
                // 🔥 KEEP old image (important)
                contactFromDb.ProfileImage = contact.ProfileImage;
            }

            // ✅ Save changes
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            return View(contact);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (!string.IsNullOrEmpty(contact.ProfileImage))
            {
                string filePath = Path.Combine(_env.WebRootPath, "images", contact.ProfileImage);
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            }
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

