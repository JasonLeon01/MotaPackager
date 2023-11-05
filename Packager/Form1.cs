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
            MessageBox.Show("打包完成！");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string path = @"package.txt";
            Files = System.IO.File.ReadAllText(path).Split(Environment.NewLine.ToCharArray()).Where(s => !string.IsNullOrEmpty(s)).ToList();
        }
        static void CopyDirectory(string sourceDirectory, string destinationDirectory)
        {
            // 获取源文件夹中的所有文件和子文件夹
            string[] files = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                // 构建目标路径
                string relativePath = filePath.Substring(sourceDirectory.Length);
                string destinationPath = Path.Combine(destinationDirectory, relativePath);
                // 确保目标文件夹存在
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                // 复制文件
                File.Copy(filePath, destinationPath, true);
            }
        }
    }
}