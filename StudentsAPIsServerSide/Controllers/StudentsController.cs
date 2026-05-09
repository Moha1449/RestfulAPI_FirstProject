using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsBusinessLayer.StudentsBusiness;

namespace StudentsAPIsServerSide.Controllers
{
    [Route("Students")]
    [ApiController]
    public class StudentsApiController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<clsStudentDTO>> GetAllStudents()
        {
            var Students = clsStudent.GetAllStudents();

            if (Students.Count == 0) return NotFound("There are no students in the database");

            return Ok(Students);
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<clsStudentDTO>> GetPassedStudents()
        {
            var Students = clsStudent.GetPassedStudents();

            if (Students.Count == 0) return NotFound("There are no students passed");

            return Ok(Students);
        }

        [HttpGet("Avg", Name = "GetStudentsAvg")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<double> GetStudentsAvg()
        {
            return Ok(clsStudent.GetStudentsAvg());
        }

        [HttpGet("{ID}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<clsStudentDTO> GetStudentByID(int ID)
        {

            if (ID <= 0) return BadRequest($"Invalid Input {ID}");

            var Student = clsStudent.Find(ID);

            if (Student == null) return NotFound($"StudentData With this ID is not found : {ID}");

            return Ok(Student.DTO);
        }

        [HttpPost(Name ="AddNewStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<int> AddNewStudent(clsStudentDTO StudentData)
        {
            if (StudentData == null||StudentData.Name == null) return BadRequest("Invalid StudentData Data");

            clsStudent NewStudent = new clsStudent(StudentData);

            if(!NewStudent.Save())
                return StatusCode(500,new { message = "Add failed" });

            return CreatedAtRoute("GetStudentByID", new {ID = NewStudent.ID}, NewStudent.DTO);
        }


        [HttpPut(Name="UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsStudentDTO> UpdateStudent(clsStudentDTO Student)
        {
            if (Student == null||Student.ID <= 0|| Student.Name ==null) return BadRequest("Invalid Student Data");

            var StudentData = clsStudent.Find(Student.ID);

            if (StudentData == null) return NotFound($"Student with {Student.ID} not found");

            StudentData.SetValues(Student);

            if (!StudentData.Save())
                return StatusCode(500,new {message =  "Updated failed" });

            return Ok(StudentData.DTO);

        }


        [HttpDelete("{ID}",Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsStudentDTO> DeleteStudentByID(int ID)
        {
            if ( ID <= 0 ) return BadRequest("Invalid ID");

            var StudentData = clsStudent.Find(ID);

            if (StudentData == null) return NotFound($"Student with {ID} not found");

            if (!clsStudent.DeleteStudentByID(ID)) return StatusCode(500,new { message = "Delete failed" });

            return Ok($"The Student with {ID} is deleted");

        }

    }

}
