using System;
using System.Collections.Generic;
using System.Linq;

public enum EntityType
{
    Student,
    Teacher,
    Course
}

public abstract class BaseEntityData
{
    public string Name { get; protected set; }
    
    public BaseEntityData(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя не может быть пустым.");
        Name = name;
    }

    public virtual string GetInfo()
    {
        return $"Имя: {Name}";
    }
}

public class TeacherData : BaseEntityData
{
    public string CourseName { get; protected set; } = "Не назначен";
    
    public TeacherData(string name) : base(name) {}

    public void AssignCourse(string courseName)
    {
        if (string.IsNullOrWhiteSpace(courseName)) return;
        CourseName = courseName;
    }

    public override string GetInfo()
    {
        return $"{base.GetInfo()}, Роль: Преподаватель, Ведет курс: {CourseName}";
    }
}

public class StudentData : BaseEntityData
{
    public List<string> Courses { get; private set; } = new(); 

    public StudentData(string name) : base(name) {}

    public void EnrollCourse(string courseName)
    {
        if (!string.IsNullOrWhiteSpace(courseName) && !Courses.Contains(courseName))
        {
            Courses.Add(courseName);
        }
    }

    public override string GetInfo()
    {
        return $"{base.GetInfo()}, Роль: Студент, Курсов: {Courses.Count}";
    }
}

public class CourseData
{
    public string Title { get; private set; }
    public string TeacherName { get; private set; } = "Не назначен";
    public List<string> Students { get; private set; } = new();

    public CourseData(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название курса не может быть пустым.");
        Title = title;
    }

    public void SetTeacher(string teacherName)
    {
        TeacherName = teacherName;
    }

    public void AddStudent(string studentName)
    {
        if (!Students.Contains(studentName))
        {
            Students.Add(studentName);
        }
    }

    public string GetInfo()
    {
        return $"Курс: {Title}, Преподаватель: {TeacherName}, Записано студентов: {Students.Count}";
    }
}

public class UniversityManager
{
    private readonly List<StudentData> _students = new();
    private readonly List<TeacherData> _teachers = new();
    private readonly List<CourseData> _courses = new();

    public void AddStudent(string name) => _students.Add(new StudentData(name));
    public void AddTeacher(string name) => _teachers.Add(new TeacherData(name));
    public void AddCourse(string title) => _courses.Add(new CourseData(title));

    public StudentData GetStudent(string name) => _students.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    public TeacherData GetTeacher(string name) => _teachers.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    public CourseData GetCourse(string title) => _courses.FirstOrDefault(c => c.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

    public bool EnrollStudentInCourse(string studentName, string courseTitle)
    {
        var student = GetStudent(studentName);
        var course = GetCourse(courseTitle);

        if (student == null || course == null) return false;

        student.EnrollCourse(courseTitle);
        course.AddStudent(studentName);
        return true;
    }

    public bool AssignTeacherToCourse(string teacherName, string courseTitle)
    {
        var teacher = GetTeacher(teacherName);
        var course = GetCourse(courseTitle);

        if (teacher == null || course == null) return false;

        teacher.AssignCourse(courseTitle);
        course.SetTeacher(teacherName);
        return true;
    }

    public void DisplayAll(EntityType type)
    {
        Console.WriteLine("\n--- Список ---");
        IEnumerable<BaseEntityData> data;

        if (type == EntityType.Student)
        {
            data = _students;
            Console.WriteLine("ВСЕ СТУДЕНТЫ:");
        }
        else if (type == EntityType.Teacher)
        {
            data = _teachers;
            Console.WriteLine("ВСЕ ПРЕПОДАВАТЕЛИ:");
        }
        else
        {
            Console.WriteLine("ВСЕ КУРСЫ:");
            foreach (var course in _courses)
            {
                Console.WriteLine($"- {course.GetInfo()}");
            }
            return;
        }

        if (!data.Any())
        {
            Console.WriteLine("Список пуст.");
        }
        else
        {
            foreach (var entity in data)
            {
                Console.WriteLine($"- {entity.GetInfo()}");
            }
        }
    }
}

public class Program
{
    private static readonly UniversityManager Manager = new();

    public static void Main(string[] args)
    {
        SeedData();
        
        bool isRunning = true;
        while (isRunning)
        {
            DisplayMenu();
            string input = Console.ReadLine();
            
            if (int.TryParse(input, out int choice))
            {
                isRunning = ProcessChoice(choice);
            }
            else
            {
                Console.WriteLine("Неверный ввод. Пожалуйста, введите число.");
            }
            
            Console.WriteLine("\nНажмите Enter для продолжения...");
            Console.ReadLine();
        }
    }

    private static bool ProcessChoice(int choice)
    {
        switch (choice)
        {
            case 1: AddNewEntity(); break;
            case 2: EnrollStudent(); break;
            case 3: AssignTeacher(); break;
            case 4: ViewPersonInfo(); break;
            case 5: ViewCourseInfo(); break;
            case 6: ViewAllLists(); break;
            case 0: return false;
            default: Console.WriteLine("Неизвестная команда. Попробуйте снова."); break;
        }
        return true;
    }
    
