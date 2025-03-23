export interface StudentDTO {
  id: string;
  name: string;
  email: string;
  password: string;
  status: boolean;
  createdAt: Date;
  lastLogin: Date;
  studentCourses?: StudentCourseDTO[];
}

export interface StudentDataExternDTO {
  name: string;
  gender: string;
  age: number;
  gpa: number;
  major: string;
  interestedDomain: string;
  projects: string;
  futureCareer: string;
  python: string;
  sql: string;
  java: string;
}

export interface StudentCourseDTO {
  id: string;
  studentId: string;
  courseId: string;
}

export interface StudentPredictions {
  averageGrade: number;
  percentageCompletedCourses: number;
  learningPath: string;
}

export interface StudentPredictionsExtern {
  futureCareer: string;
}


