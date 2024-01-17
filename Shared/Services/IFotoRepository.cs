using BlazorFotoWASMDotnet7.Shared.Models;

namespace BlazorFotoWASMDotnet7.Shared.Services
{
    public interface IRepository
    {
        public Task<FotoResponse> GetFotos(string? search, int page = 1);
        public Task<Foto> GetFoto(int id);
        public bool CreateFoto(Foto foto);
        public bool UpdateFoto(Foto foto, int id);
        public bool DeleteFoto(int id);
    }
}