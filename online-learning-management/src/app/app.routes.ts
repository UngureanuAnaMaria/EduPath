import { Routes } from '@angular/router';
import { StudentUpdateComponent } from './components/student-update/student-update.component';
import { StudentDetailComponent } from './components/student-detail/student-detail.component';
import { StudentListComponent } from './components/student-list/student-list.component';
import { StudentCreateComponent } from './components/student-create/student-create.component';

export const routes: Routes = [
  { path: '', redirectTo: '/student-list', pathMatch: 'full' },
  { path: 'student-update', component: StudentUpdateComponent },
  { path: 'student-detail', component: StudentDetailComponent },
  { path: 'student-list', component: StudentListComponent },
  { path: 'student-create', component: StudentCreateComponent }
];

