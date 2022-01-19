import { Bioskop } from "./bioskop.js";
import { Datum } from "./datum.js";
import { Repertoar } from "./repertoar.js";
import { Filmovi } from "./filmovi.js";

var listaBioskopa = [];
var listaDatuma = [];
var listaFilmova = [];

fetch("https://localhost:5001/Bioskop/Bioskopi")
.then(p => {
    p.json().then(bioskopi => {
        bioskopi.forEach(bioskop => {
            var p = new Bioskop(bioskop.id, bioskop.naziv, bioskop.mesto);
            listaBioskopa.push(p);
        });
        fetch("https://localhost:5001/Datum/Datumi")
        .then(q => {
            q.json().then(datumi => {
                datumi.forEach(datum => {
                    var q = new Datum(datum.id, datum.datum);
                    listaDatuma.push(q);
                });
                fetch("https://localhost:5001/Film/Filmovi")
                .then(e => {
                    e.json().then(filmovi => {
                        filmovi.forEach(film => {
                            var e = new Filmovi(film.id, film.naziv);
                            listaFilmova.push(e);
                        });
                        var r = new Repertoar(listaBioskopa, listaDatuma, listaFilmova);
                        r.crtaj(document.body);
                    })
                })
            })
        })
    })
})

console.log(listaBioskopa);

console.log(listaDatuma);
console.log(listaFilmova);
