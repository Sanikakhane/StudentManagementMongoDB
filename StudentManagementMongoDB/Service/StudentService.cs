using StudentManagementMongoDB.Data;
using StudentManagementMongoDB.IService;
using MongoDB.Driver;

namespace BlazorApp1.Service
{
    public class StudentService : IStudentService
    {
        private MongoClient _mongoClient ;
        private IMongoDatabase _database;
        private IMongoCollection<Student> _studentTable ;
        public StudentService()
        {
            _mongoClient = new MongoClient("mongodb://localhost:30114/");

            _database = _mongoClient.GetDatabase("SchoolDB");
            _studentTable = _database.GetCollection<Student>("Students");
        }
        public string Delete(string studentId)
        {
            _studentTable.DeleteOne(st => st.Id == studentId);
            return "Deleted";
        }

        public Student GetStudent(string studentId)
        {
            return _studentTable.Find(st => st.Id == studentId).FirstOrDefault();
        }

        public List<Student> GetStudents()
        {
            return _studentTable.Find(FilterDefinition<Student>.Empty).ToList();
        }

        public void SaveOrUpdate(Student student)
        {
            

            if (string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.Roll) || student.Age == 0)
            {
                throw new ArgumentException("Student data is incomplete. Please provide all required fields.");
            }
            var studentObj = _studentTable.Find(x => x.Id == student.Id).FirstOrDefault();
            if (studentObj == null)
            {
                _studentTable.InsertOne(student);
            }
            else
            {
                _studentTable.ReplaceOne(x => x.Id == student.Id, student);
            }
        }
 

    }
}
