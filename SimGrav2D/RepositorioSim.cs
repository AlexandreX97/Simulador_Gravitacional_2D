using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace SimGrav2D
{
    public class RepositorioSim
    {
        private readonly ConexaoBanco _conexao;

        public RepositorioSim(ConexaoBanco conexao)
        {
            _conexao = conexao; // guarda a conexão para usar nas funções
        }

        // salva o resultado de uma interação e todos os corpos no banco
        // numSimulacao = código da simulação
        // numInteracao = número da interação dentro da simulação
        // corpos = array com todos os corpos dessa interação
        public void SalvarSimTransaction(int numSimulacao, int numInteracao, Corpo[] corpos)
        {
            using var conn = _conexao.CriarConexao();
            using var trans = conn.BeginTransaction(); // inicia a transação

            try
            {
                // salva na tabela resultados
                using var cmdRes = conn.CreateCommand();
                cmdRes.Transaction = trans;
                cmdRes.CommandText = "INSERT INTO resultados (NumSimulacao, NumInteracao) VALUES (@_NumSimulacao, @_NumInteracao)";
                cmdRes.Parameters.AddWithValue("@_NumSimulacao", numSimulacao);
                cmdRes.Parameters.AddWithValue("@_NumInteracao", numInteracao);
                cmdRes.ExecuteNonQuery(); // executa insert

                // salva cada corpo
                using var cmdCorpo = conn.CreateCommand();
                cmdCorpo.Transaction = trans;
                cmdCorpo.CommandText = @"
                    INSERT INTO corpos 
                    (NumSimulacao, NumInteracao, IdCorpo, NomeCorpo, MassaCorpo, PosX, PosY, VelX, VelY, DensidadeCorpo)
                    VALUES (@_NumSimulacao, @_NumInteracao, @IdCorpo, @NomeCorpo, @MassaCorpo, @PosX, @PosY, @VelX, @VelY, @DensidadeCorpo)";

                for (int i = 0; i < corpos.Length; i++)
                {
                    var c = corpos[i];
                    cmdCorpo.Parameters.Clear();
                    cmdCorpo.Parameters.AddWithValue("@_NumSimulacao", numSimulacao);
                    cmdCorpo.Parameters.AddWithValue("@_NumInteracao", numInteracao);
                    cmdCorpo.Parameters.AddWithValue("@IdCorpo", i + 1);
                    cmdCorpo.Parameters.AddWithValue("@NomeCorpo", c.Nome);
                    cmdCorpo.Parameters.AddWithValue("@MassaCorpo", c.Massa);
                    cmdCorpo.Parameters.AddWithValue("@PosX", c.PosX);
                    cmdCorpo.Parameters.AddWithValue("@PosY", c.PosY);
                    cmdCorpo.Parameters.AddWithValue("@VelX", c.VelX);
                    cmdCorpo.Parameters.AddWithValue("@VelY", c.VelY);
                    cmdCorpo.Parameters.AddWithValue("@DensidadeCorpo", c.Densidade);
                    cmdCorpo.ExecuteNonQuery(); // salva no banco
                }

                trans.Commit(); // confirma tudo
            }
            catch
            {
                trans.Rollback(); // desfaz tudo se deu erro
                throw;
            }
        }

        // salva uma simulação nova
        // data = data da simulação
        // qtdCorpos = quantidade inicial de corpos
        // numIteracoes = total de interações limite
        // tempoInteracoes = intervalo entre interações
        // retorna o id da simulação criada
        public int SalvarSimulacao(DateTime data, int qtdCorpos, int numIteracoes, int tempoInteracoes)
        {
            using var conn = _conexao.CriarConexao();
            string sql = @"INSERT INTO simulacao (DataSimulacao, QtdCorposInicial, NumInteracoes, TempoInteracoes) 
                           VALUES (@DataSimulacao, @QtdCorposInicial, @NumInteracoes, @TempoInteracoes)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@DataSimulacao", data.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@QtdCorposInicial", qtdCorpos);
            cmd.Parameters.AddWithValue("@NumInteracoes", numIteracoes);
            cmd.Parameters.AddWithValue("@TempoInteracoes", tempoInteracoes);
            cmd.ExecuteNonQuery();

            return (int)cmd.LastInsertedId; // retorna o id
        }

        // carrega todos os frames de uma simulação
        // numSimulacao = código da simulação
        // essa versão carrega tudo de uma vez e agrupa por numInteracao para não travar
        public List<Corpo[]> CarregarTodosFrames(int numSimulacao)
        {
            var resultados = new List<Corpo[]>();

            using var conn = _conexao.CriarConexao();

            // faz uma query só para pegar todos os corpos da simulação
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT NumInteracao, NomeCorpo, MassaCorpo, DensidadeCorpo, PosX, PosY, VelX, VelY
                FROM corpos
                WHERE NumSimulacao=@_NumSimulacao
                ORDER BY NumInteracao, IdCorpo";
            cmd.Parameters.AddWithValue("@_NumSimulacao", numSimulacao);

            // cria um dicionário para agrupar por numInteracao
            var dict = new Dictionary<int, List<Corpo>>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int numInt = reader.GetInt32("NumInteracao");

                if (!dict.ContainsKey(numInt))
                    dict[numInt] = new List<Corpo>();

                dict[numInt].Add(new Corpo(
                    reader.GetString("NomeCorpo"),
                    reader.GetDouble("MassaCorpo"),
                    reader.GetDouble("DensidadeCorpo"),
                    reader.GetDouble("PosX"),
                    reader.GetDouble("PosY"),
                    reader.GetDouble("VelX"),
                    reader.GetDouble("VelY")
                ));
            }

            // adiciona cada interação na lista final
            foreach (var kv in dict)
                resultados.Add(kv.Value.ToArray());

            return resultados;
        }

        // retorna o total de interações salvas de uma simulação
        public int ObterTotalInteracoes(int numSimulacao)
        {
            using var conn = _conexao.CriarConexao();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM resultados WHERE NumSimulacao=@_NumSimulacao";
            cmd.Parameters.AddWithValue("@_NumSimulacao", numSimulacao);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // carrega a simulação completa com o último frame
        // numSimulacao = código da simulação
        public Simulacao CarregarSimulacao(int numSimulacao)
        {
            var simulacao = new Simulacao();

            // pega os dados da simulação
            using (var conn = _conexao.CriarConexao())
            {
                using var cmdSim = conn.CreateCommand();
                cmdSim.CommandText = @"
                    SELECT NumSimulacao, DataSimulacao, QtdCorposInicial, NumInteracoes, TempoInteracoes 
                    FROM simulacao 
                    WHERE NumSimulacao=@_NumSimulacao";
                cmdSim.Parameters.AddWithValue("@_NumSimulacao", numSimulacao);

                using var readerSim = cmdSim.ExecuteReader();
                if (!readerSim.Read()) return null;

                simulacao.NumSimulacao = readerSim.GetInt32("NumSimulacao");
                simulacao.DataSimulacao = readerSim.GetString("DataSimulacao");
                simulacao.QtdCorposInicial = readerSim.GetInt32("QtdCorposInicial");
                simulacao.NumInteracoes = readerSim.GetInt32("NumInteracoes");
                simulacao.TempoInteracoes = readerSim.GetInt32("TempoInteracoes");
            }

            // pega os corpos do último frame
            using (var conn = _conexao.CriarConexao())
            {
                using var cmdCorpos = conn.CreateCommand();
                cmdCorpos.CommandText = @"
                    SELECT NomeCorpo, MassaCorpo, DensidadeCorpo, PosX, PosY, VelX, VelY
                    FROM corpos
                    WHERE NumSimulacao=@_NumSimulacao AND NumInteracao = (
                        SELECT MAX(NumInteracao) FROM corpos WHERE NumSimulacao=@_NumSimulacao
                    )
                    ORDER BY IdCorpo";
                cmdCorpos.Parameters.AddWithValue("@_NumSimulacao", numSimulacao);

                var lista = new List<Corpo>();
                using (var reader = cmdCorpos.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Corpo(
                            reader.GetString("NomeCorpo"),
                            reader.GetDouble("MassaCorpo"),
                            reader.GetDouble("DensidadeCorpo"),
                            reader.GetDouble("PosX"),
                            reader.GetDouble("PosY"),
                            reader.GetDouble("VelX"),
                            reader.GetDouble("VelY")
                        ));
                    }
                }

                simulacao.Corpos = lista; // guarda na simulação
            }

            return simulacao;
        }
    }
}
