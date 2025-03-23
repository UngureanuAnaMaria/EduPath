import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl, AbstractControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { StudentDTO } from '../../models/student.model';
import { StudentService } from '../../services/student.service';
import { StudentPredictions } from '../../models/student.model';

@Component({
  selector: 'app-student-prediction',
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  templateUrl: './student-prediction.component.html',
  styleUrl: './student-prediction.component.css'
})
export class StudentPredictionComponent implements OnInit {
  studentForm: FormGroup;
  studentIdControl: FormControl;
  student: StudentDTO | null = null;
  predictions: StudentPredictions | null = null;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.studentIdControl = new FormControl('', [Validators.required, this.validateGuid]);
    this.studentForm = this.fb.group({
      studentId: ['', [Validators.required, this.validateGuid]]
    });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.studentIdControl.setValue(id);
        this.studentForm.get('studentId')?.setValue(id);
        this.searchStudent();
      }
    });
  }

  searchStudent() {
    if (this.studentForm.valid) {
      this.studentService.getStudentById(this.studentForm.get('studentId')?.value).subscribe(
        (data) => {
          if (this.isInvalidStudent(data)) {
            this.student = null;
            this.errorMessage = 'Invalid student ID.';
          } else {
            this.student = data;
            this.errorMessage = null;
            this.getPredictions();
          }
        },
        (error) => {
          this.student = null;
          this.errorMessage = 'Error fetching student.';
          console.error('Error fetching student', error);
        }
      );
    }
  }

  getPredictions() {
    if (this.student) {
      this.studentService.getStudentPredictions(this.student.id).subscribe(
        (data) => {
          this.predictions = data;
          this.errorMessage = null;
        },
        (error) => {
          this.predictions = null;
          this.errorMessage = 'Error fetching predictions.';
          console.error('Error fetching predictions', error);
        }
      );
    }
  }

  validateGuid(control: AbstractControl) {
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
