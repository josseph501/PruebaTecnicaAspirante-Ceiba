using BibliotecaDominio;
using BibliotecaRepositorio.Contexto;
using BibliotecaRepositorio.Repositorio;
using DominioTest.TestDataBuilders;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DominioTest.Integracion
{

    [TestClass]
    public class BibliotecarioTest
    {
        public const String CRONICA_UNA_MUERTE_ANUNCIADA = "Cronica de una muerte anunciada";
        private BibliotecaContexto contexto;
        private RepositorioLibroEF repositorioLibro;
        private RepositorioPrestamoEF repositorioPrestamo;


        [TestInitialize]
        public void setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BibliotecaContexto>();
            contexto = new BibliotecaContexto(optionsBuilder.Options);
            repositorioLibro = new RepositorioLibroEF(contexto);
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
            bibliotecario.Prestar(libro.Isbn, "Juan");
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


        /// <summary>
        /// unit test donde se prueba que , que no esta disponible el libro por ser un isbn palidromo. esto respetando la regla de negocio.
        /// </summary>
        /// <param name="parameter">parametro que recibe el caso de prueba que en este caso es un numero isbn palindromo</param>
        [DataTestMethod]
        [DataRow("335533")]
        public void PrestarLibroNoDisponiblePorSerPalindromo(string parameter)
        {
            // Arrange
            Libro libro = new LibroTestDataBuilder().ConIsbn(parameter).Build();

            repositorioLibro.Agregar(libro);
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro, repositorioPrestamo);

            // Act
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


        /// <summary>
        /// Uni test que prueba el meto prestar para cuando el numero isbn es superior a 30 digitos, 
        /// por lo que tiene que cumplir un par de condiciones para ue se pueda ejecutar el prestamo segun la regla de negocios.
        /// </summary>
        /// <param name="parameter"> parametro que permite plantar caso de prueba numero isbn mayor a 30 digitos.</param>
        [DataTestMethod]
        [DataRow("11111111111111111222222222222434545")]
        public void PrestarLibroCuandoIsbnEsMayorA30Digitos(string parameter)
        {
            // Arrange
            Libro libro = new LibroTestDataBuilder().ConIsbn(parameter).Build();

            repositorioLibro.Agregar(libro);
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro, repositorioPrestamo);

            // Act

            bibliotecario.Prestar(libro.Isbn, "Juan");

            // Assert
            Assert.AreEqual(bibliotecario.EsPrestado(libro.Isbn), true);
            Assert.IsNotNull(repositorioPrestamo.ObtenerLibroPrestadoPorIsbn(libro.Isbn));


        }
    }
}
