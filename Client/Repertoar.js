import { Film } from "./Film.js";

export class Repertoar{

    constructor(listaBioskopa, listaDatuma, listaFilmova) {
        this.listaBioskopa = listaBioskopa;
        this.listaDatuma = listaDatuma;
        this.listaFilmova = listaFilmova;
        this.kont = null;
    }

    crtaj(host){
        this.kont = document.createElement("div");
        this.kont.className = "GlavniKontejner";
        host.appendChild(this.kont);

        let kontForma = document.createElement("div");
        kontForma.className = "Forma";
        this.kont.appendChild(kontForma);

        let naslov = document.createElement("h2");
        naslov.className = "Naslov";
        naslov.innerHTML = "Pregled bioskopskog repertoara:";
        kontForma.appendChild(naslov);

        this.crtajFormu(kontForma);
        this.crtajPrikaz(this.kont);

    }

    crtajPrikaz(host){
        let kontPrikaz = document.createElement("div");
        kontPrikaz.className = "Prikaz";
        host.appendChild(kontPrikaz);

        var tabela = document.createElement("table");
        tabela.className = "tabela";
        kontPrikaz.appendChild(tabela);

        var tabelaBody = document.createElement("tbody");
        tabelaBody.className ="TabelaPodaci";
        tabela.appendChild(tabelaBody);
        this.prikaziFilmove();

    }

    crtajFormu(host){
        let red = this.crtajRed(host);
        let l = document.createElement("label");
        l.innerHTML = "Bioskop";
        red.appendChild(l);

        let selBioskopa = document.createElement("select");
        selBioskopa.id = "bioskop";
        red.appendChild(selBioskopa);

        let op;
        this.listaBioskopa.forEach(p => {
            op = document.createElement("option");
            op.innerHTML = p.naziv + " " + p.mesto;
            op.value = p.id;
            selBioskopa.appendChild(op);
        })

        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Datum";
        red.appendChild(l);

        let cbox = document.createElement("div");
        cbox.className = "cbox";
        red.appendChild(cbox);

        let cboxLevi = document.createElement("div");
        cboxLevi.className = "cboxLevi";
        cbox.appendChild(cboxLevi);

        let cboxDesni = document.createElement("div");
        cboxDesni.className = "cboxDesni";
        cbox.appendChild(cboxDesni);

        let cb;
        let cbDiv;
        this.listaDatuma.forEach((p, index) =>{
            cbDiv = document.createElement("div");
            cb = document.createElement("input");
            cb.type = "checkbox";
            cb.value = p.id;
            cbDiv.appendChild(cb);
            
            l = document.createElement("label");
            l.innerHTML = p.datum;
            cbDiv.appendChild(l);
            if(index%2==0)
            {
                cboxLevi.appendChild(cbDiv);
            }
            else
            {
                cboxDesni.appendChild(cbDiv);
            }
            
        })

        red = this.crtajRed(host);
        let btnPrikazi = document.createElement("button");       
        btnPrikazi.innerHTML = "Prikaži";
        red.appendChild(btnPrikazi);
        btnPrikazi.onclick = (ev) => this.prikaziFilmove();

        red = this.crtajRed(host);
        let naslovUnesi = document.createElement("h2");
        naslovUnesi.className = "NaslovUnesi";
        naslovUnesi.innerHTML = "Izmena bioskopskog repertoara:";
        red.appendChild(naslovUnesi);

        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Naziv filma:";
        red.appendChild(l);

        let selFilma = document.createElement("select");
        selFilma.id = "film";
        red.appendChild(selFilma);

        let opcija;
        this.listaFilmova.forEach(p => {
            opcija = document.createElement("option");
            opcija.innerHTML = p.naziv;
            opcija.value = p.id;
            selFilma.appendChild(opcija);
        })

        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Vreme projekcije:";
        red.appendChild(l);

        red = this.crtajRed(host);
        var vremeProjekcije = document.createElement("input");
        //vremeProjekcije.className = "VremeProjekcije";
        vremeProjekcije.id = "VremeProjekcije";
        red.appendChild(vremeProjekcije); 
        
        red = this.crtajRed(host);
        let btnUpisi = document.createElement("button");       
        btnUpisi.innerHTML = "Upiši";
        red.appendChild(btnUpisi);      
        btnUpisi.onclick = (ev) => this.upisi();

        let btnAzuriraj = document.createElement("button");       
        btnAzuriraj.innerHTML = "Ažuriraj";
        red.appendChild(btnAzuriraj);      
        btnAzuriraj.onclick = (ev) => this.azuriraj();

        let btnObrisi = document.createElement("button");       
        btnObrisi.innerHTML = "Obriši";
        red.appendChild(btnObrisi);      
        btnObrisi.onclick = (ev) => this.obrisi();

    }

