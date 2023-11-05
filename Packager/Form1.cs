using System.Diagnostics;
using System.IO.Compression;

namespace Packager
{
    public partial class Form1 : Form
    {
        List<string> Files;
        public Form1()
        {
            Files = new List<string>();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sourceDirectory = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            string destinationDirectory = Path.Combine(Directory.GetCurrentDirectory(), Files[0]);
            string zipFilePath = Path.Combine(Directory.GetCurrentDirectory(), string.Format("{0}.zip", Files[0]));
            if (File.Exists(zipFilePath))
                File.Delete(zipFilePath);
            Directory.CreateDirectory(destinationDirectory);
            for (int i = 1; i < Files.Count; i++)
            {
                string sourcePath = Path.Combine(sourceDirectory, Files[i]);
                string destinationPath = Path.Combine(destinationDirectory, Files[i]);
                if (Files[i].EndsWith("\\"))
                    CopyDirectory(sourcePath, destinationPath);
                else
                    File.Copy(sourcePath, destinationPath, true);
            }
            string savefolder = Path.Combine(destinationDirectory, "save");
            if (!Directory.Exists(savefolder))
                Directory.CreateDirectory(savefolder);
            ZipFile.CreateFromDirectory(destinationDirectory, zipFilePath);
            Directory.Delete(destinationDirectory, true);
            MessageBox.Show("�����ɣ�");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string path = @"package.txt";
            Files = System.IO.File.ReadAllText(path).Split(Environment.NewLine.ToCharArray()).Where(s => !string.IsNullOrEmpty(s)).ToList();
        }
        static void CopyDirectory(string sourceDirectory, string destinationDirectory)
        {
            // ��ȡԴ�ļ����е������ļ������ļ���
            string[] files = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                // ����Ŀ��·��
                string relativePath = filePath.Substring(sourceDirectory.Length);
                string destinationPath = Path.Combine(destinationDirectory, relativePath);
                // ȷ��Ŀ���ļ��д���
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                // �����ļ�
                File.Copy(filePath, destinationPath, true);
            }
        }
    }
}