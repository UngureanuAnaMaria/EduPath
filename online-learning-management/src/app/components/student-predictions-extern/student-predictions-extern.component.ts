import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, AbstractControl, ReactiveFormsModule, ValidationErrors } from '@angular/forms';
import { StudentService } from '../../services/student.service';
import { StudentDataExternDTO, StudentPredictionsExtern } from '../../models/student.model';
import { Router, RouterModule } from '@angular/router';

export function genderValidator(control: AbstractControl): ValidationErrors | null {
  const validGenders = ["Male", "Female"];
  const value = control.value;
  if (value && !validGenders.includes(value.trim())) {
    return { invalidGender: true };
  }
  return null;
}

export function levelValidator(control: AbstractControl): ValidationErrors | null {
  const validLevels = ["Weak", "Strong", "Average"];
  const value = control.value;
  if (value && !validLevels.includes(value.trim())) {
    return { invalidLevel: true };
  }
  return null;
}

@Component({
  selector: 'app-student-predictions-extern',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './student-predictions-extern.component.html',
  styleUrls: ['./student-predictions-extern.component.css']
})
export class StudentPredictionsExternComponent {
  studentForm: FormGroup;
  predictions: StudentPredictionsExtern | null = null;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private router: Router
  ) {
    this.studentForm = this.fb.group({
      name: ['', [Validators.required, Validators.pattern('[a-zA-Z ]*')]],
      gender: ['', [Validators.required, genderValidator]],
      age: ['', [Validators.required, Validators.min(1), Validators.max(100)]],
      gpa: ['', [Validators.required, Validators.min(0), Validators.max(4), this.gpaValidator]],
      major: ['', Validators.required],
      interestedDomain: ['', Validators.required],
      projects: ['', Validators.required],
      python: ['', [Validators.required, levelValidator]],
      sql: ['', [Validators.required, levelValidator]],
      java: ['', [Validators.required, levelValidator]]
    });
  }
  gpaValidator(control: AbstractControl) {
    const gpa = control.value;
    if (gpa < 0 || gpa > 4) {
      return { invalidGpa: true };
    }
    return null;
  }

  submitForm() {
    if (this.studentForm.valid) {
      const studentData: StudentDataExternDTO = this.studentForm.value;
      this.studentService.getStudentPredictionsExtern(studentData).subscribe(
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
