using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace MoverPasta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool IsFileInUse(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // O arquivo não está em uso
                    return false;
                }
            }
            catch (IOException)
            {
                // O arquivo está em uso
                return true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string sourceDirectory = @"\\629.0.0.1\a\Teste_Uma_Pasta_Com_Nome_Que_Tem_Mais_De_272_Caracteres_Para_Testar_Comprimento_DoNomeDeCaminhoDePastaNoWindowsQuePossuiUmLimiteDeTamanhoDe255CaracteresPortantoEsseCaminhoExcedeEsseLimite_PrimeiroA22\CTesteUmaPastaComNomeQueSegundoA2"; // Seu diretório de origem aqui
            string destinationBaseDirectory = @"\\629.0.0.1\b\Teste_Uma_Pasta_Com_Nome_Que_Tem_Mais_De_272_Caracteres_Para_Testar_Comprimento_DoNomeDeCaminhoDePastaNoWindowsQuePossuiUmLimiteDeTamanhoDe255CaracteresPortantoEsseCaminhoExcedeEsseLimite_SegundoB22\CTesteUmaPastaComNomeQueSegundoB2"; // Seu diretório de destino aqui

            List<string> openFiles = new List<string>();

            foreach (string filePath in Directory.GetFiles(sourceDirectory))
            {
                string fileName = Path.GetFileName(filePath);

                // Ignora arquivos ocultos, arquivos temporários do Office e o arquivo thumbs.db
                if (fileName.StartsWith("~$") ||
                    fileName.Equals("thumbs.db", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (IsFileInUse(filePath))
                {
                    openFiles.Add(fileName);
                }
            }

            if (openFiles.Count > 0)
            {
                string openFilesList = string.Join(", ", openFiles);

                MessageBox.Show($"Os seguintes arquivos estão abertos e não podem ser movidos: {openFilesList}. Por favor, feche os arquivos e tente novamente.", "Arquivos em Uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    // Move a pasta, não exibindo a caixa de diálogo de substituição
                    FileSystem.CopyDirectory(sourceDirectory, destinationBaseDirectory, UIOption.OnlyErrorDialogs);

                    // Move a pasta desabilitando a caixa de diálogo de confirmação de substituição
                    FileSystem.MoveDirectory(sourceDirectory, destinationBaseDirectory, UIOption.OnlyErrorDialogs);
                }
                catch (Exception ex)
                {
                    // Outros tipos de erros
                    MessageBox.Show($"Ocorreu um erro específico: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
