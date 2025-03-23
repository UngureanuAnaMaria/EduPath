//using Application.AI_ML_Module_Extern;
//using Microsoft.ML;
//using Microsoft.ML.Data;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace Application.AI_ML_Module_Generator
//{
//    public class StudentPerformancePredictionModel
//    {
//        private readonly MLContext mlContext;
//        private ITransformer averageGradeModel;
//        private ITransformer percentageCompletedCoursesModel;
//        private ITransformer learningPathModel;
//        private ITransformer futureCareerModel;

//        public StudentPerformancePredictionModel() => mlContext = new MLContext();

//        public void TrainAverageGradeModel(List<StudentData> trainingData, string modelPath)
//        {
//            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

//            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(StudentData.AverageGrade))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NameEncoded", inputColumnName: nameof(StudentData.Name)))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "EmailEncoded", inputColumnName: nameof(StudentData.Email)))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PasswordEncoded", inputColumnName: nameof(StudentData.Password)))
//                .Append(mlContext.Transforms.Conversion.ConvertType("CreatedAtString", nameof(StudentData.CreatedAt), DataKind.String))
//                .Append(mlContext.Transforms.Text.FeaturizeText("CreatedAtFeaturized", "CreatedAtString"))
//                .Append(mlContext.Transforms.Conversion.ConvertType("LastLoginString", nameof(StudentData.LastLogin), DataKind.String))
//                .Append(mlContext.Transforms.Text.FeaturizeText("LastLoginFeaturized", "LastLoginString"))
//                .Append(mlContext.Transforms.Concatenate("Features", "NameEncoded", "EmailEncoded", "PasswordEncoded", "CreatedAtFeaturized", "LastLoginFeaturized", nameof(StudentData.PercentageCompletedCourses)))
//                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
//                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));

//            averageGradeModel = pipeline.Fit(dataView);
//            SaveModel(averageGradeModel, modelPath);
//        }

//        public void TrainPercentageCompletedCoursesModel(List<StudentData> trainingData, string modelPath)
//        {
//            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

//            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(StudentData.PercentageCompletedCourses))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NameEncoded", inputColumnName: nameof(StudentData.Name)))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "EmailEncoded", inputColumnName: nameof(StudentData.Email)))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PasswordEncoded", inputColumnName: nameof(StudentData.Password)))
//                .Append(mlContext.Transforms.Conversion.ConvertType("CreatedAtString", nameof(StudentData.CreatedAt), DataKind.String))
//                .Append(mlContext.Transforms.Text.FeaturizeText("CreatedAtFeaturized", "CreatedAtString"))
//                .Append(mlContext.Transforms.Conversion.ConvertType("LastLoginString", nameof(StudentData.LastLogin), DataKind.String))
//                .Append(mlContext.Transforms.Text.FeaturizeText("LastLoginFeaturized", "LastLoginString"))
//                .Append(mlContext.Transforms.Concatenate("Features", "NameEncoded", "EmailEncoded", "PasswordEncoded", "CreatedAtFeaturized", "LastLoginFeaturized", nameof(StudentData.PercentageCompletedCourses)))
//                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
//                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));

//            percentageCompletedCoursesModel = pipeline.Fit(dataView);
//            SaveModel(percentageCompletedCoursesModel, modelPath);
//        }

//        public void TrainLearningPathModel(List<StudentData> trainingData, string modelPath)
//        {
//            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

//            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(StudentData.LearningPath))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NameEncoded", inputColumnName: nameof(StudentData.Name)))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "EmailEncoded", inputColumnName: nameof(StudentData.Email)))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PasswordEncoded", inputColumnName: nameof(StudentData.Password)))
//                .Append(mlContext.Transforms.Conversion.ConvertType("CreatedAtString", nameof(StudentData.CreatedAt), DataKind.String))
//                .Append(mlContext.Transforms.Text.FeaturizeText("CreatedAtFeaturized", "CreatedAtString"))
//                .Append(mlContext.Transforms.Conversion.ConvertType("LastLoginString", nameof(StudentData.LastLogin), DataKind.String))
//                .Append(mlContext.Transforms.Text.FeaturizeText("LastLoginFeaturized", "LastLoginString"))
//                .Append(mlContext.Transforms.Concatenate("Features",
//                    "NameEncoded",
//                    "EmailEncoded",
//                    "PasswordEncoded",
//                    "CreatedAtFeaturized",
//                    "LastLoginFeaturized",
//                    nameof(StudentData.PercentageCompletedCourses)))
//                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
//                .Append(mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated(labelColumnName: "Label", featureColumnName: "Features"))
//                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

