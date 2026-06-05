// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Diagnostics;

// CONFIG
string mover_file = "mover.bat";
string shell = "/bin/bash";
string github_repo_link = "";

void generate_rat(string target, string file) {
	// CREATE A DIRECTORY FOR THE TARGET
	System.Diagnostics.Process.Start("/bin/bash", $"-c \"mkdir {target}\"");
	string file_path = target + "/" + file + ".bat";
	File.WriteAllText(file_path, "@echo off");	
	File.WriteAllText(file_path, "setlocal enabledelayedexpansion");
	File.WriteAllText(file_path, ":loop");
	File.WriteAllText(file_path, "");
	File.WriteAllText(file_path, "");
	File.WriteAllText(file_path, "");
	File.WriteAllText(file_path, "");
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
    	Console.Write("\nEnter target's name: ");
	string target_name = Console.ReadLine();
	Console.Write("\nEnter RAT file name (eg: antivirus) : ");
	string file = Console.ReadLine();
	// GENERATES THE RAT FILE
	generate_rat(target_name, file);
	
    } else if (opn == "2") {

    } else if (opn == "3") {

    } else {
         return;
    } 
    
}
