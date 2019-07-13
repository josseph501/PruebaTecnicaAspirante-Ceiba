using BibliotecaDominio;
using BibliotecaDominio.IRepositorio;
using DominioTest.TestDataBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DominioTest.Unitarias
{
    [TestClass]
    public class BibliotecarioTest
    {
        public BibliotecarioTest()
        {

        }
        public Mock<IRepositorioLibro> repositorioLibro;
        public Mock<IRepositorioPrestamo> repositorioPrestamo;

        [TestInitialize]
        public void setup()
        {
            repositorioLibro = new Mock<IRepositorioLibro>();
            repositorioPrestamo = new Mock<IRepositorioPrestamo>();
        }

        [TestMethod]

        public void EsPrestado()
        {
            // Arrange
            var libroTestDataBuilder = new LibroTestDataBuilder();
            Libro libro = libroTestDataBuilder.Build();

            repositorioPrestamo.Setup(r => r.ObtenerLibroPrestadoPorIsbn(libro.Isbn)).Returns(libro);

            // Act
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);
            var esprestado = bibliotecario.EsPrestado(libro.Isbn);

            // Assert
            Assert.AreEqual(esprestado, true);
        }

        [TestMethod]
        public void LibroNoPrestadoTest()
        {
            // Arrange
            var libroTestDataBuilder = new LibroTestDataBuilder();
            Libro libro = libroTestDataBuilder.Build();
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);
            repositorioPrestamo.Setup(r => r.ObtenerLibroPrestadoPorIsbn(libro.Isbn)).Equals(null);

            // Act
            var esprestado = bibliotecario.EsPrestado(libro.Isbn);

            // Assert
            Assert.IsFalse(esprestado);
        }

        /// <summary>
        ///   para  prueba unit test del metodo EsPalindromo, se realiza dos casos de pruebas donde el primer caso se valida que 
        ///   es un isbn palindromo y el segundo no lo es.
        /// </summary>
        /// <param name="parameter"> parametro en donde se pone el caso de prueba</param>
        /// <param name="response"> parametro donde se pone el resultado que se deve evaluar para comprobar si fue efectiva la prueba.</param>
        [DataTestMethod]
        [DataRow("1221", true)]
        [DataRow("1234", false)]
        public void EsPalindromo(string parameter, bool response)
        {
            // Arrange
            var libroTestDataBuilder = new LibroTestDataBuilder();
            Libro libro = libroTestDataBuilder.Build();
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);

            // Act
            var result = bibliotecario.IsPalindrome(parameter);

            // Assert
            Assert.AreEqual(result, response);

        }


        /// <summary>
        /// Para la prueba Unit Test del metodo ObtenerFechaMaxima, se realizan dos casos de prueba donde el primer caso corresponde cuando la fecha es ingresada
        /// y se evalua que se obtiene laa fecha de 15 despues. el otro caso de prueba evalua la fecha ingresada y que 15 dias despues coninside con el dia de la semana domingo por lo
        /// que se adiciona un dia mas por regla de negocio.
        /// </summary>
        /// <param name="DateTest"></param>
        /// <param name="DateFinally"></param>
        [DataTestMethod]
        [DataRow("17/07/2019", "01/08/2019")]
        [DataRow("13/07/2019", "29/07/2019")]
        public void ObtenerFechaMaxima(string DateTest, string DateFinally)
        {
            // Arrange
            DateTime dateFirst = DateTime.Parse(DateTest);
            DateTime dateMax = DateTime.Parse(DateFinally);

            var libroTestDataBuilder = new LibroTestDataBuilder();
            Libro libro = libroTestDataBuilder.Build();
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);

            // Act
            var result = bibliotecario.FechaMaximaEntrega(dateFirst);

            // Assert
            Assert.AreEqual(result.ToString("MM/dd/yyyy"), dateMax.ToString("MM/dd/yyyy"));

        }

    }
}
