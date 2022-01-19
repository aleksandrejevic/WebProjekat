using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;

namespace Repertoar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjekcijeController : ControllerBase
    {

        public RepertoarContext Context { get; set; }

        public ProjekcijeController(RepertoarContext context)
        {
            Context = context;
        }

        [Route("DodajPocetakPrikazivanja/{idBioskopa}/{idFilma}/{idDatuma}/{sati}")]
        [HttpPost]
        public async Task<ActionResult> Dodaj(int idBioskopa, int idFilma, int idDatuma, string sati)
        {
            DateTimeFormatInfo fmt = (new CultureInfo("hr-HR")).DateTimeFormat;
            if (idBioskopa < 0) 
            {
                return BadRequest("Pogrešan ID bioskopa!");
            }
            if (idFilma < 0) 
            {
                return BadRequest("Pogrešan ID filma!");
            }
            if (idDatuma < 0) 
            {
                return BadRequest("Pogrešan ID datuma!");
            }
            Regex r = new Regex("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");  
            if (string.IsNullOrWhiteSpace(sati) || !r.IsMatch(sati))
            {
                return BadRequest("Početak prikazivanja nije ispravan!");
            }
            try
            {
                var bioskop = await Context.Bioskopi.Where(p => p.ID == idBioskopa).FirstOrDefaultAsync();
                var film = await Context.Filmovi.Where(p => p.ID == idFilma).FirstOrDefaultAsync();
                var datum = await Context.Datumi.FindAsync(idDatuma);
                var pom = await Context.PoceciPrikazivanja.Where(p => p.Sati == sati && p.Datum.DatumPrikazivanja == datum.DatumPrikazivanja).FirstOrDefaultAsync();
                if (pom == null)
                {
                    PocetakPrikazivanja poc = new PocetakPrikazivanja
                    {
                        Datum = datum,
                        Sati = sati

                    };

                    Context.PoceciPrikazivanja.Add(poc);
                    await Context.SaveChangesAsync();
                }
                
                var pocPrikazivanja = await Context.PoceciPrikazivanja.Where(p => p.Sati == sati && p.Datum.DatumPrikazivanja == datum.DatumPrikazivanja).FirstOrDefaultAsync();
                var projekcijaPostoji = await Context.BioskopiFilmovi.Where(p => p.PocetakPrikazivanja.ID == pocPrikazivanja.ID && p.Bioskop.ID == idBioskopa).FirstOrDefaultAsync();
                if(projekcijaPostoji != null)
                {
                    return BadRequest("Postoji projekcija u unetom terminu!");
                }
                else
                {
                    Spoj s = new Spoj
                    {
                        Bioskop = bioskop,
                        Film = film,
                        PocetakPrikazivanja = pocPrikazivanja

                    };

                    Context.BioskopiFilmovi.Add(s);
                    await Context.SaveChangesAsync();

                    var podaciOBioskopu = Context.BioskopiFilmovi
                        .Include(p => p.Bioskop)
                        .Include(p => p.PocetakPrikazivanja)
                        .ThenInclude(p => p.Datum)
                        .Include(p => p.Film)
                        .Where(p => p.Bioskop.ID == idBioskopa && idDatuma==p.PocetakPrikazivanja.Datum.ID);


                    var result = await podaciOBioskopu.ToListAsync();

                    var sortiranResult = from a in result orderby a.Film.Naziv,
                    a.PocetakPrikazivanja.Datum.DatumPrikazivanja, a.PocetakPrikazivanja.Sati
                    select a;

                    return Ok
                    (
                        sortiranResult.Select(p => 
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
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [Route("IzbrisatiProjekciju/{idBioskopa}/{idFilma}/{idDatuma}/{sati}")]
        [HttpDelete]
        public async Task<ActionResult> Izbrisati(int idBioskopa, int idFilma, int idDatuma, string sati)
        {
            DateTimeFormatInfo fmt = (new CultureInfo("hr-HR")).DateTimeFormat;
             
            try
            {
                var bioskop = await Context.Bioskopi.Where(p => p.ID == idBioskopa).FirstOrDefaultAsync();
                var film = await Context.Filmovi.Where(p => p.ID == idFilma).FirstOrDefaultAsync();
                var datum = await Context.Datumi.FindAsync(idDatuma);
                //validacija
                var pom = await Context.PoceciPrikazivanja.Where(p => p.Sati == sati && p.Datum.DatumPrikazivanja == datum.DatumPrikazivanja).FirstOrDefaultAsync();
                if (pom == null)
                {
                    return BadRequest("Ne postoji uneta projekcija!");
                }
                else{
                    var pocPrikazivanja = await Context.PoceciPrikazivanja.Where(p => p.Sati == sati && p.Datum.DatumPrikazivanja == datum.DatumPrikazivanja).FirstOrDefaultAsync();
                    var s = await Context.BioskopiFilmovi.Where(p => p.Bioskop.ID == idBioskopa && p.Film.ID == idFilma && p.PocetakPrikazivanja.ID ==pom.ID).FirstOrDefaultAsync();

                    Context.BioskopiFilmovi.Remove(s);
                    await Context.SaveChangesAsync();

                    var podaciOBioskopu = Context.BioskopiFilmovi
                    .Include(p => p.Bioskop)
                    .Include(p => p.PocetakPrikazivanja)
                    .ThenInclude(p => p.Datum)
                    .Include(p => p.Film)
                    .Where(p => p.Bioskop.ID == idBioskopa && idDatuma==p.PocetakPrikazivanja.Datum.ID);


                var result = await podaciOBioskopu.ToListAsync();

                var sortiranResult = from a in result orderby a.Film.Naziv,
                a.PocetakPrikazivanja.Datum.DatumPrikazivanja, a.PocetakPrikazivanja.Sati
                select a;

                return Ok
                (
                    sortiranResult.Select(p => 
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
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("AzurirajProjekciju/{idBioskopa}/{idFilma}/{idDatuma}/{sati}")]
        [HttpPut]
        public async Task<ActionResult> Azuriraj(int idBioskopa, int idFilma, int idDatuma, string sati)
        {
            DateTimeFormatInfo fmt = (new CultureInfo("hr-HR")).DateTimeFormat;
            if (idBioskopa < 0) 
            {
                return BadRequest("Pogrešan ID bioskopa!");
            }
            if (idFilma < 0) 
            {
                return BadRequest("Pogrešan ID filma!");
            }
            if (idDatuma < 0) 
            {
                return BadRequest("Pogrešan ID datuma!");
            }
            Regex r = new Regex("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");  
            if (string.IsNullOrWhiteSpace(sati) || !r.IsMatch(sati))
            {
                return BadRequest("Početak prikazivanja nije ispravan!");
            }
            try
            {
                var bioskop = await Context.Bioskopi.Where(p => p.ID == idBioskopa).FirstOrDefaultAsync();
                var film = await Context.Filmovi.Where(p => p.ID == idFilma).FirstOrDefaultAsync();
                var datum = await Context.Datumi.FindAsync(idDatuma);
                var pom = await Context.PoceciPrikazivanja.Where(p => p.Sati == sati && p.Datum.DatumPrikazivanja == datum.DatumPrikazivanja).FirstOrDefaultAsync();
                if (pom == null)
                {
                    return BadRequest("Ne postoji uneta projekcija!");
                }
                else
                {
                    var projekcija = await Context.BioskopiFilmovi.Where(p => p.PocetakPrikazivanja.ID == pom.ID && p.Bioskop.ID == idBioskopa).FirstOrDefaultAsync();
                    if(projekcija == null)
                    {
                        return BadRequest("Ne postoji projekcija u unetom terminu!");
                    }
                    else
                    {
                    
                        projekcija.Film = film;
                        projekcija.Bioskop = bioskop;
                        projekcija.PocetakPrikazivanja = pom;
                        await Context.SaveChangesAsync();
                        
                        var podaciOBioskopu = Context.BioskopiFilmovi
                        .Include(p => p.Bioskop)
                        .Include(p => p.PocetakPrikazivanja)
                        .ThenInclude(p => p.Datum)
                        .Include(p => p.Film)
                        .Where(p => p.Bioskop.ID == idBioskopa && idDatuma==p.PocetakPrikazivanja.Datum.ID);


                        var result = await podaciOBioskopu.ToListAsync();

                        var sortiranResult = from a in result orderby a.Film.Naziv,
                        a.PocetakPrikazivanja.Datum.DatumPrikazivanja, a.PocetakPrikazivanja.Sati
                        select a;

                        return Ok
                        (
                            sortiranResult.Select(p => 
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
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        
    }
}
