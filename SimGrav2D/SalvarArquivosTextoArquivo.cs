using SimGrav2D;

// Classe concreta que implementa os métodos de salvar e ler arquivos de texto
public class SalvarArquivosTextoArquivo : SalvarArquivosTexto
{
    // Método para salvar o conteúdo no arquivo .txt
    public override void Salvar(string caminho, string conteudo)
    {
        try { File.WriteAllText(caminho, conteudo); }
        catch (Exception ex) { Console.WriteLine($"Erro ao salvar arquivo: {ex.Message}"); }
    }

    // Método que lê o conteúdo de um arquivo .txt
    public override string Ler(string caminho)
    {
        try
        {
            if (!File.Exists(caminho)) return string.Empty;
            return File.ReadAllText(caminho);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao ler arquivo: {ex.Message}");
            return string.Empty;
        }
    }
}
