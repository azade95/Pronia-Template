using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities.Exstensions;
using Pronia.ViewModels;
using Pronia.ViewModels.Employee;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ClientController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Client> employees = _context.Clients.Include(c => c.Profession).ToList();
            return View(employees);
        }

        public IActionResult Create()
        {
           
            ViewBag.Professions = _context.Professions.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateClientVM clientVM)
        {
            
           
            Client client=new Client
            {
                Name = clientVM.Name,
                Surname = clientVM.Surname,
                Description = clientVM.Description,
                Image = await clientVM.Photo.CreateFile(_env.WebRootPath, @"assets/images/website-images/client"),
                ProfessionId = clientVM.ProfessionId
            };
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id == null || id < 0) return BadRequest();
            Client existed= _context.Clients.FirstOrDefault(c => c.Id == id);
            if (existed == null) return NotFound();
            UpdateClientVM clientVM = new UpdateClientVM
            {
                Name = existed.Name,
                Surname = existed.Surname,
                Description = existed.Description,
                Image = existed.Image,
                ProfessionId = existed.ProfessionId
            };
            ViewBag.Professions = _context.Professions.ToList();
            return View(clientVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateClientVM clientVM)
        {
            if (id == null || id < 0) return BadRequest();
            Client existed = _context.Clients.FirstOrDefault(c => c.Id == id);
            if (existed == null) return NotFound();
            
            if (clientVM.Photo != null)
            {
                if (!clientVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Fayl tipi dogru deyil");
                    return View(clientVM);
                }
                if (!clientVM.Photo.CheckFileLength(2048))
                {

                    ModelState.AddModelError("Photo", "Fayl olcusu 2mbdan chox ola bilmez");
                    return View(clientVM);
                }
                existed.Image.DeleteFile(_env.WebRootPath, @"assets/images/website-images/client");
                existed.Image = await clientVM.Photo.CreateFile(_env.WebRootPath, @"assets/images/website-images/client");
            }
            existed.Name = clientVM.Name;
            existed.Surname = clientVM.Surname;
            existed.Description = clientVM.Description;
            existed.ProfessionId = clientVM.ProfessionId;
            _context.Clients.Update(existed);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete( int? id)
        {
            if (id == null || id < 0) return BadRequest();
            Client existed = _context.Clients.FirstOrDefault(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Clients.Remove(existed);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
