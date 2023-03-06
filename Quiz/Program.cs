using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Quiz.Program;

namespace Quiz
{
    internal class Program
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
            public static void InitPassword(User user)
            {
                Console.Write("Ввеедите ваше имя: ");
                user.Name = Console.ReadLine();
                Console.Write("Введите ваш логин: ");
                user.Login = Console.ReadLine();
                Console.Write("Введите ваш пароль: ");
                user.Password = Console.ReadLine();
                if (IsUserExist(user.Login, user.Password))
                {
                    Console.WriteLine("Пользователь с таким логином и паролем уже существует");
                    return;
                }
                SaveUser(user);

            }
            public static void SaveUser(User user)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine($"{user.Name},{user.Login},{user.Password}");
                }
            }
            public static bool IsUserAuthenticated(string login, string password)
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
                            Console.WriteLine("Вы успешно авторизованы");
                            return true;
                        }
                    }
                }
                Console.WriteLine("Вас нет в базе данных");
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
        /// 

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
                foreach (Question question in _questions)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(question.Text);
                    foreach (string option in question.Options)
                    {
                        Console.WriteLine(option);
                    }
                    Console.Write("Ответ: ");
                    char answer = Convert.ToChar(Console.ReadLine());

                    if (answer == question.Answer.ToCharArray()[0])
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.WriteLine("Верно!");
                        score++;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Неверно. Правильный ответ: " + question.Answer);
                    }
                }
                Console.WriteLine("Ваш счет: " + score + "/" + _questions.Count);
                return score;
            }
        }

        public static List<Question> History()
        {
            List<Question> historyQuestions = new List<Question>();
            historyQuestions.Add(new Question
            {
                Text = "В каком году была создана Киевская Русь?",
                Options = new List<string> { "1. 862 год", "2. 882 год", "3. 902 год", "4. 922 год" },
                Answer = "1.862 год"
            });
            historyQuestions.Add(new Question
            {
                Text = "Кто был первым князем Киевской Руси?",
                Options = new List<string> { "1.Олег", "2.Игорь", "3.Рюрик", "4.Олександр" },
                Answer = "3.Рюрик"
            });
            historyQuestions.Add(new Question
            {
                Text = "Кто был основателем Львовской школы?",
                Options = new List<string> { "1.Иван Франко", "2.Леся Українка", "3.Михайло Грушевський", "4.Франциск Скарбинский" },
                Answer = "4.Франциск Скарбинский"
            });
            historyQuestions.Add(new Question
            {
                Text = "В каком году Украина стала независимым государством?",
                Options = new List<string> { "1. 1991 год", "2. 1985 год", "3. 1993 год", "4. 1995 год" },
                Answer = "1. 1991 год"
            });
            historyQuestions.Add(new Question
            {
                Text = "Кто был первым президентом независимой Украины?",
                Options = new List<string> { "1.Виктор Янукович", "2.Леонид Кравчук", "3.Виктор Ющенко", "4.Петр Порошенко" },
                Answer = "2.Леонид Кравчук"
            });
            historyQuestions.Add(new Question
            {
                Text = "Кто был основателем украинского национального движения \"Революционная Украинская Партия\" в начале XX века?",
                Options = new List<string> { "1.Бандера Степан", "2.Петро Дорошенко", "3.Андрей Шептицкий", "4.Симон Петлюра" },
                Answer = "4.Симон Петлюра"
            });
            historyQuestions.Add(new Question
            {
                Text = "Кто стал лидером национально-освободительного движения в Западной Украине в 1940-х годах?",
                Options = new List<string> { "1.Бандера Степан", "2.Иван Мазепа", "3.Роман Шухевич", "4.Левко Лукьяненко" },
                Answer = "1.Бандера Степан"
            });
            historyQuestions.Add(new Question
            {
                Text = "В каком году была проведена Оранжевая революция в Украине?",
                Options = new List<string> { "1. 2000 год", "2. 2002 год", "3. 2004 год", "4. 2006 год" },
                Answer = "3. 2004 год"
            });
            historyQuestions.Add(new Question
            {
                Text = "Кто был лидером Оранжевой революции?",
                Options = new List<string> { "1.Юлия Тимошенко", "2.Виктор Янукович", "3.Виктор Ющенко", "4.Петр Порошенко" },
                Answer = "3.Виктор Ющенко"
            });
            historyQuestions.Add(new Question
            {
                Text = "В каком году произошла Революция гидности в Украине?",
                Options = new List<string> { "1. 2012 год", "2. 2013 год", "3. 2014 год", "4. 2015 год" },
                Answer = "3. 2014 год"
            });
            historyQuestions.Add(new Question
            {
                Text = "В каком году был подписан договор о создании Европейского Союза?",
                Options = new List<string> { "1. 1986 год", "2. 1991 год", "3. 1993 год", "4. 1999 год" },
                Answer = "4. 1999 год"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая партия победила на выборах в Украине в 2019 году?",
                Options = new List<string> { "1.Партия регионов", "2.Партия \"Слуга народа\"", "3.Батькивщина", "4.Самопомощь" },
                Answer = "2.Партия \"Слуга народа\""
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой президент был избран в Украине в 2019 году?",
                Options = new List<string> { "1.Петр Порошенко", "2.Юлия Тимошенко", "3.Владимир Зеленский", "4.Арсений Яценюк" },
                Answer = "3.Владимир Зеленский"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой украинский город был столицей гетманства?",
                Options = new List<string> { "1.Чернигов", "2.Киев", "3.Харьков", "4.Полтава" },
                Answer = "4.Полтава"
            });
            historyQuestions.Add(new Question
            {
                Text = "Кто является автором государственного гимна Украины?",
                Options = new List<string> { "1.Михаил Вербицкий", "2.Павел Чубинский", "3.Иван Франко", "4.Михаил Журавский" },
                Answer = "1.Михаил Вербицкий"
            });
            historyQuestions.Add(new Question
            {
                Text = "Кто является автором слов государственного гимна Украины??",
                Options = new List<string> { "1.Тарас Шевченко", "2.Иван Франко", "3.Павел Чубинский", "4.Михаил Старицкий" },
                Answer = "3.Павел Чубинский"
            });
            historyQuestions.Add(new Question
            {
                Text = "Как называется национальный праздник Украины, который отмечается 24 августа?",
                Options = new List<string> { "1.День Конституции", "2.День независимости", "3.День защитника Украины", "4.День единства Украины" },
                Answer = "2.День независимости"
            });
            historyQuestions.Add(new Question
            {
                Text = "Как назывался первый украинский государственный герб, принятый в 1918 году?",
                Options = new List<string> { "1.Тризуб", "2.Герб Киевского княжества", "3.Герб Великого княжества Литовского", "4.Вилы" },
                Answer = "1.Тризуб"
            });
            historyQuestions.Add(new Question
            {
                Text = "Как называется памятник, установленный на месте катастрофы Чернобыльской АЭС?",
                Options = new List<string> { "1.Памятник жертвам Чернобыля", "2.Чернобыльская колокольня", "3.Дуга", "4.Памятник \"Скорбящая матерь\"" },
                Answer = "4.Памятник \"Скорбящая матерь\""
            });
            historyQuestions.Add(new Question
            {
                Text = "Как называлась первая украинская космонавтка, отправившаяся в космос в 1997 году?",
                Options = new List<string> { "1.Людмила Павличенко", "2.Святослава Фёдорова", "3.Юлия Тимошенко", "4.Леся Украинка" },
                Answer = "2.Святослава Фёдорова"
            });
            return historyQuestions;
        }
        public static List<Question> Geography() 
        {
            List<Question> historyQuestions = new List<Question>();
            historyQuestions.Add(new Question
            {
                Text = "В какой стране находится Эверест?",
                Options = new List<string> { "1.Индия", "2.Непал", "3.Тибет", "4.Бутан" },
                Answer = "2.Непал"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какое море является самым соленным на Земле?",
                Options = new List<string> { "1.Красное море", "2.Аравийское море", "3.Каспийское море", "4.Мертвое море" },
                Answer = "4.Мертвое море"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какое вещество является наиболее распространенным в земной коре?",
                Options = new List<string> { "1.Железо", "2.Кремний", "3.Кальций", "4.Алюминий" },
                Answer = "2.Кремний"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая из этих стран расположена в Южной Америке?",
                Options = new List<string> { "1.Мексика", "2.Колумбия", "3.Венесуэла", "4.Чили" },
                Answer = "4.Чили"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какое озеро является самым глубоким в мире?",
                Options = new List<string> { "1.Байкал", "2.Каспийское", "3.Танганьика", "4.Мичиган" },
                Answer = "1.Байкал"
            });
            historyQuestions.Add(new Question
            {
                Text = "В каком государстве находится Сахара?",
                Options = new List<string> { "1.Алжир", "2.Тунис", "3.Египет", "4.Ливия" },
                Answer = "1.Алжир"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какое государство является самым маленьким по территории в мире?",
                Options = new List<string> { "1.Ватикан", "2.Монако", "3.Лихтенштейн", "4.Сан-Марино" },
                Answer = "1.Ватикан"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какое государство является самым населенным в мире?",
                Options = new List<string> { "1.Китай", "2.Индия", "3.США", "4.Япония" },
                Answer = "2.Индия"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая страна является наибольшим островом в мире?",
                Options = new List<string> { "1.Австралия", "2.Канада", "3.Индонезия", "4.Гренландия" },
                Answer = "4.Гренландия"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая из этих стран не имеет выхода к морю?",
                Options = new List<string> { "1.Боливия", "2.Узбекистан", "3.Австрия", "4.Монголия" },
                Answer = "3.Австри"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая из этих стран является крупнейшим производителем нефти в мире?",
                Options = new List<string> { "1.Саудовская Аравия", "2.россия", "3.США", "4.Иран" },
                Answer = "1.Саудовская Аравия"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какое море отделяет Испанию от Африки?",
                Options = new List<string> { "1.Адриатическое море", "2.Средиземное море", "3.Красное море", "4.Черное море" },
                Answer = "2.Средиземное море"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая страна является крупнейшим производителем кофе в мире?",
                Options = new List<string> { "1.Бразилия", "2.Индия", "3.Кения", "4.Эфиопия" },
                Answer = "1.Бразилия"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая из этих стран расположена на двух континентах?",
                Options = new List<string> { "1.Китай", "2.Турция", "3.Казахстан", "4.Узбекистан" },
                Answer = "2.Турция"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая страна является крупнейшим производителем риса в мире?",
                Options = new List<string> { "1.Индия", "2.Вьетнам", "3.Китай", "4.Япония" },
                Answer = "3.Китай"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая река является самой длинной в мире?",
                Options = new List<string> { "1.Амазонка", "2.Нил", "3.Янцзы", "4.Миссисипи" },
                Answer = "2.Нил"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой город является столицей Австралии?",
                Options = new List<string> { "1.Сидней", "2.Мельбурн", "3.Канберра", "4.Брисбен" },
                Answer = "3.Канберра"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая из этих пород образуется в результате остывания лавы на поверхности земли?",
                Options = new List<string> { "1.Известняк", "2.Гранит", "3.Базальт", "4.Кварцит" },
                Answer = "3.Базальт"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какое явление приводит к образованию геоморфологических форм, таких как каньоны и овраги?",
                Options = new List<string> { "1.Вулканизм", "2.Эрозия", "3.Метаморфизм", "4.Плутонизм" },
                Answer = "2.Эрозия"
            });
            historyQuestions.Add(new Question
            {
                Text = "Каково научное объяснение феномену затмения?",
                Options = new List<string> { "1.Затмение возникает из-за того, что Луна перекрывает Солнце;", "2.Затмение возникает из-за того, что Земля проходит между Солнцем и Луной", "3.Затмение происходит из-за преломления света в атмосфере Земли", "4.Затмение является следствием вращения Земли вокруг своей оси" },
                Answer = "2.Затмение возникает из-за того, что Земля проходит между Солнцем и Луной"
            });
            return historyQuestions;
        }

        public static void WriteToResultFile(int rating, User user)
        {
           string fileAnswer = "AnswerResults.txt";

            using (StreamWriter sw = File.AppendText(fileAnswer))
            {
                sw.WriteLine($"{user.Name} - {rating}/20");
            }

        }        
    
    static void Main(string[] args)
        {
            User user = new User();
            UserService.InitPassword(user);
            UserService.IsUserAuthenticated(user.Login, user.Password);

            Quiz quiz = new Quiz(History());
            int answer = quiz.Run();
            WriteToResultFile(answer, user);
        }
    }
}

