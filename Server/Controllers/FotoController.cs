using BlazorFotoWASMDotnet7.Shared.Models;
using BlazorFotoWASMDotnet7.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorFotoWASMDotnet7.Server.Controllers
{
    [Route("api/foto/[action]")]
    [ApiController]
    public class FotoController : ControllerBase
    {
        private readonly IRepository _repo;

        public FotoController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<FotoResponse>> GetFotos(string? search="", int page = 1)
        {
            try
            {
                FotoResponse? result = await _repo.GetFotos(search, page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Errore: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Foto>> GetFoto(int id)
        {
            try
            {
                Foto? foto = await _repo.GetFoto(id);
                return Ok(foto);

            }
            catch (Exception ex)
            {
                return BadRequest($"Errore: {ex.Message}");

            }
        }

        [HttpPost]
        public async Task<FotoCreateResponse> CreateFoto([FromBody] Foto foto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.CreateFoto(foto);
                    FotoCreateResponse response = new FotoCreateResponse()
                    {
                        Foto = foto,
                        Message = $"Foto {foto.Name} creata con successo"
                    };
                    return response;
                }
                else
                {
                    return new FotoCreateResponse()
                    {
                        Message = "Dati non validi"
                    };
                }
            }
            catch (Exception ex)
            {
                return new FotoCreateResponse { Message = $"Errore durante la creazione della foto. Dettagli: {ex.Message}" };

            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> DeleteFoto(int id)
        {
            bool success = _repo.DeleteFoto(id);

            if (success)
            {
                return Ok($"Foto con id {id} cancellata");
            }
            else
            {
                return NotFound($"Foto con id {id} non trovata");
            }
        }


    }
}
