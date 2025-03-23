import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CourseService } from '../../services/course.service';
import { CourseDTO } from '../../models/course.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
  courses: CourseDTO[] = [];
  filteredCourses: CourseDTO[] = [];
  totalCount: number = 0;
  pageSize: number = 3;
  pageNumber: number = 1;
  pageSizes: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
  filters = {
    name: '',
    description: ''
  };

  getTotalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  constructor(private courseService: CourseService, private router: Router) {}

  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses(): void {
    this.courseService.getCourses(this.pageNumber, this.pageSize).subscribe((data: any) => {
      this.courses = data.courses;
      console.log('Courses:', this.courses);
      this.filteredCourses = this.courses;
      this.totalCount = data.totalCount;
      this.applyFilters();
    });
  }

  applyFilters(): void {
    this.filteredCourses = this.courses.filter(course => {
      return (!this.filters.name || course.name.toLowerCase().includes(this.filters.name.toLowerCase())) &&
             (!this.filters.description || course.description.toLowerCase().includes(this.filters.description.toLowerCase()))
    });
  }

  onPageChange(page: number): void {
    if (page > 0 && page <= this.getTotalPages()) {
      this.pageNumber = page;
      this.loadCourses();
    }
  }

  onPageSizeChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.pageSize = Number(selectElement.value);
    this.pageNumber = 1; // Reset to first page when page size changes
    this.loadCourses();
  }

  onCreate() {
    this.router.navigate(['/course-create']);
  }

  onGetInfo(course: CourseDTO) {
    this.router.navigate([`/course-detail/${course.id}`]);
  }

  onDelete(course: CourseDTO) {
    if (confirm('Are you sure you want to delete this course?')) {
      this.courseService.deleteCourse(course.id).subscribe(
        () => {
          console.log('Course deleted successfully');
          alert('Course deleted successfully!');
          this.loadCourses();
        },
        (error) => {
          console.error('Error deleting course', error);
          alert('An error occurred while deleting the course.');
        }
      );
    }
  }

  onLogout(): void {
    localStorage.removeItem('token');
    alert('Logged out successfully!');
    //this.toastr.success('Logged out successfully!', 'Logout');
    //localStorage.clear();
    this.router.navigate(['/welcome-page']);
  }

}
