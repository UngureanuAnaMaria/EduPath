import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SelectRoleComponent } from './select-role.component';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { By } from '@angular/platform-browser';

describe('SelectRoleComponent', () => {
  let component: SelectRoleComponent;
  let fixture: ComponentFixture<SelectRoleComponent>;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, SelectRoleComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SelectRoleComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should display the title and description', () => {
    const titleElement = fixture.debugElement.query(By.css('h2')).nativeElement;
    const descriptionElement = fixture.debugElement.query(By.css('p')).nativeElement;

    expect(titleElement.textContent).toContain('What type of account would you like to create?');
    expect(descriptionElement.textContent).toContain('Choose your role to continue with the registration process.');
  });

  it('should call selectRole with "student" when the Student button is clicked', () => {
    spyOn(component, 'selectRole');
    const studentButton = fixture.debugElement.query(By.css('.btn-primary')).nativeElement;
    studentButton.click();

    expect(component.selectRole).toHaveBeenCalledWith('student');
  });

  it('should call selectRole with "instructor" when the Instructor button is clicked', () => {
    spyOn(component, 'selectRole');
    const instructorButton = fixture.debugElement.query(By.css('.btn-success')).nativeElement;
    instructorButton.click();

    expect(component.selectRole).toHaveBeenCalledWith('instructor');
  });

  it('should store the selected role in localStorage and navigate to /register', () => {
    spyOn(localStorage, 'setItem');
    spyOn(router, 'navigate');

    component.selectRole('student');

    expect(localStorage.setItem).toHaveBeenCalledWith('selectedRole', 'student');
    expect(router.navigate).toHaveBeenCalledWith(['/register']);
  });
});