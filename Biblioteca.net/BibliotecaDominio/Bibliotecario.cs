using BibliotecaDominio.IRepositorio;
using System;
using System.Linq;

namespace BibliotecaDominio
{
    public class Bibliotecario
    {
        public const string EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE = "El libro no se encuentra disponible";
        private IRepositorioLibro libroRepositorio;
        private IRepositorioPrestamo prestamoRepositorio;
        private DateTime? EnteredDate { get; set; }


        public Bibliotecario(IRepositorioLibro libroRepositorio, IRepositorioPrestamo prestamoRepositorio)
        {
            this.libroRepositorio = libroRepositorio;
            this.prestamoRepositorio = prestamoRepositorio;
        }

        public void Prestar(string isbn, string nombreUsuario)
        {
            var response = this.prestamoRepositorio.ObtenerLibroPrestadoPorIsbn(isbn);

            if (response != null)
            {
                throw new Exception(EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE);
            }


            if (!(IsPalindrome(isbn)))
            {
                var libro = this.libroRepositorio.ObtenerPorIsbn(isbn);

                if (isbn.Length > 30)
                {
                    EnteredDate = FechaMaximaEntrega(DateTime.Now);
                }
                else
                {
                    EnteredDate = null;
                }

                Prestamo prestamo = new Prestamo(DateTime.Now, libro, EnteredDate, nombreUsuario);

                this.prestamoRepositorio.Agregar(prestamo);

            }
            else
            {
                throw new Exception(EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE);
            }

        }


        public bool EsPrestado(string isbn)
        {

            bool result = false;

            /* se verifica si fue prestado, ya que por regla de negocio el isbn no se pude prestar mas de una vez.
             * si retorna un objeto, es por que ya fue prestado, de lo contrario no a sido prestado. 
             */
            var response = this.prestamoRepositorio.ObtenerLibroPrestadoPorIsbn(isbn);

            if (response != null)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Operacion que permite validar si una cadena es palindromo
        /// </summary>
        /// <param name="str">parametro de tipos alfanumerico</param>
        /// <returns>retorna valor booleano</returns>
        public bool IsPalindrome(string str)
        {
            string cadena = str.Replace(" ", "").ToLower();
            return cadena.SequenceEqual(cadena.Reverse());
        }

        /// <summary>
        /// Metodo que permite obtener una nueva fecha pasado los 15 dias; +1 si concide con un domingo.
        /// </summary>
        /// <param name="fechaActual">parametro que recibe fecha actual</param>
        /// <returns>Retorna nueva fecha</returns>
        public DateTime FechaMaximaEntrega(DateTime fechaActual)
        {
            DateTime newFecha = new DateTime();

            newFecha = fechaActual.AddDays(15);

            if (newFecha.DayOfWeek == DayOfWeek.Sunday)
            {
                newFecha = newFecha.AddDays(1);
            }

            return newFecha;
        }

    }
}
