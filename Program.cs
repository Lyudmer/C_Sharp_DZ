using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZGame
{
   
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Угадай число";

            var inputval = new ConsoleInput();
            var validator = new GameSettingsValid();
            var displaymess = new ConsoleMessage();
            var settings = new GameSettings
            {
                MinVal = inputval.GetInputVal("Введите минимальное значение: "),
                MaxVal = inputval.GetInputVal("Введите максимальное значение: "),
                Attempts = inputval.GetInputVal("Введите количество попыток: ")
            };

            var game = new GuessingGame(new RandomNumGenerator(), inputval, validator, settings, displaymess);
            game.RunGame();
        }
    }
   public interface IRandomNumGenerator
   {
       int Generate(int min, int max);
   }

   public class RandomNumGenerator : IRandomNumGenerator
   {
       private readonly Random _random = new Random();

       public int Generate(int min, int max)
       {
           return _random.Next(min, max + 1);
       }
   }
    public interface IGameMessage
    {
        void DisplayMessage(string message);
    }
    public class ConsoleMessage : IGameMessage
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
    public interface IGameInputVal
    {
        int GetInputVal(string message);
    }
    
    public class ConsoleInput : IGameInputVal
    {
        
        public int GetInputVal(string message)
        {
            int inNum;
            while (true) 
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine().Trim(), out inNum))break;
                Console.WriteLine("Не удалось распознать число, попробуйте еще раз.");
            } 
            return inNum;
        }
        
    }
    public interface IInputValid
    {
        bool ValidateGameSettings(GameSettings settings);
    }

    public class GameSettingsValid : IInputValid
    {
        public bool ValidateGameSettings(GameSettings settings)
        {
            if (settings.MinVal >= settings.MaxVal)
            {
                Console.WriteLine("Ошибка: минимальное значение должно быть меньше максимального.");
                return false;
            }

            if (settings.Attempts <= 0)
            {
                Console.WriteLine("Ошибка: количество попыток должно быть больше нуля.");
                return false;
            }

            return true;
        }
    }
    public class GameSettings
    {
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
        public int Attempts { get; set; }
    }

    public class GuessingGame
    {
        private readonly IRandomNumGenerator _randomNumGenerator;
        private readonly IGameInputVal _gameInputVal;
        private readonly IGameMessage _gamemess;
        private readonly IInputValid _inputValid;
        private readonly GameSettings _settings;
        private int _secretNumber;
        private int _attempts;

        public GuessingGame(IRandomNumGenerator randomNumGenerator, IGameInputVal gameInputVal, IInputValid inputValid, GameSettings settings, IGameMessage gamemess)
        {
            _randomNumGenerator = randomNumGenerator;
            _gameInputVal = gameInputVal;
            _inputValid = inputValid;
            _settings = settings;
            _gamemess = gamemess;
        }

        public void RunGame()
        {
            if (!_inputValid.ValidateGameSettings(_settings))
            {
                return;
            }
            _secretNumber = _randomNumGenerator.Generate(_settings.MinVal, _settings.MaxVal);

            _attempts = 0;
            _gamemess.DisplayMessage($"Угадайте число от {_settings.MinVal} до {_settings.MaxVal}. У вас есть {_settings.Attempts} попыток.");

            while (_attempts < _settings.Attempts)
            {
                _attempts++;
                int inVal = _gameInputVal.GetInputVal("Введите Ваше число: ");

                if (inVal == _secretNumber)
                {
                    _gamemess.DisplayMessage($"Вы угадали за {_attempts} попыток! Поздравляем!");
                    return;
                }
                else if (inVal < _secretNumber)
                {
                    _gamemess.DisplayMessage("Загаданное число больше.");
                }
                else
                {
                    _gamemess.DisplayMessage("Загаданное число меньше.");
                }
            }

            _gamemess.DisplayMessage($"Вы не угадали. Загаданное число было {_secretNumber}.");
            Console.ReadKey();
        }

    }
    
}
