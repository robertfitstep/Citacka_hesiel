using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Citacka_hesiel
{
    class ProcessPasswordFile
    {

        private string fileName;
        private string hashedFileName;
        private string logFileName;
        private string fileExtention = ".txt";
        private string currentDirectory;
        private bool logToConsole;
        private bool logToFile;

        public ProcessPasswordFile(string fileName, bool logToConsole, bool logToFile)
        {
            currentDirectory = Directory.GetCurrentDirectory() + @"\";
            this.fileName = fileName;
            hashedFileName = fileName + "_Hashed";
            logFileName = fileName + ".log";
            this.logToConsole = logToConsole;
            this.logToFile = logToFile;
            createEmptyHahedFile(currentDirectory + hashedFileName + fileExtention);
        }

        public void processFile()
        {
            User currentUser;
            string timeStamp;
            string[] lines = readAllLines(currentDirectory + fileName + fileExtention);
            foreach (string line in lines)
            {
                currentUser = returnUser(line);
                timeStamp = getTimeStamp();
                writeToHashedFile(currentUser);
                logProcessedUser(currentUser, timeStamp);
            }
        }

        private void writeToHashedFile(User currentUser)
        {
            try
            {
                using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(currentDirectory + hashedFileName + fileExtention, true))
                {
                    file.WriteLine("{0} {1}", currentUser.Name, currentUser.HasehedPassword);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        private void logProcessedUser(User currentUser, string timeStamp)
        {
            if (logToConsole) writeLogToConsole(returnLogLine(currentUser, timeStamp));
            if (logToFile) writeLogToFile(returnLogLine(currentUser, timeStamp));
        }

        private void logExeption(string line)
        {
            if (logToConsole) writeLogToConsole(line);
            if (logToFile) writeLogToFile(line);
        }

        private string returnLogLine(User currentUser, string timeStamp)
        {
            if (currentUser.ValidUser) return string.Format("{0}: {1} processed successfully", timeStamp, currentUser.Name);
            else return string.Format("{0}: {1}", timeStamp, currentUser.ThrownExeption);
        }

        private string returnLogLine(string line)
        {
            return string.Format("{0}: {1}", getTimeStamp(), line);
        }

        private void writeLogToConsole(string line)
        {
            Console.WriteLine(line);
        }

        private void writeLogToFile(string line)
        {
            try
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(currentDirectory + logFileName + fileExtention, true))
                {
                    file.WriteLine(line);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                logToFile = false;
                logExeption(returnLogLine(e.Message));
            }
            catch(Exception e)
            {
                logExeption(returnLogLine(e.Message));
            }
            
        }

        private string[] readAllLines(string pathToFile)
        {
            try
            {
                return System.IO.File.ReadAllLines(pathToFile);
            }
            catch(Exception e)
            {
                logExeption(returnLogLine(e.Message));
                return new string[] {null};
            }
        }

        private User returnUser(string line)
        {
            string hashedPass;
            string userName;
            string exeptionMessage;
            bool validUser = parseNameAndHashPass(line, out hashedPass, out userName, out exeptionMessage);
            return new User(validUser, userName, hashedPass, exeptionMessage);
        }

        private bool parseNameAndHashPass(string line, out string hashedPass, out string userName, out string exeptionMessage)
        {
            try
            {
                string[] values = line.Split(' ');
                userName = values[0];
                hashedPass = getMd5Hash(concateratePass(values));
                exeptionMessage = null;
                return true;
            }
            catch(Exception e)
            {
                userName = null;
                hashedPass = null;
                exeptionMessage = e.Message;
                return false;
            }
        }

        private string concateratePass(string[] values)
        {
            //cocaterate pass (just in case there is a gap)
            string concateratedPass = null;
            for(int i = 1; i < values.Length;i++) concateratedPass = concateratedPass + values[i];
            return concateratedPass;
        }

        private string getMd5Hash(string input)
        {
            byte[] data;
            using (MD5 md5Hash = MD5.Create()) data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++) sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        private void createEmptyHahedFile(string pathToHashedFile)
        {
            try
            {
                using (File.Create(pathToHashedFile)) { }
            }
            catch(Exception e)
            {
                logExeption(returnLogLine(e.Message));
            }
        }
        
        private string getTimeStamp()
        {
            return DateTime.Now.ToString("dd.MM yyyy HH:mm:ss.ffff");
        }
    }
}
