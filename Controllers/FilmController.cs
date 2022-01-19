using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repertoar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;
        public RepertoarContext Context { get; set; }

        public FilmController(IWebHostEnvironment webHostEnvironment, RepertoarContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            Context = context;
        }

        [Route("Filmovi")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiFilmove()
        {
            try
            {
                return Ok(await Context.Filmovi.Select(p => 
                new
                {
                    ID = p.ID,
                    Naziv = p.Naziv
                }).ToListAsync()
                );
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("Dodaj")]
        [HttpPost]
        public async Task<string> SnimiFilm(string naziv, string originalniNaziv, string reziser, int trajanje, int godina, string uloge, string zanr, string zemljaPorekla, [FromForm] FileUpload fileUpload)
        {
            try
            {
                if(fileUpload.files.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + fileUpload.files.FileName))
                    {
                        fileUpload.files.CopyTo(fileStream);
                        fileStream.Flush();

                        Film film = new Film
                        {
                            Naziv = naziv,
                            OriginalniNaziv = originalniNaziv,
                            Reziser = reziser,
                            Trajanje = trajanje,
                            Godina = godina,
                            Uloge = uloge,
                            Zanr = zanr,
                            ZemljaPorekla = zemljaPorekla,
                            Image = fileUpload.files.FileName

                        };

                        Context.Filmovi.Add(film);
                        await Context.SaveChangesAsync();
                        return "Upload Done.";
                    }
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

       [Route("PromeniFilm")]
        [HttpPut]
        public async Task<ActionResult> Promeni(int id, string naziv, string originalniNaziv, string reziser, int trajanje, int godina, string uloge, string zanr, string zemljaPorekla, [FromForm] FileUpload fileUpload)
        {
            if (id < 0) 
            {
                return BadRequest("Pogrešan ID!");
            }
            if (string.IsNullOrWhiteSpace(naziv) || naziv.Length > 50) 
            {
                return BadRequest("Pogrešan naziv!");
            }

            try
            {
                var filmZaIzmenu = Context.Filmovi.Where(p => p.ID == id).FirstOrDefault();
                if (filmZaIzmenu != null) 
                {
                    if(fileUpload.files.Length > 0)
                    {
                        string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                        if(!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        using (FileStream fileStream = System.IO.File.Create(path + fileUpload.files.FileName))
                        {
                            filmZaIzmenu.Naziv = naziv;
                            filmZaIzmenu.OriginalniNaziv = originalniNaziv;
                            filmZaIzmenu.Reziser = reziser;
                            filmZaIzmenu.Trajanje = trajanje;
                            filmZaIzmenu.Godina = godina;
                            filmZaIzmenu.Uloge = uloge;
                            filmZaIzmenu.Zanr = zanr;
                            filmZaIzmenu.ZemljaPorekla = zemljaPorekla;
                            filmZaIzmenu.Image = fileUpload.files.FileName;
                            await Context.SaveChangesAsync();
                            return Ok($"Uspešno promenjen film! ID: {filmZaIzmenu.ID}");
                        }
                    }
                    else
                    {
                        return BadRequest("Failed!");
                    }
                }
                else
                {
                    return BadRequest("Film nije pronadjen!");
                }
                
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("Izbrisati")]
        [HttpDelete]
        public async Task<ActionResult> Izbrisati(int id)
        {
             if (id < 0) 
            {
                return BadRequest("Pogrešan ID!");
            }
            try
            {
                var filmZaBrisanje = await Context.Filmovi.FindAsync(id);
                Context.Filmovi.Remove(filmZaBrisanje);
                await Context.SaveChangesAsync();
                return Ok($"Uspešno izbrisan film: {filmZaBrisanje.Naziv}!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
