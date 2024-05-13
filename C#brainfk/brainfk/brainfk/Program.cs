using System;
using System.Collections.Generic;
using System.Text;

public class BrainfuckInterpreter
{
    class Program
    {
        static void Main()
        {
            BrainfuckInterpreter interpreter = new BrainfuckInterpreter();

            // Example usage:
            string brainfuckProgram =
                ">++++++++[<+++++++++>-]<.>++++[<+++++++>-]<+.+++++++..+++.>>++++++[<+++++++>-]<+\n+.------------.>++++++[<+++++++++>-]<+.<.+++.------.--------.>>>++++[<++++++++>-\n]<+.";

            string brainfucktest1 = "+++++++++[>++++++++<-]>.";
            string brainfucktest2 =
                "++++>++><<[- >[->>+<<]>>[-<+<+>>]<<<]>>++++++++++++++++++++++++++++++++++++++++++++++++.";
            string brainfucktest3 =
                ">>++++++++[->++++++++<]>>>>+++++++++[->++++++++++<]>[<<,[->+<<+<<+>>>]<<<[->>>+<<<]>>>>>[->+>>+<<<]>[<<[->+>>+<<<]>>>[-<<<+>>>]<<[[-]<->]>-]>>[-<<<+>>>]<<<<<<<[-<+<<+>>>]<[>>[-<+<<+>>>]<<<[->>>+<<<]>>[[-]>-<]<-]<<[->>>+<<<]>>>>><[[-]>++++++++++++++++++++++++++++++++>[[-]<-------------------------------->]<<]>>[-]<.>>]";
                
            string inputString = "8";
            interpreter.Interpret(brainfucktest3, inputString);
            interpreter.ResetState();
            
            interpreter.Interpret(brainfucktest2, inputString);
            interpreter.ResetState();
            
            interpreter.Interpret(brainfucktest1, inputString);
            interpreter.ResetState();
        }
    }
    
    #region InterpreterVariables
    
    const int MEMORY_SIZE = 30000;
    int[] memory = new int[MEMORY_SIZE];
    // Instruction pointer (Points to the current INSTRUCTION)
    int instructionPointer = 0;
    // Memory pointer (Points to a cell in MEMORY)
    int memoryPointer = 0;
    // Address stack. Used to track addresses (index) of left brackets
    Stack<int> bracketAdressStack = new Stack<int>();

    string program = "";
    string input = "6";
    StringBuilder output = new StringBuilder();

    #endregion
    
    public void ResetState()
    {
        // Clear memory, reset pointers to zero.
        Array.Fill(memory, 0);
        instructionPointer = 0;
        memoryPointer = 0;
        output.Clear();
        input = "";
        program = "";
        bracketAdressStack.Clear();
    }

    public void SendOutput(int value)
    {
        output.Append((char)value);
    }

    public int GetInput()
    {
        int val = 0;
        
        if (!string.IsNullOrEmpty(input))
        {
            val = input[0];
            input = input.Substring(1);
        }

        return val;
    }

    public string Interpret(string inputProgram, string inputString)
    {
        program = inputProgram;
        input = inputString;
        bool end = false;

        while (!end)
        {
            if (instructionPointer >= program.Length)
            {
                end = true;
                break;
            }
            
            switch (program[instructionPointer])
            {
                case '>':
                    if (memoryPointer == memory.Length - 1)
                        Array.Resize(ref memory, memory.Length + 5);
                    memoryPointer++;
                    break;
                case '<':
                    if (memoryPointer > 0)
                        memoryPointer--;
                    break;
                case '+':
                    memory[memoryPointer]++;
                    break;
                case '-':
                    memory[memoryPointer]--;
                    break;
                case '.':
                    SendOutput(memory[memoryPointer]);
                    break;
                case ',':
                    memory[memoryPointer] = GetInput();
                    break;
                case '[':
                    if (memory[memoryPointer] != 0) // If non-zero
                    {
                        bracketAdressStack.Push(instructionPointer);
                    }
                    else // Skip to matching right bracket
                    {
                        int count = 0;
                        while (true)
                        {
                            instructionPointer++;
                            if (instructionPointer >= program.Length) break;
                            if (program[instructionPointer] == '[') count++;
                            else if (program[instructionPointer] == ']')
                            {
                                if (count != 0) count--;
                                else break;
                            }
                        }
                    }
                    break;
                case ']':
                    // Pointer is automatically incremented every iteration, therefore we must decrement to get the correct value
                    instructionPointer = bracketAdressStack.Pop() - 1;
                    break;
            }
            instructionPointer++;
        }

        Console.WriteLine(output);
        return output.ToString();
    }
}


