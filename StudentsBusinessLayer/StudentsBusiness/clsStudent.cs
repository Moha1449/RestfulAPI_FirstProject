using StudentsDataAccessLayer.StudentsDataAccess;

namespace StudentsBusinessLayer.StudentsBusiness
{
    public class clsStudent
    {
        public int ID { get; private set; }

        public string Name { get; private set; }

        public float Grade { get; private set; }

        public clsStudentDTO DTO { get { return new clsStudentDTO { ID = this.ID, Name = this.Name, Grade = this.Grade }; } }

        private enum enModes { Add = 1, Update }

        private enModes Mode;

        public clsStudent(clsStudentDTO Student)
        {
            ID = -1;
            Name = Student.Name;
            Grade = Student.Grade;

            Mode = enModes.Add;
        }

        private clsStudent(clsStudentEntity Student, enModes Mode)
        {
            this.ID = Student.ID;
            this.Name = Student.Name;
            this.Grade = Student.Grade;

            this.Mode = Mode;
        }

        public static List<clsStudentDTO> GetAllStudents()
        {
            var Students = clsStudentsDataProvider.GetAllStudents();

            return Students?
                .Select(S => new clsStudentDTO { ID = S.ID, Name = S.Name, Grade = S.Grade })
                .ToList() ?? new List<clsStudentDTO>();
        }

        public static List<clsStudentDTO> GetPassedStudents()
        {
            return clsStudentsDataProvider.GetPassedStudents()?
                 .Select(S => new clsStudentDTO { ID = S.ID, Name = S.Name, Grade = S.Grade })
                 .ToList() ?? new List<clsStudentDTO>();
        }

        public static double GetStudentsAvg()
        {
            return clsStudentsDataProvider.GetStudentsAvg();
        }

        public static clsStudent Find(int ID)
        {
            var Student = clsStudentsDataProvider.GetStudentByID(ID);

            if (Student == null) return null;

            return new clsStudent(Student, enModes.Update);
        }

        private bool _AddNewStudent()
        {
            this.ID = clsStudentsDataProvider.AddNewStudent(new clsStudentEntity { Name = this.Name, Grade = this.Grade });

            return (this.ID > 0);
        }

        public bool SetValues(clsStudentDTO Student)
        {
            if (!(Mode == enModes.Update))
                return false;

            this.Name = Student.Name;
            this.Grade = Student.Grade;

            return true;
        }

        private bool _UpdateStudent()
        {
            return clsStudentsDataProvider.UpdateStudent(new clsStudentEntity { ID = this.ID, Name = this.Name, Grade = this.Grade });
        }

        public static bool DeleteStudentByID(int ID)
        {
            return clsStudentsDataProvider.DeleteStudentByID(ID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enModes.Add:
                    {

                        return _AddNewStudent();

                    }
                    case enModes.Update:
                    {
                        return _UpdateStudent();
                    }
            }

            return false;
        }
    }
}
