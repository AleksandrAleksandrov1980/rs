// See https://aka.ms/new-console-template for more information
using ASTRALib;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
Console.WriteLine("Hello, World!");
Rastr rastr = new ASTRALib.Rastr();

//IResultValues resultValues = new ASTRALib.ResultValues();

Dynamic d= rastr.Dynamic;
FWDynamic fw= rastr.FWDynamic();
rastr.FWSnapshotFiles();
var ssd = fw.MacroControl;

// вариант 1
void add_log( LogErrorCodes Code,  int Level, int StageId,  string TableName, int TableIndex,  string Description, string FormName)
{
    Console.WriteLine(Description);
}
rastr.OnLog += add_log;

// вариант 2 
rastr.OnLog += (LogErrorCodes Code, int Level, int StageId, string TableName, int TableIndex, string Description, string FormName) =>
{
    Console.WriteLine( "2 ->" + Description);
};

rastr.Load(ASTRALib.RG_KOD.RG_REPL, @"D:\Vms\SHARA\crosses\ODU_SZ\2022_12_07_new_sechs\12_58_00\mdp_debug_1_1", "");
rastr.rgm("");

rastr.prot += (string str) =>
{
    Console.WriteLine("3 ->" + str);
};
rastr.ExecMacroPath(@"D:\Vms\SHARA\crosses\ODU_SZ\2022_12_07_new_sechs\12_58_00\tst.rbs");

Console.WriteLine("finished!");
Console.ReadKey();