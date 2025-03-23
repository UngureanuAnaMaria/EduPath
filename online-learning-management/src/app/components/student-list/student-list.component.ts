import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { StudentService } from '../../services/student.service';
import { StudentDTO } from '../../models/student.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.css']
})
export class StudentListComponent implements OnInit {
  students: StudentDTO[] = [];
  filteredStudents: StudentDTO[] = [];
  totalCount: number = 0;
  pageSize: number = 3;
  pageNumber: number = 1;
  pageSizes: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
  filters = {
    name: '',
    email: '',
    status: '',
    createdAt: '',
    lastLogin: '',
    courses: ''
  };

  getTotalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  constructor(private studentService: StudentService, private router: Router) {}

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.studentService.getStudents(this.pageNumber, this.pageSize).subscribe((data: any) => {
      this.students = data.students;
      this.filteredStudents = this.students;
      this.totalCount = data.totalCount;
      this.applyFilters();
    });
  }

  applyFilters(): void {
    this.filteredStudents = this.students.filter(student => {
      return (!this.filters.name || student.name.toLowerCase().includes(this.filters.name.toLowerCase())) &&
             (!this.filters.email || student.email.toLowerCase().includes(this.filters.email.toLowerCase())) &&
             (!this.filters.status || student.status.toString() === this.filters.status) &&
             (!this.filters.createdAt || new Date(student.createdAt).toDateString() === new Date(this.filters.createdAt).toDateString()) &&
             (!this.filters.lastLogin || (student.lastLogin && new Date(student.lastLogin).toDateString() === new Date(this.filters.lastLogin).toDateString())) &&
             (!this.filters.courses || (student.studentCourses && student.studentCourses.some(course => course.courseId.toLowerCase().includes(this.filters.courses.toLowerCase()))));
    });
  }

  onPageChange(page: number): void {
    if (page > 0 && page <= this.getTotalPages()) {
      this.pageNumber = page;
      this.loadStudents();
    }
  }

  onPageSizeChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.pageSize = Number(selectElement.value);
    this.pageNumber = 1; // Reset to first page when page size changes
    this.loadStudents();
  }

  onCreate() {
    this.router.navigate(['/student-create']);
  }

  onUpdate(student: StudentDTO) {
    this.router.navigate([`/student-update/${student.id}`]);
  }

  onGetInfo(student: StudentDTO) {
    this.router.navigate([`/student-detail/${student.id}`]);
  }

  onDelete(student: StudentDTO) {
    if (confirm('Are you sure you want to delete this student?')) {
      this.studentService.deleteStudent(student.id).subscribe(
        () => {
          console.log('Student deleted successfully');
          alert('Student deleted successfully!');
          this.loadStudents();
        },
        (error) => {
          console.error('Error deleting student', error);
          alert('An error occurred while deleting the student.');
        }
      );
    }
  }

  onPredict(student: StudentDTO) {
    this.router.navigate([`/student-prediction/${student.id}`]);
  }

  onGetPredictions() {
    this.router.navigate(['/student-predictions-extern']);
  }

  onLogout(): void {
    localStorage.removeItem('token');
    alert('Logged out successfully!');
    //this.toastr.success('Logged out successfully!', 'Logout');
    //localStorage.clear();
    this.router.navigate(['/welcome-page']);
  }

}
