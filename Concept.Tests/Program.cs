using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Artisan.Tools.Logger;
using Artisan.Tools.Exceptions;
using Artisan.Tools.Configurator;
using Artisan.Tools.Reflector;
using System.IO;
using Artisan.Tools.Store;

namespace Artisan.Tools.Concept.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggerInit();

            //LoggerTest();

            //ExceptionTest();

            DbTest();

            //QeryTest();

            //Tool2 tool = Config.Manager.GetSection<Tool2>();


            //Student student = Config.Section<Student>();
            /*
            Log.Write(Level.Info, "{0}", args:student.ToString());
            if (student.Enrollments != null)
            {
                foreach (var e in student.Enrollments)
                {
                    Log.Write(Level.Info, "\t{0}", args: e.Course.ToString());
                }
            }
            
            TextSerializer<Student> serializer = (TextSerializer<Student>)TextSerializerBuilder.Instance.Create<Student>();
            StringBuilder doc = new StringBuilder();
            serializer.Serialize(student, doc);
            Log.Write(Level.Info, "{0}", args: doc.ToString());
            */

            //TestDeserializer();
            //TestSerializer();

            Console.WriteLine("Finalizado");
            Console.ReadLine();
        }

        private static void QeryTest()
        {
            /*
            Query<Student> qs = new Query<Student>();
            var r = qs.Where(s => s.FirstMidName == "juan");
            foreach (var i in r)
            {
                Log.Write(Level.Debug, i.ToString());
            }
            */
        }

        private static void LoggerInit()
        {
            Log.Config.SetRootLevel(Level.Verbose);
            Log.Config.Apply();
        }

        private static void LoggerTest()
        {
            Log.Write(Level.Fatal, "Probando");
            Log.Write(Level.Error, "Probando en {0}", args: Level.Error);
            Log.Write(Level.Warn, "Probando en {0}", args: Level.Warn);
            Log.Write(Level.Info, "Probando en {0}", args: Level.Info);
            Log.Write(Level.Debug, "Probando en {0}", args: Level.Debug);
            Log.Write(Level.Verbose, "Probando en {0}", args: Level.Verbose);

        }

        private static void ExceptionTest()
        {
            try
            {
                throw new AppException("Prueba y error");
            }
            catch
            {

            }
            try
            {
                throw new AppException(Level.Debug, "Prueba y error");
            }
            catch
            {

            }
            try
            {
                try
                {
                    throw new ApplicationException("Prueba de inner exception");
                }
                catch (Exception ex)
                {
                    throw new AppException(ex, "Y aca nos salimos a ver que onda");
                }
            }
            catch
            {

            }

        }

        private static void DbTest()
        {
            StoreSet<Student> store = new StoreSet<Student>(
                typeof(SchoolContext)
            );

            Student student = store.FirstOrDefault(t => t.Id == 1, "Enrollments.Course");
            //Student student = store.FirstOrDefault(t => t.Id == 1);
            student.DumpToLog(Level.Info);

            Student stu2 = new Copier<Student>().Copy(student);
            stu2.DumpToLog(Level.Warn);

        }

        private static void TestDeserializer()
        {
            XmlSerializer<Student> serializer = (XmlSerializer<Student>)XmlSerializerBuilder.Instance.Create<Student>();
            
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "student.xml"));
            
            //StringBuilder builder = new StringBuilder();
            Student student = serializer.Deserialize( null, doc.DocumentElement);

            Console.WriteLine(student);

            return;
        }

        private static void TestSerializer()
        {

            Student student = new Student { Id = 1, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2005-09-01") };

            Course c1 = new Course { Id = 1050, Title = "Chemistry", Credits = 3, };
            Course c2 = new Course { Id = 4022, Title = "Microeconomics", Credits = 3, };
            Course c3 = new Course { Id = 4041, Title = "Macroeconomics", Credits = 3, };

            
            Enrollment e1 = new Enrollment { Id = 1, StudentID = 1, CourseID = 1050, Grade = Grade.A };
            e1.Course = c1;
            Enrollment e2 = new Enrollment { Id = 2, StudentID = 1, CourseID = 4022, Grade = Grade.C };
            e2.Course = c2;
            Enrollment e3 = new Enrollment { Id = 3, StudentID = 1, CourseID = 4041, Grade = Grade.B };
            e3.Course = c3;

            List < Enrollment> es = new List<Enrollment>();
            es.Add(e1);
            es.Add(e2);
            es.Add(e3);
            student.Enrollments = es;//.ToArray();

            XmlSerializer <Student> serializer = (XmlSerializer<Student>)XmlSerializerBuilder.Instance.Create<Student>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<configurator></configurator>");

            serializer.Serialize(student, doc.DocumentElement);

            doc.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "student2.xml"));
        }

    }
}
