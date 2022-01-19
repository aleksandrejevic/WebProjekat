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
    public class DatumController : ControllerBase
    {

        public RepertoarContext Context { get; set; }

        public DatumController(RepertoarContext context)
        {
            Context = context;
        }

        [Route("Datumi")]
        [HttpGet]
        public async Task<ActionResult> Preuzmi()
        {
            DateTimeFormatInfo fmt = (new CultureInfo("hr-HR")).DateTimeFormat;
            try
            {
                var datumi = await Context.Datumi.Where(t => t.DatumPrikazivanja.Date >= DateTime.Today).ToListAsync();

                var orderByResult = from s in datumi
                   orderby s.DatumPrikazivanja
                   select s;
                return Ok(orderByResult
                .Select(p => 
                new
                {
                  
                        ID = p.ID,
                        Datum = p.DatumPrikazivanja.ToString("d", fmt)
                    
                }).ToList()
                );
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("DodajDatum")]
        [HttpPost]
        public async Task<ActionResult> DodajDatum([FromBody] Datum datum)
        {
            if (datum.DatumPrikazivanja < DateTime.Today)
            {
                return BadRequest("Početak prikazivanja nije ispravan!");
            }
            if (datum.DatumPrikazivanja > DateTime.Now.AddDays(7))
            {
                return BadRequest("Možete uneti repertoar samo za narednih 7 dana!");
            }
            try
            {
                Context.Datumi.Add(datum);
                await Context.SaveChangesAsync();
                return Ok($"Datum prikazivanja je dodat! ID je: {datum.ID}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [Route("PromeniDatum")]
        [HttpPut]
        public async Task<ActionResult> Promeni([FromBody] Datum datum)
        {
            if (datum.ID < 0) 
            {
                return BadRequest("Pogrešan ID!");
            }
            if (datum.DatumPrikazivanja < DateTime.Today)
            {
                return BadRequest("Početak prikazivanja nije ispravan!");
            }
            if (datum.DatumPrikazivanja > DateTime.Now.AddDays(7))
            {
                return BadRequest("Možete uneti repertoar samo za narednih 7 dana!");
            }

            try
            {
                Context.Datumi.Update(datum);
                await Context.SaveChangesAsync();
                return Ok($"Datum sa ID: {datum.ID} je uspešno izmenjen!");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [Route("IzbrisatiDatum")]
        [HttpDelete]
        public async Task<ActionResult> Izbrisati(int id)
        {
             if (id < 0) 
            {
                return BadRequest("Pogrešan ID!");
            }
            try
            {
                var datumZaBrisanje = await Context.Datumi.FindAsync(id);
                Context.Datumi.Remove(datumZaBrisanje);
                await Context.SaveChangesAsync();
                return Ok($"Uspešno izbrisan datum: {datumZaBrisanje.DatumPrikazivanja}!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
