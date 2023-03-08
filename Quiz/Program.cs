using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Quiz.Program;
using static System.Net.Mime.MediaTypeNames;

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
                    Console.WriteLine("Пользователь с таким логином и паролем уже существует");
                    return false;
                }
                SaveUser(user);
                return true;
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
                            Console.WriteLine($"Вы успешно авторизованы {user.Name}");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.ReadKey();
                            return true;
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Вас нет в базе данных");
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
                    Console.Clear();
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
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Ваш счет: " + score + "/" + _questions.Count);
                return score;
            }
        }

        public static List<Question> Biology()
        {
            List<Question> historyQuestions = new List<Question>();
            historyQuestions.Add(new Question
            {
                Text = "Какое вещество обеспечивает жизнедеятельность всех клеток на Земле?",
                Options = new List<string> { "1.Кислород", "2.Углекислый газ", "3.Вода", "4.Азот" },
                Answer = "3.Вода"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая молекула является основной формой энергии для живых организмов?",
                Options = new List<string> { "1.АТФ", "2.РНК", "3.ДНК", "4.Глюкоза" },
                Answer = "1.АТФ (аденозинтрифосфат) "
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой процесс ответственен за передачу генетической информации от родителей к потомству?",
                Options = new List<string> { "1.Фотосинтез", "2.Репликация ДНК", "3.Транскрипция РНК", "4.Трансляция РНК" },
                Answer = "2.Репликация ДНК"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой органеллой является место проведения синтеза белков?",
                Options = new List<string> { "1.Лизосома", "2.Гольджи", "3.Рибосома", "4.Митохондрия" },
                Answer = "3.Рибосома"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой тип клеток обеспечивает иммунитет в организме человека?",
                Options = new List<string> { "1.Мышечные клетки", "2.Нейроны", "3.Красные кровяные клетки", "4.Белые кровяные клетки" },
                Answer = "4.Белые кровяные клетки (Лейкоциты)"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая часть растения отвечает за производство питательных веществ?",
                Options = new List<string> { "1.Корневая система", "2.Листья", "3.Стебель", "4.Цветки" },
                Answer = "2.Листья"
            });
            historyQuestions.Add(new Question
            {
                Text = "Как называется процесс, при котором растения используют энергию солнечного света для производства питательных веществ?",
                Options = new List<string> { "1.Фотосинтез", "2.Респирация", "3.Ассимиляция", "4.Окисление" },
                Answer = "1.Фотосинтез"
            });
            historyQuestions.Add(new Question
            {
                Text = "Как называется образование новых клеток в организме?",
                Options = new List<string> { "1.Дифференциация", "2.Специализация", "3.Репликация", "4.Митоз" },
                Answer = "4.Митоз"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой орган отвечает за производство инсулина?",
                Options = new List<string> { "1.Печень", "2.Поджелудочная железа", "3.Почки", "4.Сердце" },
                Answer = "2.Поджелудочная железа"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой орган отвечает за очищение крови от шлаков и отходов?",
                Options = new List<string> { "1.Сердце", "2.Легкие", "3.Почки", "4.Печень" },
                Answer = "3.Почки"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой элемент необходим для образования гемоглобина, отвечающего за транспортировку кислорода в организме?",
                Options = new List<string> { "1.Железо", "2.Кальций", "3.Фосфор", "4.Магний" },
                Answer = "1.Железо"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой из следующих процессов не является формой асексуального размножения?",
                Options = new List<string> { "1.Двоение", "2.Отросток", "3.Партеногенез", "4.Мейоз" },
                Answer = "4.Мейоз"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая группа животных является позвоночными, но не имеет челюстей?",
                Options = new List<string> { "1.Рыбы", "2.Амфибии", "3.Рептилии", "4.Круглоротые" },
                Answer = "1.Рыбы"
            });
            historyQuestions.Add(new Question
            {
                Text = "Как называется процесс, при котором организм перестраивается для выживания в условиях нехватки питательных веществ?",
                Options = new List<string> { "1.Адаптация", "2.Метаболизм", "3.Катаболизм", "4.Криптобиоз" },
                Answer = "1.Адаптация"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая часть мозга отвечает за координацию движений и равновесие?",
                Options = new List<string> { "1.Гипоталамус", "2.Мозжечок", "3.Гиппокамп", "4.Кора головного мозга" },
                Answer = "2.Мозжечок"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая группа животных отличается наличием хитина в своей клеточной стенке?",
                Options = new List<string> { "1.Членистоногие", "2.Моллюски", "3.Хордовые", "4.Круглые черви" },
                Answer = "1.Членистоногие"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой орган у кальмаров является аналогом кости у позвоночных животных?",
                Options = new List<string> { "1.Мантия", "2.Жабры", "3.Хоботок", "4.Хитиновая пластина" },
                Answer = "4.Хитиновая пластина"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какие органы у человека отвечают за балансирование и координацию движений?",
                Options = new List<string> { "1.Сердце и лёгкие", "2.Мозг и спинной мозг", "3.Печень и почки", "4.Желудок и кишечник" },
                Answer = "2.Мозг и спинной мозг"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая клетка является наименьшей по размеру?",
                Options = new List<string> { "1.Эритроцит", "2.Лейкоцит", "3.Бактерия", "4.Вирус" },
                Answer = "4.Вирус"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой из перечисленных типов мышечной ткани является инволюционной, то есть уменьшающейся в объеме со временем?",
                Options = new List<string> { "1.Скелетная мышечная ткань", "2.Гладкая мышечная ткань", "3.Кардиомиоциты", "4.Все типы мышечной ткани не являются инволюционными" },
                Answer = "2.Гладкая мышечная ткань"
            });
            return historyQuestions;
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
        public static List<Question> Sundry()
        {
            List<Question> historyQuestions = new List<Question>();
            historyQuestions.Add(new Question
            {
                Text = "Какой вид морской жизни является самым большим на планете?",
                Options = new List<string> { "1.Кит", "2.Кальмар", "3.Акула", "4.Медуза" },
                Answer = "1.Кит"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой океан самый глубокий на Земле?",
                Options = new List<string> { "1.Атлантический", "2.Индийский", "3.Тихий", "4.Южный" },
                Answer = "3.Тихий"
            });
            historyQuestions.Add(new Question
            {
                Text = "Кто является автором книги \"Гарри Поттер\"?",
                Options = new List<string> { "1.Дж. К. Роулин", "2.Джеймс Паттерсон", "3.Стивен Кинг", "4.Дэн Браун" },
                Answer = "1.Дж. К. Роулин"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая планета является самой близкой к Солнцу?",
                Options = new List<string> { "1.Марс", "2.Юпитер", "3.Венера", "4.Меркурий" },
                Answer = "4.Меркурий"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой орган является самым большим в человеческом теле?",
                Options = new List<string> { "1.Печень", "2.Сердце", "3.Легкие", "4.Кожа" },
                Answer = "4.Кожа"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой вид музыки был придуман в Ямайке?",
                Options = new List<string> { "1.Рэгги", "2.Рок", "3.Джаз", "4.Соул" },
                Answer = "1.Рэгги"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая страна производит самое большое количество шоколада в мире?",
                Options = new List<string> { "1.Швейцария", "2.Бельгия", "3.Франция", "4.Германия" },
                Answer = "1.Швейцария"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какое животное является символом Австралии?",
                Options = new List<string> { "1.Кенгуру", "2.Коала", "3.Эму", "4.Вомбат" },
                Answer = "1.Кенгуру"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой цвет имеет кровь кальмара?",
                Options = new List<string> { "1.Красный", "2.Зеленый", "3.Синий", "4.Черный" },
                Answer = "3.Синий"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какое растение используется для производства водки?",
                Options = new List<string> { "1.Виноград", "2.Кукуруза", "3.Ячмень", "4.Картофель" },
                Answer = "4.Картофель"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой воинственный народ заселял территорию современной Мексики?",
                Options = new List<string> { "1.Ацтеки", "2.Майя", "3.Инки", "4.Спартанцы" },
                Answer = "1.Ацтеки"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой монарх является длиннейшим правителем в истории Великобритании?",
                Options = new List<string> { "1.Елизавета II", "2.Виктория", "3.Генрих VIII", "4.Эдуард VII" },
                Answer = "1.Елизавета II"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой сорт яблок наиболее популярен в мире?",
                Options = new List<string> { "1.Красный Делишес", "2.Голден Делишес", "3.Фуджи", "4.Гренни Смит" },
                Answer = "2.Голден Делишес"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая страна является родиной шахмат?",
                Options = new List<string> { "1.Китай", "2.Турция", "3.Индия", "4.Иран" },
                Answer = "3.Индия"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой плотоядный зверь является самым быстрым на Земле?",
                Options = new List<string> { "1.Тигр", "2.Лев", "3.Гепард", "4.Пума" },
                Answer = "3.Гепард"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой главный инструмент в оркестре скрипачей?",
                Options = new List<string> { "1.Виолончель", "2.Контрабас", "3.Скрипка", "4.Арфа" },
                Answer = "3.Скрипка"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой музыкальный стиль был создан в США в 20-х годах XX века?",
                Options = new List<string> { "1.Рок-н-ролл", "2.Регги", "3.Джаз", "4.Блюз" },
                Answer = "3.Джаз"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой газ является самым распространенным в атмосфере Земли?",
                Options = new List<string> { "1.Кислород", "2.Азот", "3.Углекислый газ", "4.Аргон" },
                Answer = "2.Азот"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какая страна изображена на флаге Швейцарии?",
                Options = new List<string> { "1.Франция", "2.Италия", "3.Германия", "4.Австрия" },
                Answer = "4.Австрия"
            });
            historyQuestions.Add(new Question
            {
                Text = "Какой металл является самым драгоценным?",
                Options = new List<string> { "1.Золото", "2.Серебро", "3.Платина", "4.Медь" },
                Answer = "1.Золото"
            });
            return historyQuestions;
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

            int i = 0;
            foreach (KeyValuePair<string, int> kvp in values)
            {
                string line = kvp.Key + " - " + kvp.Value.ToString() + "%";
                lines.Add(line);
            }

            Console.WriteLine($"\t*** Топ-20 ***");
            foreach (KeyValuePair<string, int> kvp in values)
            {
                i++;
                string line = kvp.Key + " - " + kvp.Value.ToString() + "%";
                Console.WriteLine($"{i++}.{line}");
                if (i > 20)
                    return;
            }
            Console.WriteLine("__________________________________________");
            File.WriteAllLines(filePath, lines);

        }
        public static void WriteToResultFile(int rating, User user, string questions)
        {
            string fileAnswer = $"{questions}Results.txt";

            using (StreamWriter sw = File.AppendText(fileAnswer))
            {
                sw.WriteLine($"{user.Name} - {(double)rating / 20 * 100}");
            }

        }
        public static bool Authorization(User user)
        {
            bool authorization = false;
            bool IsInvalid = false;
            while (!IsInvalid)
            {
                Console.Clear();
                Console.WriteLine("1.Авторизация\n2.Регистрация\n3.Выход");
                int num = Convert.ToInt32(Console.ReadLine());
                if (num == 1)
                {
                    Console.Clear();
                    Console.Write("Введите ваш логин -> ");
                    user.Login = Console.ReadLine();
                    Console.Write("Введите ваш пароль -> ");
                    user.Password = Console.ReadLine();
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
        public static void QuizChoice(User user)
        {
            Console.WriteLine("Выберите викторину\n1.История Украины\n2.Биология\n3.География\n4.Разное");
            int value = Convert.ToInt32(Console.ReadLine());
            Quiz quiz = null;
            Console.Clear();
            switch (value)
            {
                case 1:
                    quiz = new Quiz(History());
                    WriteToResultFile(quiz.Run(), user, "History");
                    break;
                case 2:
                    quiz = new Quiz(Biology());
                    WriteToResultFile(quiz.Run(), user, "Biology"); ;
                    break;
                case 3:
                    quiz = new Quiz(Geography());
                    WriteToResultFile(quiz.Run(), user, "Geography");
                    break;
                case 4:
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
            Console.WriteLine("По какой викторине вы хотите увидеть таблицу лидеров\n1.История Украины\n2.Биология\n3.География\n4.Разное");
            int value = Convert.ToInt32(Console.ReadLine());
            switch (value)
            {
                case 1:
                    SortLider("History");
                    break;
                case 2:
                    SortLider("Biology");
                    break;
                case 3:
                    SortLider("Geography");
                    break;
                case 4:
                    SortLider("Sundry");
                    break;
                default:
                    Console.WriteLine("Некорректный ввод");
                    LeaderboardSelection();
                    break;
            }
        }
        public static void ChangeCredentials(User user)
        {
            string filePath = "users.txt";
            Console.Write("Введите новый логин -> ");
            string newUsername = Console.ReadLine();
            Console.Write("Введите новый пароль -> ");
            string newPassword = Console.ReadLine();

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
            Console.Clear();
            Console.WriteLine("1.Cтартовать новую викторину\n2.Посмотреть результаты своих прошлых викторин\n3.Посмотреть Топ-20 по конкретной викторине\n4.Настройки\n5.Выход");
            int num = Convert.ToInt32(Console.ReadLine());
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
                Console.WriteLine("Желаете изминить логин и пароль?\n1.Да\n2.Нет");
                int a = Convert.ToInt32(Console.ReadLine());
                if(a == 1)
                {
                    ChangeCredentials(user);
                    Console.ReadKey();
                    Console.Clear();
                    ActionChoice(user);
                }
                else
                ActionChoice(user);

            }
            if (num == 5)
            {
                Console.WriteLine("До скорых встреч :)");
            }

        }
        public static void Interface()
        {
            User user = new User();
            if (Authorization(user))
            {
                ActionChoice(user);
            }

        }
        static void Main(string[] args)
        {
            // Interface();
            int a = 10;
            int b = 2;
            Console.WriteLine(a * b);
        }
    }
}

