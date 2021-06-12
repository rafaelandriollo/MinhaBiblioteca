using System;
using System.Collections.Generic;
using System.Text;

namespace MinhaBiblioteca
{
    class Livro
    {
        //Atributos
        private int id;
        private string titulo;
        private string autor;
        private string assunto;
        private string editora;

        //Construtor sem parâmetros
        public Livro()
        {
        }

        //Construtor com parâmetros
        public Livro(int id, string titulo, string autor, string assunto, string editora)
        {
            this.id = id;
            this.titulo = titulo;
            this.autor = autor;
            this.assunto = assunto;
            this.editora = editora;
        }

        public int Id { get => id; set => id = value; }

        public string Titulo { get => titulo; set => titulo = value; }

        public string Autor { get => autor; set => autor = value; }

        public string Assunto { get => assunto; set => assunto = value; }

        public string Editora { get => editora; set => editora = value; }

        public override string ToString()
        {
            return (String.Format("Id: {0} - Titulo: {1} - Autor: {2} - Assunto: {3} - Editora: {4}", id, titulo, autor, assunto, editora));
        }
    }
}
