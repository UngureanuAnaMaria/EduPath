import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-select-role',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule, CommonModule],
  templateUrl: './select-role.component.html',
  styleUrls: ['./select-role.component.css']
})


export class SelectRoleComponent {
  constructor(private router: Router) {}

  selectRole(role: string): void {
    localStorage.setItem('selectedRole', role);
    this.router.navigate(['/register']);
  }
}
