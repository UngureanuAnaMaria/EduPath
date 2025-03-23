import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl, AbstractControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { StudentDTO } from '../../models/student.model';
import { StudentService } from '../../services/student.service';

@Component({
  selector: 'app-student-update',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  templateUrl: './student-detail.component.html',
  styleUrls: ['./student-detail.component.css']
})
export class StudentDetailComponent implements OnInit {
  studentIdControl: FormControl;
  student: StudentDTO | null = null;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.studentIdControl = new FormControl('', [Validators.required, this.validateGuid]);
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.studentIdControl.setValue(id);
        this.searchStudent();
      }
    });
  }

  searchStudent() {
    if (this.studentIdControl.valid) {
      this.studentService.getStudentById(this.studentIdControl.value).subscribe(
        (data) => {
          if (this.isInvalidStudent(data)) {
            this.student = null;
            this.errorMessage = 'Invalid student ID.';
          } else {
            this.student = data;
            this.errorMessage = null;
          }
          //console.log('Student data:', data);
        },
        (error) => {
          this.student = null;
          this.errorMessage = 'Error fetching student.';
          console.error('Error fetching student', error);
        }
      );
    }
  }

  deleteStudent(): void {
    if (confirm('Are you sure you want to delete this student?')) {
      if (this.student) {
        this.studentService.deleteStudent(this.student.id).subscribe(
          () => {
            console.log('Student deleted successfully');
            alert('Student deleted successfully!');
            this.student = null;
            this.studentIdControl.reset();
            this.router.navigate(['/student-list']);
          },
          (error) => {
            console.error('Error deleting student', error);
            alert('An error occurred while deleting the student.');
          }
        );
      } else {
        alert('No student selected to delete.');
      }
    }
  }

  validateGuid(control: any) {
    const guidPattern = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;
    return guidPattern.test(control.value) ? null : { invalidGuid: true };
  }

  isInvalidStudent(student: StudentDTO): boolean {
    return student.id === '00000000-0000-0000-0000-000000000000' || !student.name || !student.email;
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
