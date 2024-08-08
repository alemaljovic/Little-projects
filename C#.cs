using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class Program
{
    static void Main()
    {
        try
        {
            string difficultyLevel = GetDifficulty();
            //In terminal write " dotnet run " to run this code 
            List<Question> questions = LoadAndShuffleQuestions("question.json", difficultyLevel);
             //HERE YOU CAN CHANGE THE NUMBER OF SECONDS FOR EACH QUESTION
            int totalQuizTime = questions.Count * 3; 
            int rightAnswer = 0;

            for (int i = 0; i < Math.Min(5, questions.Count); i++)
            {
                Question question = questions[i];
                //If you change time for questions, you will need the update the Console.WriteLine but that is not important
                Console.WriteLine($"\nTime limit for this question: 3 seconds");
                ShowQuestion(question);

                DateTime startTime = DateTime.Now;
                string userAnswer = GetUserAnswer(question);
                double elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;

                bool isCorrect = CheckAnswer(question, userAnswer, elapsedSeconds, 3);

                if (isCorrect)
                {
                    rightAnswer++;
                }

                Console.WriteLine($"Current Score: {rightAnswer}/{i + 1}");
            }

            GenerateReport(rightAnswer, Math.Min(5, questions.Count), totalQuizTime);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }

        Console.ReadLine();
    }

    static void ShowQuestion(Question question)
    {
        Console.WriteLine(question.type == "true_false" ? $"\n {question.q}" : $"\n {question.q}");
        if (question.type == "true_false")
        {
            ShowTrueFalseOptions();
        }
        else if (question.type == "fill_in_the_blank")
        {
            Console.WriteLine(" Fill in the blank: ");
        }
    }

    static void ShowTrueFalseOptions()
    {
        Console.WriteLine(" <a> True");
        Console.WriteLine(" <b> False");
    }

    static string GetUserAnswer(Question question)
    {
        string answer;
        bool answered = false;

        do
        {
            answer = Console.ReadLine()?.ToLower();

            if (question.type == "true_false")
            {
                if (answer != "true" && answer != "false")
                {
                    Console.WriteLine("\nInvalid option. Please enter 'true' or 'false'.");
                    continue;
                }
            }
            else if (question.type == "fill_in_the_blank")
            {
                if (string.IsNullOrWhiteSpace(answer))
                {
                    Console.WriteLine("\nPlease enter a non-empty answer.");
                    continue;
                }
            }

            answered = true;

        } while (!answered);

        return answer;
    }

    static bool CheckAnswer(Question question, string userAnswer, double elapsedSeconds, int timeLimit)
    {
        string correctAnswer = question.an.ToLower();

        if (userAnswer == correctAnswer && elapsedSeconds <= timeLimit)
        {
            Console.WriteLine("\nCorrect Answer!");
            return true;
        }
        else
        {
            if (elapsedSeconds > timeLimit)
            {
                Console.WriteLine("\nTime's up! You did not give the answer on time.");
            }
            else
            {
                Console.WriteLine("\nYour answer is wrong");
                Console.WriteLine($"Correct answer is {correctAnswer}");
            }
            return false;
        }
    }
static List<Question> LoadAndShuffleQuestions(string filePath, string difficulty)
{
    try
    {
        string json = File.ReadAllText(filePath);
        List<Question> questions = JsonSerializer.Deserialize<List<Question>>(json)
            .Where(q => q.difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (questions.Count == 0)
        {
            Console.WriteLine($"No questions found for difficulty level: {difficulty}");
            Environment.Exit(1);
        }

        Shuffle(questions);

        return questions;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading questions: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
        Environment.Exit(1);
        return null;
    }
}


    static string GetDifficulty()
    {
        Console.WriteLine("Choose difficulty level (starter/advanced): ");
        string input = Console.ReadLine()?.ToLower();

        while (input != "starter" && input != "advanced")
        {
            Console.WriteLine("Invalid difficulty level. Please enter 'starter' or 'advanced'.");
            input = Console.ReadLine()?.ToLower();
        }

        return input;
    }

    static void GenerateReport(int rightAnswer, int totalQuestions, int totalQuizTime)
    {
        Console.WriteLine("\n!!!Game Over");
        Console.WriteLine($"\nYou made {rightAnswer} right out of {totalQuestions} answers. Your score is {rightAnswer}");
        Console.WriteLine($"Total quiz time: {totalQuizTime} seconds");
        Environment.Exit(0);
    }

    static void Shuffle<T>(List<T> list)
    {
        Random rand = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Question
{
    public string q { get; set; } = "";
    public string an { get; set; } = "";
    public string type { get; set; }
    public string difficulty { get; set; }
}
