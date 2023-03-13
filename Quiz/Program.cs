using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Quiz
{
    public class Program
    {
        public class User
        {
            public string Name { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public User() { }
            public User(string name, string login, string password)
            {
                Name = name;
                Login = login;
                Password = password;
            }

        }
        public static class UserService
        {
            private static readonly string filePath = "users.txt";
            public static bool InitPassword(User user)
            {
                Console.Write("Ввеедите ваше имя: ");
                user.Name = Console.ReadLine();
                Console.Write("Введите ваш логин: ");
                user.Login = Console.ReadLine();
                Console.Write("Введите ваш пароль: ");
                user.Password = Console.ReadLine();
                if (IsUserExist(user.Login, user.Password))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.OutputEncoding = System.Text.Encoding.Unicode;
                    Console.WriteLine("Пользователь с таким логином и паролем уже существует (╯°□°）╯︵ ┻━┻");
                    Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    return false;
                }
                if (!IsValidPassword(user.Password))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.OutputEncoding = System.Text.Encoding.Unicode;
                    Console.WriteLine("Пароль не соответствует требованиям (╯°□°）╯︵ ┻━┻:");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("- должен быть не меньше 4 символов и не больше 8");
                    Console.WriteLine("- должна быть хотя бы одна заглавная буква");
                    Console.ReadLine();
                    return false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.OutputEncoding = System.Text.Encoding.Unicode;
                    Console.WriteLine("Вы успешно зарегистрировались (⁀ᗢ⁀)");
                    Console.ForegroundColor = ConsoleColor.White;
                    SaveUser(user);
                    Console.ReadLine();
                    return true;
                }
            }
            public static void SaveUser(User user)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine($"{user.Name},{user.Login},{user.Password}");
                }
            }
            public static bool IsUserAuthenticated(User user)
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файла нет");
                    return false;
                }

                using (StreamReader sr = File.OpenText(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] userCredentials = line.Split(',');
                        if (userCredentials.Length != 3)
                        {
                            continue;
                        }
                        if (userCredentials[1] == user.Login && userCredentials[2] == user.Password)
                        {
                            user.Name = userCredentials[0];
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.OutputEncoding = System.Text.Encoding.Unicode;
                            Console.WriteLine($"Вы успешно авторизованы {user.Name} (⁀ᗢ⁀)");
                            if (user.Login == "admin" && user.Password == "admin")
                            {
                                Console.OutputEncoding = System.Text.Encoding.Unicode;
                                Console.WriteLine("✬ Вы вошли как администратор ✬");
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.ReadKey();
                            return true;
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                Console.WriteLine("Вас нет в базе данных (╯°□°）╯︵ ┻━┻");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
                return false;
            }
            public static bool IsUserExist(string login, string password)
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файла нет");
                    return false;
                }

                using (StreamReader sr = File.OpenText(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] userCredentials = line.Split(',');
                        if (userCredentials.Length != 3)
                        {
                            continue;
                        }
                        if (userCredentials[1] == login && userCredentials[2] == password)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public class Question
        {
            public string Text { get; set; }
            public List<string> Options { get; set; }
            public string Answer { get; set; }
        }
        public class Quiz
        {
            private string _category;
            private List<Question> _questions;
            public Quiz(List<Question> questions)
            {
                _questions = questions;
            }

            public int Run()
            {
                int score = 0;
                bool Test = false;
                char answer = '\0';
                foreach (Question question in _questions)
                {
                    while (!Test)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine(question.Text);
                        foreach (string option in question.Options)
                        {
                            Console.WriteLine(option);
                        }
                        Console.Write("Ответ: ");
                        try
                        {
                            answer = Convert.ToChar(Console.ReadLine());
                            if (answer.ToString().Length != 1)
                            {
                                throw new FormatException("Длина строки должна составлять один знак.");
                            }
                            if (answer == '1' || answer == '2' || answer == '3' || answer == '4')
                            {
                                Test = true;
                            }
                            else
                            {
                                throw new Exception("Вы ввели некорректное значение");
                            }
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\n" + ex.Message + "\n");
                        }
                        Console.Clear();

                    }
                    Test = false;
                    if (answer == question.Answer.ToCharArray()[0])
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.OutputEncoding = System.Text.Encoding.Unicode;
                        Console.WriteLine("Верно! (⁀ᗢ⁀)");
                        score++;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.OutputEncoding = System.Text.Encoding.Unicode;
                        Console.WriteLine("Неверно (╯°□°）╯︵ ┻━┻ Правильный ответ: " + question.Answer);
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Ваш счет: " + score + "/" + _questions.Count);
                return score;
            }
        }
        public static List<Question> ReadQuestionsFromXml(string filename)
        {
            List<Question> questions = null;

            XmlSerializer serializer = new XmlSerializer(typeof(List<Question>));
            FileStream stream = new FileStream(filename, FileMode.Open);

            try
            {
                questions = (List<Question>)serializer.Deserialize(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                stream.Close();
            }

            return questions;
        }
        public static List<Question> Biology()
        {
            List<Question> biologyQuestions = ReadQuestionsFromXml("biology.xml");

            return biologyQuestions;
        }
        public static List<Question> History()
        {
            List<Question> historyQuestions = ReadQuestionsFromXml("history.xml");

            return historyQuestions;
        }
        public static List<Question> Geography()
        {
            List<Question> geographyQuestions = ReadQuestionsFromXml("geography.xml");

            return geographyQuestions;
        }
        public static List<Question> Sundry()
        {
            List<Question> sundryQuestions = ReadQuestionsFromXml("sundry.xml");

            return sundryQuestions;
        }
        ///

        public static bool IsValidPassword(string password)
        {
            if (password.Length < 4 || password.Length > 8)
            {
                return false;
            }

            if (!Regex.IsMatch(password, "[A-Z]|[А-Я]"))
            {
                return false;
            }

            return true;
        }
        public static void SortLider(string questions)
        {
            string filePath = $"{questions}Results.txt";
            List<string> lines = File.ReadAllLines(filePath).ToList();

            List<KeyValuePair<string, int>> values = new List<KeyValuePair<string, int>>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(new[] { '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2) continue;
                string name = parts[0].Trim();
                int percent = int.Parse(parts[1].Replace("%", "").Trim());

                values.Add(new KeyValuePair<string, int>(name, percent));
            }

            values = values.OrderByDescending(x => x.Value).ToList();

            lines.Clear();

            foreach (KeyValuePair<string, int> kvp in values)
            {
                string line = kvp.Key + " - " + kvp.Value.ToString() + "%";
                lines.Add(line);
            }

            int i = 0;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine($"\t★☆★ Топ-20 ★☆★");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (KeyValuePair<string, int> kvp in values)
            {
                i++;
                string line = kvp.Key + " - " + kvp.Value.ToString() + "%";
                Console.WriteLine($"{i}.{line}");
                if (i > 20)
                    return;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine("★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★★☆★☆★☆★☆★☆");
            Console.ForegroundColor = ConsoleColor.White;
            File.WriteAllLines(filePath, lines);

        }
        public static void WriteToResultFile(int rating, User user, string questions)
        {
            string fileAnswer = $"{questions}Results.txt";

            using (StreamWriter sw = File.AppendText(fileAnswer))
            {
                sw.WriteLine($"{user.Name} - {(double)rating / 20 * 100}%");
            }

        }
        public static void CheckForAdmin(List<Question> questions, User user, string file)
        {

            if (user.Name == "admin" && user.Password == "admin")
            {
                int? num = null;
                while (num != 1 || num != 2)
                {
                    Console.Clear();
                    Console.WriteLine("Желаете видоизменить данную викторину?\n1.Да\n2.Нет");
                    try
                    {
                        num = Convert.ToInt32(Console.ReadLine());
                        if (num.ToString().Length != 1)
                        {
                            throw new FormatException("Длина строки должна составлять один знак.");
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    if (num == 1)
                    {
                        ModifyQuestions(file);
                        ActionChoice(user);
                    }
                    if (num == 2)
                    {
                        return;
                    }
                }
            }
        }
        public static void QuizChoice(User user)
        {
            Console.WriteLine("Выберите викторину\n1.История Украины\n2.Биология\n3.География\n4.Разное");
            int value = Convert.ToInt32(Console.ReadLine());
            Quiz quiz = null;
            Console.Clear();
            switch (value)
            {
                case 1:
                    CheckForAdmin(History(), user, "history");
                    quiz = new Quiz(History());
                    WriteToResultFile(quiz.Run(), user, "History");
                    break;
                case 2:
                    CheckForAdmin(Biology(), user, "biology");
                    quiz = new Quiz(Biology());
                    WriteToResultFile(quiz.Run(), user, "Biology"); ;
                    break;
                case 3:
                    CheckForAdmin(Geography(), user, "geography");
                    quiz = new Quiz(Geography());
                    WriteToResultFile(quiz.Run(), user, "Geography");
                    break;
                case 4:
                    CheckForAdmin(Sundry(), user, "sundry");
                    quiz = new Quiz(Sundry());
                    WriteToResultFile(quiz.Run(), user, "Sundry");
                    break;
                default:
                    Console.WriteLine("Некорректный ввод");
                    QuizChoice(user);
                    break;
            }
        }
        public static void ShowResults(string pathFile, User user)
        {

            if (!File.Exists(pathFile))
            {
                Console.WriteLine("Файла нет");
                return;
            }

            using (StreamReader sr = File.OpenText(pathFile))
            {
                string line;
                bool check = true;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] userCredentials = line.Split(' ');
                    if (userCredentials.Length != 3)
                    {
                        continue;
                    }
                    if (userCredentials[0] == user.Name)
                    {
                        Console.WriteLine($"{userCredentials[0]} {userCredentials[1]} {userCredentials[2]}");
                        check = false;
                    }
                }
                if (check)
                {
                    Console.WriteLine("Вы не проходили данную викторину");
                }
            }
        }
        public static void PastQuizResults(User user)
        {
            string fileHistory = "HistoryResults.txt";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("* Результаты по истории");
            Console.ForegroundColor = ConsoleColor.White;
            ShowResults(fileHistory, user);

            string fileBiology = "BiologyResults.txt";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("* Результаты по биологии");
            Console.ForegroundColor = ConsoleColor.White;
            ShowResults(fileBiology, user);

            string fileGeography = "GeographyResults.txt";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("* Результаты по географии");
            Console.ForegroundColor = ConsoleColor.White;
            ShowResults(fileGeography, user);

            string filSundry = "SundryResults.txt";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("* Результаты по разному");
            Console.ForegroundColor = ConsoleColor.White;
            ShowResults(filSundry, user);
        }
        public static void LeaderboardSelection()
        {
            bool test = false;
            int value = 0;
            while (!test)
            {
                Console.Clear();
                Console.WriteLine("По какой викторине вы хотите увидеть таблицу лидеров\n1.История Украины\n2.Биология\n3.География\n4.Разное");
                try
                {
                    value = Convert.ToInt32(Console.ReadLine());
                    if (value.ToString().Length != 1)
                    {
                        throw new FormatException("Длина строки должна составлять один знак.");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                switch (value)
                {
                    case 1:
                        SortLider("History");
                        return;
                    case 2:
                        SortLider("Biology");
                        return;
                    case 3:
                        SortLider("Geography");
                        return;
                    case 4:
                        SortLider("Sundry");
                        return;
                    default:
                        Console.WriteLine("Некорректный ввод");
                        LeaderboardSelection();
                        return;
                }
            }
        }
        public static void ChangeCredentials(User user)
        {
            string filePath = "users.txt";
            bool IsLook = false;

            string newUsername = null;
            string newPassword = null;
            while (!IsLook)
            {
                Console.Clear();
                newUsername = null;
                newPassword = null;
                Console.WriteLine("1.Скрыть пароль(◠‿◠)   2.Показать пароль(◕‿◕)");
                int newNum = Convert.ToInt32(Console.ReadLine());
                if (newNum == 1)
                {
                    Console.Write("Введите новый логин -> ");
                    ConsoleKeyInfo key;
                    do
                    {
                        key = Console.ReadKey(true);

                        if (key.Key != ConsoleKey.Enter)
                        {
                            newUsername += key.KeyChar;
                            Console.Write("*");
                        }
                    } while (key.Key != ConsoleKey.Enter);
                    Console.WriteLine();
                    Console.Write("Введите новый пароль -> ");
                    ConsoleKeyInfo key1;
                    do
                    {
                        key1 = Console.ReadKey(true);

                        if (key1.Key != ConsoleKey.Enter)
                        {
                            newPassword += key1.KeyChar;
                            Console.Write("*");
                        }
                    } while (key1.Key != ConsoleKey.Enter);
                    Console.WriteLine();
                    
                    if (!IsValidPassword(newPassword))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.OutputEncoding = System.Text.Encoding.Unicode;
                        Console.WriteLine("Пароль не соответствует требованиям (╯°□°）╯︵ ┻━┻:");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine("- должен быть не меньше 4 символов и не больше 8");
                        Console.WriteLine("- должна быть хотя бы одна заглавная буква");
                        Console.ReadLine();
                    }
                    else
                    {
                        IsLook = true;
                    }
                    
                }
                if (newNum == 2)
                {
                    Console.Clear();
                    Console.Write("Введите новый логин -> ");
                    newUsername = Console.ReadLine();

                    Console.Write("Введите новый пароль -> ");
                    newPassword = Console.ReadLine();
                    if (!IsValidPassword(newPassword))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.OutputEncoding = System.Text.Encoding.Unicode;
                        Console.WriteLine("Пароль не соответствует требованиям (╯°□°）╯︵ ┻━┻:");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine("- должен быть не меньше 4 символов и не больше 8");
                        Console.WriteLine("- должна быть хотя бы одна заглавная буква");
                        Console.ReadLine();
                    }
                    else
                    {
                        IsLook = true;
                    }
                }
            }
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] users = lines[i].Split(',');
                string username = users[0];
                string password = users[1];

                if (username == user.Name)
                {
                    users[1] = newUsername;
                    users[2] = newPassword;

                    lines[i] = string.Join(",", users);
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Вы успешно изменили пароль и логин {user.Name}");
            Console.ForegroundColor = ConsoleColor.White;
            File.WriteAllLines(filePath, lines);
        }

        public static void ActionChoice(User user)
        {
            int num = 0;
            bool test = false;
            Console.Clear();
            while (!test)
            {
                Console.WriteLine("1.Cтартовать новую викторину\n2.Посмотреть результаты своих прошлых викторин\n3.Посмотреть Топ-20 по конкретной викторине\n4.Настройки\n5.Выход");
                try
                {
                    num = Convert.ToInt32(Console.ReadLine());
                    if (num.ToString().Length != 1)
                    {
                        throw new FormatException("Длина строки должна составлять один знак.");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.Clear();

                if (num == 1)
                {
                    QuizChoice(user);
                    Console.ReadKey();
                    Console.Clear();
                    ActionChoice(user);
                }
                if (num == 2)
                {
                    PastQuizResults(user);
                    Console.ReadKey();
                    Console.Clear();
                    ActionChoice(user);
                }
                if (num == 3)
                {
                    LeaderboardSelection();
                    Console.ReadKey();
                    Console.Clear();
                    ActionChoice(user);
                }
                if (num == 4)
                {
                    int a = 0;
                    //bool test1 = false;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Желаете изминить логин и пароль?\n1.Да\n2.Нет");
                        try
                        {
                            a = Convert.ToInt32(Console.ReadLine());
                            if (a.ToString().Length != 1)
                            {
                                throw new FormatException("Длина строки должна составлять один знак.");
                            }
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        if (a == 1)
                        {
                            ChangeCredentials(user);
                            Console.ReadKey();
                            Console.Clear();
                            ActionChoice(user);
                        }
                        if (a == 2)
                        {
                            ActionChoice(user);
                        }
                    }

                }
                if (num == 5)
                {
                    Console.WriteLine("До скорых встреч :)");
                    Environment.Exit(0);
                }
            }

        }
        public static bool Authorization(User user)
        {
            bool authorization = false;
            bool IsInvalid = false;
            while (!IsInvalid)
            {
                int num = 0;
                Console.Clear();
                Console.WriteLine("1.Авторизация\n2.Регистрация\n3.Выход");
                try
                {
                    num = Convert.ToInt32(Console.ReadLine());
                    if (num.ToString().Length != 1)
                    {
                        throw new FormatException("Длина строки должна составлять один знак.");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (num == 1)
                {
                    bool IsLook = false;
                    while (!IsLook)
                    {
                        Console.Clear();
                        int newNum = 0;
                        user.Login = null;
                        user.Password = null;
                        Console.OutputEncoding = System.Text.Encoding.Unicode;
                        Console.WriteLine("1.Скрыть пароль(◠‿◠)   2.Показать пароль(◕‿◕)");
                        try
                        {
                            newNum = Convert.ToInt32(Console.ReadLine());
                            if (num.ToString().Length != 1)
                            {
                                throw new FormatException("Длина строки должна составлять один знак.");
                            }
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        if (newNum == 1)
                        {
                            Console.Clear();
                            Console.Write("Введите ваш логин -> ");
                            ConsoleKeyInfo key;
                            do
                            {
                                key = Console.ReadKey(true);

                                if (key.Key != ConsoleKey.Enter)
                                {
                                    user.Login += key.KeyChar;
                                    Console.Write("*");
                                }
                            } while (key.Key != ConsoleKey.Enter);
                            Console.WriteLine();
                            Console.Write("Введите ваш пароль -> ");
                            ConsoleKeyInfo key1;
                            do
                            {
                                key1 = Console.ReadKey(true);

                                if (key1.Key != ConsoleKey.Enter)
                                {
                                    user.Password += key1.KeyChar;
                                    Console.Write("*");
                                }
                            } while (key1.Key != ConsoleKey.Enter);
                            Console.WriteLine();
                            IsLook = true;
                        }
                        if (newNum == 2)
                        {
                            Console.Clear();
                            Console.Write("Введите ваш логин -> ");
                            user.Login = Console.ReadLine();

                            Console.Write("Введите ваш пароль -> ");
                            user.Password = Console.ReadLine();
                            IsLook = true;
                        }
                    }
                    authorization = UserService.IsUserAuthenticated(user);
                    if (authorization)
                    {
                        IsInvalid = true;
                    }
                }
                if (num == 2)
                {
                    Console.Clear();
                    authorization = UserService.InitPassword(user);
                    if (authorization)
                    {
                        IsInvalid = true;
                    }
                }
                if (num == 3)
                {
                    Console.WriteLine("До скорых встреч :)");
                }
            }
            return authorization;

        }
        public static void Interface()
        {
            User user = new User();
            if (Authorization(user))
            {
                ActionChoice(user);
            }
        }
        /// 

        public static void ModifyQuestions(string file)
        {
            List<Question> questions = ReadQuestionsFromXml($"{file}.xml");
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить вопрос");
                Console.WriteLine("2. Изменить вопрос");
                Console.WriteLine("3. Удалить вопрос");
                Console.WriteLine("4. Выйти");

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                {
                    Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                }

                switch (choice)
                {
                    case 1:
                        AddQuestion(questions);
                        break;
                    case 2:
                        ModifyQuestion(questions);
                        break;
                    case 3:
                        DeleteQuestion(questions);
                        break;
                    case 4:
                        WriteQuestionsToXml(questions, $"{file}.xml");
                        return;
                }
            }
        }
        public static void WriteQuestionsToXml(List<Question> questions, string fileName)
        {
            XElement root = new XElement("ArrayOfQuestion");

            foreach (Question q in questions)
            {
                XElement question = new XElement("Question");
                question.Add(new XElement("Text", q.Text));
                XElement options = new XElement("Options");
                foreach (string o in q.Options)
                {
                    options.Add(new XElement("string", o));
                }
                question.Add(options);
                question.Add(new XElement("Answer", q.Answer));
                root.Add(question);
            }

            XDocument doc = new XDocument(root);
            doc.Save(fileName);
        }
        private static void AddQuestion(List<Question> questions)
        {
            Console.Clear();
            Console.WriteLine("Введите текст вопроса:");
            string text = Console.ReadLine();

            List<string> options = new List<string>();
            while (true)
            {
                Console.WriteLine("Введите вариант ответа или нажмите Enter, чтобы закончить:");
                string option = Console.ReadLine();
                if (string.IsNullOrEmpty(option))
                {
                    break;
                }
                options.Add(option);
            }

            Console.WriteLine("Введите правильный ответ:");
            string answer = Console.ReadLine();

            Question question = new Question
            {
                Text = text,
                Options = options,
                Answer = answer
            };

            questions.Add(question);
            Console.WriteLine("Вопрос добавлен.");
        }
        private static void ModifyQuestion(List<Question> questions)
        {
            Console.Clear();
            Console.WriteLine("Введите номер вопроса, который хотите изменить:");
            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {questions[i].Text}");
            }

            int index;
            while (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > questions.Count)
            {
                Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
            }

            Question question = questions[index - 1];

            Console.WriteLine("Введите новый текст вопроса или нажмите Enter, чтобы оставить без изменений:");
            string text = Console.ReadLine();
            if (!string.IsNullOrEmpty(text))
            {
                question.Text = text;
            }

            Console.WriteLine("Введите новые варианты ответа или нажмите Enter, чтобы оставить без изменений:");
            List<string> options = new List<string>();
            while (true)
            {
                Console.WriteLine("Введите вариант ответа или нажмите Enter, чтобы закончить:");
                string option = Console.ReadLine();
                if (string.IsNullOrEmpty(option))
                {
                    break;
                }
                options.Add(option);
            }
            if (options.Count > 0)
            {
                question.Options = options;
            }

            Console.WriteLine("Введите новый правильный ответ или нажмите Enter, чтобы оставить без изменений:");
            string answer = Console.ReadLine();
            if (!string.IsNullOrEmpty(answer))
            {
                question.Answer = answer;
            }

            Console.WriteLine("Вопрос изменен.");
        }
        public static void DeleteQuestion(List<Question> questions)
        {
            Console.Clear();
            Console.WriteLine("Введите номер вопроса, который хотите удалить:");
            int index = int.Parse(Console.ReadLine()) - 1;

            if (index >= 0 && index < questions.Count)
            {
                questions.RemoveAt(index);
                Console.WriteLine("Вопрос успешно удален.");
            }
            else
            {
                Console.WriteLine("Неверный номер вопроса.");
            }
        }
        static void Main(string[] args)
        {
            Interface();
        }
    }
}

