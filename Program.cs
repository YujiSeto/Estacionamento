using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

/*
Uma empresa gerenciadora de estacionamento decidiu inovar e criou um sistema com semáforos para o gerenciamento das vagas de seu estacionamento.
A ideia consiste em indicar para o usuário que chega na cancela em qual vaga ele deve estacionar.
Se não houver vagas o sistema já vai avisar antes do usuário entrar no estacionamento.
Faça a implementação desse sistema utilizando programação multithread e mecanismos de sincronização
* Cada thread deve gerenciar uma das N cancelas do estacionamento, que podem executar as funções de entrada e saída de veículos
* Seu sistema não pode permitir que dois carros estacionem na mesma vaga e nem que um carro entre em um estacionamento lotado
* Sua equipe deve discutir como implementar a parte interativa das cancelas
 */

namespace Estacionamento
{
    public class Program
    {
        private static Semaphore Semaforo;

        public static void Main(string[] args)
        {
            //Criação de Variáveis
            string opcao = "0";             // Navegação no Menu
            int qtdcancelas = 0;            // Quantidade de Cancelas
            int qtdvagas = 0;               // Quantidade de Vagas Total
            int qtdvagasdisponiveis = 0;    // Quantidade de Vagas Disponível
            int qtdvagasocupadas = 0;       // Quantidade de Vagas Ocupadas
            int qtdveiculos = 0;            // Quantidade de Veículos Entrando ou Saindo
            int i;                          // Controle de FOR
            int numeroveiculo = 1;          // Número do Veículo Entrando ou Saindo
            int numerovaga = 0;             // Número da Vaga
            int tempodeespera = 000;        // Tempo de Espera para Entrada e Saída de Veículos
            Semaforo = new Semaphore(1, 1); // Configuração do Semaforo que permite uma Thread por vez

            do
            {
                if (qtdvagas == 0) // Proteção para não acessar a Tela de Configuração Novamente
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("|       CONFIGURAÇÃO ESTACIONAMENTO        |");
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("");
                        Console.WriteLine("Digite o número de Vagas que o Estacionamento possui:");
                        qtdvagas = Convert.ToInt32(Console.ReadLine()); //Cadastrar a quantidade Total de Vagas do Estacionamento

                        if (qtdvagas <= 0)
                        {
                            Console.WriteLine("ERRO!!!");
                            Console.WriteLine("O número de vagas não pode ser 0 ou menor que 0.");
                            Console.ReadKey();
                        }

                    } while (qtdvagas <= 0);

                    qtdvagasdisponiveis = qtdvagas; //Inicializar a quantidade de Vagas Disponíveis do Estacionamento

                    do
                    {
                        Console.Clear();
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("|       CONFIGURAÇÃO ESTACIONAMENTO        |");
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("");
                        Console.WriteLine("Digite o número de Cancelas:");
                        qtdcancelas = Convert.ToInt32(Console.ReadLine()); //Cadastrar a quantidade Total de Cancelas do Estacionamento

                        if (qtdcancelas <= 0)
                        {
                            Console.WriteLine("ERRO!!!");
                            Console.WriteLine("O número de cancelas não pode ser 0 ou menor que 0.");
                            Console.ReadKey();
                        }

                    } while (qtdcancelas <= 0);
                }

                //Menu Principal
                Console.Clear();
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("|       ESTACIONAMENTO                     |");
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("  Quantidade Total de Vagas : {0}", qtdvagas);
                Console.WriteLine("  Quantidade de Cancelas    : {0}", qtdcancelas);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("  Quantidade de Vagas Disponíveis : {0}", qtdvagasdisponiveis);
                Console.WriteLine("  Quantidade de Vagas Ocupadas : {0}", qtdvagasocupadas);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("+------------------------------------------+");
                Console.WriteLine("|       Selecione uma opção                |");
                Console.WriteLine("|  1  - Entrada de Veículos                |");
                Console.WriteLine("|  2  - Saída de Veículos                  |");
                Console.WriteLine("|  0  - Sair do Programa                   |");
                Console.WriteLine("+------------------------------------------+");

                opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    if (qtdvagasdisponiveis == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("|       ESTACIONAMENTO LOTADO              |");
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("");
                    }
                    else
                    {
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("+------------------------------------------+");
                            Console.WriteLine("|       ENTRADA DE VEÍCULOS                |");
                            Console.WriteLine("+------------------------------------------+");
                            Console.WriteLine("");
                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("  Quantidade de Cancelas          : {0}", qtdcancelas);
                            Console.WriteLine("  Quantidade de Vagas Disponíveis : {0}", qtdvagasdisponiveis);
                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("");
                            Console.WriteLine("Digite o número de Veículos Entrando:");
                            qtdveiculos = Convert.ToInt32(Console.ReadLine()); //Quantidade de Veículos Entrando

                            if (qtdveiculos > qtdcancelas)
                            {
                                Console.WriteLine("ERRO!!!");
                                Console.WriteLine("O número de Veiculos Entrando não pode ser maior que número de Cancelas.");
                                Console.ReadKey();
                            }
                            if (qtdveiculos > qtdvagasdisponiveis)
                            {
                                Console.WriteLine("ERRO!!!");
                                Console.WriteLine("O número de Veiculos Entrando não pode ser maior que número de Vagas Disponíveis.");
                                Console.ReadKey();
                            }

                        } while (qtdveiculos > qtdcancelas || qtdveiculos > qtdvagasdisponiveis);