    private static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("===================================");
        Console.WriteLine("  СИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ");
        Console.WriteLine("===================================");
        Console.WriteLine("1. Добавить Студента/Преподавателя/Курс");
        Console.WriteLine("2. Записать студента на курс");
        Console.WriteLine("3. Назначить преподавателя на курс");
        Console.WriteLine("4. Просмотр информации о Студенте/Преподавателе");
        Console.WriteLine("5. Просмотр информации о Курсе и списке студентов");
        Console.WriteLine("6. Вывести полный список (Студенты/Преподаватели/Курсы)");
        Console.WriteLine("0. Выход");
        Console.Write("Ваш выбор: ");
    }
    
    private static void AddNewEntity()
    {
        Console.WriteLine("\nЧто добавить? (1 - Студент, 2 - Преподаватель, 3 - Курс):");
        string typeInput = Console.ReadLine();
        Console.Write("Введите имя/название: ");
        string name = Console.ReadLine();

        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("⚠️ Ошибка: Имя не может быть пустым.");
                return;
            }
            
            if (typeInput == "1") Manager.AddStudent(name);
            else if (typeInput == "2") Manager.AddTeacher(name);
            else if (typeInput == "3") Manager.AddCourse(name);
            else Console.WriteLine("Неверный тип.");
            
            Console.WriteLine("Успешно добавлено.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
    
    private static void EnrollStudent()
    {
        Console.Write("Имя студента: ");
        string studentName = Console.ReadLine();
        Console.Write("Название курса: ");
        string courseTitle = Console.ReadLine();

        if (Manager.EnrollStudentInCourse(studentName, courseTitle))
        {
            Console.WriteLine($"Студент {studentName} успешно записан на курс '{courseTitle}'.");
        }
        else
        {
            Console.WriteLine("Ошибка: Студент или курс не найден. Проверьте имена.");
        }
    }
    
    private static void AssignTeacher()
    {
        Console.Write("Имя преподавателя: ");
        string teacherName = Console.ReadLine();
        Console.Write("Название курса для назначения: ");
        string courseTitle = Console.ReadLine();

        if (Manager.AssignTeacherToCourse(teacherName, courseTitle))
        {
            Console.WriteLine($"Преподаватель {teacherName} успешно назначен на курс '{courseTitle}'.");
        }
        else
        {
            Console.WriteLine("Ошибка: Преподаватель или курс не найден. Проверьте имена.");
        }
    }
    
    private static void ViewPersonInfo()
    {
        Console.WriteLine("Что посмотреть? (1 - Студент, 2 - Преподаватель):");
        string typeInput = Console.ReadLine();
        Console.Write("Введите имя: ");
        string name = Console.ReadLine();

        BaseEntityData entity = null;
        if (typeInput == "1") entity = Manager.GetStudent(name);
        else if (typeInput == "2") entity = Manager.GetTeacher(name);

        if (entity == null)
        {
            Console.WriteLine("Ошибка: Сущность не найдена.");
            return;
        }

        Console.WriteLine($"\n--- Информация ---");
        Console.WriteLine(entity.GetInfo());
        
        if (entity is StudentData student)
        {
            Console.WriteLine("Записан на курсы:");
            if (student.Courses.Any())
            {
                foreach (var course in student.Courses)
                {
                    Console.WriteLine($"- {course}");
                }
            }
            else
            {
                Console.WriteLine("- Нет записей.");
            }
        }
    }
    
    private static void ViewCourseInfo()
    {
        Console.Write("Введите название курса: ");
        string courseTitle = Console.ReadLine();
        var course = Manager.GetCourse(courseTitle);

        if (course == null)
        {
            Console.WriteLine("Ошибка: Курс не найден.");
            return;
        }

        Console.WriteLine($"\n--- Детали курса '{course.Title}' ---");
        Console.WriteLine(course.GetInfo());
        Console.WriteLine("Студенты на курсе:");
        
        if (course.Students.Any())
        {
            foreach (var studentName in course.Students)
            {
                Console.WriteLine($"- {studentName}");
            }
        }
        else
        {
            Console.WriteLine("- Студентов нет.");
        }
    }
    
    private static void ViewAllLists()
    {
        Console.WriteLine("Какой список показать? (1 - Студенты, 2 - Преподаватели, 3 - Курсы):");
        string input = Console.ReadLine();
        
        if (input == "1") Manager.DisplayAll(EntityType.Student);
        else if (input == "2") Manager.DisplayAll(EntityType.Teacher);
        else if (input == "3") Manager.DisplayAll(EntityType.Course);
        else Console.WriteLine("Неверный выбор.");
    }
    
    private static void SeedData()
    {
        Manager.AddStudent("Анна Смирнова");
        Manager.AddStudent("Иван Петров");
        Manager.AddTeacher("Дмитрий Сидоров");
        Manager.AddTeacher("Елена Кузнецова");
        Manager.AddCourse("Введение в C#");
        Manager.AddCourse("Теория баз данных");

        Manager.AssignTeacherToCourse("Дмитрий Сидоров", "Введение в C#");
        Manager.AssignTeacherToCourse("Елена Кузнецова", "Теория баз данных");
        
        Manager.EnrollStudentInCourse("Анна Смирнова", "Введение в C#");
        Manager.EnrollStudentInCourse("Анна Смирнова", "Теория баз данных");
        Manager.EnrollStudentInCourse("Иван Петров", "Введение в C#");
    }
}