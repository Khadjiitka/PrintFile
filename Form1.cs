using System.Drawing.Printing;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Програма дає змогу відкрити в стандартному діалозі текстовий файл,
        // переглянути його в текстовому полі без можливості зміни тексту (ReadOnly) 
        // і, за бажання користувача, вивести цей текст на принтер
        System.IO.StreamReader Reader;
        public Form1()
        {
            InitializeComponent();
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Open/Print Text File";
            textBox1.Clear();
            openFileDialog1.FileName = null;

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Reader = new System.IO.StreamReader(openFileDialog1.FileName, System.Text.Encoding.GetEncoding(1251));
                try
                {
                    printDocument1.Print();
                }
                finally
                {
                    Reader.Close();
                }
            }
            catch (Exception Status)
            {
                MessageBox.Show(Status.Message);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Текстові файли (*.txt)|*.txt|All files (*.*)|*.*"; 
            openFileDialog1.ShowDialog();
            if (string.IsNullOrEmpty(openFileDialog1.FileName)) return;
            try
            {
                // Створення потоку StreamReader для читання з файлу
                Reader = new System.IO.StreamReader(openFileDialog1.FileName, System.Text.Encoding.GetEncoding(1251));
                // замовлення кодової сторінки Win1251 для російських літер
                textBox1.Text = Reader.ReadToEnd();
                Reader.Close();
            }
            catch (System.IO.FileNotFoundException Status)
            {
                MessageBox.Show(Status.Message +
                "\nНемає такого файлу", "Помилка",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception Status)
            {
                // Звіт про інші помилки:
                MessageBox.Show(Status.Message, "Помилка",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Single RowsОnPage = 0.0F;
            Single Y = 0;
            var LeftEdge = e.MarginBounds.Left;
            var UpperEdge = e.MarginBounds.Top;
            var Rows = String.Empty;
            var Font = new Font("Times New Roman", 12.0F);
            // Обчислюємо кількість рядків на одній сторінці
            RowsОnPage = e.MarginBounds.Height /
            Font.GetHeight(e.Graphics);
            var i = 0; // рахунок рядків
            while (i < RowsОnPage)
            {
                Rows = Reader.ReadLine();
                if (Rows == null) break;
                // Для VB: If Рядок Is Nothing Then Exit While
                Y = UpperEdge + i * Font.GetHeight(e.Graphics);
                // Друк рядка
                e.Graphics.DrawString(Rows, Font, Brushes.Black,
                LeftEdge, Y, new StringFormat());
                i += 1;
            }
            // Друк наступної сторінки, якщо є ще рядки файлу
            if (Rows != null) e.HasMorePages = true;
            // Для VB: If Рядок <> Null Then ...
            else e.HasMorePages = false;
        }

        private void exiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
