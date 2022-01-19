using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repertoar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PocetakPrikazivanjaController : ControllerBase
    {

        public RepertoarContext Context { get; set; }

        public PocetakPrikazivanjaController(RepertoarContext context)
        {
            Context = context;
        }

        [Route("PoceciPrikazivanja")]
        [HttpGet]
        public ActionResult Preuzmi()
        {
            return Ok(Context.PoceciPrikazivanja);
        }

        [Route("DodajPocetakPrikazivanja")]
        [HttpPost]
        public async Task<ActionResult> DodajPocetakPrikazivanja([FromBody] PocetakPrikazivanja pocetakPrikazivanja)
        {
            Regex r = new Regex("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");  
            if (!r.IsMatch(pocetakPrikazivanja.Sati))
            {
                return BadRequest("Početak prikazivanja nije ispravan!");
            }
            
            try
            {
                Context.PoceciPrikazivanja.Add(pocetakPrikazivanja);
                await Context.SaveChangesAsync();
                return Ok($"Početak prikazivanja je dodat! ID je: {pocetakPrikazivanja.ID}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [Route("PromeniPocetakPrikazivanja")]
        [HttpPut]
        public async Task<ActionResult> Promeni([FromBody] PocetakPrikazivanja pocetakPrikazivanja)
        {
            if (pocetakPrikazivanja.ID < 0) 
            {
                return BadRequest("Pogrešan ID!");
            }
            Regex r = new Regex("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");  
            if (!r.IsMatch(pocetakPrikazivanja.Sati))
            {
                return BadRequest("Početak prikazivanja nije ispravan!");
            }

            try
            {
                Context.PoceciPrikazivanja.Update(pocetakPrikazivanja);
                await Context.SaveChangesAsync();
                return Ok($"Početak prikazivanja sa ID: {pocetakPrikazivanja.ID} je uspešno izmenjen!");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [Route("IzbrisatiVreme")]
        [HttpDelete]
        public async Task<ActionResult> Izbrisati(int id)
        {
             if (id < 0) 
            {
                return BadRequest("Pogrešan ID!");
            }
            try
            {
                var vremeZaBrisanje = await Context.PoceciPrikazivanja.FindAsync(id);
                Context.PoceciPrikazivanja.Remove(vremeZaBrisanje);
                await Context.SaveChangesAsync();
                return Ok($"Uspešno izbrisano vreme: {vremeZaBrisanje.Sati}!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
