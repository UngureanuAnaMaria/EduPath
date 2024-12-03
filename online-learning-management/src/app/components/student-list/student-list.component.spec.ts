import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StudentListComponent } from './student-list.component';
import { StudentService } from '../../services/student.service';
import { of } from 'rxjs';

describe('StudentListComponent', () => {
  let component: StudentListComponent;
  let fixture: ComponentFixture<StudentListComponent>;
  let mockStudentService: any;

  beforeEach(async () => {
    mockStudentService = {
      getStudents: jasmine.createSpy('getStudents').and.returnValue(of({
        students: [
          { name: 'John Doe', email: 'john@example.com', status: true },
          { name: 'Jane Doe', email: 'jane@example.com', status: false }
        ],
        totalCount: 2
      }))
    };

    await TestBed.configureTestingModule({
      imports: [StudentListComponent], // Mutăm componenta aici
      providers: [{ provide: StudentService, useValue: mockStudentService }]
    }).compileComponents();

    fixture = TestBed.createComponent(StudentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should fetch and display students', () => {
    expect(component.students.length).toBe(2);
    expect(mockStudentService.getStudents).toHaveBeenCalled();
  });

  it('should disable the Previous button on the first page', () => {
    component.pageNumber = 1;
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    const prevButton = compiled.querySelector('.pagination button:first-child');
    expect(prevButton.disabled).toBeTrue();
  });

  it('should calculate total pages correctly', () => {
    component.totalCount = 10;
    component.pageSize = 2;
    expect(component.getTotalPages()).toBe(5);
  });
});
