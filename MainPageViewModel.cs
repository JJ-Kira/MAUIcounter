using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MoneyCounter.ViewModels
{
    // Zarządza logiką aplikacji i stanem.
    // Jest niezależna od widoku, co pozwala łatwo testować logikę.
    public class MainPageViewModel : INotifyPropertyChanged
    {
        // Pole przechowujące sumę wprowadzonych liczb
        private decimal totalSum;

        // Limit maksymalnej sumy
        private const decimal Limit = 1000;

        // Pole do przechowywania wprowadzonej liczby
        private string numberInput;

        // Komunikat wyświetlany użytkownikowi
        private string message;

        // Właściwość powiązana z etykietą sumy w widoku
        public decimal TotalSum
        {
            get => totalSum;
            set
            {
                if (totalSum != value)
                {
                    totalSum = value;
                    // Informujemy widok o zmianie właściwości
                    OnPropertyChanged();
                }
            }
        }

        // Właściwość dla wprowadzonej liczby w polu tekstowym
        public string NumberInput
        {
            get => numberInput;
            set
            {
                if (numberInput != value)
                {
                    numberInput = value;
                    OnPropertyChanged();
                }
            }
        }

        // Właściwość dla komunikatu, np. "Liczba za duża"
        public string Message
        {
            get => message;
            set
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged();
                }
            }
        }

        // Właściwość określająca, czy pole tekstowe jest aktywne
        public bool IsInputEnabled => TotalSum < Limit;

        // Komendy do obsługi przycisków
        public ICommand AddCommand { get; }
        public ICommand ResetCommand { get; }

        // Konstruktor ViewModel, inicjalizujący komendy i wczytujący stan
        public MainPageViewModel()
        {
            AddCommand = new Command(OnAdd);
            ResetCommand = new Command(OnReset);
            LoadState();
        }

        // Logika dla przycisku "Add"
        private void OnAdd()
        {
            if (decimal.TryParse(NumberInput, out decimal number))
            {
                if (TotalSum + number > Limit)
                {
                    // Informujemy użytkownika, że suma przekroczyłaby limit
                    Message = "The number you entered would exceed the limit of 1000.";
                }
                else
                {
                    // Aktualizujemy sumę i czyścimy pole wejściowe
                    TotalSum += number;
                    SaveState();
                    NumberInput = string.Empty;
                    Message = string.Empty;

                    // Informujemy widok, że właściwość IsInputEnabled mogła się zmienić
                    OnPropertyChanged(nameof(IsInputEnabled));
                }
            }
            else
            {
                // Informujemy użytkownika, że wprowadzono nieprawidłową wartość
                Message = "Please enter a valid number.";
            }
        }

        // Logika dla przycisku "Reset"
        private void OnReset()
        {
            // Resetujemy sumę, pole wejściowe i komunikaty
            TotalSum = 0;
            NumberInput = string.Empty;
            Message = string.Empty;
            SaveState();

            // Informujemy widok, że właściwość IsInputEnabled mogła się zmienić
            OnPropertyChanged(nameof(IsInputEnabled));
        }

        // Zapisanie stanu aplikacji
        private void SaveState()
        {
            Preferences.Set("TotalSum", (double)TotalSum);
        }

        // Wczytanie stanu aplikacji
        private void LoadState()
        {
            TotalSum = (decimal)Preferences.Get("TotalSum", 0.0);
        }

        // Mechanizm powiadamiania widoku o zmianie właściwości
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
