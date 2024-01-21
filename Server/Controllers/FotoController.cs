﻿using BlazorFotoWASMDotnet7.Shared.Models;
using BlazorFotoWASMDotnet7.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
        public async Task<ActionResult<FotoResponse>> GetFotos(string? search="", int page = 1, bool publicview = false)
        {
            try
            {
                FotoResponse result = await _repo.GetFotos(search, page, publicview);
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
        public async Task<FotoCreateResponse> CreateFoto([FromForm] FotoForm fotoForm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        byte[] fileBytes;
                        if (fotoForm.Image != null)
                        {
                            await fotoForm.Image.CopyToAsync(stream);
                            fileBytes = stream.ToArray();
                        }
                        else
                        {
                            fileBytes = null;
                        }

                        Foto newFoto = new Foto
                        {
                            Name = fotoForm.Foto?.Name,
                            Description = fotoForm.Foto?.Description,
                            IsVisible = fotoForm.Foto?.IsVisible ?? false,
                            ImageFile = fileBytes
                        };

                        _repo.CreateFoto(newFoto);

                        FotoCreateResponse response = new FotoCreateResponse
                        {
                            Foto = newFoto,
                            Message = $"Foto {fotoForm.Foto?.Name} creata con successo"
                        };
                        return response;
                    }
                }
                else
                {
                    // Gestisci il caso in cui il modello non è valido
                    return new FotoCreateResponse
                    {
                        Message = "Dati non validi"
                    };
                }
            }
            catch (Exception ex)
            {
                return new FotoCreateResponse
                {
                    Message = $"Errore durante la creazione della foto. Dettagli: {ex.Message}"
                };
            }
        }


        [HttpDelete("{id}")]
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
