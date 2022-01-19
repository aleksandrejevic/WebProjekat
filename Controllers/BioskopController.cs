using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repertoar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BioskopController : ControllerBase
    {

        public RepertoarContext Context { get; set; }

        public BioskopController(RepertoarContext context)
        {
            Context = context;
        }

        [Route("BioskopPretraga/{datumi}/{bioskopID}")]
        [HttpGet]
        public async Task<ActionResult> BioskopPretraga(string datumi, int bioskopID)
        {
            DateTimeFormatInfo fmt = (new CultureInfo("hr-HR")).DateTimeFormat;
            try
            {
                var datumIds = datumi.Split('a')
                .Where(x => int.TryParse(x, out _))
                .Select(int.Parse)
                .ToList();

                var bioskopi = Context.BioskopiFilmovi
                    .Include(p => p.Bioskop)
                    .Include(p => p.PocetakPrikazivanja)
                    .ThenInclude(p => p.Datum)
                    .Include(p => p.Film)
                    .Where(p => p.Bioskop.ID == bioskopID && datumIds.Contains(p.PocetakPrikazivanja.Datum.ID));


                var bioskop = await bioskopi.ToListAsync();

                var orderByResult = from s in bioskop
                   orderby s.Film.Naziv, s.PocetakPrikazivanja.Datum.DatumPrikazivanja, s.PocetakPrikazivanja.Sati
                   select s;
                return Ok
                (
                    orderByResult.Select(p => 
                    new
                    {
                        NazivFilma = p.Film.Naziv,
                        SlikaFilma = p.Film.Image,
                        OriginalniNaziv = p.Film.OriginalniNaziv,
                        Trajanje = p.Film.Trajanje,
                        Godina = p.Film.Godina,
                        ZemljaPorekla = p.Film.ZemljaPorekla,
                        Reziser = p.Film.Reziser,
                        Uloge = p.Film.Uloge,
                        Zanr = p.Film.Zanr,
                        DatumPrikazivanja = p.PocetakPrikazivanja.Datum.DatumPrikazivanja.ToString("d", fmt),
                        VremePrikazivanja = p.PocetakPrikazivanja.Sati

                    }).ToList()
                );  
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [Route("Bioskopi")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiBioskope()
        {
            try
            {
                return Ok(await Context.Bioskopi.Select(p => 
                new
                {
                    ID = p.ID,
                    Naziv = p.Naziv,
                    Mesto = p.Mesto
                }).ToListAsync()
                );
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajBioskop")]
        [HttpPost]
        public async Task<ActionResult> DodajBioskop([FromBody] Bioskop bioskop)
        {
            if (string.IsNullOrWhiteSpace(bioskop.Naziv) || bioskop.Naziv.Length > 50) 
            {
                return BadRequest("Pogrešan naziv!");
            }

            if (string.IsNullOrWhiteSpace(bioskop.Mesto) || bioskop.Mesto.Length > 50) 
            {
                return BadRequest("Pogrešno mesto!");
            }

            try
            {
                Context.Bioskopi.Add(bioskop);
                await Context.SaveChangesAsync();
                return Ok($"Bioskop je dodat! ID je: {bioskop.ID}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [Route("PromeniBioskop/{id}/{naziv}/{mesto}")]
        [HttpPut]
        public async Task<ActionResult> Promeni(int id, string naziv, string mesto)
        {
            if (id < 0) 
            {
                return BadRequest("Pogrešan ID!");
            }
            if (string.IsNullOrWhiteSpace(naziv) || naziv.Length > 50) 
            {
                return BadRequest("Pogrešan naziv!");
            }

            if (string.IsNullOrWhiteSpace(mesto) || mesto.Length > 50) 
            {
                return BadRequest("Pogrešno mesto!");
            }
            try
            {
                var bioskop = Context.Bioskopi.Where(p => p.ID == id).FirstOrDefault();
                if (bioskop != null) 
                {
                    bioskop.Mesto = mesto;
                    bioskop.Naziv = naziv;
                    await Context.SaveChangesAsync();
                    return Ok($"Uspešno promenjen bioskop! ID: {bioskop.ID}");
                }
                else
                {
                    return BadRequest("Bioskop nije pronadjen!");
                }
                
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisatiBioskop")]
        [HttpDelete]
        public async Task<ActionResult> Izbrisati(int id)
        {
             if (id < 0) 
            {
                return BadRequest("Pogrešan ID!");
            }
            try
            {
                var bioskopZaBrisanje = await Context.Bioskopi.FindAsync(id);
                Context.Bioskopi.Remove(bioskopZaBrisanje);
                await Context.SaveChangesAsync();
                return Ok($"Uspešno izbrisan bioskop: {bioskopZaBrisanje.Naziv}!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