    upisi()
    {
        var vreme = document.getElementById('VremeProjekcije').value;
        console.log("proba:" + vreme);
        var status = /^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/.test(vreme);

        if(vreme === null || vreme === undefined || vreme === "")
        {
            alert("Unesite vreme projekcije filma!");
            return;
        }

        if(!status)
        {
            alert("Uneli ste pogrešan format za vreme projekcije!");
            return;
        }

        let datumi = this.kont.querySelectorAll("input[type = 'checkbox']:checked");
        if (datumi === null || datumi.length !=1)
        {
            alert("Morate izabrati samo jedan datum!");
            return;
        }
        
        var bioskopId = document.querySelector('#bioskop option:checked').value;
        var filmId = document.querySelector('#film option:checked').value;

        console.log("bioskop:" + bioskopId + " datum:" + datumi[0].value + " id filma:" + filmId + " vreme:" + vreme);

        fetch("https://localhost:5001/Projekcije/DodajPocetakPrikazivanja/" + bioskopId + "/" + filmId 
        + "/" + datumi[0].value + "/" + vreme,
        {
            method:"POST"
        }).then(s => {
            if(s.ok){
                var teloTabele = this.obrisiPrethodniSadrzaj();
                s.json().then(data => {
                    
                    this.prikazi(data, teloTabele);
                })
            }
            else{
                if(s.status == 400) {
                    alert("Postoji projekcija u unetom terminu!");
                    return;
                }
            }
        })
        
    }

    obrisi()
    {
        var vreme = document.getElementById('VremeProjekcije').value;
        var status = /^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/.test(vreme);

        if(vreme === null || vreme === undefined || vreme === "")
        {
            alert("Unesite vreme projekcije filma!");
            return;
        }

        if(!status)
        {
            alert("Uneli ste pogrešan format za vreme projekcije!");
            return;
        }

        let datumi = this.kont.querySelectorAll("input[type = 'checkbox']:checked");
        if (datumi === null || datumi.length !=1)
        {
            alert("Morate izabrati samo jedan datum!");
            return;
        }
        
        var bioskopId = document.querySelector('#bioskop option:checked').value;
        var filmId = document.querySelector('#film option:checked').value;

        console.log("bioskop:" + bioskopId + " datum:" + datumi[0].value + " id filma:" + filmId + " vreme:" + vreme);

        fetch("https://localhost:5001/Projekcije/IzbrisatiProjekciju/" + bioskopId + "/" + filmId 
        + "/" + datumi[0].value + "/" + vreme,
        {
            method:"DELETE"
        }).then(s => {
            if(s.ok){
                var teloTabele = this.obrisiPrethodniSadrzaj();
                s.json().then(data => {
                    //console.log(data);
                    this.prikazi(data, teloTabele);
                })
            }
            else{
                if(s.status == 400) {
                    alert("Ne postoji uneta projekcija!");
                    return;
                }
            }
        })
        
    }

    azuriraj()
    {
        var vreme = document.getElementById('VremeProjekcije').value;
        console.log("proba:" + vreme);
        var status = /^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/.test(vreme);

        if(vreme === null || vreme === undefined || vreme === "")
        {
            alert("Unesite vreme projekcije filma!");
            return;
        }

        if(!status)
        {
            alert("Uneli ste pogrešan format za vreme projekcije!");
            return;
        }

        let datumi = this.kont.querySelectorAll("input[type = 'checkbox']:checked");
        if (datumi === null || datumi.length !=1)
        {
            alert("Morate izabrati samo jedan datum!");
            return;
        }
        
        var bioskopId = document.querySelector('#bioskop option:checked').value;
        var filmId = document.querySelector('#film option:checked').value;

        console.log("bioskop:" + bioskopId + " datum:" + datumi[0].value + " id filma:" + filmId + " vreme:" + vreme);

        fetch("https://localhost:5001/Projekcije/AzurirajProjekciju/" + bioskopId + "/" + filmId 
        + "/" + datumi[0].value + "/" + vreme,
        {
            method:"PUT"
        }).then(s => {
            if(s.ok){
                var teloTabele = this.obrisiPrethodniSadrzaj();
                s.json().then(data => {
                    this.prikazi(data, teloTabele);
                })
            }
            else{
                if(s.status == 400) {
                    alert("Ne postoji uneta projekcija!");
                    return;
                }
            }
        })
        
    }