//            learningPathModel = pipeline.Fit(dataView);
//            SaveModel(learningPathModel, modelPath);
//        }

//        public void TrainFutureCareerModel(string csvFilePath)
//        {
//            if (!File.Exists(csvFilePath))
//            {
//                throw new FileNotFoundException("CSV file not found.", csvFilePath);
//            }

//            var dataView = mlContext.Data.LoadFromTextFile<StudentDataExtern>(csvFilePath, hasHeader: true, separatorChar: ',');

//            var preview = dataView.Preview();
//            if (preview.RowView.Length == 0)
//            {
//                throw new InvalidOperationException("Training set has 0 instances, aborting training.");
//            }

//            foreach (var row in preview.RowView)
//            {
//                foreach (var column in row.Values)
//                {
//                    Console.Write($"{column.Key}: {column.Value} ");
//                }
//                Console.WriteLine();
//            }

//            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(StudentDataExtern.FutureCareer))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NameEncoded", inputColumnName: nameof(StudentDataExtern.Name)))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "MajorEncoded", inputColumnName: nameof(StudentDataExtern.Major)))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "InterestedDomainEncoded", inputColumnName: nameof(StudentDataExtern.InterestedDomain)))
//                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "ProjectsEncoded", inputColumnName: nameof(StudentDataExtern.Projects)))
//                .Append(mlContext.Transforms.Concatenate("Features",
//                    nameof(StudentDataExtern.StudentID),
//                    "NameEncoded",
//                    nameof(StudentDataExtern.Gender),
//                    nameof(StudentDataExtern.Age),
//                    nameof(StudentDataExtern.GPA),
//                    "MajorEncoded",
//                    "InterestedDomainEncoded",
//                    "ProjectsEncoded",
//                    nameof(StudentDataExtern.Python),
//                    nameof(StudentDataExtern.SQL),
//                    nameof(StudentDataExtern.Java)))
//                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
//                .Append(mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated(labelColumnName: "Label", featureColumnName: "Features"))
//                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

//            futureCareerModel = pipeline.Fit(dataView);
//        }

//        public void SaveFutureCareerModel(string modelPath)
//        {
//            mlContext.Model.Save(futureCareerModel, null, modelPath);
//        }

//        public string PredictFutureCareer(StudentDataExtern student)
//        {
//            var predictionEngine = mlContext.Model.CreatePredictionEngine<StudentDataExtern, StudentFutureCareerPrediction>(futureCareerModel);
//            var prediction = predictionEngine.Predict(student);
//            return prediction.FutureCareer;
//        }

//        public float PredictAverageGrade(StudentData student)
//        {
//            var predictionEngine = mlContext.Model.CreatePredictionEngine<StudentData, StudentAverageGradePrediction>(averageGradeModel);
//            var prediction = predictionEngine.Predict(student);
//            return prediction.AverageGrade;
//        }

//        public float PredictPercentageCompletedCourses(StudentData student)
//        {
//            var predictionEngine = mlContext.Model.CreatePredictionEngine<StudentData, StudentPercentageCompletedCoursesPrediction>(percentageCompletedCoursesModel);
//            var prediction = predictionEngine.Predict(student);
//            return prediction.PercentageCompletedCourses;
//        }

//        public string PredictLearningPath(StudentData student)
//        {
//            var predictionEngine = mlContext.Model.CreatePredictionEngine<StudentData, StudentLearningPathPrediction>(learningPathModel);
//            var prediction = predictionEngine.Predict(student);
//            return prediction.LearningPath;
//        }

//        public void LoadAverageGradeModel(string modelPath)
//        {
//            averageGradeModel = LoadModel(modelPath);
//        }

//        public void LoadPercentageCompletedCoursesModel(string modelPath)
//        {
//            percentageCompletedCoursesModel = LoadModel(modelPath);
//        }

//        public void LoadLearningPathModel(string modelPath)
//        {
//            learningPathModel = LoadModel(modelPath);
//        }

//        public void LoadFutureCareerPathModel(string modelPath)
//        {
//            futureCareerModel = LoadModel(modelPath);
//        }

