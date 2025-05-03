// See https://aka.ms/new-console-template for more information

using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using TaskManager.Domain;

Console.WriteLine("What is your name?");
string userName = Console.ReadLine();
Console.WriteLine("What is your age?");
string userAgeInput = Console.ReadLine();
int userAge = 0;
// Console.WriteLine($"Ok {userName}, you are {userAgeInput} years old.");
// int nextYearUserAge = userAge + 1;
// Console.WriteLine($"Next year you will be {nextYearUserAge}");
if (!int.TryParse(userAgeInput, out userAge))
{
    Console.WriteLine("Invalid age entered, using default age 0.");
}
else
{
    Console.WriteLine($"You are {userAge} years old.");
}


// The code below is commented out, as i was getting warning about the way i was initializing the list, so tried different approach
// List<string> taskList = new List<string>();
// taskList.Add("Learn programming");
// taskList.Add("Learn databases");
// taskList.Add("Learn Probability and Statistics");
// Console.WriteLine("Tasks: ");

List<string> taskList =
[
    "Learn programming",
    "Learn Databases",
    "Learn Probability and Statistics",
];

void PrintTasks(List<string> tasksToPrint)
{
    foreach (string item in tasksToPrint)
    {
        Console.WriteLine($"- {item} ");
    }
}
PrintTasks(taskList);

// ---------- Other data types ------------
int userLevel = 5;
double averageScore = 88.39;
bool isActive = true;
DateTime registrationTime = new DateTime(2024, 2, 23);

Console.WriteLine($"User level: {userLevel}");
Console.WriteLine($"Average score: {averageScore}");
Console.WriteLine($"Is active: {isActive}");
Console.WriteLine($"Registration Date: {registrationTime}");

// Working with objects:
Console.WriteLine("\n--- My first Task object ---");

// create a new object instance of the SimpleTask class
// SimpleTask myFirstTask = new SimpleTask("Learn Blazor", false);
// myFirstTask.MarkComplete();
// Console.WriteLine($"Task: {myFirstTask.Description}");
// Console.WriteLine($"Status: {myFirstTask.IsDone}");



// Console.WriteLine("\n--- My second Task object ---");
// SimpleTask mySecondTask = new SimpleTask("Learn .net", true);

// Console.WriteLine($"Task : {mySecondTask.Description}");
// Console.WriteLine($"Status: {mySecondTask.IsDone}");

Console.WriteLine($"Total tasks created: {SimpleTask.TaskCounter}");


DeadlineTask myDeadlineTask = new DeadlineTask(
    "Create a nice application in c#",
    false,
    new DateTime(2025, 11, 30)
);
Console.WriteLine($"\n ---My first Deadline Task ---");
Console.WriteLine($"Task: {myDeadlineTask.Description}");
Console.WriteLine($"State: {myDeadlineTask.IsDone}");
Console.WriteLine($"Due: {myDeadlineTask.DueDate}");

List<SimpleTask> allTasks = new List<SimpleTask>();
// allTasks.Add(myFirstTask);
// allTasks.Add(mySecondTask);
allTasks.Add(myDeadlineTask);

Console.WriteLine("\n--- Polymorphic Task Statuses ---");

AppointmentTask myAppointment = new AppointmentTask(
    "meeting with John Skeet.",
    false,
    new DateTime(2025, 06, 15)
);

allTasks.Add(myAppointment);

foreach (SimpleTask task in allTasks)
{
    if (task is IDisplayable displayableTask)
    {
        Console.WriteLine(displayableTask.GetDisplayString());
    }
    else
    {
        Console.WriteLine($"Task is not displayable {task.Description}");
    }
}

Console.WriteLine("\n --- Value Type Demo (struct) ---");
PointStruct point1 = new PointStruct(); //create a struct instance
point1.X = 10;
point1.Y = 20;

PointStruct point2 = point1; //Assign point 1 to point2 (value is copied)

Console.WriteLine($"Initial: point1.X = {point1.X}, point2.X = {point2.X}");
//Modify the copy
point2.X = 99;

Console.WriteLine($"After changing point2 point1.X = {point1.X}, point2.X = {point2.X}");

Console.WriteLine("\n --- Reference Type Demo (class) ---");
//We already have myDeadlineTask created earlier
DeadlineTask taskRef1 = myDeadlineTask;
DeadlineTask taskRef2 = taskRef1; // Assign taskRef1 to taskRef2 (reference is COPIED)

Console.WriteLine(
    $"Initial: taskRef1.IsDone = {taskRef1.IsDone}, taskRef2.IsDone = {taskRef2.IsDone}"
);
Console.WriteLine($"   (Both reference the task: '{taskRef1.Description}')");

//Value type parameter
int myNumber = 5;
Console.WriteLine($"Value Type - before method: myNumber = {myNumber}");
TryChangeValue(myNumber);
Console.WriteLine($"Value Type - After Method: myNumber = {myNumber}");

