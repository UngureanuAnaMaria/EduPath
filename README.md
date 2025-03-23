# EduPath - Intelligent Online Learning Management System 🎓💡

## Description
EduPath is an intelligent online learning management system designed to provide a seamless and personalized learning experience for both students and instructors. The platform integrates machine learning to predict student performance and suggest tailored learning paths, ensuring that each student achieves their educational goals efficiently. The system is built with a focus on security, scalability, and user-friendliness, leveraging modern technologies and architectural patterns for a robust and maintainable solution.

---

## Key Features 🌟

### 1. **User Authentication 🔐**
- **Secure Access**: Role-based access control ensures that students and instructors have appropriate permissions within the system.
- **Authentication & Authorization**: JSON Web Tokens (JWT) are used for secure authentication and user data protection.

### 2. **Intuitive Interface 🖥️**
- **User-Friendly Design**: Developed using Angular and Angular Material, EduPath offers an intuitive and responsive interface to simplify course management, progress tracking, and navigation.

### 3. **Courses & Enrollment 📚**
- **Course Details**: Students can view detailed course information, including descriptions and instructors.
- **Easy Enrollment**: Quickly enroll in courses that align with personal interests and learning goals.

### 4. **Lessons & Content 📖**
- **Lesson Access**: Courses consist of multiple lessons, allowing students to explore content and access supplementary materials.
- **Organized Data**: Data is structured using Clean Architecture, and PostgreSQL handles the storage of all data.

### 5. **Performance Tracking 📊**
- **Progress Monitoring**: Student progress is tracked through recorded assessments, scores, and dates, ensuring ongoing monitoring and personalized feedback.

### 6. **Course Management by Instructors 🧑‍🏫**
- **Content Control**: Instructors can manage courses, add lessons and track student progress.
- **Efficiency Boost**: Using CQRS (Command Query Responsibility Segregation) and MediatR enhances the system's efficiency and separates course management and student enrollment tasks.

---

## Personalized Learning Paths & Performance Predictions 🤖

- **Tailored Recommendations**: Recommendation algorithms predict student performance and suggest courses based on past learning history.
  
---

## Technical & Architectural Structure ⚙️

### **Clean Architecture** 🏗️
- A clear separation of concerns allows for easy maintenance, scalability, and extensibility. The architecture ensures that the user interface, business logic, and data access layers are decoupled.

### **CQRS & MediatR** 🔄
- **CQRS**: Separates read and write operations to enhance performance and simplify the system's structure.
- **MediatR**: Handles communication between commands and queries, promoting a decoupled and modular codebase.

### **Result Pattern** 📈
- The Result Pattern ensures standardized operation results, with each operation returning a `Result<T>`, indicating success or failure along with relevant error messages.

---

## Technologies Used 🛠️

### **Backend** ⚙️
- **ASP.NET Core**: For backend logic and API endpoints, using MediatR and CQRS.
- **PostgreSQL**: Structured data storage, ensuring data integrity and complex query support.
- **JWT (JSON Web Tokens)**: Used for secure authentication and user authorization.

### **Frontend** 🎨
- **Angular & Angular Material**: Provides an intuitive, responsive, and dynamic user interface.

### **Machine Learning** 📊
- **Recommendation Algorithms**: For predicting student performance and suggesting personalized learning paths.

### **Testing** 🧪
- **Unit Testing**: Mock dependencies with NSubstitute.
- **Integration Testing**: Verifies interactions between different components.
- **Code Coverage**: Achieved 80% coverage for both frontend and backend to ensure reliable and maintainable code.

---

## System Components 🛠️

### **Students** 👩‍🎓👨‍🎓
- Browse courses, enroll, view content and receive personalized recommendations.

### **Instructors** 👨‍🏫
- Manage courses, add lessons and monitor student progress.

### **Web Application** 🌐
- Built on .NET, it serves as the primary interface for users, delivering static content and handling requests.

### **Database** 🗄️
- PostgreSQL stores structured data, including course content, user data, and progress tracking.

### **Single Page Application (SPA)** 📱
- Built with Angular, it enables seamless interactions between instructors, students, and system features.

### **API Application** 🌐
- Built on the .NET framework, it provides data and functionality through API endpoints.

### **Machine Learning Module** 🤖
- Provides personalized learning recommendations and performance predictions for students.

---
## Documentation & Specifications 📄

For detailed project specifications, including system architecture, API details, and user guides, please refer to the provided **PDF** documentation:

[Download the Project Specifications PDF](https://github.com/user-attachments/files/19411742/Projects-ITONET.pdf)

---

## How to Run the Project in Visual Studio and VSCode 💻

1. **Clone the Repository**:  
   Clone the repository to your local machine using Git:
   ```bash
   https://github.com/UngureanuAnaMaria/EduPath.git

2. **Open the Project**:  
   - Open the cloned repository in **Visual Studio** for the backend API.
   - Open the frontend folder in **VSCode** for the Angular application.

3. **Install Dependencies**:  
   - In **Visual Studio**, ensure that all required dependencies for the backend are installed (e.g., NuGet packages).
   - In **VSCode**, navigate to the frontend directory in the terminal and run:
   ```bash
   npm install

4. **Build the Solution**:  
   - In **Visual Studio**, right-click the solution and select **Build** to compile the backend.
   - In **VSCode**, run the Angular development server by typing:
   ```bash
   ng serve
This will start the frontend application on the default port (usually http://localhost:4200).

5. **Run the Project:**
   After building the solution in Visual Studio, press F5 or click on Start to run the backend API.
   The frontend will be running in VSCode through ng serve.
6. **Access the Application:**
   Open a web browser and navigate to http://localhost:<port> (Visual Studio will display the port number for the backend API, and http://localhost:4200 for the frontend). This will open the EduPath application, where you can begin using it.
The backend can also be accessed through Swagger at http://localhost:<port>/swagger.