                        numeroveiculo = 1; //Reset do Número do Veículo

                        for (i = 0; i < qtdveiculos; i++)
                        {
                            Thread t = new Thread(new ThreadStart(OcuparVaga)); //Criação da Thread que chama a função Ocupar Vaga
                            t.Name = numeroveiculo + "º Veículo"; //Nome com o número do Veículo Entrando para Controle da Thread
                            numeroveiculo++; //Aumentar a númeração do Veículo
                            t.Start();
                        }
                    }
                    Console.ReadKey();

                }
                if (opcao == "2")
                {
                    if (qtdvagasocupadas == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("|       ESTACIONAMENTO VAZIO               |");
                        Console.WriteLine("+------------------------------------------+");
                        Console.WriteLine("");
                    }
                    else
                    {
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("+------------------------------------------+");
                            Console.WriteLine("|       SAÍDA DE VEÍCULOS                  |");
                            Console.WriteLine("+------------------------------------------+");
                            Console.WriteLine("");
                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("  Quantidade de Cancelas       : {0}", qtdcancelas);
                            Console.WriteLine("  Quantidade de Vagas Ocupadas : {0}", qtdvagasocupadas);
                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("");
                            Console.WriteLine("Digite o número de veículos saindo:");
                            qtdveiculos = Convert.ToInt32(Console.ReadLine()); //Quantidade de Veículos Saindo

                            if (qtdveiculos > qtdcancelas)
                            {
                                Console.WriteLine("ERRO!!!");
                                Console.WriteLine("O número de veiculos saindo não pode ser maior que número de Cancelas.");
                                Console.ReadKey();
                            }
                            if (qtdveiculos > qtdvagasocupadas)
                            {
                                Console.WriteLine("ERRO!!!");
                                Console.WriteLine("O número de veiculos saindo não pode ser maior que número de Vagas Ocupadas.");
                                Console.ReadKey();
                            }

                        } while (qtdveiculos > qtdcancelas || qtdveiculos > qtdvagasocupadas);

                        numeroveiculo = 1; //Reset do Número do Veículo

                        for (i = 0; i < qtdveiculos; i++)
                        {
                            Thread t = new Thread(new ThreadStart(LiberarVaga)); //Criação da Thread que chama a função Liberar Vaga
                            t.Name = numeroveiculo + "º Veículo"; //Nome com o número do Veículo Saindo para Controle da Thread
                            numeroveiculo++; //Aumentar a númeração do Veículo
                            t.Start();
                        }
                    }
                    Console.ReadKey();
                }
            } while (opcao != "0"); //Sair
            Console.Clear();
            Console.WriteLine("Programa Finalizado");
            Console.ReadKey();

            void OcuparVaga()
            {
                Semaforo.WaitOne(); //Inicio do Semaforo que permite apenas uma Thread por vez
                Thread.Sleep(tempodeespera); //Espera de 0,5 Segundos
                qtdvagasdisponiveis--; //Diminuir a quantidade de Vagas Disponíveis
                qtdvagasocupadas++; //Aumentar a quantidade de Vagas Ocupadas
                numerovaga++; //Avançar para a próxima Vaga Livre
                Console.WriteLine("O " + System.Threading.Thread.CurrentThread.Name + " preencheu a vaga nº" + numerovaga); //Descrição da Thread que está atuando
                Semaforo.Release(); //Fim do Semáforo
            }

            void LiberarVaga()
            {
                Semaforo.WaitOne(); //Inicio do Semaforo que permite apenas uma Thread por vez
                Thread.Sleep(tempodeespera); //Espera de 0,5 Segundos
                Console.WriteLine("O " + System.Threading.Thread.CurrentThread.Name + " liberou a vaga nº" + numerovaga); //Descrição da Thread que está atuando
                qtdvagasdisponiveis++; //Aumentar a quantidade de Vagas Disponíveis
                qtdvagasocupadas--; //Diminuir a quantidade de Vagas Ocupadas
                numerovaga--; //Liberar Vaga e retornar para a Vaga Anterior
                Semaforo.Release(); //Fim do Semáforo
            }

        }
    }
}