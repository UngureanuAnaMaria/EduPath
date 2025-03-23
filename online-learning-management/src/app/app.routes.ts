import { Routes } from '@angular/router';
import { StudentUpdateComponent } from './components/student-update/student-update.component';
import { StudentDetailComponent } from './components/student-detail/student-detail.component';
import { StudentListComponent } from './components/student-list/student-list.component';
import { StudentCreateComponent } from './components/student-create/student-create.component';
import { StudentPredictionComponent }from './components/student-prediction/student-prediction.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { CourseListComponent } from './components/course-list/course-list.component';
import { CourseCreateComponent } from './components/course-create/course-create.component';
import { CourseDetailComponent } from './components/course-detail/course-detail.component';
import { AuthGuard } from './guards/auth.guard';
import { StudentPredictionsExternComponent } from './components/student-predictions-extern/student-predictions-extern.component';
import { WelcomePageComponent } from './components/welcome-page/welcome-page.component';
import { AboutComponent } from './components/about/about.component';
import { ContactComponent } from './components/contact/contact.component';
import { SelectRoleComponent } from './components/select-role/select-role.component';

export const routes: Routes = [
  { path: '', redirectTo: '/welcome-page', pathMatch: 'full' },
  { path: 'student-update/:id', component: StudentUpdateComponent, canActivate: [AuthGuard]  },
  { path: 'student-detail/:id', component: StudentDetailComponent, canActivate: [AuthGuard]  },
  { path: 'student-list', component: StudentListComponent, canActivate: [AuthGuard]  },
  { path: 'student-prediction/:id', component: StudentPredictionComponent, canActivate: [AuthGuard]  },
  { path: 'student-create', component: StudentCreateComponent, canActivate: [AuthGuard]  },
  { path: 'student-predictions-extern', component: StudentPredictionsExternComponent, canActivate: [AuthGuard]  },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'welcome-page', component: WelcomePageComponent },
  { path: 'about', component: AboutComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'select-role', component: SelectRoleComponent },
  { path: 'course-list', component: CourseListComponent, canActivate: [AuthGuard] },
  { path: 'course-create', component: CourseCreateComponent, canActivate: [AuthGuard] },
  { path: 'course-detail/:id', component: CourseDetailComponent, canActivate: [AuthGuard] }
];
