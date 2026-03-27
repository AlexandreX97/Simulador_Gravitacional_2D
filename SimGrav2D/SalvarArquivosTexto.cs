using System;
using System.IO;

namespace SimGrav2D
{
    // Classe abstrata para salvar e ler arquivos de texto
    public abstract class SalvarArquivosTexto
    {
        // Método abstrato para salvar o conteúdo em um arquivo .txt
        public abstract void Salvar(string caminho, string conteudo);

        // Método abstrato para ler conteúdo em um arquivo .txt
        public abstract string Ler(string caminho);
    }


}