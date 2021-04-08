/*  
    Solution: ED E-Registry
    Developer: Omeiza Alabi
    E-mail: omeiza.alabi@edsinetechnologiesltd.com
    Phone Number: +234705583-564
    Company's Website: www.edsinetechnologiesltd.com
*/

using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace project_csv
{
    class Program
    {
        
        
        static void Main(string[] args)
        {

            string option = String.Empty;
            string[] questions = {"First Name", "Last Name", "Email", "NIN", "Gender", "Date of birth: Day/Month/Year"};
            string[] answers = new string[5];
            string filePath = @"C:\EdsineProjects\project_csv\";
            string fileName = "data.csv";

            Console.WriteLine("Welcome to Ed E-Registry!!");
            Console.WriteLine("===========================");
            Console.WriteLine();
            do 
            {
                
                Console.WriteLine("Please select Task/Action:");
                Console.WriteLine("1: Create record in E-Registry!");
                Console.WriteLine("2: Update record in E-Registry!");
                Console.WriteLine("3: Delete record in E-Registry!");
                Console.WriteLine("4: Search for record in E-Registry!");
                Console.WriteLine("5: Print Record");
                Console.WriteLine("6: Send E-mails to all clients");
                Console.WriteLine("7: Exit E-Registry Application");
                Console.WriteLine();
                Console.Write("Select an Option: ");

                option = Console.ReadLine();
                // check for invalid option entry
                switch(option) 
                {
                    case "1":
                        Console.WriteLine("Creating record in E-Registry......"); 
                        CreateRecord(filePath, fileName, questions); 
                        break;
                    case "2":
                        Console.WriteLine("Updating record in E-Registry......"); 
                        UpdateRecord(filePath, fileName, questions); 
                        break;
                    case "3":
                        Console.WriteLine("Deleting record in E-Registry......"); 
                        DeleteRecord(filePath, fileName); 
                        break;
                    case "4":
                        Console.WriteLine("Searching for record in E-Registry......"); 
                        SearchRecord(filePath, fileName); 
                        break;
                    case "5":
                        Console.WriteLine("Printing record......"); 
                        PrintRecord(filePath, fileName, questions);
                        break;
                    case "6":
                        Console.WriteLine("Sending E-mails to all clients......"); 
                        SendEmailToClients(filePath, fileName);
                        break;
                    case "7":
                        Console.WriteLine("Exiting E-Registry Application......"); 
                        Thread.Sleep(3000);
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("You selected an invalid entry"); 
                        Console.WriteLine(""); 
                        break; 
                }
            }

            while(option != "7");
            

        }

        static void CreateRecord(string filePath, string fileName, string[] questions) {

                string idString = "";
                string oldId = "";
                string text = "";
                int id = 1;

                while(!Directory.Exists(filePath)) {
                    Directory.CreateDirectory(filePath);
                }
                
                while(!File.Exists(filePath + "lastId.txt")) {
                    File.Create(filePath + "lastId.txt").Dispose();
                }

                idString =  File.ReadAllText(filePath + "lastId.txt");
                

                if(idString == "") {
                    idString = "1";
                }
                
                // File.AppendAllText(filePath + fileName, "S/N,First Name,Last Name,NIN,Gender,DOB\n");
                File.AppendAllText(filePath + fileName, $"{idString},");

                Console.WriteLine("Input the data to be stored");
                for(int i = 0; i< questions.Length; i++) {
                    Console.Write(questions[i] + ": ");
                    string answer = Console.ReadLine();
                    File.AppendAllText(filePath + fileName, $"{answer},");
                }
                File.AppendAllText(filePath + fileName, "\n");
                Console.WriteLine("Data Recorded Successfully \n");

                oldId = idString;
                id = int.Parse(idString);
                id++;
                idString = Convert.ToString(id);
                text = File.ReadAllText(filePath + "lastId.txt");
                text = text.Replace(oldId, idString);
                File.WriteAllText(filePath + "lastId.txt", text);

            }

        static void UpdateRecord(string filePath, string fileName, string[] questions) {
            DeleteRecord(filePath, fileName);
            CreateRecord(filePath, fileName, questions);
            Console.WriteLine("Update successful");
        }

        static int SearchRecord(string filePath, string fileName) {

            int index = 1;
            Console.Write("Please enter the user's NIN: ");
            string nin = Console.ReadLine();
            string replyText = "No user found. Please check NIN and try again";
            string[] lines = File.ReadAllLines(filePath + fileName);
            foreach(var line in lines.Select((value, i) => new {value, i}))
            {

                if (line.value.Contains(nin)) {
                    replyText = "";
                    index = line.i;
                    string[] columns = line.value.Split(',');
                    if(columns[3] == nin) {
                        foreach (string column in columns) {
                            Console.Write(column + " ");
                        }
                        Console.WriteLine("\n");
                        break;
                    } 
                    else {
                        Console.WriteLine("No user found. Please check NIN and try again"); 
                    }
                    
                } 
            }
            Console.WriteLine(replyText);
            return index;
        }

        static void PrintRecord(string filePath, string fileName, string[] questions) {
            string[] lines = File.ReadAllLines(filePath + fileName);
            Console.Write("S/N" + "\t");
            foreach(string question in questions) {
                Console.Write(question + "\t");
            }
            Console.WriteLine();
            foreach(string line in lines) {
                string[] columns = line.Split(',');
                foreach(string column in columns) {
                    Console.Write(column + "\t");
                }
                Console.WriteLine();
            }
        }

        static void SendEmailToClients(string filePath, string fileName) {
            string firstName, lastName, email, nin, gender, dob;
            string[] lines = File.ReadAllLines(filePath + fileName);

            foreach(string line in lines) {
                string[] columns = line.Split(',');
                firstName = columns[1];
                lastName = columns[2];
                email = columns[3];
                nin = columns[4];
                gender = columns[5];
                dob = columns[6];
                try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("mail.edsinetechnologiesltd.com");

                        mail.From = new MailAddress("no-reply@edsinetechnologiesltd.com");
                        mail.To.Add(email);
                        mail.Subject = "Success!!!";

                        // using (StreamReader reader = File.OpenText(filePath + "index.html")) 
                        // {                                                         
                              
                        // }
                        mail.Body = File.ReadAllText(filePath + "index.html");
                        // mail.Body = new StreamReader(filePath + "index.html").ReadToEnd();
                        mail.Body = mail.Body.Replace("{firstName}", firstName);
                        mail.Body = mail.Body.Replace("{lastName}", lastName);
                        mail.Body = mail.Body.Replace("{email}", email);
                        mail.Body = mail.Body.Replace("{nin}", nin);
                        mail.Body = mail.Body.Replace("{gender}", gender);
                        mail.Body = mail.Body.Replace("{dob}", dob);

                        mail.IsBodyHtml = true;
                        SmtpServer.Port = 26;
                        SmtpServer.Credentials = new NetworkCredential("omeiza.alabi@edsinetechnologiesltd.com", "Skippy24*#");
                        // SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        Console.WriteLine("Email sent successfully to " + email);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Emails send failure");
                        Console.WriteLine(ex.ToString());
                    }
            }
        }

        static void DeleteRecord(string filePath, string fileName) {
            int index;
            index = SearchRecord(filePath, fileName);

            string[] lines = File.ReadAllLines(filePath + fileName);
            lines = lines.Where(e => e != lines[index]).ToArray();
            File.WriteAllLines(filePath + fileName, lines);
            Console.WriteLine("Record deleted successfully");
        }

    }

}

