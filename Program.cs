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

            var output = new ConsoleOutput();
            var settings = new GameSettings
            {
                MinVal = output.GetIntInput("Введите минимальное значение: "),
                MaxVal = output.GetIntInput("Введите максимальное значение: "),
                Attempts = output.GetIntInput("Введите количество попыток: ")
            };

            var game = new GuessingGame(new RandomNumGenerator(), output, settings);
            game.RunGame();
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

        public interface IGameOutput
        {
            void OutMessage(string message);
            int GetInput();
            int GetIntInput(string message);
        }

        public class ConsoleOutput : IGameOutput
        {
            public void OutMessage(string message)
            {
                Console.WriteLine(message);
            }

            public int GetInput()
            {
                return Convert.ToInt32(Console.ReadLine());
            }

            public int GetIntInput(string message)
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
            bool ValidGameSettings(GameSettings settings);
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

            public bool ValidGameSettings(GameSettings settings)
            {
                throw new NotImplementedException();
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
            private readonly IGameOutput _gameOutput;
            private readonly GameSettings _settings;
            private int _secretNumber;
            private int _attempts;

            public GuessingGame(IRandomNumGenerator randomNumGenerator, IGameOutput gameOutput, GameSettings settings)
            {
                _randomNumGenerator = randomNumGenerator;
                _gameOutput = gameOutput;
                _settings = settings;
            }

            public void RunGame()
            {
                
                _secretNumber = _randomNumGenerator.Generate(_settings.MinVal, _settings.MaxVal);

                _attempts = 0;
                _gameOutput.OutMessage($"Угадайте число от {_settings.MinVal} до {_settings.MaxVal}. У вас есть {_settings.Attempts} попыток.");

                while (_attempts < _settings.Attempts)
                {
                    _attempts++;
                    int inVal = _gameOutput.GetInput();

                    if (inVal == _secretNumber)
                    {
                        _gameOutput.OutMessage($"Вы угадали за {_attempts} попыток! Поздравляем!");
                        return;
                    }
                    else if (inVal < _secretNumber)
                    {
                        _gameOutput.OutMessage("Загаданное число больше.");
                    }
                    else
                    {
                        _gameOutput.OutMessage("Загаданное число меньше.");
                    }
                }

                _gameOutput.OutMessage($"Вы не угадали. Загаданное число было {_secretNumber}.");
            }
        }
    }
}
