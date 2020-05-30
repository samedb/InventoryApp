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
        public DbSet<Predmet> Predmeti { get; set; }
        public DbSet<Inventar> Inventar { get; set; }
        public DbSet<Zaduzenje> Zaduzenja { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=inventory.db");

    }

    public class Radnik
    {
        [Key]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string JMBG { get; set; }
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Required]
        public string Pol { get; set; }
        public string StrucnaSprema { get; set; }
        [Required]
        public bool IsAdmin { get; set; }

        public override string ToString()
        {
            return Username;
        }
    }

    public class Prostorija
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NazivProstorije { get; set; }
        public string Sprat { get; set; }
        public string Sirina { get; set; }
        public string Visina { get; set; }
        public string Duzina { get; set; }
        public Radnik SefProstorije{ get; set; }

        public override string ToString()
        {
            return $"{Id}  Naziv: {NazivProstorije}  Sprat: {Sprat}  Sef: {SefProstorije?.Username}";
        }
    }

    public class Predmet
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Naziv { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int Cena { get; set; }

        public override string ToString()
        {
            return $"({Id}) {Naziv} {Marka} {Model}";
        }
    }

    public class Inventar
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Predmet Predmet { get; set; }
        [Required]
        public Prostorija Prostorija { get; set; }
        [Required]
        public int Kolicina { get; set; }

        //internal Inventar Clone()
        //{
        //    return new Inventar
        //    {
        //        Id = this.Id,
        //        Naziv = this.Naziv,
        //        Marka = this.Marka,
        //        Model = this.Model,
        //        Cena = this.Cena,
        //        Kolicina = this.Kolicina,
        //        DatumZaduzivanja = this.DatumZaduzivanja,
        //        UsernameRadnikaKojiJeZaduzio = this.UsernameRadnikaKojiJeZaduzio,
        //        IdProstorije = this.IdProstorije
        //    };
        //}
    }

    public class Zaduzenje
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Radnik Radnik { get; set; }
        [Required]
        public Predmet Predmet { get; set; }
        [Required]
        public Prostorija Prostorija { get; set; }
        [Required]
        public int Kolicina { get; set; }
        [Required]
        public DateTime DatumZaduzivanja { get; set; }
    }
}
