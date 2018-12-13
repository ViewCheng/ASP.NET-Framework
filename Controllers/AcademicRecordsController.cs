using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab7.Models.DataAccess;
using Lab7.Models;
using Microsoft.AspNetCore.Http;

namespace Lab7.Controllers
{
    public class AcademicRecordsController : Controller
    {
        private readonly StudentRecordContext _context;

        public AcademicRecordsController(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: AcademicRecords
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Edit");
        }

        // GET: AcademicRecords/Details/5

        // GET: AcademicRecords/Create
        public IActionResult Create()
        {
            ViewBag.ErrorStyle = "Color:red; display:none";

            SetCourseAndStudentViewData();
            return View();
        }

        private void SetCourseAndStudentViewData()
        {
            var co = from c in _context.Course select new { Code = c.Code, Text = c.Code + " - " + c.Title };
            ViewData["CourseCode"] = new SelectList(co, "Code", "Text");

            var st = from s in _context.Student select new { Id = s.Id, Text = s.Id + " - " + s.Name };
            ViewData["StudentId"] = new SelectList(st, "Id", "Text");

        }
        private bool AcademicRecordExists(string id, string code)
        {
            return _context.AcademicRecord.Any(e => e.StudentId == id && e.CourseCode == code);
        }

        // POST: AcademicRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            if (ModelState.IsValid)
            {
                if (!AcademicRecordExists(academicRecord.StudentId, academicRecord.CourseCode))
                {
                    _context.Add(academicRecord);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorStyle = "color:red; display:block";
                }

            }

            SetCourseAndStudentViewData();
            return View(academicRecord);
        }

        // GET: AcademicRecords/Edit/5
        public async Task<IActionResult> Edit(string sort)
        {
            var studentRecordContext = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(a => a.Student);
            AcademicRecord[] records = studentRecordContext.ToArray();
            AcademicRecord[] sortedRecords = records;
            if (sort == "course")
            {
                sortedRecords = records.OrderBy(r => r.CourseCodeNavigation.Title).ToArray();
            }
            else
            {
                sortedRecords = records.OrderBy(r => r.Student.Name).ToArray();
            }

            if (HttpContext.Session.GetString("SortOrder") == null || HttpContext.Session.GetString("SortOrder") == "Descending")
            {
                HttpContext.Session.SetString("SortOrder", "Ascending");
            }
            else
            {
                HttpContext.Session.SetString("SortOrder", "Descending");
                sortedRecords = sortedRecords.Reverse().ToArray();
            }



            return View(sortedRecords);
        }

        // POST: AcademicRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AcademicRecord[] academicRecords)
        { 

            if (ModelState.IsValid)
            { 
                foreach(AcademicRecord record in academicRecords)
                {
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Edit");
            }
            
            return View(academicRecords);
        }

        // GET: AcademicRecords/Delete/5

        // POST: AcademicRecords/Delete/5



    }
}
