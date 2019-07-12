using System;
using System.Collections.Generic;
using System.Text;
using BibliotecaDominio;
using BibliotecaRepositorio.Contexto;
using BibliotecaRepositorio.Repositorio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DominioTest.TestDataBuilders;
using Microsoft.EntityFrameworkCore;

namespace DominioTest.Integracion
{

    [TestClass]
    public class BibliotecarioTest
    {
        public const String CRONICA_UNA_MUERTE_ANUNCIADA = "Cronica de una muerte anunciada";
        private  BibliotecaContexto contexto;
        private  RepositorioLibroEF repositorioLibro;
        private RepositorioPrestamoEF repositorioPrestamo;


        [TestInitialize]
        public void setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BibliotecaContexto>();
            contexto = new BibliotecaContexto(optionsBuilder.Options);
            repositorioLibro  = new RepositorioLibroEF(contexto);
            repositorioPrestamo = new RepositorioPrestamoEF(contexto, repositorioLibro);
        }

        [TestMethod]
        public void PrestarLibroTest()
        {
            // Arrange
            Libro libro = new LibroTestDataBuilder().ConTitulo(CRONICA_UNA_MUERTE_ANUNCIADA).Build();
            repositorioLibro.Agregar(libro);
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro, repositorioPrestamo);

            // Act
            bibliotecario.Prestar(libro.Isbn, "Juan");

            // Assert
            Assert.AreEqual(bibliotecario.EsPrestado(libro.Isbn), true);
            Assert.IsNotNull(repositorioPrestamo.ObtenerLibroPrestadoPorIsbn(libro.Isbn));

        }

        [TestMethod]
        public void PrestarLibroNoDisponibleTest()
        {
            // Arrange
            Libro libro = new LibroTestDataBuilder().ConTitulo(CRONICA_UNA_MUERTE_ANUNCIADA).Build();
            repositorioLibro.Agregar(libro);
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro, repositorioPrestamo);

            // Act
            bibliotecario.Prestar(libro.Isbn,"Juan");
            try
            {
                bibliotecario.Prestar(libro.Isbn, "Juan");
                Assert.Fail();
            }
            catch (Exception err)
            {
                // Assert
                Assert.AreEqual("El libro no se encuentra disponible", err.Message);
            }
        
        }

    }
}
