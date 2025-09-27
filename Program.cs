// See https://aka.ms/new-console-template for more information


// command shift P -> Open Nuget Package -> Tellegram.Bot -> choose version and check checkbox
// version 16.02
using System;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    static TelegramBotClient bot;
    static Random rnd = new Random();

    static void Main()
    {
        bot = new TelegramBotClient("token");

        var me = bot.GetMeAsync().GetAwaiter().GetResult();
        Console.WriteLine($"Bot @{me.Username} is running... Press Enter to stop");

        bot.OnMessage += Bot_OnMessage;
        bot.StartReceiving();

        Console.ReadLine();
        bot.StopReceiving();
    }

    private static async void Bot_OnMessage(object sender, MessageEventArgs e)
    {


        var chatId = e.Message.Chat.Id;



        if (e.Message.Text != null)
        {
            string text = e.Message.Text.ToLower();

            if (text == "/start")
            {
                await bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: "I am your Math teacher 👩‍🏫\nYou can write words: 'hello', 'math', 'test'"
                );
                return;
            }


            if (text == "hello")
            {
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat.Id,
                    text: "Hi there 💗!"
                );
            }

            if (text == "math")
            {
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat.Id,
                    text: "Write the word 'test' and i will give you exercises 📚!"
                );
            }


            if (text == "test")
            {

                int a = rnd.Next(1, 21);
                int b = rnd.Next(1, 21);

                string[] ops = { "+", "-", "*", "/" };
                string op = ops[rnd.Next(ops.Length)];

                double correctAnswer = 0;
                string questionText = "";

                switch (op)
                {
                    case "+":
                        correctAnswer = a + b;
                        questionText = $"{a} + {b} = ?";
                        break;
                    case "-":
                        correctAnswer = a - b;
                        questionText = $"{a} - {b} = ?";
                        break;
                    case "*":
                        correctAnswer = a * b;
                        questionText = $"{a} × {b} = ?";
                        break;
                    case "/":
                        b = rnd.Next(1, 10);
                        a = rnd.Next(1, 10) * b;
                        correctAnswer = a / b;
                        questionText = $"{a} ÷ {b} = ?";
                        break;

                }


                double[] options = new double[4];
                options[0] = correctAnswer;

                for (int i = 1; i < 4; i++)
                {
                    double wrong;
                    do
                    {
                        wrong = correctAnswer + rnd.Next(-10, 11);
                    } while (Array.Exists(options, x => x == wrong));
                    options[i] = wrong;
                }


                for (int i = 0; i < options.Length; i++)
                {
                    int j = rnd.Next(i, options.Length);
                    var tmp = options[i];
                    options[i] = options[j];
                    options[j] = tmp;
                }


                await bot.SendPollAsync(
                    chatId: chatId,
                    question: questionText,
                    options: Array.ConvertAll(options, x => x.ToString()),
                    type: PollType.Quiz,
                    correctOptionId: Array.IndexOf(options, correctAnswer)
                );
            }

        }
        }
}


