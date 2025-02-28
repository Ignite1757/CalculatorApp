using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // Для работы с файлами

namespace CalculatorApp
{
    public partial class Form1: Form
    {
        // Переменные для отслеживания состояния калькулятора
        double firstNumber = 0;
        double secondNumber = 0;
        string currentOperation = ""; // Текущая операция
        bool isFirstNumberEntered = false; // Был ли введен первый оператор
        bool isSecondNumberEntered = false; // Было ли введено второе число
        private double savedResult = 0; // Хранит последний сохраненный результат

        public Form1()
        {
            InitializeComponent();
        }

        // Обработчик для кнопок с цифрами
        private void buttonDigit_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (isSecondNumberEntered)
            {
                // Если уже был введен второй оператор, очищаем текстовое поле
                textBoxResult.Clear();
                isSecondNumberEntered = false;
            }
            textBoxResult.Text += btn.Text; // Добавляем цифру в текстовое поле
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxResult.Clear();
        }

        // Обработчик для кнопки "="
        private void buttonEqual_Click(object sender, EventArgs e)
        {
            if (isFirstNumberEntered && !isSecondNumberEntered)
            {
                // Сохраняем текущее число, если оно было введено
                secondNumber = double.Parse(textBoxResult.Text);
                PerformOperation();
                isFirstNumberEntered = false; // Готовимся к новому циклу вычислений
                isSecondNumberEntered = false;
            }
        }

        // Обработчик для кнопок с операциями
        private void buttonOperation_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            currentOperation = btn.Text; // Запоминаем операцию
            string number = textBoxResult.Text;
            if (IsValidInput(number))
            {
                if (!isFirstNumberEntered)
                {
                    // Сохраняем текущее число, если оно было введено
                    firstNumber = double.Parse(number);
                    isFirstNumberEntered = true;
                    textBoxResult.Clear(); // Очищаем текстовое поле для следующего числа
                }
                else
                {
                    // Выполняем предыдущую операцию перед установкой новой
                    secondNumber = double.Parse(number);
                    PerformOperation();
                    textBoxResult.Clear(); // Очищаем текстовое поле для следующего числа
                }
            }
            else
            {
                MessageBox.Show("Ошибка: некорректный ввод!");
                textBoxResult.Clear();
            }
            
        }

        // Метод для выполнения текущей операции
        private void PerformOperation()
        {
            switch (currentOperation)
            {
                case "+":
                    firstNumber += secondNumber;
                    break;
                case "-":
                    firstNumber -= secondNumber;
                    break;
                case "*":
                    firstNumber *= secondNumber;
                    break;
                case "/":
                    if (secondNumber != 0)
                        firstNumber /= secondNumber;
                    else
                        MessageBox.Show("Ошибка: деление на ноль!");
                        textBoxResult.Clear();
                    break;
            }
            textBoxResult.Text = firstNumber.ToString(); // Показываем результат
            isSecondNumberEntered = true; // Операция выполнена, теперь можно ввести новое число
        }

        // Проверка корректности ввода
        private bool IsValidInput(string input)
        {
            return Double.TryParse(input, out _);
        }

        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли текст в текстовом поле
            if (textBoxResult.Text.Length > 0)
            {
                // Удаляем последний символ
                textBoxResult.Text = textBoxResult.Text.Substring(0, textBoxResult.Text.Length - 1);
            }
        }
        private void SaveCurrentResultToFile()
        {
            // Сохраняем текущее значение из текстового поля
            double currentResult = double.Parse(textBoxResult.Text);
            File.WriteAllText("result.txt", currentResult.ToString());
            savedResult = currentResult;
            MessageBox.Show("Результат успешно сохранён!");
        }

        private void LoadSavedResultFromFile()
        {
            if (File.Exists("result.txt"))
            {
                string savedValue = File.ReadAllText("result.txt");
                savedResult = double.Parse(savedValue);
                textBoxResult.Text = savedResult.ToString();
                MessageBox.Show("Результат успешно загружен!");
            }
            else
            {
                MessageBox.Show("Нет сохранённых результатов.");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveCurrentResultToFile();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            LoadSavedResultFromFile();
        }

        private void buttonFontUp_Click(object sender, EventArgs e)
        {
            int currentFontSize = (int)textBoxResult.Font.SizeInPoints;
            if (currentFontSize < 40) // Ограничиваем максимальный размер шрифта
            {
                textBoxResult.Font = new Font(textBoxResult.Font.FontFamily, currentFontSize + 2, textBoxResult.Font.Style);
                textBoxResult.Height = 98;
            }
        }

        private void buttonFontDown_Click(object sender, EventArgs e)
        {
            int currentFontSize = (int)textBoxResult.Font.SizeInPoints;
            if (currentFontSize > 8) // Ограничиваем минимальный размер шрифта
            {
                textBoxResult.Font = new Font(textBoxResult.Font.FontFamily, currentFontSize - 2, textBoxResult.Font.Style);
                textBoxResult.Height = 98;
            }
        }
    }
}