// Reference Type paramater
//Assuming myDeadlineTask exists and IsDone is initially false
//(if not, create a new one: DeadLineTask taskToComplete = new DeadlineTask("test Param", false, DateTime.Now); )

DeadlineTask taskToComplete = myDeadlineTask;
Console.WriteLine(
    $"Reference Type - Before method: Task  '{taskToComplete.Description}' IsDone = {taskToComplete.IsDone}"
);
CompleteTheTask(taskToComplete);
Console.WriteLine(
    $"Reference Type - After the method: task '{taskToComplete.Description}' isDone = {taskToComplete.IsDone}"
);


//  -------   LINQ --------
// Example: Find all tasks that are not yet complete
// The lambda expression 'task => !task.IsDone is applied to each task on the list
// 'Where' returns a sequence of tasks for which the Lambda returns true.

var incompleteTasks = allTasks.Where(task => !task.IsDone); // Note: !task.IsDone is the same as task.IsDone = false
Console.WriteLine("\n --- LINQ: Incomplete tasks ---");
foreach (var task in incompleteTasks)
{
    // We can still use IDosplayable here if we want
    if (task is IDisplayable displayable)
    {
        Console.WriteLine(displayable.GetDisplayString());
    }
}

// --- LINQ select ---
// Example: get just the description of all tasks
// The lambda 'task => task.Description' takes a task and returns its description string.
// 'Select' returns a sequence of strings

var taskDescriptions = allTasks.Select(task => task.Description);
Console.WriteLine("\n--- LINQ: Task Descriptions ---");
foreach (string description in taskDescriptions)
{
    Console.WriteLine($"- {description}");
}

// Example: get a LIST of descriptions immediately:
List<string> descriptionList = allTasks.Select(task => task.Description).ToList();
// Now descriptionList is a regular List<string> containing the results

var allAppointmentTasks = allTasks.Where(task => task is AppointmentTask);
Console.WriteLine("\n --- LINQ exercise: Appointment tasks ---");
foreach (var task in allAppointmentTasks)
{
    Console.WriteLine($"- {task.Description}");
}

List<string> appointmentTasksDescriptions = allAppointmentTasks.Select(task => task.Description).ToList();
foreach (var description in appointmentTasksDescriptions)
{
    Console.WriteLine(description);
}

// --- LINQ: FirstOfDefault - a first item matching the condition ---
// Example: Find the first task that os a;ready completed
SimpleTask firstCompletedTask = allTasks.FirstOrDefault(task => task.IsDone);

static void TryChangeValue(int number)
{
    number = number + 10;
    Console.WriteLine($" Inside TryChangeValue: number = {number}");
}

static void CompleteTheTask(DeadlineTask task)
{
    Console.WriteLine($" Inside CompleteTheTask: Marking task '{task.Description}' as complete...");
    task.MarkComplete();
}
public struct PointStruct
{
    public int X; public int Y;
}

namespace TaskManager.Domain
{
    public interface IDisplayable
    {
        string GetDisplayString();
    }

    public abstract class SimpleTask
    {
        public static int TaskCounter;
        public string Description { get; set; }
        public bool IsDone { get; private set; }

        public SimpleTask(string Description, bool IsDone)
        {
            this.Description = Description;
            this.IsDone = IsDone;
            Console.WriteLine($"SimpleTask object created with description: '{this.Description}'");
            TaskCounter++;
        }
        public void MarkComplete()
        {
            this.IsDone = true;
        }

        public virtual string GetStatusMessage()
        {
            return $"Task '{Description}' - Status: {(IsDone ? "Completed" : "Pending")}";
        }

        public abstract string GetTaskType();
    }

    public class DeadlineTask : SimpleTask, IDisplayable
    {
        public DateTime DueDate { get; set; }

        public DeadlineTask(string Description, bool IsDone, DateTime DueDate) : base(Description, IsDone)
        {
            this.DueDate = DueDate;
            Console.WriteLine(
                $"DeadlineTask object created with due date: {this.DueDate.ToShortDateString()}"
            );
        }

        public override string GetStatusMessage()
        {
            return $"{base.GetStatusMessage()} - Due: {DueDate.ToShortDateString()}";
        }

        public override string GetTaskType()
        {
            return "Deadline Task";
        }

        public string GetDisplayString()
        {
            return $"{GetStatusMessage()}";
        }
    }

    public class AppointmentTask : SimpleTask, IDisplayable
    {
        public DateTime AppointmentTime { get; set; }

        public AppointmentTask(string Description, bool IsDone, DateTime appointmentTime) : base(Description, IsDone)
        {
            this.AppointmentTime = appointmentTime;
        }

        public override string GetTaskType()
        {
            return "Appointment Task";
        }

        public override string GetStatusMessage()
        {
            return $"{base.GetStatusMessage()} - Appointment time: {AppointmentTime.ToShortDateString()}";
        }

        public string GetDisplayString()
        {
            return $"{GetStatusMessage()}";
        }

    }

}



