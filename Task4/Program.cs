using FinalTask;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Task4;
class Program
{

    static public void Main(string[] args)
    {
        var students = Deserialize("Students.dat");

        foreach (var student in students)
            Console.WriteLine($"{student.Name} [{student.DateOfBirth}]: {student.Group}");

        TryCreateDB(students);
    }

    private static void TryCreateDB(Student[] students)
    {
        Dictionary<string, List<Student>> groups = GroupingStudents(students);

        var dir = @"C:\temp\task4";
        WriteToFiles(dir, groups);
        
    }

    private static void WriteToFiles(string dir, Dictionary<string, List<Student>> groups)
    {
        Directory.CreateDirectory(dir);
        foreach (var group in groups)
        {
            var path = Path.Combine(dir, $"{group.Key}.txt");

            using (var sr = new StreamWriter(path))
                foreach (var stud in group.Value)
                    sr.WriteLine($"{stud.Name}\t{stud.DateOfBirth}");
        }

    }

    private static Dictionary<string, List<Student>> GroupingStudents(Student[] students)
        => students
            .GroupBy(s => s.Group, (k, v) => new { k, v = v.ToList() })
            .ToDictionary(s => s.k, s => s.v);

    private static Dictionary<string, List<Student>> GroupingStudents2(Student[] students)
    { 
        var groups = new Dictionary<string,List<Student>>();
        foreach (var stud in students) { 
            if(! groups.ContainsKey(stud.Group))
                groups.Add(stud.Group, new List<Student>());
            groups[stud.Group].Add(stud);
        }
        return groups;
    }

    static Student[] Deserialize(string file) // спасибо мелкомягкие
    {
        // Declare the hashtable reference.
       // List<Student> students = null;

        // Open the file containing the data that you want to deserialize.
        FileStream fs = new FileStream(file, FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();

            // Deserialize the hashtable from the file and
            // assign the reference to the local variable.
            return (Student[]) formatter.Deserialize(fs);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }
}
