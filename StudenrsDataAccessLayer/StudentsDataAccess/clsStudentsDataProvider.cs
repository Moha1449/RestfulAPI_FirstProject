using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace StudentsDataAccessLayer.StudentsDataAccess
{
    public static class clsStudentsDataProvider
    {
        public static List<clsStudentEntity> GetAllStudents()
        {
            try
            {
                using (SqlConnection ObjConnection = new SqlConnection(clsDataBaseSettings.SqlServerConnectionString))
                {
                    using (SqlCommand ObjCommand = new SqlCommand("sp_GetAllStudents", ObjConnection))
                    {
                        ObjCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        ObjConnection.Open();

                        using (SqlDataReader ObjDataReader = ObjCommand.ExecuteReader())
                        {
                            if (ObjDataReader.Read())
                            {
                                List<clsStudentEntity> Students = new List<clsStudentEntity>();

                                while (ObjDataReader.Read())
                                {
                                    Students.Add(new clsStudentEntity
                                    {
                                        ID = ObjDataReader.GetInt32(ObjDataReader.GetOrdinal("ID")),
                                        Name = ObjDataReader.GetString(ObjDataReader.GetOrdinal("Name")),
                                        Grade = Convert.ToSingle(ObjDataReader.GetDouble(ObjDataReader.GetOrdinal("Grades")))
                                    });
                                }

                                return Students;
                            }

                            return null;

                        }
                    }
                }
            }
            catch (Exception Message)
            {
                return null;
            }
        }

        public static List<clsStudentEntity> GetPassedStudents()
        {
            try
            {
                using (SqlConnection ObjConnection = new SqlConnection(clsDataBaseSettings.SqlServerConnectionString))
                {
                    using (SqlCommand ObjCommand = new SqlCommand("sp_GetPassedStudents", ObjConnection))
                    {
                        ObjCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        ObjConnection.Open();

                        using (SqlDataReader ObjDataReader = ObjCommand.ExecuteReader())
                        {
                            if (ObjDataReader.Read())
                            {
                                List<clsStudentEntity> Students = new List<clsStudentEntity>();

                                while (ObjDataReader.Read())
                                {
                                    Students.Add(new clsStudentEntity
                                    {
                                        ID = ObjDataReader.GetInt32(ObjDataReader.GetOrdinal("ID")),
                                        Name = ObjDataReader.GetString(ObjDataReader.GetOrdinal("Name")),
                                        Grade = Convert.ToSingle(ObjDataReader.GetDouble(ObjDataReader.GetOrdinal("Grades")))
                                    });
                                }

                                return Students;
                            }

                            return null;

                        }
                    }
                }
            }
            catch (Exception Message)
            {
                return null;
            }
        }

        public static double GetStudentsAvg()
        {
            try
            {
                using (SqlConnection ObjConnection = new SqlConnection(clsDataBaseSettings.SqlServerConnectionString))
                {
                    using (SqlCommand ObjCommand = new SqlCommand("sp_GetStudentsAvg", ObjConnection))
                    {
                        ObjCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        ObjConnection.Open();

                        return Convert.ToDouble(ObjCommand.ExecuteScalar());
                    }
                }
            }
            catch (Exception Message)
            {
                return 0;
            }
        }

        public static clsStudentEntity GetStudentByID (int ID)
        {
            if (ID <= 0) return null;

            try
            {
                using (SqlConnection ObjConnection = new SqlConnection(clsDataBaseSettings.SqlServerConnectionString))
                {
                    using (SqlCommand ObjCommand = new SqlCommand("sp_GetStudentByID", ObjConnection))
                    {
                        ObjCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        ObjCommand.Parameters.AddWithValue("@ID", ID);

                        ObjConnection.Open();

                        using (SqlDataReader ObjDataReader = ObjCommand.ExecuteReader())
                        {
                            if (ObjDataReader.Read())
                            {
                                return new clsStudentEntity
                                {
                                    ID = ObjDataReader.GetInt32(ObjDataReader.GetOrdinal("ID")),
                                    Name = ObjDataReader.GetString(ObjDataReader.GetOrdinal("Name")),
                                    Grade = Convert.ToSingle(ObjDataReader.GetDouble(ObjDataReader.GetOrdinal("Grades")))
                                };
                            }

                            return null;

                        }
                    }
                }
            }
            catch (Exception Message)
            {
                return null;
            }
        }

        public static int AddNewStudent(clsStudentEntity Student)
        {
            if (Student == null || Student.Name == null) return -1;

            try
            {
                using (SqlConnection ObjConnection = new SqlConnection(clsDataBaseSettings.SqlServerConnectionString))
                {
                    using (SqlCommand ObjCommand = new SqlCommand("sp_AddNewStudent", ObjConnection))
                    {
                        ObjCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        ObjCommand.Parameters.AddWithValue("@Name", Student.Name);
                        ObjCommand.Parameters.AddWithValue("@Grade", Student.Grade);

                        ObjCommand.Parameters.Add("NewStudentID", System.Data.SqlDbType.Int).Direction 
                            = System.Data.ParameterDirection.ReturnValue;

                        ObjConnection.Open();
                        ObjCommand.ExecuteNonQuery();

                       int NewStudentID =  Convert.ToInt32(ObjCommand.Parameters["NewStudentID"].Value);

                        if (NewStudentID > 0)
                            return NewStudentID;
                    }
                }
            }
            catch (Exception Message)
            {
                return -1;
            }

            return -1;
        }

        public static bool UpdateStudent(clsStudentEntity Student)
        {

            if(Student == null||Student.Name == null ||Student.ID <= 0) return false;

            try
            {
                using (SqlConnection ObjConnection = new SqlConnection(clsDataBaseSettings.SqlServerConnectionString))
                {
                    using (SqlCommand ObjCommand = new SqlCommand("sp_UpdateStudentByID", ObjConnection))
                    {
                        ObjCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        ObjCommand.Parameters.AddWithValue("@Name", Student.Name);
                        ObjCommand.Parameters.AddWithValue("@Grade", Student.Grade);
                        ObjCommand.Parameters.AddWithValue("@ID", Student.ID);
                       
                        ObjConnection.Open();

                        int Affected = Convert.ToInt32(ObjCommand.ExecuteNonQuery());

                        if (Affected == 1)
                        {
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch (Exception Message)
            {
                return false;
            }

        }

        public static bool DeleteStudentByID(int ID)
        {
            if (ID <= 0) return false;

            try
            {
                using (SqlConnection ObjConnection = new SqlConnection(clsDataBaseSettings.SqlServerConnectionString))
                {
                    using (SqlCommand ObjCommand = new SqlCommand("sp_DeleteStudentByID", ObjConnection))
                    {
                        ObjCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        ObjCommand.Parameters.AddWithValue("@ID", ID);

                        ObjConnection.Open();

                        int Affected = Convert.ToInt32(ObjCommand.ExecuteNonQuery());

                        if (Affected == 1)
                        {
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch (Exception Message)
            {
                return false;
            }

        }

    }
}
