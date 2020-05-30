using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory
{
    public class InventoryContext : DbContext
    {
        public DbSet<Radnik> Radnici { get; set; }
        public DbSet<Prostorija> Prostorije { get; set; }
        public DbSet<Inventar> Inventar { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=inventory.db");
    }

    public class Radnik
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public string JMBG { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Pol { get; set; }
        public string StrucnaSprema { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class Prostorija
    {
        [Key]
        public int Id { get; set; }
        public string NazivProstorije { get; set; }
        public string Sprat { get; set; }
        public string Sirina { get; set; }
        public string Visina { get; set; }
        public string Duzina { get; set; }
        public string UsernameSefa { get; set; }

        public override string ToString()
        {
            return $"{Id}  Naziv: {NazivProstorije}  Sprat: {Sprat}  Sef: {UsernameSefa}";
        }
    }

    public class Inventar
    {
        [Key]
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int Cena { get; set; }
        public int Kolicina { get; set; }
        public DateTime DatumZaduzivanja { get; set; }
        [ForeignKey("Radnik")]
        public string UsernameRadnikaKojiJeZaduzio { get; set; }
        [ForeignKey("Prostorija")]
        public int IdProstorije { get; set; }

        internal Inventar Clone()
        {
            return new Inventar
            {
                Id = this.Id,
                Naziv = this.Naziv,
                Marka = this.Marka,
                Model = this.Model,
                Cena = this.Cena,
                Kolicina = this.Kolicina,
                DatumZaduzivanja = this.DatumZaduzivanja,
                UsernameRadnikaKojiJeZaduzio = this.UsernameRadnikaKojiJeZaduzio,
                IdProstorije = this.IdProstorije
            };
        }
    }
}