//        private void SaveModel(ITransformer model, string modelPath)
//        {
//            mlContext.Model.Save(model, null, modelPath);
//        }

//        private ITransformer LoadModel(string modelPath)
//        {
//            using var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
//            return mlContext.Model.Load(stream, out var modelInputSchema);
//        }
//    }
//}

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;


namespace Application.AI_ML_Module
{
    public class StudentPerformancePredictionModel
    {
        private readonly MLContext mlContext;
        private ITransformer averageGradeModel;
        private ITransformer percentageCompletedCoursesModel;
        private ITransformer learningPathModel;
        private ITransformer futureCareerModel;

        public StudentPerformancePredictionModel() => mlContext = new MLContext();

        public void TrainAverageGradeModel(List<StudentData> trainingData, string modelPath)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(StudentData.AverageGrade))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NameEncoded", inputColumnName: nameof(StudentData.Name)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "EmailEncoded", inputColumnName: nameof(StudentData.Email)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PasswordEncoded", inputColumnName: nameof(StudentData.Password)))
                .Append(mlContext.Transforms.Conversion.ConvertType("CreatedAtString", nameof(StudentData.CreatedAt), DataKind.String))
                .Append(mlContext.Transforms.Text.FeaturizeText("CreatedAtFeaturized", "CreatedAtString"))
                .Append(mlContext.Transforms.Conversion.ConvertType("LastLoginString", nameof(StudentData.LastLogin), DataKind.String))
                .Append(mlContext.Transforms.Text.FeaturizeText("LastLoginFeaturized", "LastLoginString"))
                .Append(mlContext.Transforms.Concatenate("Features", "NameEncoded", "EmailEncoded", "PasswordEncoded", "CreatedAtFeaturized", "LastLoginFeaturized", nameof(StudentData.PercentageCompletedCourses)))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));

            averageGradeModel = pipeline.Fit(dataView);
            SaveModel(averageGradeModel, modelPath);
        }

        public void TrainPercentageCompletedCoursesModel(List<StudentData> trainingData, string modelPath)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(StudentData.PercentageCompletedCourses))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NameEncoded", inputColumnName: nameof(StudentData.Name)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "EmailEncoded", inputColumnName: nameof(StudentData.Email)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PasswordEncoded", inputColumnName: nameof(StudentData.Password)))
                .Append(mlContext.Transforms.Conversion.ConvertType("CreatedAtString", nameof(StudentData.CreatedAt), DataKind.String))
                .Append(mlContext.Transforms.Text.FeaturizeText("CreatedAtFeaturized", "CreatedAtString"))
                .Append(mlContext.Transforms.Conversion.ConvertType("LastLoginString", nameof(StudentData.LastLogin), DataKind.String))
                .Append(mlContext.Transforms.Text.FeaturizeText("LastLoginFeaturized", "LastLoginString"))
                .Append(mlContext.Transforms.Concatenate("Features", "NameEncoded", "EmailEncoded", "PasswordEncoded", "CreatedAtFeaturized", "LastLoginFeaturized", nameof(StudentData.PercentageCompletedCourses)))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));

            percentageCompletedCoursesModel = pipeline.Fit(dataView);
            SaveModel(percentageCompletedCoursesModel, modelPath);
        }

        public void TrainLearningPathModel(List<StudentData> trainingData, string modelPath)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(StudentData.LearningPath))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NameEncoded", inputColumnName: nameof(StudentData.Name)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "EmailEncoded", inputColumnName: nameof(StudentData.Email)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PasswordEncoded", inputColumnName: nameof(StudentData.Password)))
                .Append(mlContext.Transforms.Conversion.ConvertType("CreatedAtString", nameof(StudentData.CreatedAt), DataKind.String))
                .Append(mlContext.Transforms.Text.FeaturizeText("CreatedAtFeaturized", "CreatedAtString"))
                .Append(mlContext.Transforms.Conversion.ConvertType("LastLoginString", nameof(StudentData.LastLogin), DataKind.String))
                .Append(mlContext.Transforms.Text.FeaturizeText("LastLoginFeaturized", "LastLoginString"))
                .Append(mlContext.Transforms.Concatenate("Features",
                    "NameEncoded",
                    "EmailEncoded",
                    "PasswordEncoded",
                    "CreatedAtFeaturized",
                    "LastLoginFeaturized",
                    nameof(StudentData.PercentageCompletedCourses)))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated(labelColumnName: "Label", featureColumnName: "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            learningPathModel = pipeline.Fit(dataView);
            SaveModel(learningPathModel, modelPath);
        }

        public void TrainFutureCareerModel(string csvFilePath, string modelPath)
        {
            if (!File.Exists(csvFilePath))
            {
                throw new FileNotFoundException("CSV file not found.", csvFilePath);
            }

            var dataView = mlContext.Data.LoadFromTextFile<StudentDataExtern>(
                csvFilePath, hasHeader: true, separatorChar: ',');

            var preview = dataView.Preview();
            if (preview.RowView.Length == 0)
            {
                throw new InvalidOperationException("Training set has 0 instances, aborting training.");
            }

            foreach (var row in preview.RowView)
            {
                foreach (var column in row.Values)
                {
                    Console.Write($"{column.Key}: {column.Value} ");
                }
                Console.WriteLine();
            }

            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(StudentDataExtern.FutureCareer))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NameEncoded", inputColumnName: nameof(StudentDataExtern.Name)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "MajorEncoded", inputColumnName: nameof(StudentDataExtern.Major)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "InterestedDomainEncoded", inputColumnName: nameof(StudentDataExtern.InterestedDomain)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "ProjectsEncoded", inputColumnName: nameof(StudentDataExtern.Projects)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PythonEncoded", inputColumnName: nameof(StudentDataExtern.Python)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "SQLEncoded", inputColumnName: nameof(StudentDataExtern.SQL)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "JavaEncoded", inputColumnName: nameof(StudentDataExtern.Java)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "GenderEncoded", inputColumnName: nameof(StudentDataExtern.Gender)))
                .Append(mlContext.Transforms.Concatenate("Features",
                    "NameEncoded",
                    "GenderEncoded",
                    nameof(StudentDataExtern.Age),
                    nameof(StudentDataExtern.GPA),
                    "MajorEncoded",
                    "InterestedDomainEncoded",
                    "ProjectsEncoded",
                    "PythonEncoded",
                    "SQLEncoded",
                    "JavaEncoded"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated(
                    labelColumnName: "Label",
                    featureColumnName: "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            futureCareerModel = pipeline.Fit(dataView);
            SaveModel(futureCareerModel, modelPath);
        }

        public string PredictFutureCareer(StudentDataExtern student)
        {
            var predictionEngine = mlContext.Model.CreatePredictionEngine<StudentDataExtern, StudentFutureCareerPrediction>(futureCareerModel);
            var prediction = predictionEngine.Predict(student);
            return prediction.FutureCareer;
        }

        public float PredictAverageGrade(StudentData student)
        {
            var predictionEngine = mlContext.Model.CreatePredictionEngine<StudentData, StudentAverageGradePrediction>(averageGradeModel);
            var prediction = predictionEngine.Predict(student);
            return prediction.AverageGrade;
        }

        public float PredictPercentageCompletedCourses(StudentData student)
        {
            var predictionEngine = mlContext.Model.CreatePredictionEngine<StudentData, StudentPercentageCompletedCoursesPrediction>(percentageCompletedCoursesModel);
            var prediction = predictionEngine.Predict(student);
            return prediction.PercentageCompletedCourses;
        }

        public string PredictLearningPath(StudentData student)
        {
            var predictionEngine = mlContext.Model.CreatePredictionEngine<StudentData, StudentLearningPathPrediction>(learningPathModel);
            var prediction = predictionEngine.Predict(student);
            return prediction.LearningPath;
        }

        public void LoadAverageGradeModel(string modelPath)
        {
            averageGradeModel = LoadModel(modelPath);
        }

        public void LoadPercentageCompletedCoursesModel(string modelPath)
        {
            percentageCompletedCoursesModel = LoadModel(modelPath);
        }

        public void LoadLearningPathModel(string modelPath)
        {
            learningPathModel = LoadModel(modelPath);
        }

        public void LoadFutureCareerPathModel(string modelPath)
        {
            futureCareerModel = LoadModel(modelPath);
        }

        private void SaveModel(ITransformer model, string modelPath)
        {
            mlContext.Model.Save(model, null, modelPath);
        }

        private ITransformer LoadModel(string modelPath)
        {
            using var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return mlContext.Model.Load(stream, out var modelInputSchema);
        }
    }
}



