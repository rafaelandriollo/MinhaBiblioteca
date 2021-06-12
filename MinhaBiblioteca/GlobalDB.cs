using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace MinhaBiblioteca
{
    class GlobalDB
    {
        //Responsável pela conexão com banco de dados
        public static MySqlConnection conexao;

        //Responsável pelas instruções à serem executadas
        public static MySqlCommand comando;

        //Responsável por inserir dados em uma DataTable
        //public static MySqlDataAdapter adaptador;

        //Responsavel por ligar o Banco e controles com a propriedade DatSource
        //public static DataTable dataTabela;

        public static MySqlDataReader resultado;

        //public static MySqlConnectionStringBuilder b;

        public static void conectar()
        {
            /*b = new MySqlConnectionStringBuilder();
            b.Server = "localhost";
            b.Port = 3306;
            //b.Database = "testdb";
            b.UserID = "root";
            b.Password = "root";
            b.CharacterSet = "utf8";
            var connstr = b.ToString();
            conexao = new MySqlConnection(connstr);
            */
            //Estabelece os parâmetros para conexão com o BD
            conexao = new MySqlConnection("server=localhost;uid=root;pwd=root;");

            //Abre a conexão com o BD
            conexao.Open();

            //Informa a instrução SQL
            comando = new MySqlCommand("CREATE DATABASE IF NOT EXISTS dbbibli; use dbbibli;", conexao);

            //Executa a instrução no MySQL (workbench)
            comando.ExecuteNonQuery();

            //Informa nova instrução SQL
            comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS livro " +
                                        "(id integer auto_increment primary key, " +
                                        "titulo varchar(50), " +
                                        "autor varchar(50), " +
                                        "assunto varchar(20), " +
                                        "editora varchar(20)); ", conexao);

            //Executa a instrução 
            comando.ExecuteNonQuery();

            //Fecha a conexão com o Banco de Dados
            conexao.Close();

        }

        public static void cadastrar(string titulo, string autor, string assunto, string editora)
        {
            try
            {
                conexao.Open();

                //Cria o comando SQL
                comando = new MySqlCommand("INSERT INTO livro (titulo, autor, assunto, editora) values ('" +
                                        titulo + "','" + autor + "','" + assunto + "','"+ editora + "');", conexao);

                //Executa o comando SQL
                comando.ExecuteNonQuery();
                MessageBox.Show("Cadastro realizado com sucesso!", "Cadastro OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception erro)
            {
                MessageBox.Show("ERRO ==> " + erro.Message, "Falha no Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                //Fecha a conexão com o Banco de Dados
                conexao.Close();
            }
        }

        public static string ultimoId()
        {
            string id = "0";

            try
            {
                conexao.Open();

                comando = new MySqlCommand("SELECT MAX(id) AS maior FROM livro;", conexao);

                resultado = comando.ExecuteReader();

                if (resultado.HasRows)
                {
                    resultado.Read();
                    id = resultado["maior"].ToString();
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("ERRO ==> " + erro.Message, "Falha na recuperação do último ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!resultado.IsClosed)
                    resultado.Close();
             
                conexao.Close();
            }
            return id;
        }

        public static List<Livro> consultar(string nome)
        {
            Livro l = new Livro();
            List<Livro> listaLivro = new List<Livro>();

            try
            {
                conexao.Open();

                comando = new MySqlCommand("SELECT * FROM livro where titulo like '%" + nome + "%' order by id;", conexao);

                MySqlDataReader resultado = comando.ExecuteReader();

                // if (resultado.HasRows)
                // {
                //while (resultado.Read())
                //  do
                //  {
                while (resultado.Read())
                {
                    //Console.WriteLine();
                    //string r = resultado.GetInt32(0) + "\n" + resultado.GetString(1) + "\n" + resultado.GetString(2) + "\n" + resultado.GetString(3);
                    //MessageBox.Show(r, "Consulta dentro Global", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //  p.Id = Convert.ToInt32(resultado["id"].ToString());
                    //  p.Nome = resultado["nome"].ToString();
                    //  p.Email = resultado["email"].ToString();
                    //    p.Telefone = resultado["telefone"].ToString();
                    //                    MessageBox.Show(p.ToString(), "Consulta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listaLivro.Add(new Livro(resultado.GetInt32(0), resultado.GetString(1), resultado.GetString(2), resultado.GetString(3), resultado.GetString(4)));
                   // MessageBox.Show(listaPessoas.ToString(), "Consulta dentro Global", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // listaPessoas.Add(p);
                }

                    //} while (resultado.);
               /* }
                else
                {
                    MessageBox.Show("Nome não encontrado!", "Consulta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }*/
            }
            catch (Exception erro)
            {
                MessageBox.Show("ERRO ==> " + erro.Message, "Falha na consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!resultado.IsClosed)
                    resultado.Close();

                conexao.Close();
            }

          /*  foreach (var x in listaPessoas)
            {
                MessageBox.Show(x.ToString(), "Consulta dentro da Global", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
                return listaLivro;
        }

        public static void alterar(Livro l)
        {
            try
            {
                conexao.Open();

                comando = new MySqlCommand("SELECT * FROM livro where id=" + l.Id + ";", conexao);

                resultado = comando.ExecuteReader();

                if (resultado.HasRows)
                {
                    resultado.Read();
                    string instrucao = "UPDATE livro set ";
                    instrucao += "titulo = '" + l.Titulo + "',";
                    instrucao += "autor = '" + l.Autor + "',";
                    instrucao += "assunto = '" + l.Assunto + "',";
                    instrucao += "editora = '" + l.Editora + "' WHERE id=" + l.Id + ";";

                    if (!resultado.IsClosed) 
                        resultado.Close();

                    comando = new MySqlCommand(instrucao, conexao);

                    comando.ExecuteNonQuery();
                    
                    MessageBox.Show("Alteração realizada com sucesso!", "Alteração", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Registro não encontrado", "Falha na Alteração", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("ERRO ==> " + erro.Message, "Falha na alteração", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!resultado.IsClosed)
                    resultado.Close();

                conexao.Close();
            }
        }

        public static void excluir(string id)
        {
            try
            {
                conexao.Open();

                comando = new MySqlCommand("SELECT * FROM livro where id=" + id + ";", conexao);

                MySqlDataReader resultado = comando.ExecuteReader();

                if (resultado.HasRows)
                {
                    resultado.Read();
                    string instrucao = "DELETE FROM livro WHERE id=" + id + ";";

                    if (!resultado.IsClosed)
                        resultado.Close();

                    comando = new MySqlCommand(instrucao, conexao);

                    comando.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Registro não encontrado!", "Falha na exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }catch(Exception erro)
            {
                MessageBox.Show("ERRO ==> " + erro.Message, "Falha na exclusão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!resultado.IsClosed)
                    resultado.Close();

                conexao.Close();
            }
        }

        public static string listar()
        {
            string listagem = "Lista vazia!";
            try
            {
                conexao.Open();

                comando = new MySqlCommand("SELECT * FROM livro ORDER BY titulo;", conexao);

                MySqlDataReader resultado = comando.ExecuteReader();

                Livro l = new Livro();

                if (resultado.HasRows)
                {
                    listagem = "Listagem de Cadastros\n\n";
                    while (resultado.Read())
                    {
                        l.Id = Convert.ToInt32(resultado["id"].ToString());
                        l.Titulo = resultado["titulo"].ToString();
                        l.Autor = resultado["autor"].ToString();
                        l.Assunto = resultado["assunto"].ToString();
                        l.Editora = resultado["editora"].ToString();
                        listagem += l.ToString() + '\n';
                    }
                }
            }catch(Exception erro)
            {
                MessageBox.Show("ERRO ==> " + erro.Message, "Falha na listagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!resultado.IsClosed)
                    resultado.Close();

                conexao.Close();
            }

            return listagem;

        }
    }
}


//https://www.youtube.com/watch?v=UgzRaOXOZmY