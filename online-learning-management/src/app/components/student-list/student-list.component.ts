import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StudentService } from '../../services/student.service';
import { StudentDTO } from '../../models/student.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.css']
})
export class StudentListComponent implements OnInit {
  students: StudentDTO[] = [];
  totalCount: number = 0;
  pageSize: number = 2;
  pageNumber: number = 1;

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
      this.totalCount = data.totalCount;
    });
  }

  onPageChange(page: number): void {
    if (page > 0 && page <= this.getTotalPages()) {
      this.pageNumber = page;
      this.loadStudents();
    }
  }
}
