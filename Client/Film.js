export class Film{
    constructor(naziv, slika, onaziv, trajanje, godina, zemlja, reziser, uloge, zanr, datum, vreme = []) {
        this.naziv = naziv;
        this.slika = slika;
        this.onaziv = onaziv;
        this.trajanje = trajanje;
        this.godina = godina;
        this.zemlja = zemlja;
        this.reziser = reziser;
        this.uloge = uloge;
        this.zanr = zanr;
        this.datum = datum;
        this.vreme = vreme;
    }

    crtaj(host){
        var tr = document.createElement("tr");
        host.appendChild(tr);

        var elNaziv = document.createElement("td");
        elNaziv.innerHTML = this.naziv;
        elNaziv.className = "naziv";
        tr.appendChild(elNaziv);

        var el = document.createElement("td");
        let sl = document.createElement("img");
        sl.className = "slika";
        console.log("Slika:" + this.slika);
        sl.src = "/wwwroot/uploads/" + this.slika;
        el.appendChild(sl);
        
        tr.appendChild(el);

        el = document.createElement("td");
        if(this.onaziv != null)
            el.innerHTML = "Originalni naziv: " + this.onaziv;
        if(this.trajanje !== undefined)
            el.innerHTML += "<br />" + "Trajanje(min): " + this.trajanje;
        if(this.godina !== undefined)
            el.innerHTML += "<br />" + "Godina: " + this.godina;
        if(this.zemlja != null)
            el.innerHTML += "<br />" + "Zemlja porekla: " + this.zemlja;
        if(this.reziser != null)
            el.innerHTML += "<br />" + "Režiser: " + this.reziser;
        if(this.uloge != null)
            el.innerHTML += "<br />" + "Uloge: " + this.uloge;
        if(this.zanr != null)
            el.innerHTML += "<br />" + "Žanr: " + this.zanr;
        tr.appendChild(el);

        el = document.createElement("td");
        el.className = "Vreme";
        el.innerHTML = this.datum + "<br />" + this.vreme;
        tr.appendChild(el);

    }
}