using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MmfLab
{
    public partial class Form1 : Form
    {
        // ===================== Константы WinAPI =====================

        // Вместо дескриптора файла передаём INVALID_HANDLE_VALUE (-1, в Delphi $FFFFFFFF):
        // тогда проекция создаётся не на обычном файле, а в системном страничном файле.
        static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        const uint PAGE_READWRITE = 0x04;        // страницы можно читать и записывать
        const uint FILE_MAP_ALL_ACCESS = 0xF001F; // полный доступ к проекции
        const int ERROR_ALREADY_EXISTS = 183;     // объект с таким именем уже существует

        const string MAPPING_NAME = "MMFSharedData"; // имя объекта "проецируемый файл"
        const int MAPFILESIZE = 1024;                // размер проекции в байтах

        // ===================== Функции WinAPI (kernel32.dll) =====================

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr CreateFileMapping(
            IntPtr hFile,                 // дескриптор файла (у нас INVALID_HANDLE_VALUE)
            IntPtr lpFileMappingAttributes, // атрибуты безопасности (у нас null)
            uint flProtect,               // атрибуты защиты страниц
            uint dwMaximumSizeHigh,       // старшие 32 бита размера
            uint dwMaximumSizeLow,        // младшие 32 бита размера
            string lpName);               // имя объекта

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr OpenFileMapping(
            uint dwDesiredAccess,         // права доступа
            bool bInheritHandle,          // наследуют ли дочерние процессы дескриптор
            string lpName);               // имя объекта

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,    // дескриптор объекта "проецируемый файл"
            uint dwDesiredAccess,         // вид доступа к данным
            uint dwFileOffsetHigh,        // смещение начала окна (старшие 32 бита)
            uint dwFileOffsetLow,         // смещение начала окна (младшие 32 бита)
            UIntPtr dwNumberOfBytesToMap); // сколько байт проецировать (0 = всё)

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        // ===================== Поля формы =====================

        // Дескриптор созданного нами объекта "проецируемый файл".
        // Пока он открыт — объект (и данные в нём) существует в системе.
        IntPtr hMapping = IntPtr.Zero;

        public Form1()
        {
            InitializeComponent();

            // В заголовке показываем ID процесса, чтобы различать две копии программы
            Text = "MMF - процесс " + System.Diagnostics.Process.GetCurrentProcess().Id;

            // 500 символов * 2 байта (Unicode) + 2 байта на завершающий ноль < 1024
            editData.MaxLength = 500;
        }

        // ===================== Кнопка 1: Создать проекцию =====================
        private void btnCreate_Click(object sender, EventArgs e)
        {
            // 1. Создаём объект ядра "проецируемый файл" с именем MMFSharedData
            hMapping = CreateFileMapping(INVALID_HANDLE_VALUE, IntPtr.Zero,
                PAGE_READWRITE, 0, MAPFILESIZE, MAPPING_NAME);

            // GetLastError нужно читать сразу после вызова функции
            bool isNew = Marshal.GetLastWin32Error() != ERROR_ALREADY_EXISTS;

            if (hMapping == IntPtr.Zero)
            {
                MessageBox.Show("Can't create file mapping", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // 2. Проецируем объект на адресное пространство нашего процесса
            IntPtr pMapData = MapViewOfFile(hMapping, FILE_MAP_ALL_ACCESS, 0, 0, UIntPtr.Zero);

            if (pMapData == IntPtr.Zero)
            {
                CloseHandle(hMapping);
                hMapping = IntPtr.Zero;
                MessageBox.Show("Can't view memory map", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            if (isNew)
            {
                // 3. Копируем текст из поля ввода в общую память.
                // Строка хранится в Unicode, в конце ставим нулевой символ.
                byte[] bytes = Encoding.Unicode.GetBytes(editData.Text + "\0");
                Marshal.Copy(bytes, 0, pMapData, bytes.Length);

                // 4. Окно (view) больше не нужно - отключаем его.
                // Сам объект остаётся, т.к. hMapping ещё открыт.
                UnmapViewOfFile(pMapData);

                // Второй раз создать нельзя, зато теперь можно закрыть
                btnCreate.Enabled = false;
                btnClose.Enabled = true;
                editData.Text = "";
            }
            else
            {
                // Объект уже создан другой копией программы.
                // Освобождаем то, что только что получили, и сообщаем об этом.
                UnmapViewOfFile(pMapData);
                CloseHandle(hMapping);
                hMapping = IntPtr.Zero;

                MessageBox.Show("Mapping Already Exists", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== Кнопка 2: Открыть проекцию и получить данные =====================
        private void btnOpen_Click(object sender, EventArgs e)
        {
            // 1. Открываем уже существующий объект по имени
            IntPtr opMapping = OpenFileMapping(FILE_MAP_ALL_ACCESS, false, MAPPING_NAME);

            if (opMapping == IntPtr.Zero)
            {
                // По заданию: сообщение и завершение программы
                MessageBox.Show("Can't Open Mapping", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // 2. Проецируем объект на адресное пространство процесса
            IntPtr pMapData = MapViewOfFile(opMapping, FILE_MAP_ALL_ACCESS, 0, 0, UIntPtr.Zero);

            if (pMapData == IntPtr.Zero)
            {
                CloseHandle(opMapping);
                MessageBox.Show("Can't map view", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // 3. Читаем строку из общей памяти (до нулевого символа) и выводим в поле
            editData.Text = Marshal.PtrToStringUni(pMapData);

            // 4. Отключаем окно и закрываем свой дескриптор
            UnmapViewOfFile(pMapData);
            CloseHandle(opMapping);
        }

        // ===================== Кнопка 3: Закрыть проекцию =====================
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (hMapping != IntPtr.Zero)
            {
                CloseHandle(hMapping);
                hMapping = IntPtr.Zero;
            }

            btnClose.Enabled = false;
            btnCreate.Enabled = true;
        }

        // ===================== Кнопка 4: Выход =====================
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // При закрытии окна освобождаем дескриптор, если он ещё открыт
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hMapping != IntPtr.Zero)
            {
                CloseHandle(hMapping);
                hMapping = IntPtr.Zero;
            }
        }
    }
}
