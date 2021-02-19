using System;
using Dapper;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using System.Linq;

namespace SantaSHOP
{
    // Tabela crianças sem Foreign Key
    [Table("criancas")]
    class crianca
    {
        [Key]
        public int crianca_id { get; set; }
        public int idade { get; set; }
        public string nome { get; set; }
    }
    // Tabela crianças com Foreign Key Presente
    [Table("criancas")]
    class criancawithfkpresente
    {
        [Key]
        public int crianca_id { get; set; }
        public int presente_id { get; set; }

    }
    
    // Tabela crianças com Foreign Key Comportamento
    [Table("criancas")]
    class criancawithfkcomportamento
    {
        [Key]
        public int crianca_id { get; set; }
        public int comportamento_id { get; set; }

    }
    // Tabela crianças com todas as colunas
    [Table("criancas")]
    class criancall
    {
        [Key]
        public int crianca_id { get; set; }
        public int idade { get; set; }
        public string nome { get; set; }
        public int presente_id { get; set; }
        public int comportamento_id { get; set; }

    }
    // Tabela Presentes
    [Table("presentes")]
    class presentes
    {
        [Key]
        public int presente_id { get; set; }
        public int quantidade { get; set; }
        public string nome { get; set; }

    }
    //Tabela Comportamento
    [Table("comportamento")]
    class comportamento
    {
        [Key]
        public int comportamento_id { get; set; }
        public string descricao { get; set; }
        public bool condicao { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("A receber dados ...");

            //Conection Database
            string connString = @"Data Source=localhost;Initial Catalog=santa_shop;Persist Security Info=True;User ID=root;Password=";

            //create instanace of database connection
            MySqlConnection connection = new MySqlConnection(connString);

            try
            {
                Console.WriteLine("A carregar dados ...");

                //open connection
                connection.Open();

                Console.WriteLine("Conexão bem sucedida!");
                Console.ReadKey();

            Start:
                int OpcaoMenu = 0;
                int OpcaoMenuPresente = 0;
                int OpcaoMenuCiranca = 0;
                int OpcaoComportamento = 0;

                Console.Clear();
                Console.WriteLine("! MENU ! \n 1- Crianças\n 2- Presentes\n 3- Comportamento\n 4- Voltar");

                try
                {
                    OpcaoMenu = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Clear();
                    Console.WriteLine("Caractere/s não esperados... Tente novamente!\nClica ENTER para continuar...");
                    Console.ReadKey();
                    goto Start;
                }

                switch (OpcaoMenu)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("! Crianças ! \n 1- Mostrar Crianças\n 2- Criar Crianças\n 3- Remover Crianças\n 4- Sair");
                        OpcaoMenuCiranca = Convert.ToInt32(Console.ReadLine());

                        switch (OpcaoMenuCiranca)
                        {
                            // Kids Menu
                            case 1:
                                try
                                {
                                    Console.Clear();
                                    var criancaLista = connection.GetAll<criancall>().ToList();
                                    criancaLista.ForEach(i => Console.Write("\n\n\nID: " + i.crianca_id + "\nnome: " + i.nome + "\nIdade: " + i.idade + "\nID do Presente:" + i.presente_id + "\nID do Comportamento:" + i.comportamento_id));

                                    Console.WriteLine("\n\nLista Completa\n Carrega ENTER para continuar ...");
                                    Console.ReadKey();

                                    goto Start;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Erro: " + e.Message);
                                }
                                break;
                            case 2:
                                Console.WriteLine("Qual o nome da Criança:");
                                string nomeCrianca = Convert.ToString(Console.ReadLine());

                                Console.WriteLine("Qual a idade de " + nomeCrianca + "?");
                                int idadeCrianca = Convert.ToInt32(Console.ReadLine());

                                try
                                {
                                    var addCrianca = new crianca
                                    {
                                        idade = idadeCrianca,
                                        nome = nomeCrianca
                                    };

                                    connection.Insert<crianca>(addCrianca);
                                    Console.WriteLine("\nParabéns!" + nomeCrianca + " foi adicionado/a com sucesso! Clica numa tecla para continuar...");
                                    Console.ReadKey();

                                    goto Start;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Erro: " + e.Message);
                                }
                                break;
                            case 3:
                                var criancaListaM = connection.GetAll<crianca>().ToList();
                                criancaListaM.ForEach(i => Console.Write("\nID: " + i.crianca_id + "\nNome: " + i.nome + "\nIdade: " + i.idade + "\n ---"));

                                Console.WriteLine("\n\n Digite o ID da Criança que pretende remover: ");
                                int idMatar = Convert.ToInt32(Console.ReadLine());

                                connection.Delete(new crianca { crianca_id = idMatar });
                                Console.WriteLine("\nCriança removida com sucesso! \n Clique numa tecla para continuar...");
                                Console.ReadKey();

                                goto Start;
                        }
                        break;

                    case 2:
                        // Gift Menu

                        Console.Clear();
                        Console.WriteLine("! Presente !\n 1- Dar Presentes\n 2- Criar Presente\n 3- Remover Presente\n 4- Voltar");
                        OpcaoMenuPresente = Convert.ToInt32(Console.ReadLine());

                        switch (OpcaoMenuPresente)
                        {
                            case 1:
                                try
                                {
                                    Console.WriteLine("\nLista de Crianças\n");
                                    var listaCriancas = connection.GetAll<crianca>().ToList();
                                    listaCriancas.ForEach(i => Console.Write("\nID: " + i.crianca_id + "\nnome: " + i.nome + "\nIdade: " + i.idade + "\n ---"));

                                    Console.WriteLine("\nLista de Presentes\n");
                                    var criancaLista = connection.GetAll<presentes>().ToList();
                                    criancaLista.ForEach(i => Console.Write("\n ID: " + i.presente_id + "\n Nome: " + i.nome + "\n Quantidade: " + i.quantidade));

                                    Console.WriteLine("\nSelecione o ID da Criança: ");
                                    int idCrianca = Convert.ToInt32(Console.ReadLine());

                                    Console.WriteLine("\nSelecione o ID do presente: ");
                                    int idAdicionarpresente = Convert.ToInt32(Console.ReadLine());

                                    //Add gift to child

                                    var presenteLista = connection.GetAll<presentes>().ToList();

                                    var addPresentecrianca = new criancawithfkpresente
                                    {
                                        crianca_id = idCrianca,
                                        presente_id = idAdicionarpresente
                                    };


                                    connection.Update<criancawithfkpresente>(addPresentecrianca);
                                    Console.WriteLine("\nPresente foi adicionado à Criança!\n\n\n Clica numa tecla para continuar...");

                                    Console.ReadKey();

                                    goto Start;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Erro: " + e.Message);
                                }

                                break;
                            case 2:
                                Console.WriteLine("Qual o nome do presente?");
                                string nome = Convert.ToString(Console.ReadLine());

                                Console.WriteLine("Qual a quantidade de " + nome + "s que pretende adicionar ?");
                                int quantidade = Convert.ToInt32(Console.ReadLine());

                                try
                                {
                                    var addPresente = new presentes
                                    {
                                        quantidade = quantidade,
                                        nome = nome
                                    };

                                    connection.Insert(addPresente);

                                    Console.WriteLine("\n Parabéns! " + nome + " foi adicionado\n Carrega ENTER para continuar ...");
                                    Console.ReadKey();

                                    goto Start;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Error: " + e.Message);
                                }

                                break;
                            case 3:

                                goto Start;
                        }
                        break;
                    case 3:
                        //Behavior Menu
                        Console.Clear();
                        Console.WriteLine("! Comportamento !\n 1- Dar/Atualizar Comportamento\n 2 - Voltar");
                        OpcaoComportamento = Convert.ToInt32(Console.ReadLine());

                        switch (OpcaoComportamento)
                        {
                            case 1:
                                try
                                {
                                    Console.Clear();

                                    Console.WriteLine("Adicionar um comportamento a uma criança!");

                                    Console.WriteLine("\nLista de Crianças\n");
                                    var listaCriancas = connection.GetAll<criancall>().ToList();
                                    listaCriancas.ForEach(i => Console.Write("\nID: " + i.crianca_id + "\nnome: " + i.nome + "\nIdade: " + i.idade + "\n Comportamento: " + i.comportamento_id + "\n"));

                                    Console.WriteLine("\nLista de Comportamentos\n");
                                    var listaComportamentos = connection.GetAll<comportamento>().ToList();
                                    listaComportamentos.ForEach(i => Console.Write("\nComportamento: " + i.comportamento_id + "\n" + "\n Condição: " + i.condicao + "\n Descrição: " + i.descricao));

                                    Console.WriteLine("\nNão recebe é selecionado por DEFAULT\n");

                                    Console.WriteLine("\n\n\nSelecione o ID da Criança: ");
                                    int idCrianca = Convert.ToInt32(Console.ReadLine());

                                    Console.WriteLine("\n\nSelecione o ID do comportamento: ");
                                    int idAdicionarcomportamento = Convert.ToInt32(Console.ReadLine());

                                    //Add behavior to child

                                    var Listacomportamento = connection.GetAll<comportamento>().ToList();

                                    var addComportamentocrianca = new criancawithfkcomportamento
                                    {
                                        crianca_id = idCrianca,
                                        comportamento_id = idAdicionarcomportamento
                                    };


                                    connection.Update<criancawithfkcomportamento>(addComportamentocrianca);
                                    Console.WriteLine("\nComportamento foi adicionado à Criança!\n\n\n Clica numa tecla para continuar...");

                                    Console.ReadKey();

                                    goto Start;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Erro: " + e.Message);
                                }

                                Console.ReadKey();

                                goto Start;

                            case 2:
                                goto Start;
                        }
                        goto Start;
                    default: goto Start;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }
        }
    }
}