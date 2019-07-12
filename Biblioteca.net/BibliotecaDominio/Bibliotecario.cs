using BibliotecaDominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDominio
{
    public class Bibliotecario
    {
        public const string EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE = "El libro no se encuentra disponible";
        private  IRepositorioLibro libroRepositorio;
        private  IRepositorioPrestamo prestamoRepositorio;

        public Bibliotecario(IRepositorioLibro libroRepositorio, IRepositorioPrestamo prestamoRepositorio)
        {
            this.libroRepositorio = libroRepositorio;
            this.prestamoRepositorio = prestamoRepositorio;
        }

        public void Prestar(string isbn, string nombreUsuario)
        {
            throw new Exception("se debe implementar este método");
        }


        public bool EsPrestado(string isbn)
        {
            return false;
        }
    }
}