    prikaziFilmove() {
        let optionEl = this.kont.querySelector("select");
        var bioskopId = optionEl.options[optionEl.selectedIndex].value;
        console.log(bioskopId);

        let datumi = this.kont.querySelectorAll("input[type = 'checkbox']:checked");
        let nizDatuma = "";
        if(datumi.length == 0){
            let checkboxes = this.kont.querySelectorAll("input[type = 'checkbox']");
            checkboxes[0].checked = true;

            let datumi = this.kont.querySelectorAll("input[type = 'checkbox']:checked");
            for(let i=0; i<datumi.length; i++){
                nizDatuma = nizDatuma.concat(datumi[i].value, "a");
            }
            console.log(nizDatuma);
            this.ucitajRepertoar(bioskopId, nizDatuma);
        }
      
        for(let i=0; i<datumi.length; i++){
            nizDatuma = nizDatuma.concat(datumi[i].value, "a");
        }

        console.log(nizDatuma);
        this.ucitajRepertoar(bioskopId, nizDatuma);
    }

    ucitajRepertoar(bioskopId, nizDatuma){
        fetch("https://localhost:5001/Bioskop/BioskopPretraga/" + nizDatuma + "/" + bioskopId,
        {
            method:"GET"
        }).then(p=>{
            if(p.ok){
                var teloTabele = this.obrisiPrethodniSadrzaj();
                p.json().then(data => {               
                this.prikazi(data, teloTabele);
                    
                })
            }
        })

    }

    prikazi(data, teloTabele)
    {
        if(data.length != 0)
        {
            console.log(data);
            let naziv = data[0].nazivFilma;
            let datum = data[0].datumPrikazivanja;
            let vremena = [];
            let j = 0;
            for(let i = 0; i<data.length; i++)
            {
                
                if(naziv == data[i].nazivFilma && datum == data[i].datumPrikazivanja)
                {
                    vremena[j] = data[i].vremePrikazivanja;
                    j++;
                }
                else
                {
                    naziv = data[i].nazivFilma;
                    datum = data[i].datumPrikazivanja;
                    let film = new Film(data[i-1].nazivFilma, data[i-1].slikaFilma, data[i-1].originalniNaziv, 
                        data[i-1].trajanje, data[i-1].godina, data[i-1].zemljaPorekla, data[i-1].reziser, data[i-1].uloge, 
                        data[i-1].zanr, data[i-1].datumPrikazivanja, vremena);
                    film.crtaj(teloTabele);
                    j = 0;
                    vremena.length = 0;
                    vremena[j] = data[i].vremePrikazivanja;
                    j++;
                }
            }
            let film = new Film(data[data.length-1].nazivFilma, data[data.length-1].slikaFilma, data[data.length-1].originalniNaziv, 
                data[data.length-1].trajanje, data[data.length-1].godina, data[data.length-1].zemljaPorekla, data[data.length-1].reziser, data[data.length-1].uloge, 
                data[data.length-1].zanr, data[data.length-1].datumPrikazivanja, vremena);
            film.crtaj(teloTabele);
                               
        }
    }

    obrisiPrethodniSadrzaj()
    {
        var teloTabele = document.querySelector(".TabelaPodaci");
        var roditelj = teloTabele.parentNode;
        roditelj.removeChild(teloTabele);

        teloTabele = document.createElement("tbody");
        teloTabele.className = "TabelaPodaci";
        roditelj.appendChild(teloTabele);
        return teloTabele;
    }

    crtajRed(host)
    {
        var red = document.createElement("div");
        red.className = "red";
        host.appendChild(red);
        return red;
    }

}