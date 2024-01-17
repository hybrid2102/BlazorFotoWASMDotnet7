using BlazorFotoWASMDotnet7.Shared.DB;
using BlazorFotoWASMDotnet7.Shared.Models;

namespace BlazorFotoWASMDotnet7.Shared.Services
{
    public class FotoRepository : IRepository
    {
        private FotoContext _context;

        public FotoRepository(FotoContext context)
        {
            _context = context;
        }

        public async Task<FotoResponse> GetFotos(string? search = "", int page = 1)
        {
            try
            {
                FotoResponse fotoResponse = new FotoResponse();
                IQueryable<Foto> query = _context.Foto.AsQueryable();
                int pageSize = 10;
                int totalItems;

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(f => f.Name.Contains(search));
                    totalItems = query.Count();
                }


                List<Foto>? fotos = query.OrderBy(f => f.Id)
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                totalItems = fotos.Count();
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                fotoResponse.Fotos = fotos;
                fotoResponse.CurrentPage = page;
                fotoResponse.TotalResult = totalItems;
                fotoResponse.TotalPages = totalPages;

                return fotoResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero delle foto.", ex);
            }

        }
        public async Task<Foto> GetFoto(int id)
        {
            return await _context.Foto.FindAsync(id);
        }

        public bool CreateFoto(Foto foto)
        {
            _context.Foto.Add(foto);
            _context.SaveChanges();
            return true;
        }
        public bool UpdateFoto(Foto foto, int id)
        {
            Foto? fotoEdit = _context.Foto.Find(id);

            fotoEdit.Name = foto.Name;
            fotoEdit.Description = foto.Description;
            fotoEdit.IsVisible = foto.IsVisible;
            fotoEdit.ImageFile = foto.ImageFile;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteFoto(int id)
        {
            Foto? fotoDelete = _context.Foto.Find(id);
            _context.Foto.Remove(fotoDelete);
            _context.SaveChanges();
            return true;
        }

    }
}
